import {
  Component,
  OnInit,
  Input,
  OnDestroy,
  forwardRef,
  Output,
  EventEmitter,
  HostBinding,
  ElementRef,
  Renderer2
} from '@angular/core';
import {
  MatDialogConfig,
  MatDialog,
  MatFormFieldControl
} from '@angular/material';
import { TreeFormDialogComponent } from 'app/shared/components/tree-form-dialog/tree-form-dialog.component';
import {
  FormGroup,
  FormBuilder,
  ControlValueAccessor,
  Validator,
  NG_VALUE_ACCESSOR,
  NG_VALIDATORS,
  AbstractControl,
  ValidationErrors
} from '@angular/forms';
import { TreeNode } from 'primeng/components/common/treenode';
import { Observable } from 'rxjs/Observable';
import { DictionaryType } from 'app/shared/services/models/dictionary-type.enum';
import { DictionaryService } from 'app/shared/services/dictionary.service';
import { Subject } from 'rxjs/Subject';
import { coerceBooleanProperty } from '@angular/cdk/coercion';
import { FocusMonitor } from '@angular/cdk/a11y';
import { Subscription } from 'rxjs/Subscription';
import { IntellectualPropertyDetails } from 'app/shared/models/intellectual-property-details';
import { BiblioField } from 'app/bibliographic-data/models/field-config';
import { isStageFormationAppData, isStageFullExpertise, isStageFormalExam } from '../description/description.component';

