import {
  Component,
  Input,
  OnChanges,
  OnDestroy,
  OnInit,
  SimpleChanges,
  forwardRef,
  EventEmitter,
  Output,
  HostBinding,
  ElementRef,
  Renderer2
} from '@angular/core';
import {
  ControlValueAccessor,
  FormBuilder,
  FormGroup,
  Validator,
  NG_VALUE_ACCESSOR,
  NG_VALIDATORS,
  AbstractControl,
  ValidationErrors
} from '@angular/forms';
import { MatFormFieldControl } from '@angular/material';
import { ConfigService } from 'app/core';
import { AddressResponse, PostKzService } from 'app/modules/postkz';
import {
  concatAddresseeAddress,
  getPart,
  SubjectConstants
} from 'app/subjects/models/subject.model';
import { Subject } from 'rxjs/Subject';
import { Subscription } from 'rxjs/Subscription';
import { BiblioField } from 'app/bibliographic-data/models/field-config';
import { coerceBooleanProperty } from '@angular/cdk/coercion';
import { FocusMonitor } from '@angular/cdk/a11y';

@Component({
  selector: 'app-address-search',
  templateUrl: './address-search.component.html',
  styleUrls: ['./address-search.component.scss'],
  // Провайдеры для контрола
  providers: [
    {
      // Интерфейс, переопределяющий поведение компонента как контрол Angular Material
      provide: MatFormFieldControl,
      useExisting: AddressSearchComponent
    },
    {
      provide: BiblioField,
      useExisting: AddressSearchComponent
    },
    {
      // Дает возможность обращения к компоненту через formControlName
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => AddressSearchComponent),
      multi: true
    },
    {
      provide: NG_VALIDATORS,
      useExisting: forwardRef(() => AddressSearchComponent),
      multi: true
    }
  ]
})
export class AddressSearchComponent extends BiblioField
  implements
    OnInit,
    OnChanges,
    OnDestroy,
    MatFormFieldControl<string>,
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
    this.toggleEditMode(!value);
    this.stateChanges.next();
  }
  // Занчение контрола
  @Input()
  get value() {
    if (this.formGroup) {
      return this.formGroup.get('address').value;
    }
    return null;
  }
  set value(value: string) {
    this.writeValue(value);
    this.stateChanges.next();
  }
  // уникальный идентификатор контрола, селектор компонента + айдишник
  @HostBinding()
  id = `${this.controlType}-${AddressSearchComponent.nextId++}`;
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
    return !n.bulletinId || !n.selectedBulletinDate;
  }
  // Здесь ведем учет изменениям состояния контрола
  stateChanges = new Subject<void>();
  ngControl = null;
  focused = false;
  shouldPlaceholderFloat?: boolean;
  errorState = false;
  // Тип контрола, должен совпадать с селектором компонента
  controlType = 'app-address-search';

  formGroup: FormGroup;
  filteredPostAddresses: any[];
  editableControls: string[];

  @Input()
  editMode: boolean;

  private onDestroy = new Subject();
  private _required = false;
  private _placeholder: string;
  private _disabled = false;
  private subs: Subscription[] = [];

  constructor(
    private configService: ConfigService,
    private fb: FormBuilder,
    private postKzService: PostKzService,
    private fm: FocusMonitor,
    private elRef: ElementRef,
    private renderer: Renderer2
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
    this.subscribeAddressInput();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes.editMode && changes.editMode.currentValue) {
      this.toggleEditMode(this.editMode);
    }
  }

  ngOnDestroy(): void {
    this.formGroup.reset();
    this.stateChanges.complete();
    this.subs.forEach(s => s.unsubscribe());
    this.onDestroy.next();
  }

  getAddresseeAddress(address: any) {
    const apartment = this.formGroup.get('apartment').value;
    return concatAddresseeAddress(address, apartment);
  }

  onAddressSelect(address: any) {
    if (address === this.postKzService.unreachableText) {
      return;
    }

    const apartment = this.formGroup.get('apartment').value;
    this.formGroup
      .get('address')
      .setValue(concatAddresseeAddress(address, apartment));
    this.formGroup
      .get('republic')
      .setValue(getPart(address, SubjectConstants.republicPartIds));
    this.formGroup
      .get('oblast')
      .setValue(getPart(address, SubjectConstants.oblastPartIds));
    this.formGroup
      .get('city')
      .setValue(getPart(address, SubjectConstants.cityPartIds));
    this.formGroup
      .get('street')
      .setValue(getPart(address, SubjectConstants.streetPartIds));
    this.filteredPostAddresses = [];
    this.formGroup.markAsDirty();
  }

  getFullAdress(selectedAddress: any): string {
    return `${selectedAddress.addressRus}, индекс: ${selectedAddress.postcode}`;
  }

  setDescribedByIds(ids: string[]): void {
    this.describedBy = ids.join(' ');
  }

  onContainerClick(event: MouseEvent): void {
    if ((event.target as Element).tagName.toLowerCase() !== 'input') {
      this.elRef.nativeElement.querySelector('input').focus();
    }
  }

  writeValue(obj: string): void {
    if (obj) {
      if (this.formGroup) {
        this.formGroup.get('address').setValue(obj);
        this.changed.emit(obj);
        this.propagateChange(obj);
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

  public getValue() {
    return this.value;
  }

  /**
   * Подписывается на события изменения значения в поле addresseeAddress и запрашивает данные с казпочты
   *
   * @private
   * @memberof RequestComponent
   */
  private subscribeAddressInput() {
    let postKzSubs: Subscription;
    const address = this.formGroup.get('address');
    address.valueChanges
      .takeUntil(this.onDestroy)
      .debounceTime(this.configService.debounceTime)
      .filter(
        value => value && value.length > 4 && address.dirty && address.valid
      )
      .distinctUntilChanged()
      .subscribe((value: string) => {
        if (postKzSubs && !postKzSubs.closed) {
          postKzSubs.unsubscribe();
        }
        postKzSubs = this.postKzService
          .get(value)
          .takeUntil(this.onDestroy)
          .subscribe((addresses: AddressResponse) => {
            this.filteredPostAddresses = addresses ? addresses.data : [];
          });
      });
  }

  private buildForm() {
    this.formGroup = this.fb.group({
      address: [{ value: '', disabled: true }],
      apartment: [{ value: '', disabled: true }],
      republic: [{ value: '', disabled: true }],
      oblast: [{ value: '', disabled: true }],
      city: [{ value: '', disabled: true }],
      street: [{ value: '', disabled: true }]
    });

    this.editableControls = ['address', 'apartment'];
  }

  private toggleEditMode(value: boolean) {
    this.filteredPostAddresses = [];

    this.editableControls.forEach(c => {
      value ? this.formGroup.get(c).enable() : this.formGroup.get(c).disable();
    });
  }

  private propagateChange = (_: any) => {};
}
