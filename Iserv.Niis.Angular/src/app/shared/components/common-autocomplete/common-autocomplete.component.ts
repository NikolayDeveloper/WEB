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
import { Subject, Subscription } from 'rxjs';
import { Observable } from 'rxjs/Observable';
import { SelectOption } from '../../services/models/select-option';

@Component({
  selector: 'app-common-autocomplete',
  templateUrl: './common-autocomplete.component.html',
  styleUrls: ['./common-autocomplete.component.scss'],
  // Провайдеры для контрола
  providers: [
    {
      // Интерфейс, переопределяющий поведение компонента как контрол Angular Material
      provide: MatFormFieldControl,
      useExisting: CommonAutocompleteComponent
    },
    {
      // Дает возможность обращения к компоненту через formControlName
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => CommonAutocompleteComponent),
      multi: true
    },
    {
      provide: NG_VALIDATORS,
      useExisting: forwardRef(() => CommonAutocompleteComponent),
      multi: true
    }
  ]
})
export class CommonAutocompleteComponent
  implements
    OnInit,
    OnDestroy,
    OnChanges,
    MatFormFieldControl<number>,
    ControlValueAccessor,
    Validator {
  static nextId = 0;
  // уникальный идентификатор контрола, селектор компонента + айдишник
  @HostBinding()
  id = `app-common-autocomplete-${CommonAutocompleteComponent.nextId++}`;
  // Идентификаторы вложенных контролов
  @HostBinding('attr.aria-describedby') describedBy = '';
  // Определяет, как контрол отображается, т.е. выходит ли он на передний план при фокусе
  @HostBinding('class.floating')
  get shouldLabelFloat() {
    return this.focused || !this.empty;
  }
  // Здесь ведем учет изменениям состояния контрола
  stateChanges = new Subject<void>();
  // Вызывается при уничтожении контрола
  private onDestroy = new Subject();

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

  // Пусто ли значение контрола
  get empty() {
    const n = this.formGroup.value;
    return !n.nameRu;
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
    return this.formGroup.get('id').value;
  }
  set value(number: number) {
    this.writeValue(number);
    this.stateChanges.next();
  }

  // Событие, поднимающееся при изменении значения
  @Output() changed = new EventEmitter<any>();

  ngControl = null;
  focused = false;
  shouldPlaceholderFloat?: boolean;
  errorState = false;

  // Тип контрола, должен совпадать с селектором компонента
  controlType = 'app-common-autocomplete';

  filteredDictionary: SelectOption[];
  formGroup: FormGroup;
  selectedId: number;

  private _required = false;
  private _placeholder: string;
  private _disabled = false;

  private subs: Subscription[] = [];

  @Input() dictionary: SelectOption[];

  constructor(
    private fb: FormBuilder,
    private fm: FocusMonitor,
    private elRef: ElementRef,
    private renderer: Renderer2
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

  private propagateChange = (_: any) => {};

  ngOnInit() {
    this.formGroup
      .get('nameRu')
      .valueChanges.takeUntil(this.onDestroy)
      .filter(value => !!value)
      .subscribe((value: string) => {
        this.filteredDictionary = value
          ? this.dictionary.filter(
              s =>
                s.nameRu &&
                s.nameRu.toLowerCase().includes(value.toLowerCase())
            )
          : this.dictionary;
        this.filteredDictionary.sort(function(a, b){
          if(a.nameRu.indexOf(value) < b.nameRu.indexOf(value))
            return -1;    
          if(a.nameRu.indexOf(value) > b.nameRu.indexOf(value))
            return 1;        
          return 0;
        });
      });
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes.dictionary && changes.dictionary.currentValue) {
      this.writeValue(this.selectedId);
      this.stateChanges.next();
    }
  }

  ngOnDestroy(): void {
    this.formGroup.reset();
    this.stateChanges.complete();
    this.subs.forEach(s => s.unsubscribe());
    this.onDestroy.next();
  }

  // вывод полченного значение на форму
  writeValue(newValue: number): void {
    this.selectedId = newValue;
    if (newValue && this.dictionary) {
      const record = this.dictionary.find(d => d.id === newValue);
      if (record) {
        this.formGroup.get('id').setValue(record.id);
        this.formGroup.get('nameRu').setValue(record.nameRu);
        this.formGroup.get('code').setValue(record.code);
        this.changed.emit(record);
        this.propagateChange(record);
      }
    }
    if (!newValue) {
      this.formGroup.get('id').setValue('');
      this.formGroup.get('nameRu').setValue('');
      this.formGroup.get('code').setValue('');
      this.selectedId = null;
    }
  }

  // регистрация события изменения состояния
  registerOnChange(fn: (_: any) => void): void {
    this.propagateChange = fn;
  }

  registerOnTouched(fn: (_: any) => void): void {
    return;
  }

  setDisabledState?(isDisabled: boolean): void {
    this.disabled = isDisabled;
  }

  setDescribedByIds(ids: string[]): void {
    this.describedBy = ids.join(' ');
  }

  // фокусировка на инпуте при нажатии на контрол мышкой
  onContainerClick(event: MouseEvent): void {
    if ((event.target as Element).tagName.toLowerCase() !== 'input') {
      this.elRef.nativeElement.querySelector('input').focus();
    }
  }

  onSelect(value: SelectOption) {
    this.formGroup.get('id').setValue(value.id);
    this.formGroup.get('nameRu').setValue(value.nameRu);
    this.formGroup.get('code').setValue(value.code);
    this.changed.emit(value);
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

  // todo обновление валидатора
  registerOnValidatorChange?(fn: () => void): void {
    return;
  }

  private buildForm() {
    this.formGroup = this.fb.group({
      nameRu: [''],
      id: [{ value: '' }, Validators.required],
      code: ['']
    });
  }
}
