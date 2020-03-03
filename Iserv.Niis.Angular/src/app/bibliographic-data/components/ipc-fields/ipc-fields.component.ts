import {
  Component,
  OnInit,
  Input,
  OnDestroy,
  forwardRef,
  ElementRef,
  Renderer2,
  Output,
  EventEmitter,
  HostBinding
} from '@angular/core';
import {
  MatDialogConfig,
  MatDialog,
  MatFormFieldControl
} from '@angular/material';
import { TreeFormDialogIpcComponent } from '../tree-form-dialog-ipc/tree-form-dialog-ipc.component';
import { DictionaryService } from 'app/shared/services/dictionary.service';
import {
  FormGroup,
  Validators,
  FormBuilder,
  ControlValueAccessor,
  Validator,
  NG_VALIDATORS,
  NG_VALUE_ACCESSOR,
  AbstractControl,
  ValidationErrors
} from '@angular/forms';
import { TreeNode } from 'primeng/components/common/treenode';
import { Observable } from 'rxjs/Observable';
import { Subject } from 'rxjs/Subject';
import { FocusMonitor } from '@angular/cdk/a11y';
import { Subscription } from 'rxjs/Subscription';
import { coerceBooleanProperty } from '@angular/cdk/coercion';
import { IntellectualPropertyDetails } from 'app/shared/models/intellectual-property-details';
import { BiblioField } from 'app/bibliographic-data/models/field-config';
import { isStagesForIpc } from '../description/description.component';

@Component({
  selector: 'app-ipc-fields',
  templateUrl: './ipc-fields.component.html',
  styleUrls: ['./ipc-fields.component.scss'],
  // Провайдеры для контрола
  providers: [
    {
      // Интерфейс, переопределяющий поведение компонента как контрол Angular Material
      provide: MatFormFieldControl,
      useExisting: IpcFieldsComponent
    },
    {
      provide: BiblioField,
      useExisting: IpcFieldsComponent
    },
    {
      // Дает возможность обращения к компоненту через formControlName
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => IpcFieldsComponent),
      multi: true
    },
    {
      provide: NG_VALIDATORS,
      useExisting: forwardRef(() => IpcFieldsComponent),
      multi: true
    }
  ]
})
export class IpcFieldsComponent extends BiblioField
  implements
    OnInit,
    OnDestroy,
    MatFormFieldControl<IpcFields>,
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
  set value(value: IpcFields) {
    this.writeValue(value);
    this.stateChanges.next();
  }
  // уникальный идентификатор контрола, селектор компонента + айдишник
  @HostBinding()
  id = `${this.controlType}-${IpcFieldsComponent.nextId++}`;
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
  controlType = 'app-ipc-fields-form';

  formGroup: FormGroup;
  labelsIpc: any[] = [];
  ipcTree: TreeNode[] = [];

  private onDestroy = new Subject();
  private _required = false;
  private _placeholder: string;
  private _disabled = false;
  private subs: Subscription[] = [];

  @Input()
  editMode: boolean;
  @Input()
  owner: IntellectualPropertyDetails;

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

  isDisabledIpc(): boolean {
    // return this.editMode === false || isStagesForIpc(this.owner) === false;
    return false;
  }

  openDialogIpcTree() {
    const config = new MatDialogConfig();
    config.disableClose = true;
    config.width = '80vw';
    config.height = '90vh';
    const ipcIds = this.formGroup.get('ipcIds').value;
    config.data = {
      tree: this.ipcTree,
      ids: ipcIds
    };

    const dialogRef = this.dialog.open(TreeFormDialogIpcComponent, config);
    dialogRef.afterClosed().subscribe((result: any) => {
      if (result) {
        this.formGroup.get('ipcIds').setValue(result);
        this.setLabelsIpc();
        this.formGroup.markAsDirty();
      }
    });
  }

  deleteIpc(id: number) {
    const ipcIds: number[] = this.formGroup.get('ipcIds').value;
    this.formGroup.get('ipcIds').setValue(ipcIds.filter(p => p !== id));
    this.setLabelsIpc();
    this.formGroup.markAsDirty();
  }

  clearIpc() {
    this.formGroup.get('ipcIds').setValue([]);
    this.setLabelsIpc();
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

  writeValue(obj: IpcFields): void {
    if (obj) {
      if (this.formGroup) {
        this.formGroup.get('ipcIds').setValue(obj.ipcIds);
        this.formGroup.get('mainIpcId').setValue(obj.mainIpcId);
        this.initialIpcLabelsByIds(obj.ipcIds);
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

  private setLabelsIpc() {
    this.labelsIpc = [];
    const ipcIds = this.formGroup.get('ipcIds').value;
    if (ipcIds && ipcIds.length) {
      this.initialIpcLabelsByIds(ipcIds);
    }
  }

  private initialIpcLabelsByIds(ids: number[]) {
    this.dictionaryService.getDicIpcs(ids).subscribe(nodes => {
      for (const node of nodes) {
        const selectOption = {
          id: node.data,
          nameRu: node.label
        };
        if (!this.labelsIpc.find(i => i.id === selectOption.id)) {
          this.labelsIpc.push(selectOption);
        }
      }
    });
  }

  private initSelectOptions() {
    Observable.combineLatest(this.dictionaryService.getDicIpcRoots())
      .takeUntil(this.onDestroy)
      .subscribe(([ipcTree]) => {
        this.ipcTree = ipcTree;
        this.setLabelsIpc();
      });
  }

  private buildForm() {
    this.formGroup = this.fb.group({
      ipcIds: [{ value: '' }],
      mainIpcId: [{ value: '' }]
    });
  }

  private propagateChange = (_: any) => {};
}

export class IpcFields {
  ipcIds: number[];
  mainIpcId: number;
}
