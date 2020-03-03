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
import {
  ReferatFieldConfig,
  ReferatFieldtype,
  FieldConfig,
  BiblioField
} from 'app/bibliographic-data/models/field-config';
import { RouteStageCodes } from 'app/shared/models/route-stage-codes';
import { IntellectualPropertyDetails } from 'app/shared/models/intellectual-property-details';
import {
  FormGroup,
  FormBuilder,
  NG_VALUE_ACCESSOR,
  NG_VALIDATORS,
  ControlValueAccessor,
  Validator,
  AbstractControl,
  ValidationErrors
} from '@angular/forms';
import { FocusMonitor } from '@angular/cdk/a11y';
import { MatFormFieldControl } from '@angular/material';
import { coerceBooleanProperty } from '@angular/cdk/coercion';
import { Subject } from 'rxjs/Subject';
import { Subscription } from 'rxjs/Subscription';

@Component({
  selector: 'app-referat-fields',
  templateUrl: './referat-fields.component.html',
  styleUrls: ['./referat-fields.component.scss'],
  // Провайдеры для контрола
  providers: [
    {
      // Интерфейс, переопределяющий поведение компонента как контрол Angular Material
      provide: MatFormFieldControl,
      useExisting: ReferatFieldsComponent
    },
    {
      provide: BiblioField,
      useExisting: ReferatFieldsComponent
    },
    {
      // Дает возможность обращения к компоненту через formControlName
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => ReferatFieldsComponent),
      multi: true
    },
    {
      provide: NG_VALIDATORS,
      useExisting: forwardRef(() => ReferatFieldsComponent),
      multi: true
    }
  ]
})
export class ReferatFieldsComponent extends BiblioField
  implements
    OnInit,
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
      return this.formGroup.getRawValue();
    }
    return null;
  }
  set value(value: string) {
    this.writeValue(value);
    this.stateChanges.next();
  }
  // уникальный идентификатор контрола, селектор компонента + айдишник
  @HostBinding()
  id = `${this.controlType}-${ReferatFieldsComponent.nextId++}`;
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
    return !n.genus || !n.breedCountryId || !n.breedingNumber;
  }
  // Здесь ведем учет изменениям состояния контрола
  stateChanges = new Subject<void>();
  ngControl = null;
  focused = false;
  shouldPlaceholderFloat?: boolean;
  errorState = false;
  // Тип контрола, должен совпадать с селектором компонента
  controlType = 'app-referat-fields-form';

  @Input()
  owner: IntellectualPropertyDetails;

  get commonReferatType() {
    return ReferatFieldtype.Common;
  }
  get industrialFieldType() {
    return ReferatFieldtype.IndustrialDesign;
  }

  formGroup: FormGroup;

  private onDestroy = new Subject();
  private _required = false;
  private _placeholder: string;
  private _disabled = false;
  private subs: Subscription[] = [];

  private fieldsAcivityConfig: ReferatFieldConfig[] = [
    {
      protectionDocTypeCodes: [
        ...RouteStageCodes.pdTypeInventionsCodes,
        ...RouteStageCodes.pdTypeUsefulModelsCodes
      ],
      type: ReferatFieldtype.Common
    }
  ];
  private fieldsEditConfig: FieldConfig[] = [
    {
      stageCodes: [
        ...RouteStageCodes.stagesFormationAppData,
        ...RouteStageCodes.stagesformalExam,
        ...RouteStageCodes.stagesFullExpertise,
        ...RouteStageCodes.stagesPatentExam
      ],
      fieldName: 'referat'
    }
  ];

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

  ngOnInit() {}

  ngOnDestroy(): void {
    this.formGroup.reset();
    this.stateChanges.complete();
    this.subs.forEach(s => s.unsubscribe());
    this.onDestroy.next();
  }

  isFieldActive(type: ReferatFieldtype): boolean {
    const config = this.fieldsAcivityConfig.find(fc => fc.type === type);
    if (config && this.owner) {
      return config.protectionDocTypeCodes.includes(
        this.owner.protectionDocTypeCode
      );
    }
    return false;
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
        this.formGroup.get('referat').setValue(obj);
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

  private propagateChange = (_: any) => {};

  private buildForm() {
    this.formGroup = this.fb.group({
      referat: [{ value: '', disabled: true }]
    });
  }

  private toggleEditMode(value: boolean) {
    this.fieldsEditConfig.forEach(f => {
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
}
