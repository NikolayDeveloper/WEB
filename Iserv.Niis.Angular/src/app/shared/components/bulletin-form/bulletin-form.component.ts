import { FocusMonitor } from '@angular/cdk/a11y';
import { coerceBooleanProperty } from '@angular/cdk/coercion';
import {
  Component,
  ElementRef,
  EventEmitter,
  forwardRef,
  HostBinding,
  Input,
  OnChanges,
  OnDestroy,
  OnInit,
  Output,
  Renderer2,
  SimpleChanges
} from '@angular/core';
import {
  AbstractControl,
  ControlValueAccessor,
  FormBuilder,
  FormGroup,
  NG_VALIDATORS,
  NG_VALUE_ACCESSOR,
  Validator,
  Validators
} from '@angular/forms';
import { MatFormFieldControl } from '@angular/material';
import { Subject } from 'rxjs/Subject';
import { Subscription } from 'rxjs/Subscription';
import { BulletinDto } from '../../models/bulletin-dto';
import { BulletinService } from '../../services/bulletin.service';
import { Observable } from 'rxjs/Observable';
import { Moment } from 'moment';

@Component({
  selector: 'app-bulletin-form',
  templateUrl: './bulletin-form.component.html',
  styleUrls: ['./bulletin-form.component.scss'],
  // Провайдеры для контрола
  providers: [
    {
      // Интерфейс, переопределяющий поведение компонента как контрол Angular Material
      provide: MatFormFieldControl,
      useExisting: BulletinFormComponent
    },
    {
      // Дает возможность обращения к компоненту через formControlName
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => BulletinFormComponent),
      multi: true
    },
    {
      provide: NG_VALIDATORS,
      useExisting: forwardRef(() => BulletinFormComponent),
      multi: true
    }
  ]
})
export class BulletinFormComponent
  implements
    OnInit,
    OnDestroy,
    OnChanges,
    MatFormFieldControl<number>,
    ControlValueAccessor,
    Validator {
  static nextId = 0;
  // Событие, поднимающееся при изменении значения
  @Output() changed = new EventEmitter<any>();
  @Input() isInvalidInput = false;
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
    value ? this.formGroup.disable() : this.formGroup.enable();
    this.stateChanges.next();
  }
  // Занчение контрола
  @Input()
  get value() {
    return this.formGroup.get('bulletinId').value;
  }
  set value(number: number) {
    this.writeValue(number);
    this.stateChanges.next();
  }
  // уникальный идентификатор контрола, селектор компонента + айдишник
  @HostBinding() id = `${this.controlType}-${BulletinFormComponent.nextId++}`;
  // Идентификаторы вложенных контролов
  @HostBinding('attr.aria-describedby') describedBy = '';
  // Определяет, как контрол отображается, т.е. выходит ли он на передний план при фокусе
  @HostBinding('class.floating')
  get shouldLabelFloat() {
    return this.focused || !this.empty;
  }
  // Пусто ли значение контрола
  get empty() {
    const n = this.formGroup.value;
    return !n.bulletinId || !n.selectedBulletinDate;
  }
  // Здесь ведем учет изменениям состояния контрола
  stateChanges = new Subject<void>();
  ngControl = null;
  focused = false;
  shouldPlaceholderFloat?: boolean;
  errorState = false;
  // Тип контрола, должен совпадать с селектором компонента
  controlType = 'app-bulletin-form';
  formGroup: FormGroup;
  bulletins: BulletinDto[];
  selectedBulletin: BulletinDto;
  formMode = FormMode.Standard;
  // Вызывается при уничтожении контрола
  private onDestroy = new Subject();
  private subs: Subscription[] = [];
  private _required = false;
  private _placeholder: string;
  private _disabled = false;

  writeValue(value: number): void {
    if (value && this.bulletins) {
      const record = this.bulletins.find(d => d.id === value);
      if (record) {
        this.selectedBulletin = record;
        if (this.formMode === FormMode.Standard) {
          this.formGroup.get('selectedBulletinId').setValue(value);
        } else {
          this.formGroup
            .get('selectedBulletinDate')
            .setValue(record.publishDate);
        }
        this.formGroup.get('bulletinId').setValue(value);
        this.changed.emit(record);
        this.propagateChange(record);
      }
    }
    if (!value) {
      this.formGroup.reset();
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

  registerOnValidatorChange?(fn: () => void): void {
    return;
  }

  validate(c: AbstractControl): { [key: string]: any } {
    const invalid = {
      invalid: {
        valid: false
      }
    };
    if (!this.formGroup) {
      return invalid;
    }
    return this.formGroup.valid ? null : null;
  }

  setDescribedByIds(ids: string[]): void {
    this.describedBy = ids.join(' ');
  }

  onContainerClick(event: MouseEvent): void {
    if ((event.target as Element).tagName.toLowerCase() !== 'input') {
      this.elRef.nativeElement.querySelector('input').focus();
    }
  }

  constructor(
    private fb: FormBuilder,
    private fm: FocusMonitor,
    private elRef: ElementRef,
    private renderer: Renderer2,
    private bulletinService: BulletinService
  ) {
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
    this.initBulletins();
  }

  ngOnDestroy(): void {
    this.formGroup.reset();
    this.stateChanges.complete();
    this.subs.forEach(s => s.unsubscribe());
    this.onDestroy.next();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes && changes.currentValue) {
      this.stateChanges.next();
    }
  }

  onOkClick() {
    switch (this.formMode) {
      case FormMode.Add:
        const publishDate = this.formGroup.get('selectedBulletinDate').value;
        const newBulletin = new BulletinDto();
        newBulletin.publishDate = publishDate;
        this.bulletinService
          .add(newBulletin)
          .takeUntil(this.onDestroy)
          .subscribe(() => {
            this.formMode = FormMode.Standard;
            this.initBulletins();
          });
        break;
      case FormMode.Edit:
        const updatedBulletin = this.selectedBulletin;
        updatedBulletin.publishDate = this.formGroup.get(
          'selectedBulletinDate'
        ).value;
        this.bulletinService
          .update(updatedBulletin)
          .takeUntil(this.onDestroy)
          .subscribe(() => {
            this.formMode = FormMode.Standard;
            this.initBulletins();
          });
        break;
      default:
        throw Error('Other form modes are not implemented');
    }
  }

  onCancelClick() {
    this.formMode = FormMode.Standard;
    if (this.selectedBulletin) {
      this.writeValue(this.selectedBulletin.id);
    }
  }

  onAddClick() {
    this.formMode = FormMode.Add;
    this.formGroup.get('selectedBulletinDate').reset();
  }

  onEditClick() {
    this.formMode = FormMode.Edit;
    if (this.selectedBulletin) {
      this.writeValue(this.selectedBulletin.id);
    }
  }

  onBulletinChange(id: number) {
    const selectedBulletin = this.bulletins.find(b => b.id === id);
    this.selectedBulletin = selectedBulletin;
  }

  private propagateChange = (_: any) => {};

  private buildForm() {
    this.formGroup = this.fb.group({
      selectedBulletinDate: [{ value: '' }, Validators.required],
      selectedBulletinId: [{ value: '' }, Validators.required],
      bulletinId: [{ value: '' }, Validators.required]
    });
  }

  private getTime(date?: any) {
    date = new Date(date);
    return date !== null ? date.getTime() : 0;
  }

  private initBulletins() {
    Observable.combineLatest(
      this.bulletinService.getEarliest(),
      this.bulletinService.get()
    )
      .takeUntil(this.onDestroy)
      .subscribe(([earliest, all]) => {
        this.bulletins = all;
        if (this.bulletins.findIndex(b => b.id === earliest.id) === -1) {
          this.bulletins.push(earliest);
        }
        this.selectedBulletin = earliest;
        this.writeValue(this.selectedBulletin.id);
      });
  }
}

export enum FormMode {
  Standard = 0,
  Edit = 1,
  Add = 2
}
