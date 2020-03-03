import { FocusMonitor } from '@angular/cdk/a11y';
import { coerceBooleanProperty } from '@angular/cdk/coercion';
import {
  Component,
  ElementRef,
  EventEmitter,
  forwardRef,
  HostBinding,
  Input,
  OnDestroy,
  OnInit,
  Output,
  Renderer2
} from '@angular/core';
import {
  AbstractControl,
  ControlValueAccessor,
  FormBuilder,
  FormGroup,
  NG_VALIDATORS,
  NG_VALUE_ACCESSOR,
  ValidationErrors,
  Validator,
  Validators
} from '@angular/forms';
import { MatFormFieldControl } from '@angular/material';
import {
  BiblioField,
  FieldConfig
} from 'app/bibliographic-data/models/field-config';
import { IntellectualPropertyDetails } from 'app/shared/models/intellectual-property-details';
import { RouteStageCodes } from 'app/shared/models/route-stage-codes';
import { DictionaryService } from 'app/shared/services/dictionary.service';
import { BaseDictionary } from 'app/shared/services/models/base-dictionary';
import { DictionaryType } from 'app/shared/services/models/dictionary-type.enum';
import { SelectOption } from 'app/shared/services/models/select-option';
import { Observable } from 'rxjs/Observable';
import { Subject } from 'rxjs/Subject';
import { Subscription } from 'rxjs/Subscription';

