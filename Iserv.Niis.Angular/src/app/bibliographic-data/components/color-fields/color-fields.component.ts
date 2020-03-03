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
  Renderer2,
  ChangeDetectorRef
} from '@angular/core';
import {
  MatDialogConfig,
  MatDialog,
  MatFormFieldControl
} from '@angular/material';
import {
  TreeFormDialogComponent,
  IOrderedSelectedNode
} from 'app/shared/components/tree-form-dialog/tree-form-dialog.component';
import {
  FormGroup,
  FormBuilder,
  NG_VALIDATORS,
  NG_VALUE_ACCESSOR,
  ControlValueAccessor,
  Validator,
  AbstractControl,
  ValidationErrors
} from '@angular/forms';
import { TreeNode } from 'primeng/components/common/treenode';
import { SelectOption } from 'app/shared/services/models/select-option';
import { DictionaryType } from 'app/shared/services/models/dictionary-type.enum';
import { DictionaryService } from 'app/shared/services/dictionary.service';
import { Observable } from 'rxjs/Observable';
import { IntellectualPropertyDetails } from 'app/shared/models/intellectual-property-details';
import { Subject } from 'rxjs/Subject';
import { coerceBooleanProperty } from '@angular/cdk/coercion';
import { Subscription } from 'rxjs/Subscription';
import { FocusMonitor } from '@angular/cdk/a11y';
import { BiblioField } from 'app/bibliographic-data/models/field-config';
import { isStageFormationAppData, isStageFullExpertise, isStageFormalExam } from '../description/description.component';

@Component({
  selector: 'app-color-fields',
  templateUrl: './color-fields.component.html',
  styleUrls: ['./color-fields.component.scss'],

  // Провайдеры для контрола
  providers: [
    {
      // Интерфейс, переопределяющий поведение компонента как контрол Angular Material
      provide: MatFormFieldControl,
      useExisting: ColorFieldsComponent
    },
    {
      provide: BiblioField,
      useExisting: ColorFieldsComponent
    },
    {
      // Дает возможность обращения к компоненту через formControlName
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => ColorFieldsComponent),
      multi: true
    },
    {
      provide: NG_VALIDATORS,
      useExisting: forwardRef(() => ColorFieldsComponent),
      multi: true
    }
  ]
})
export class ColorFieldsComponent extends BiblioField
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
  id = `${this.controlType}-${ColorFieldsComponent.nextId++}`;
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
  controlType = 'app-color-fields-form';

  formGroup: FormGroup;
  colorTree: TreeNode[] = [];
  labelsColor: any[] = [];
  dicColors: SelectOption[];

  @Input()
  editMode: boolean;
  @Input()
  owner: IntellectualPropertyDetails;

  private onDestroy = new Subject();
  private _required = false;
  private _placeholder: string;
  private _disabled = false;
  private subs: Subscription[] = [];
  private orderedIds: IOrderedSelectedNode[] = [];
  private selectedNodes: TreeNode[] = [];

  constructor(
    private fm: FocusMonitor,
    private elRef: ElementRef,
    private renderer: Renderer2,
    private dialog: MatDialog,
    private fb: FormBuilder,
    private dictionaryService: DictionaryService,
    private changeDetector: ChangeDetectorRef
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
  isDisabledColor(): boolean {
    // return !(
    //   this.editMode &&
    //   (
    //     isStageFormationAppData(this.owner) ||
    //     isStageFullExpertise(this.owner) ||
    //     isStageFormalExam(this.owner)
    //   )
    // );
    return false;
  }

  openDialogColorTree() {
    const config = new MatDialogConfig();
    config.disableClose = true;
    config.width = '80vw';
    config.height = '90vh';
    const colorTzIds = this.formGroup.get('colorTzIds').value;
    config.data = {
      tree: this.colorTree,
      ids: colorTzIds
    };

    const dialogRef = this.dialog.open(TreeFormDialogComponent, config);
    dialogRef.afterClosed().subscribe((result: any) => {
      if (result) {
        this.formGroup.get('colorTzIds').setValue(result.map(r => r.id));
        this.orderedIds = result;
        this.setLabelsColor();
        this.formGroup.markAsDirty();
      }
    });
  }

  deleteColor(id: number) {
    const colorTzIds: number[] = this.formGroup.get('colorTzIds').value;
    this.formGroup.get('colorTzIds').setValue(colorTzIds.filter(c => c !== id));
    this.setLabelsColor();
    this.formGroup.markAsDirty();
    this.changeDetector.detectChanges();
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
        let i = 0;
        obj.forEach(o => {
          const orderedSelection: IOrderedSelectedNode = {
            id: o,
            index: ++i
          };
          if (this.orderedIds.findIndex(oi => oi.id === o) === -1) {
            this.orderedIds.push(orderedSelection);
          }
        });
        this.formGroup.get('colorTzIds').setValue(obj);
        this.initializeSelectedNodes(obj, this.colorTree, this.labelsColor);
        this.initializeLabels(obj, this.labelsColor);
      }
    } else {
      if (this.formGroup) {
        this.formGroup.reset();
        this.changed.emit(obj);
        this.propagateChange(obj);
      }
    }

    this.setLabelsColor();
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

  getLabelsColor() {
    return this.labelsColor.map(l => l.nameRu).join('; ');
  }

  private setLabelsColor() {
    this.labelsColor = [];
    this.selectedNodes = [];
    const colorTzIds = this.formGroup.get('colorTzIds').value;
    this.initializeSelectedNodes(colorTzIds, this.colorTree, this.labelsColor);
    this.initializeLabels(colorTzIds, this.labelsColor);
  }

  private initializeSelectedNodes(
    ids: number[],
    tree: TreeNode[],
    outLabels: any[]
  ) {
    if (this.orderedIds.length > 0) {
      ids = this.orderedIds.map(o => o.id);
    }
    for (const node of tree) {
      if (ids && ids.length > 0 && ids.includes(node.data) && !node.children.length) {
        if (!this.selectedNodes.includes(node)) {
          this.selectedNodes.push(node);
        }
      }
      if (node.children && node.children.length) {
        this.initializeSelectedNodes(ids, node.children, outLabels);
      }
    }
  }

  private initializeLabels(ids: number[], outLabels: any[]) {
    ids.forEach(i => {
      const node = this.selectedNodes.find(n => n.data === i);
      if (node) {
        const selectOption = {
          id: node.data,
          nameRu: node.label
        };
        if (outLabels.findIndex(o => o.id === selectOption.id) === -1) {
          outLabels.push(selectOption);
        }
      }
    });
  }

  private initSelectOptions() {
    Observable.combineLatest(
      this.dictionaryService.getSelectOptions(DictionaryType.DicColorTZ),
      this.dictionaryService.getDicICFEMColors()
    )
      .takeUntil(this.onDestroy)
      .subscribe(([colors, colorTree]) => {
        this.dicColors = colors.sort((c1, c2) => {
          if (c1.nameRu < c2.nameRu) {
            return -1;
          }
          if (c1.nameRu > c2.nameRu) {
            return 1;
          }
          return 0;
        });
        this.colorTree = colorTree;
        this.setLabelsColor();
      });
  }

  private buildForm() {
    this.formGroup = this.fb.group({
      colorTzIds: [{ value: '' }]
    });
  }

  private propagateChange = (_: any) => {};
}
