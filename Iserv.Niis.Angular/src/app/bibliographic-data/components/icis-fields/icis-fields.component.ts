import {
  Component,
  OnInit,
  Input,
  ElementRef,
  Renderer2,
  OnDestroy,
  forwardRef,
  Output,
  EventEmitter,
  HostBinding
} from '@angular/core';
import { IntellectualPropertyDetails } from 'app/shared/models/intellectual-property-details';
import {
  MatDialogConfig,
  MatDialog,
  MatFormFieldControl
} from '@angular/material';
import { TreeFormDialogComponent } from 'app/shared/components/tree-form-dialog/tree-form-dialog.component';
import { TreeNode } from 'primeng/components/common/treenode';
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
import { FocusMonitor } from '@angular/cdk/a11y';
import { DictionaryService } from 'app/shared/services/dictionary.service';
import { Observable } from 'rxjs/Observable';
import { Subject } from 'rxjs/Subject';
import { Subscription } from 'rxjs/Subscription';
import { coerceBooleanProperty } from '@angular/cdk/coercion';
import { BiblioField } from 'app/bibliographic-data/models/field-config';
import { isStageFullExpertise, isStagesForIpc } from '../description/description.component';

@Component({
  selector: 'app-icis-fields',
  templateUrl: './icis-fields.component.html',
  styleUrls: ['./icis-fields.component.scss'],
  // Провайдеры для контрола
  providers: [
    {
      // Интерфейс, переопределяющий поведение компонента как контрол Angular Material
      provide: MatFormFieldControl,
      useExisting: IcisFieldsComponent
    },
    {
      provide: BiblioField,
      useExisting: IcisFieldsComponent
    },
    {
      // Дает возможность обращения к компоненту через formControlName
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => IcisFieldsComponent),
      multi: true
    },
    {
      provide: NG_VALIDATORS,
      useExisting: forwardRef(() => IcisFieldsComponent),
      multi: true
    }
  ]
})
export class IcisFieldsComponent extends BiblioField
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
  id = `${this.controlType}-${IcisFieldsComponent.nextId++}`;
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
  controlType = 'app-icis-fields-form';

  labelsIcis: any[] = [];
  icisTree: TreeNode[] = [];

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

  isDisabled(): boolean {
    // return (
    //   !this.editMode ||
    //   ((isStageFullExpertise(this.owner) || isStagesForIpc(this.owner)) &&
    //     !this.editMode)
    // );
    return false;
  }

  openDialogIcisTree() {
    const config = new MatDialogConfig();
    config.disableClose = true;
    config.width = '80vw';
    config.height = '90vh';
    const icisRequestIds = this.formGroup.get('icisRequestIds').value;
    config.data = {
      tree: this.icisTree,
      ids: icisRequestIds
    };

    const dialogRef = this.dialog.open(TreeFormDialogComponent, config);
    dialogRef.afterClosed().subscribe((result: any) => {
      if (result) {
        this.formGroup.get('icisRequestIds').setValue(result.map(r => r.id));
        this.setLablesIcis();
        this.formGroup.markAsDirty();
      }
    });
  }

  deleteIcis(id: number) {
    const icisRequestIds: number[] = this.formGroup.controls.icisRequestIds
      .value;
    this.formGroup.controls.icisRequestIds.setValue(
      icisRequestIds.filter(c => c !== id)
    );
    this.setLablesIcis();
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
        this.formGroup.get('icisRequestIds').setValue(obj);
        // Дублирует строки !!!!
        // this.initialLabelsByIds(obj, this.icisTree, this.labelsIcis);
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
        valid: false,
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

  private setLablesIcis() {
    this.labelsIcis = [];
    const icisRequestIds = this.formGroup.get('icisRequestIds').value;
    this.initialLabelsByIds(icisRequestIds, this.icisTree);
  }

  private initialLabelsByIds(
    ids: number[],
    tree: TreeNode[]
  ) {
    for (const node of tree) {
      if (ids && ids.length > 0 && ids.includes(node.data)) {
        const selectOption = {
          id: node.data,
          nameRu: node.label
        };
        this.labelsIcis = [...this.labelsIcis, selectOption];
      }
      if (node.children && node.children.length) {
        this.initialLabelsByIds(ids, node.children);
      }
    }
  }

  private buildForm() {
    this.formGroup = this.fb.group({
      icisRequestIds: [{ value: '' }]
    });
  }

  private initSelectOptions() {
    Observable.combineLatest(this.dictionaryService.getDicICISs())
      .takeUntil(this.onDestroy)
      .subscribe(([icisTree]) => {
        this.icisTree = icisTree;
        this.icisTree = this.icisTree.filter(t => !!t.label);
        this.prepareIcisTree(this.icisTree);
        this.setLablesIcis();
      });
  }

  private prepareIcisTree(tree: TreeNode[]) {
    tree.forEach(t => {
      if (t.children && t.children.length > 0) {
        t.children = t.children.filter(c => !!c.label);
        this.prepareIcisTree(t.children);
      }
    });
  }

  private propagateChange = (_: any) => {};
}