@Component({
  selector: 'app-trademark-fields',
  templateUrl: './trademark-fields.component.html',
  styleUrls: ['./trademark-fields.component.scss'],
  // Провайдеры для контрола
  providers: [
    {
      // Интерфейс, переопределяющий поведение компонента как контрол Angular Material
      provide: MatFormFieldControl,
      useExisting: TrademarkFieldsComponent
    },
    {
      provide: BiblioField,
      useExisting: TrademarkFieldsComponent
    },
    {
      // Дает возможность обращения к компоненту через formControlName
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => TrademarkFieldsComponent),
      multi: true
    },
    {
      provide: NG_VALIDATORS,
      useExisting: forwardRef(() => TrademarkFieldsComponent),
      multi: true
    }
  ]
})
export class TrademarkFieldsComponent extends BiblioField
  implements
    OnInit,
    OnDestroy,
    MatFormFieldControl<TrademarkFields>,
    ControlValueAccessor,
    Validator {
  static nextId = 0;
  // Событие, поднимающееся при изменении значения
  @Output()
  changed = new EventEmitter<any>();
  @Output()
  speciesTradeMarkChanged = new EventEmitter<any>();
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
  set value(value: TrademarkFields) {
    this.writeValue(value);
    this.stateChanges.next();
  }
  // уникальный идентификатор контрола, селектор компонента + айдишник
  @HostBinding()
  id = `${this.controlType}-${TrademarkFieldsComponent.nextId++}`;
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
  controlType = 'app-trademark-fields-form';

  formGroup: FormGroup;
  protectionDocSubTypeFTMCode = 'FTM'; // Общеизвестный товарный знак
  protectionDocSubtypeCtmCode = 'KTM';
  typeTrademark: BaseDictionary[];
  //словевсный = 08, буквенный = 01, комбинированный = 05
  requeredTypeTrademarkCodes = ['08','01', '05']
  speciesTradeMarks: SelectOption[] = [];
  speciesTradeMarkCodes = ['KTM', 'FTM', 'TTM'];

  @Input()
  owner: IntellectualPropertyDetails;
  @Output()
  public trademarkTypeChanged = new EventEmitter<number>();

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
      fieldName: 'disclaimerRu'
    },
    {
      stageCodes: [
        ...RouteStageCodes.stagesFormationAppData,
        ...RouteStageCodes.stagesformalExam,
        ...RouteStageCodes.stagesFullExpertise
      ],
      fieldName: 'disclaimerKz'
    },
    {
      stageCodes: [
        ...RouteStageCodes.stagesFormationAppData,
        ...RouteStageCodes.stagesformalExam,
        ...RouteStageCodes.stagesFullExpertise
      ],
      fieldName: 'disclaimerEn'
    },
    {
      stageCodes: [
        ...RouteStageCodes.stagesFormationAppData,
        ...RouteStageCodes.stagesformalExam,
        ...RouteStageCodes.stagesFullExpertise
      ],
      fieldName: 'typeTrademarkId'
    },
    {
      stageCodes: RouteStageCodes.stagesFormationAppData,
      fieldName: 'dateRecognizedKnown'
    },
    {
      stageCodes: RouteStageCodes.stagesFormationAppData,
      fieldName: 'infoDecisionToRecognizedKnown'
    },
    {
      stageCodes: RouteStageCodes.stagesFormationAppData,
      fieldName: 'infoConfirmKnownTrademark'
    },
    {
      stageCodes: RouteStageCodes.stagesFormationAppData,
      fieldName: 'speciesTradeMarkId'
    },
    {
      stageCodes: RouteStageCodes.stagesFormationAppData,
      fieldName: 'translation'
    },
    {
      stageCodes: RouteStageCodes.stagesFormationAppData,
      fieldName: 'colectiveTrademarkParticipantsInfo'
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

  writeValue(obj: TrademarkFields): void {
    if (obj) {
      if (this.formGroup) {
        this.formGroup
          .get('dateRecognizedKnown')
          .setValue(obj.dateRecognizedKnown);
        this.formGroup.get('disclaimerKz').setValue(obj.disclaimerKz);
        this.formGroup.get('disclaimerRu').setValue(obj.disclaimerRu);
        this.formGroup
          .get('infoConfirmKnownTrademark')
          .setValue(obj.infoConfirmKnownTrademark);
        this.formGroup
          .get('infoDecisionToRecognizedKnown')
          .setValue(obj.infoDecisionToRecognizedKnown);
        this.formGroup
          .get('speciesTradeMarkId')
          .setValue(obj.speciesTradeMarkId);
        this.formGroup.get('transliteration').setValue(obj.transliteration);
        this.formGroup.get('typeTrademarkId').setValue(obj.typeTrademarkId);
        this.formGroup.get('disclaimerEn').setValue(obj.disclaimerEn);
        this.formGroup.get('colectiveTrademarkParticipantsInfo').setValue(obj.colectiveTrademarkParticipantsInfo);
        this.formGroup.get('translation').setValue(obj.translation);
        this.formGroup.get('publicDate').setValue(obj.publicDate);
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

  getValue() {
    return this.value;
  }

  isSpeciesTrademarkCode(code: string) {
    const speciesId = this.formGroup.get('speciesTradeMarkId').value;
    return this.speciesTradeMarks.some(s => s.id === speciesId && s.code === code);
  }

  onTrademarkTypeChanged(id: number) {
    this.trademarkTypeChanged.emit(id);
    this.onSpeciesTradeMarkChanged(id);
  }

  isRequiredColectiveTrademarkParticipantsInfo() {
    const speciesId = this.formGroup.get('speciesTradeMarkId').value;
    const speciesCode = this.speciesTradeMarks && this.speciesTradeMarks.find(s => s.id === speciesId) && this.speciesTradeMarks.find(s => s.id === speciesId).code;
    return speciesCode === this.protectionDocSubtypeCtmCode;
  }

  isRequiredTransliteration() {
    const typeTrademarkId = this.formGroup.get('typeTrademarkId').value;
    const typeTrademarkCode = this.typeTrademark && this.typeTrademark.find(s => s.id === typeTrademarkId) && this.typeTrademark.find(s => s.id === typeTrademarkId).code;
    return this.requeredTypeTrademarkCodes.some(s => s === typeTrademarkCode);
  }

  onSpeciesTradeMarkChanged(id: number) {
    this.speciesTradeMarkChanged.emit(id);
    const isRequired = this.isRequiredColectiveTrademarkParticipantsInfo();
    if (isRequired) {
      // this.formGroup.get('colectiveTrademarkParticipantsInfo').setValidators(Validators.required);
    } else {
      this.formGroup.get('colectiveTrademarkParticipantsInfo').clearValidators();
      this.formGroup.get('colectiveTrademarkParticipantsInfo').setErrors({'incorrect':  isRequired});
    }
    this.formGroup.get('colectiveTrademarkParticipantsInfo').updateValueAndValidity();
  }

  private buildForm() {
    this.formGroup = this.fb.group({
      // поля ТЗ
      transliteration: [{ value: '', disabled: true }],
      disclaimerRu: [{ value: '', disabled: true }],
      disclaimerKz: [{ value: '', disabled: true }],
      disclaimerEn: [{ value: '', disabled: true }],
      speciesTradeMarkId: [{ value: '', disabled: true }],
      typeTrademarkId: [{ value: '', disabled: true }],
      dateRecognizedKnown: [{ value: '', disabled: true }],
      infoDecisionToRecognizedKnown: [{ value: '', disabled: true }],
      infoConfirmKnownTrademark: [{ value: '', disabled: true }],
      colectiveTrademarkParticipantsInfo: [{ value: '', disabled: true }],
      publicDate: [{ value: '', disabled: true }],
      translation: [{ value: '', disabled: true }]
    });
  }

  private initSelectOptions() {
    Observable.combineLatest(
      this.dictionaryService.getBaseDictionary(DictionaryType.DicTypeTrademark),
      this.dictionaryService.getSelectOptions(
        DictionaryType.DicProtectionDocSubType
      )
    )
      .takeUntil(this.onDestroy)
      .subscribe(([typeTrademark, protectionDocSubTypes]) => {
        this.typeTrademark = typeTrademark;
        this.typeTrademark = this.typeTrademark.filter((el, i, a) => i === a.map(ar => ar.code).indexOf(el.code));
        this.speciesTradeMarks = protectionDocSubTypes.filter(t =>
          this.speciesTradeMarkCodes.includes(t.code)
        );
        // this.speciesTradeMarks.unshift(
        //   this.dictionaryService.emptyBaseDictionary
        // );
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

export class TrademarkFields {
  transliteration: string;
  disclaimerRu: string;
  disclaimerKz: string;
  disclaimerEn: string;
  speciesTradeMarkId: number;
  typeTrademarkId: number;
  dateRecognizedKnown: Date;
  infoDecisionToRecognizedKnown: string;
  infoConfirmKnownTrademark: string;
  colectiveTrademarkParticipantsInfo: string;
  publicDate: Date;
  translation: string;
}
