import {
  Component,
  OnInit,
  OnDestroy,
  Output,
  Input,
  HostBinding,
  EventEmitter,
  ElementRef,
  Renderer2,
  forwardRef
} from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  Validators,
  ControlValueAccessor,
  Validator,
  AbstractControl,
  ValidationErrors,
  NG_VALIDATORS,
  NG_VALUE_ACCESSOR
} from '@angular/forms';
import { Subject } from 'rxjs/Subject';
import { MatFormFieldControl } from '@angular/material';
import { coerceBooleanProperty } from '@angular/cdk/coercion';
import { RouteStageCodes } from 'app/shared/models/route-stage-codes';
import { Subscription } from 'rxjs/Subscription';
import { FieldConfig, BiblioField } from 'app/bibliographic-data/models/field-config';
import { FocusMonitor } from '@angular/cdk/a11y';
import { IntellectualPropertyDetails } from 'app/shared/models/intellectual-property-details';

@Component({
  selector: 'app-product-fields',
  templateUrl: './product-fields.component.html',
  styleUrls: ['./product-fields.component.scss'],

  // Провайдеры для контрола
  providers: [
    {
      // Интерфейс, переопределяющий поведение компонента как контрол Angular Material
      provide: MatFormFieldControl,
      useExisting: ProductFieldsComponent
    },
    {
      provide: BiblioField,
      useExisting: ProductFieldsComponent
    },
    {
      // Дает возможность обращения к компоненту через formControlName
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => ProductFieldsComponent),
      multi: true
    },
    {
      provide: NG_VALIDATORS,
      useExisting: forwardRef(() => ProductFieldsComponent),
      multi: true
    }
  ]
})
export class ProductFieldsComponent extends BiblioField
  implements
    OnInit,
    OnDestroy,
    MatFormFieldControl<ProductFields>,
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
      return this.formGroup.getRawValue();
    }
    return null;
  }
  set value(value: ProductFields) {
    this.writeValue(value);
    this.stateChanges.next();
  }
  // уникальный идентификатор контрола, селектор компонента + айдишник
  @HostBinding()
  id = `${this.controlType}-${ProductFieldsComponent.nextId++}`;
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
  controlType = 'app-product-fields-form';

  formGroup: FormGroup;
  constructor(
    private fm: FocusMonitor,
    private elRef: ElementRef,
    private renderer: Renderer2,
    private fb: FormBuilder
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

  private onDestroy = new Subject();
  private _required = false;
  private _placeholder: string;
  private _disabled = false;
  private subs: Subscription[] = [];
  private fieldsConfig: FieldConfig[] = [
    {
      stageCodes: [
        ...RouteStageCodes.stagesFormationAppData,
        ...RouteStageCodes.stagesformalExam,
        ...RouteStageCodes.stagesFullExpertise
      ],
      fieldName: 'transliteration'
    },
    {
      stageCodes: [
        ...RouteStageCodes.stagesFormationAppData,
        ...RouteStageCodes.stagesformalExam,
        ...RouteStageCodes.stagesFullExpertise
      ],
      fieldName: 'productPlace'
    },
    {
      stageCodes: [
        ...RouteStageCodes.stagesFormationAppData,
        ...RouteStageCodes.stagesformalExam,
        ...RouteStageCodes.stagesFullExpertise
      ],
      fieldName: 'productSpecialProp'
    },
    {
      stageCodes: [
        ...RouteStageCodes.stagesFormationAppData,
        ...RouteStageCodes.stagesformalExam,
        ...RouteStageCodes.stagesFullExpertise
      ],
      fieldName: 'selectionFamily'
    }
  ];

  @Input()
  owner: IntellectualPropertyDetails;

  ngOnInit() {}

  ngOnDestroy(): void {
    this.formGroup.reset();
    this.stateChanges.complete();
    this.subs.forEach(s => s.unsubscribe());
    this.onDestroy.next();
  }

  setDescribedByIds(ids: string[]): void {
    this.describedBy = ids.join(' ');
  }

  onContainerClick(event: MouseEvent): void {
    if ((event.target as Element).tagName.toLowerCase() !== 'input') {
      this.elRef.nativeElement.querySelector('input').focus();
    }
  }

  writeValue(obj: ProductFields): void {
    if (obj) {
      if (this.formGroup) {
        this.formGroup.get('productPlace').setValue(obj.productPlace);
        this.formGroup
          .get('productSpecialProp')
          .setValue(obj.productSpecialProp);
        this.formGroup.get('selectionFamily').setValue(obj.selectionFamily);
        this.formGroup.get('transliteration').setValue(obj.transliteration);
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

  private buildForm() {
    this.formGroup = this.fb.group({
      // поля ТЗ
      transliteration: [{ value: '', disabled: true }],
      // поля НМПТ
      selectionFamily: [{ value: '', disabled: true }, Validators.required],
      productSpecialProp: [{ value: '', disabled: true }, Validators.required],
      productPlace: [{ value: '', disabled: true }, Validators.required]
    });
  }

  private toggleEditMode(value: boolean) {
    this.fieldsConfig.forEach(f => {
      if (this.owner) {
        // if (
        //   f.stageCodes.includes(this.owner.currentWorkflow.currentStageCode)
        // ) {
          value
            ? this.formGroup.get(f.fieldName).enable()
            : this.formGroup.get(f.fieldName).disable();
        // }
      }
    });
  }

  private propagateChange = (_: any) => {};
}

export class ProductFields {
  transliteration: string;
  selectionFamily: string;
  productSpecialProp: string;
  productPlace: string;
}
