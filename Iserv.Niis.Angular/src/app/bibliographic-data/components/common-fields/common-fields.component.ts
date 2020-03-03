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
  Renderer2,
  SimpleChanges,
  OnChanges
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
import { SnackBarHelper } from 'app/core/snack-bar-helper.service';
import { RequestService } from 'app/requests/request.service';
import { BulletinDto } from 'app/shared/models/bulletin-dto';
import { IntellectualPropertyDetails } from 'app/shared/models/intellectual-property-details';
import { RouteStageCodes } from 'app/shared/models/route-stage-codes';
import { BulletinService } from 'app/shared/services/bulletin.service';
import { DictionaryService } from 'app/shared/services/dictionary.service';
import { DicProtectionDocSubType } from 'app/shared/services/models/base-dictionary';
import { DictionaryType } from 'app/shared/services/models/dictionary-type.enum';
import { OwnerType } from 'app/shared/services/models/owner-type.enum';
import { SelectOption } from 'app/shared/services/models/select-option';
import { nmptNumberMask } from 'app/shared/services/validator/custom-validators';
import { Observable } from 'rxjs/Observable';
import { Subject } from 'rxjs/Subject';
import { Subscription } from 'rxjs/Subscription';
import { log } from 'util';
import {
  hasSelectiveAchievementType,
  isStageFormationAppData,
  isStageMakingChanges
} from '../description/description.component';
import { ProtectionDocTypes } from 'app/shared/enums/protection-doc-types.enum';
import { IGenerateNumberResponse } from 'app/requests/interfaces/generate-number-response.interface';
import { IServerStatus } from 'app/requests/interfaces/server-status.interface';
import { StatusCodes } from 'app/requests/enums/status-codes.enum';
import { ReceiveTypeCodes } from 'app/requests/enums/receive-type-codes.enum';