@Component({
  selector: 'app-icfem-fields',
  templateUrl: './icfem-fields.component.html',
  styleUrls: ['./icfem-fields.component.scss'],
  // Провайдеры для контрола
  providers: [
    {
      // Интерфейс, переопределяющий поведение компонента как контрол Angular Material
      provide: MatFormFieldControl,
      useExisting: IcfemFieldsComponent
    },
    {
      provide: BiblioField,
      useExisting: IcfemFieldsComponent
    },
    {
      // Дает возможность обращения к компоненту через formControlName
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => IcfemFieldsComponent),
      multi: true
    },
    {
      provide: NG_VALIDATORS,
      useExisting: forwardRef(() => IcfemFieldsComponent),
      multi: true
    }
  ]
})
export class IcfemFieldsComponent extends BiblioField
  implements
    OnInit,
    OnDestroy,
    MatFormFieldControl<number[]>,
    ControlValueAccessor,
    Validator {
  static nextId = 0;
  // Событие, поднимающееся при изменении значения
  @Output()
  changed = new EventEmitter<any>();
  @Input()
  isInvalidInput = false;
  // Свойство-label
  @Input()
  get placeholder() {
    return this._placeholder;
  }
  set placeholder(plh) {
    this._placeholder = plh;
    this.stateChanges.next();
  }
  // Является ли обязательным
  @Input()
  get required() {
    return this._required;
  }
  set required(value) {
    this._required = coerceBooleanProperty(value);
    this.stateChanges.next();
  }
  // Является ли отключенным
  @Input()
  get disabled() {
    return this._disabled;
  }
  set disabled(value) {
    this._disabled = coerceBooleanProperty(value);
    this.editMode = !value;
    this.stateChanges.next();
  }
  // Занчение контрола
  @Input()
  get value() {
    if (this.formGroup) {
      return this.formGroup.getRawValue();
    }
    return null;
  }
  set value(value: number[]) {
    this.writeValue(value);
    this.stateChanges.next();
  }
  // уникальный идентификатор контрола, селектор компонента + айдишник
  @HostBinding()
  id = `${this.controlType}-${IcfemFieldsComponent.nextId++}`;
  // Идентификаторы вложенных контролов
  @HostBinding('attr.aria-describedby')
  describedBy = '';
  // Определяет, как контрол отображается, т.е. выходит ли он на передний план при фокусе
  @HostBinding('class.floating')
  get shouldLabelFloat() {
    return this.focused || !this.empty;
  }
  // Пусто ли значение контрола
  get empty() {
    const n = this.formGroup.getRawValue();
    return (
      !n.transliteration ||
      !n.productPlace ||
      !n.productSpecialProp ||
      !n.selectionFamily
    );
  }
  // Здесь ведем учет изменениям состояния контрола
  stateChanges = new Subject<void>();
  ngControl = null;
  focused = false;
  shouldPlaceholderFloat?: boolean;
  errorState = false;
  // Тип контрола, должен совпадать с селектором компонента
  controlType = 'app-icfem-fields-form';

  labelsIcfem: any[] = [];
  icfemTree: TreeNode[] = [];

  @Input()
  editMode: boolean;
  @Input()
  owner: IntellectualPropertyDetails;

  formGroup: FormGroup;

  private onDestroy = new Subject();
  private _required = false;
  private _placeholder: string;
  private _disabled = false;
  private subs: Subscription[] = [];

  constructor(
    private fm: FocusMonitor,
    private elRef: ElementRef,
    private renderer: Renderer2,
    private dialog: MatDialog,
    private fb: FormBuilder,
    private dictionaryService: DictionaryService
  ) {
    super();
    this.buildForm();
    // подписываемся на событие фокуса
    this.subs.push(
      fm.monitor(elRef.nativeElement, true).subscribe(origin => {
        this.focused = !!origin;
        this.stateChanges.next();
      })
    );
    // подписываемся на событие изменения значения
    this.subs.push(
      this.formGroup.valueChanges.subscribe((value: number) => {
        this.propagateChange(value);
      })
    );
  }

  ngOnInit() {
    this.initSelectOptions();
  }

  ngOnDestroy(): void {
    this.formGroup.reset();
    this.stateChanges.complete();
    this.subs.forEach(s => s.unsubscribe());
    this.onDestroy.next();
  }

  isDisabledIcfem(): boolean {
    // return (
    //   !this.editMode ||
    //   !(
    //     isStageFormationAppData(this.owner) ||
    //     isStageFullExpertise(this.owner) ||
    //     isStageFormalExam(this.owner)
    //   )
    // );
    return false;
  }

  openDialogIcfemTree() {
    const config = new MatDialogConfig();
    config.disableClose = true;
    config.width = '80vw';
    config.height = '90vh';
    const icfemIds = this.formGroup.get('icfemIds').value;
    config.data = {
      tree: this.icfemTree,
      ids: icfemIds
    };

    const dialogRef = this.dialog.open(TreeFormDialogComponent, config);
    dialogRef.afterClosed().subscribe((result: any) => {
      if (result) {
        this.formGroup.get('icfemIds').setValue(result.map(r => r.id));
        this.setLabelsIcfem();
        this.formGroup.markAsDirty();
      }
    });
  }

  deleteIcfem(id: number) {
    const icfemIds: number[] = this.formGroup.get('icfemIds').value;
    this.formGroup.get('icfemIds').setValue(icfemIds.filter(i => i !== id));
    this.setLabelsIcfem();
    this.formGroup.markAsDirty();
  }

  setDescribedByIds(ids: string[]): void {
    this.describedBy = ids.join(' ');
  }

  onContainerClick(event: MouseEvent): void {
    if ((event.target as Element).tagName.toLowerCase() !== 'input') {
      this.elRef.nativeElement.querySelector('input').focus();
    }
  }

  writeValue(obj: number[]): void {
    if (obj) {
      if (this.formGroup) {
        this.formGroup.get('icfemIds').setValue(obj);
        this.initialLabelsByIds(obj, this.icfemTree, this.labelsIcfem);
      }
    } else {
      if (this.formGroup) {
        this.formGroup.reset();
        this.changed.emit(obj);
        this.propagateChange(obj);
      }
    }
  }

  registerOnChange(fn: any): void {
    this.propagateChange = fn;
  }

  registerOnTouched(fn: any): void {
    return;
  }

  setDisabledState?(isDisabled: boolean): void {
    this.disabled = isDisabled;
  }

  validate(c: AbstractControl): ValidationErrors {
    const invalid = {
      invalid: {
        valid: false
      }
    };
    if (!this.formGroup) {
      return invalid;
    }
    return this.formGroup.valid ? null : invalid;
  }

  registerOnValidatorChange?(fn: () => void): void {
    return;
  }

  getValue() {
    return this.value;
  }

  private setLabelsIcfem() {
    this.labelsIcfem = [];
    const icfemIds = this.formGroup.get('icfemIds').value;
    this.initialLabelsByIds(icfemIds, this.icfemTree, this.labelsIcfem);
  }

  private initialLabelsByIds(
    ids: number[],
    tree: TreeNode[],
    outLabels: any[]
  ) {
    for (const node of tree) {
      if (ids && ids.length > 0 && ids.includes(node.data)) {
        const selectOption = {
          id: node.data,
          nameRu: node.label
        };
        if (outLabels.findIndex(o => o.id === selectOption.id) === -1) {
          outLabels.push(selectOption);
        }
      }
      if (node.children && node.children.length) {
        this.initialLabelsByIds(ids, node.children, outLabels);
      }
    }
  }

  private buildForm() {
    this.formGroup = this.fb.group({
      icfemIds: [{ value: '' }]
    });
  }

  private initSelectOptions() {
    Observable.combineLatest(
      this.dictionaryService.getGetBaseTreeNode(DictionaryType.DicICFEM)
    )
      .takeUntil(this.onDestroy)
      .subscribe(([icfemTree]) => {
        this.icfemTree = icfemTree;
        this.setLabelsIcfem();
      });
  }

  private propagateChange = (_: any) => {};
}
