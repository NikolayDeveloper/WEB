import {
  Component,
  OnInit,
  ElementRef,
  Renderer2,
  Input,
  forwardRef,
  EventEmitter,
  Output,
  HostBinding,
  OnDestroy
} from '@angular/core';
import { FocusMonitor } from '@angular/cdk/a11y';
import {
  FormBuilder,
  FormGroup,
  ControlValueAccessor,
  Validator,
  NG_VALUE_ACCESSOR,
  NG_VALIDATORS,
  ValidationErrors,
  AbstractControl
} from '@angular/forms';
import { DictionaryService } from 'app/shared/services/dictionary.service';
import { SelectOption } from 'app/shared/services/models/select-option';
import { DictionaryType } from 'app/shared/services/models/dictionary-type.enum';
import { Subject } from 'rxjs/Subject';
import { IntellectualPropertyDetails } from 'app/shared/models/intellectual-property-details';
import { MatFormFieldControl } from '@angular/material';
import { coerceBooleanProperty } from '@angular/cdk/coercion';
import { RouteStageCodes } from 'app/shared/models/route-stage-codes';
import { Subscription } from 'rxjs/Subscription';
import {
  FieldConfig,
  BiblioField
} from 'app/bibliographic-data/models/field-config';
import { Observable } from 'rxjs/Observable';

@Component({
  selector: 'app-selection-fields',
  templateUrl: './selection-fields.component.html',
  styleUrls: ['./selection-fields.component.scss'],
  // Провайдеры для контрола
  providers: [
    {
      // Интерфейс, переопределяющий поведение компонента как контрол Angular Material
      provide: MatFormFieldControl,
      useExisting: SelectionFieldsComponent
    },
    {
      provide: BiblioField,
      useExisting: SelectionFieldsComponent
    },
    {
      // Дает возможность обращения к компоненту через formControlName
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => SelectionFieldsComponent),
      multi: true
    },
    {
      provide: NG_VALIDATORS,
      useExisting: forwardRef(() => SelectionFieldsComponent),
      multi: true
    }
  ]
})
export class SelectionFieldsComponent extends BiblioField
  implements
    OnInit,
    OnDestroy,
    MatFormFieldControl<SelectionFields>,
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
      const result = new SelectionFields();
      result.breedCountryId = this.formGroup.get('breedCountryId').value.id;
      result.breedingNumber = this.formGroup.get('breedingNumber').value;
      result.genus = this.formGroup.get('genus').value;
      // result.selectionAchieveTypeId = this.formGroup.get(
      //   'selectionAchieveTypeId'
      // ).value;
      return result;
    }
    return null;
  }
  set value(value: SelectionFields) {
    this.writeValue(value);
    this.stateChanges.next();
  }
  // уникальный идентификатор контрола, селектор компонента + айдишник
  @HostBinding()
  id = `${this.controlType}-${SelectionFieldsComponent.nextId++}`;
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
  controlType = 'app-selection-fields-form';

  formGroup: FormGroup;
  dicCountries: SelectOption[] = [];
  // dicSelectionAchieveTypes: SelectOption[] = [];

  @Input()
  owner: IntellectualPropertyDetails;

  private onDestroy = new Subject();
  private _required = false;
  private _placeholder: string;
  private _disabled = false;
  private subs: Subscription[] = [];
  private fieldsConfig: FieldConfig[] = [
    {
      stageCodes: [...RouteStageCodes.stagesFormationAppData],
      fieldName: 'genus'
    },
    {
      stageCodes: [...RouteStageCodes.stagesFormationAppData],
      fieldName: 'breedingNumber'
    },
    {
      stageCodes: [...RouteStageCodes.stagesFormationAppData],
      fieldName: 'breedCountryId'
    },
    {
      stageCodes: [...RouteStageCodes.stagesFormationAppData],
      fieldName: 'selectionAchieveTypeId'
    }
  ];

  constructor(
    private fm: FocusMonitor,
    private elRef: ElementRef,
    private renderer: Renderer2,
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

  setDescribedByIds(ids: string[]): void {
    this.describedBy = ids.join(' ');
  }

  onContainerClick(event: MouseEvent): void {
    if ((event.target as Element).tagName.toLowerCase() !== 'input') {
      this.elRef.nativeElement.querySelector('input').focus();
    }
  }

  writeValue(obj: SelectionFields): void {
    if (obj) {
      if (this.formGroup) {
        this.formGroup.get('genus').setValue(obj.genus);
        this.formGroup.get('breedingNumber').setValue(obj.breedingNumber);
        this.formGroup.get('breedCountryId').setValue(obj.breedCountryId);
        // this.formGroup
        //   .get('selectionAchieveTypeId')
        //   .setValue(obj.selectionAchieveTypeId);
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
      // поля селекционных достижений
      genus: [{ value: '', disabled: true }],
      breedingNumber: [{ value: '', disabled: true }],
      breedCountryId: [{ value: '', disabled: true }],
      selectionAchieveTypeId: [{ value: '', disabled: true }]
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

  private initSelectOptions() {
    // Observable.combineLatest(
    //   this.dictionaryService.getSelectOptions(DictionaryType.DicCountry),
    //   this.dictionaryService.getSelectOptions(
    //     DictionaryType.DicSelectionAchieveType
    //   )
    // )
    //   .takeUntil(this.onDestroy)
    //   .subscribe(([countries, selectiveAchievementTypes]) => {
    //     this.dicCountries = countries;
    //     this.dicSelectionAchieveTypes = selectiveAchievementTypes;
    //   });
    Observable.combineLatest(
      this.dictionaryService.getSelectOptions(DictionaryType.DicCountry)
    )
      .takeUntil(this.onDestroy)
      .subscribe(([countries]) => {
        this.dicCountries = countries;
      });
  }

  private propagateChange = (_: any) => {};
}

export class SelectionFields {
  genus: string;
  breedingNumber: string;
  breedCountryId: number;
  // selectionAchieveTypeId: number;
}