@Component({
  selector: 'app-common-fields',
  templateUrl: './common-fields.component.html',
  styleUrls: ['./common-fields.component.scss'],
  // Провайдеры для контрола
  providers: [
    {
      // Интерфейс, переопределяющий поведение компонента как контрол Angular Material
      provide: MatFormFieldControl,
      useExisting: CommonFieldsComponent
    },
    {
      provide: BiblioField,
      useExisting: CommonFieldsComponent
    },
    {
      // Дает возможность обращения к компоненту через formControlName
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => CommonFieldsComponent),
      multi: true
    },
    {
      provide: NG_VALIDATORS,
      useExisting: forwardRef(() => CommonFieldsComponent),
      multi: true
    }
  ]
})
export class CommonFieldsComponent extends BiblioField
  implements
    OnInit,
    OnChanges,
    OnDestroy,
    MatFormFieldControl<CommonFields>,
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
    this.toggleEditMode(this.editMode);
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
  set value(value: CommonFields) {
    this.writeValue(value);
    this.stateChanges.next();
  }
  // уникальный идентификатор контрола, селектор компонента + айдишник
  @HostBinding()
  id = `${this.controlType}-${CommonFieldsComponent.nextId++}`;
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
  bulletinId = 0;
  // Тип контрола, должен совпадать с селектором компонента
  controlType = 'app-common-fields-form';

  nmptNumberMask = nmptNumberMask;
  formGroup: FormGroup;
  dicRequestStatuses: SelectOption[];
  dicBeneficiaryTypes: SelectOption[];
  dicPDTypes: SelectOption[];
  onlineRequisitionStatus: SelectOption[];
  dicSelectionAchieveTypes: SelectOption[] = [];
  bulletins: BulletinDto[] = [];
  filteredPDSubTypes: DicProtectionDocSubType[];
  fieldsConfig: FieldConfig[] = [
    {
      stageCodes: [
        ...RouteStageCodes.stagesFormationAppData,
        ...RouteStageCodes.stagesformalExam,
        ...RouteStageCodes.stagesFullExpertise
      ],
      fieldName: 'requestDate'
    },
    {
      stageCodes: [
        ...RouteStageCodes.stagesFormationAppData,
        ...RouteStageCodes.stagesformalExam,
        ...RouteStageCodes.stagesFullExpertise
      ],
      fieldName: 'publicDate'
    },
    {
      stageCodes: [
        ...RouteStageCodes.stagesFormationAppData,
        ...RouteStageCodes.stagesformalExam,
        ...RouteStageCodes.stagesMakingChanges,
        ...RouteStageCodes.stagesPatentExam
      ],
      fieldName: 'nameRu'
    },
    {
      stageCodes: [
        ...RouteStageCodes.stagesFormationAppData,
        ...RouteStageCodes.stagesformalExam,
        ...RouteStageCodes.stagesMakingChanges,
        ...RouteStageCodes.stagesPatentExam
      ],
      fieldName: 'nameKz'
    },
    {
      stageCodes: [
        ...RouteStageCodes.stagesFormationAppData,
        ...RouteStageCodes.stagesformalExam,
        ...RouteStageCodes.stagesMakingChanges,
        ...RouteStageCodes.stagesPatentExam
      ],
      fieldName: 'nameEn'
    },
    {
      stageCodes: [
        ...RouteStageCodes.stagesFormationAppData,
        ...RouteStageCodes.stagesformalExam,
        ...RouteStageCodes.stagesMakingChanges,
        ...RouteStageCodes.stagesPatentExam
      ],
      fieldName: 'lastOnlineRequisitionStatusId'
    },
    {
      stageCodes: RouteStageCodes.stagesFormationAppData,
      fieldName: 'requestTypeId'
    },
    {
      stageCodes: RouteStageCodes.stagesFormationAppData,
      fieldName: 'selectionAchieveTypeId'
    },
    {
      stageCodes: [
        ...RouteStageCodes.stagesFormationAppData,
        ...RouteStageCodes.stagesformalExam
      ],
      fieldName: 'countIndependentItems'
    },
    {
      stageCodes: ['OD04.6', 'OD01.3', 'OD01.2.1', 'OD01.2.2'],
      fieldName: 'bulletinId'
    },
    {
      stageCodes: ['OD05'],
      fieldName: 'yearMaintain'
    },
    {
      stageCodes: ['OD04.3'],
      fieldName: 'validDate'
    },
    {
      stageCodes: ['OD01.6'],
      fieldName: 'bulletinId'
    }
  ];
  forViewDicProtectionDocTypeCodes = [
    '01',
    '02',
    '04',
    '05',
    '03_UsefulModel',
    '04_UsefulModel',
    '05_UsefulModel',
    '03_Industrial_Sample',
    '04_Industrial_Sample',
    '05_Industrial_Sample',
    '03_Trade_Mark',
    '04_Trade_Mark',
    'TM',
    '03_Name_of_Origin',
    '04_Name_of_Origin',
    '05_Name_of_Origin',
    '03_Selection_Achieve',
    '04_Selection_Achieve',
    '05_Selection_Achieve',
    '03_PD',
    '04_PD',
    '05_PD',
    '03_UsefulModel_PD',
    '04_UsefulModel_PD',
    '05_UsefulModel_PD',
    '03_Industrial_Sample_PD',
    '04_Industrial_Sample_PD',
    '05_Industrial_Sample_PD',
    '03_Trade_Mark_PD',
    '04_Trade_Mark_PD',
    'TM_PD',
    '03_Name_of_Origin_PD',
    '04_Name_of_Origin_PD',
    '05_Name_of_Origin_PD',
    '03_Selection_Achieve_PD',
    '04_Selection_Achieve_PD',
    '05_Selection_Achieve_PD'
  ];
  availableOnlineRequisitionStatusCodes = [
    'N',
    'F',
    'CS',
    'CA',
    'DP',
    'CP',
    '821',
    'CSP',
    'CSPOL'
  ];

  private _required = false;
  private _placeholder: string;
  private _disabled = false;
  private onDestroy = new Subject();
  private subs: Subscription[] = [];

  @Input()
  owner: IntellectualPropertyDetails;
  @Input()
  editMode: boolean;

  get pdTypeCOOCodes() {
    return RouteStageCodes.pdTypeCOOCodes;
  }

  get requestTypeSelectiveAchievementsCode() {
    return RouteStageCodes.pdTypeSelectiveAchievementsCodes;
  }

  get pdTypeInventionsCodes() {
    return RouteStageCodes.pdTypeInventionsCodes;
  }

  get pdTypeTMCodes() {
    return RouteStageCodes.pdTypeTMCodes;
  }

  constructor(
    private fm: FocusMonitor,
    private elRef: ElementRef,
    private renderer: Renderer2,
    private requestService: RequestService,
    private fb: FormBuilder,
    private snackbarHelper: SnackBarHelper,
    private dictionaryService: DictionaryService,
    private bulletinService: BulletinService
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

  ngOnChanges(changes: SimpleChanges) {
    if (changes.owner && changes.owner.currentValue && changes.owner.currentValue.bulletinId !== this.bulletinId) {
      this.bulletinId = changes.owner.currentValue.bulletinId;
      this.bulletinService
        .getById(this.bulletinId)
        .takeUntil(this.onDestroy)
        .subscribe(bulletin => {
          if (bulletin) {
            this.bulletins = [bulletin];
          }
        });
    }
    if (changes.owner && changes.owner.currentValue && changes.owner.currentValue.speciesTradeMarkId) {
      // Если коллективный товарный знак(код 93) то тип запроса всегда национальный(код85)
      if (changes.owner.currentValue.speciesTradeMarkId === 93) {
        this.formGroup.get('requestTypeId').setValue(85);
      }
    }
  }

  sendStatus(): void {
    this.requestService.sendStatus(this.owner.id, this.formGroup.get('lastOnlineRequisitionStatusId').value)
      .subscribe((status: IServerStatus) => {
        this.processSendStatus(status);
      }, (error: any) => {
        this.snackbarHelper.error(`${error.message}. Просим обратиться к администратору системы.`);
      });
  }

  isSendStatusDisabled(): boolean {
    return !this.formGroup.get('lastOnlineRequisitionStatusId').value;
  }

  canHavemask(): boolean {
    return (
      this.pdTypeCOOCodes.includes(this.owner.protectionDocTypeCode) &&
      this.editMode &&
      isStageFormationAppData(this.owner)
    );
  }

  canShowCountIndependentItems(): boolean {
    return this.owner.protectionDocTypeCode !== ProtectionDocTypes.IndustrialDesign;
  }

  isDisabledGenerateRequestNum(): boolean {
    if (!this.formGroup) {
      return true;
    }
    if (!hasSelectiveAchievementType(this.owner)) {
      return true;
    }
    const requestNum = this.formGroup.get('requestNum').value;
    if (requestNum) {
      return true;
    } else {
      return (
        this.editMode ||
        !isStageFormationAppData(this.owner) ||
        (this.requestTypeSelectiveAchievementsCode.includes(
          this.owner.protectionDocTypeCode
        ) &&
          !this.owner.selectionAchieveTypeId)
      );
    }
  }

  generateRequestNumber() {
    const subtypeId = this.formGroup.get('requestTypeId').value;
    this.requestService.generateRequestNumber(this.owner.id, subtypeId)
      .subscribe((result: IGenerateNumberResponse) => {
        if (result) {
          this.formGroup.get('requestNum').setValue(result.number);
          this.formGroup.get('requestDate').setValue(this.owner.dateCreate);
          this.changed.emit(result);

          this.processSendRegNumber(result.status);
        }
      }, err => {
        this.snackbarHelper.error(err.message);
        log(err);
      });
  }

  sendRegNumber() {
    this.requestService.sendRegNumber(this.owner.id)
      .subscribe((result: IServerStatus) => {
        this.processSendRegNumber(result);
      }, (error: any) => {
        this.snackbarHelper.error(`${error.message}. Просим обратиться к администратору системы.`);
      });
  }

  private processSendRegNumber(status: IServerStatus) {
    if (status) {
      switch (status.code) {
        case StatusCodes.Successfully:
          this.owner.isSyncRequestNum = true;
          this.snackbarHelper.success('Регистрационный номер успешно отправлен в ЛК');
          break;
        case StatusCodes.NotFound:
        case StatusCodes.UnknownType:
        case StatusCodes.BadRequest:
        case StatusCodes.ExistNumber:
          this.snackbarHelper.error(status.message);
          break;
      }
    }
  }

  private processSendStatus(status: IServerStatus) {
    if (status) {
      switch (status.code) {
        case StatusCodes.Successfully:
          this.snackbarHelper.success(status.message);
          break;
        case StatusCodes.NotFound:
        case StatusCodes.UnknownType:
        case StatusCodes.BadRequest:
        case StatusCodes.ExistNumber:
          this.snackbarHelper.error(status.message);
          break;
      }
    }
  }

  isRequestNumberButtonVisible(): boolean {
    return (
      (
        !this.owner.isSyncRequestNum &&
        this.owner.receiveTypeId === ReceiveTypeCodes.ElectronicFeed
      ) ||
      !this.owner.requestNum
    );
  }

  hasRequestNumber(): boolean {
    return !!this.owner.requestNum;
  }

  isDisabledRequestType(): boolean {
    if (!this.formGroup) {
      return true;
    }
    if (
      !!this.owner &&
      this.requestTypeSelectiveAchievementsCode.includes(
        this.owner.protectionDocTypeCode
      ) &&
      !this.owner.selectionAchieveTypeId
    ) {
      return true;
    }
    if (
      !!this.owner &&
      this.pdTypeInventionsCodes.includes(this.owner.protectionDocTypeCode) &&
      !this.formGroup.get('requestNum').value
    ) {
      return false;
    }
    if (
      !!this.owner &&
      this.pdTypeInventionsCodes.includes(this.owner.protectionDocTypeCode) &&
      !!this.formGroup.get('requestNum').value
    ) {
      return true;
    }

    if (!!this.owner && this.owner.protectionDocTypeCode === 'TM') {
      return true;
    }

    return !this.editMode || !isStageFormationAppData(this.owner);
  }

  getTitle(code: string) {
    switch (code) {
      case 'B':
      case 'B_PD':
        return 'Название изобретения';
      case 'U':
      case 'U_PD':
        return 'Название полезной модели';
      case 'TM':
      case 'TM_PD':
        return 'Название товарного знака';
      case 'S2':
      case 'S2_PD':
        return 'Название промышленного образца';
      case 'SA':
      case 'SA_PD':
        return 'Название селекционного достижения';
      case 'PN':
      case 'PN_PD':
        return 'Название наименования мест происхождения товаров';
    }
  }

  setDescribedByIds(ids: string[]): void {
    this.describedBy = ids.join(' ');
  }

  onContainerClick(event: MouseEvent): void {
    if ((event.target as Element).tagName.toLowerCase() !== 'input') {
      this.elRef.nativeElement.querySelector('input').focus();
    }
  }

  writeValue(obj: CommonFields): void {
    if (obj) {
      if (this.formGroup) {
        this.formGroup.get('beneficiaryTypeId').setValue(obj.beneficiaryTypeId);
        this.formGroup.get('bulletinId').setValue(obj.bulletinId);
        this.formGroup
          .get('countIndependentItems')
          .setValue(obj.countIndependentItems);
        this.formGroup.get('extensionDate').setValue(obj.extensionDate);
        this.formGroup.get('gosDate').setValue(obj.gosDate);
        this.formGroup.get('gosNumber').setValue(obj.gosNumber);
        this.formGroup.get('nameEn').setValue(obj.nameEn);
        this.formGroup.get('nameKz').setValue(obj.nameKz);
        this.formGroup.get('nameRu').setValue(obj.nameRu);
        this.formGroup.get('requestDate').setValue(obj.requestDate);
        this.formGroup.get('publicDate').setValue(obj.publicDate);
        this.formGroup.get('requestNum').setValue(obj.requestNum);
        this.formGroup.get('requestTypeId').setValue(obj.requestTypeId);
        this.formGroup.get('selectionAchieveTypeId').setValue(obj.selectionAchieveTypeId);
        this.formGroup.get('lastOnlineRequisitionStatusId').setValue(obj.lastOnlineRequisitionStatusId);
        this.formGroup.get('statusId').setValue(obj.statusId);
        this.formGroup.get('validDate').setValue(obj.validDate);
        this.formGroup.get('yearMaintain').setValue(obj.yearMaintain);
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
    return this.formGroup.invalid ? invalid : null;
  }

  registerOnValidatorChange?(fn: () => void): void {
    return;
  }

  doNamesHaveValue() {
    if (!(this.formGroup && this.dicPDTypes)) {
      return false;
    }
    const type = this.dicPDTypes.find(
      d => !!this.owner && d.id === this.owner.protectionDocTypeId
    );
    if (!type) {
      return false;
    }
    switch (type.code) {
      case 'TM':
        return true;
      case 'PN':
      case 'B':
      case 'SA':
      case 'U':
        return (
          this.formGroup.get('nameRu').value ||
          this.formGroup.get('nameKz').value ||
          this.formGroup.get('nameEn').value
        );
      default:
        return true;
    }
  }

  getValue() {
    return this.value;
  }

  get isSelectionAchieve(): boolean {
    return this.owner.protectionDocTypeCode === ProtectionDocTypes.SelectionAchievement;
  }

  get minPublicDate(): Date {
    const requestDate = this.formGroup.get('requestDate').value;

    if (requestDate) {
      return new Date(Date.parse(requestDate));
    } else {
      return new Date();
    }
  }

  private buildForm() {
    this.formGroup = this.fb.group({
      requestNum: [{ value: '', disabled: true }],
      requestTypeId: [{ value: '', disabled: true }],
      selectionAchieveTypeId: [{ value: '', disabled: true }],
      lastOnlineRequisitionStatusId: [{ value: '', disabled: true }],
      requestDate: [{ value: '', disabled: true }],
      publicDate: [{ value: '', disabled : true }],
      beneficiaryTypeId: [{ value: '', disabled: true }],
      nameRu: [{ value: '', disabled: true }],
      nameKz: [{ value: '', disabled: true }],
      nameEn: [{ value: '', disabled: true }],
      gosNumber: [{ value: '', disabled: true }],
      gosDate: [{ value: '', disabled: true }],
      statusId: [{ value: '', disabled: true }],
      bulletinId: [{ value: '', disabled: true }],
      countIndependentItems: [{ value: '', disabled: true }],
      validDate: [{ value: '', disabled: true }],
      extensionDate: [{ value: '', disabled: true }],
      yearMaintain: [{ value: '', disabled: true }]
    });
  }

  private initSelectOptions() {
    Observable.combineLatest(
      this.dictionaryService.getBaseDictionary(
        DictionaryType.DicProtectionDocSubType
      ),
      this.dictionaryService.getSelectOptions(
        DictionaryType.DicBeneficiaryType
      ),
      this.dictionaryService.getSelectOptions(DictionaryType.DicRequestStatus),
      this.dictionaryService.getSelectOptions(
        DictionaryType.DicProtectionDocStatus
      ),
      this.dictionaryService.getSelectOptions(
        DictionaryType.DicProtectionDocType
      ),
      this.dictionaryService.getBaseDictionaryByCodes(
        DictionaryType.DicOnlineRequisitionStatus,
        this.availableOnlineRequisitionStatusCodes
      ),
      this.dictionaryService.getSelectOptions(DictionaryType.DicSelectionAchieveType),
      this.bulletinService.get()
    )
      .takeUntil(this.onDestroy)
      .subscribe(
        ([
          subTypes,
          beneficiarytypes,
          requestStatuses,
          protectionDocStatuses,
          dicPdTypes,
          onlineRequisitionStatus,
          selectiveAchievementTypes,
          bulletins
        ]) => {
          this.dicBeneficiaryTypes = beneficiarytypes;
          this.dicBeneficiaryTypes.unshift(
            this.dictionaryService.emptyBaseDictionary
          );
          if (this.owner.ownerType === OwnerType.Request) {
            this.dicRequestStatuses = requestStatuses;
          } else {
            this.dicRequestStatuses = protectionDocStatuses;
          }
          const selectedSubtype = subTypes.find(
            s => !!this.owner && s.id === this.owner.requestTypeId
          );
          subTypes = subTypes.filter(p =>
            this.forViewDicProtectionDocTypeCodes.includes(p.code)
          );
          this.filteredPDSubTypes = !!this.owner
            ? subTypes.filter(st => {
                return st.typeId === this.owner.protectionDocTypeId;
              })
            : subTypes;
          if (
            !this.filteredPDSubTypes.includes(selectedSubtype) &&
            selectedSubtype
          ) {
            this.filteredPDSubTypes.push(selectedSubtype);
          }
          this.dicPDTypes = dicPdTypes;
          this.onlineRequisitionStatus = onlineRequisitionStatus;
          this.dicSelectionAchieveTypes = selectiveAchievementTypes;
          this.bulletins = bulletins;
        }
      );
  }

  private toggleEditMode(value: boolean) {
    this.fieldsConfig.forEach(f => {
      if (this.owner) {
        if (f.fieldName === 'lastOnlineRequisitionStatusId') {
          this.formGroup.get(f.fieldName).enable();
        }
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

export class CommonFields {
  requestNum: string;
  requestTypeId: number;
  selectionAchieveTypeId: number;
  lastOnlineRequisitionStatusId: number;
  requestDate: Date;
  publicDate: Date;
  beneficiaryTypeId: number;
  nameRu: string;
  nameKz: string;
  nameEn: string;
  gosNumber: string;
  gosDate: Date;
  statusId: number;
  bulletinId: number;
  countIndependentItems: string;
  validDate: Date;
  extensionDate: Date;
  yearMaintain: number;
}
