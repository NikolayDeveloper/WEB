import {
  AfterViewInit,
  ChangeDetectorRef,
  Component,
  EventEmitter,
  Input,
  OnChanges,
  OnDestroy,
  OnInit,
  Output,
  SimpleChanges,
  ViewChild
} from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material';
import 'app/core/utils/array-extensions';
import { PostKzService } from 'app/modules/postkz';
import { RequestService } from 'app/requests/request.service';
import { SubjectsSearchFormComponent } from 'app/subjects/components/subjects-search-form/subjects-search-form.component';
import { Observable } from 'rxjs/Observable';
import { Subject } from 'rxjs/Subject';
import { ConfigService } from '../../../../core';
import { SnackBarHelper } from '../../../../core/snack-bar-helper.service';
import { JournalService } from '../../../../journal/journal.service';
import { BarcodeDialogComponent } from '../../../../shared/components/barcode-dialog/barcode-dialog.component';
import { DeleteDialogComponent } from '../../../../shared/components/delete-dialog/delete-dialog.component';
import { Config } from '../../../../shared/components/table/config.model';
import { UndoDialogComponent } from '../../../../shared/components/undo-dialog/undo-dialog.component';
import { RouteStageCodes } from '../../../../shared/models/route-stage-codes';
import { CustomerService } from '../../../../shared/services/customer.service';
import { DictionaryService } from '../../../../shared/services/dictionary.service';
import { DicDepartment } from '../../../../shared/services/models/base-dictionary';
import { DictionaryType } from '../../../../shared/services/models/dictionary-type.enum';
import { OwnerType } from '../../../../shared/services/models/owner-type.enum';
import { SelectOption } from '../../../../shared/services/models/select-option';
import { WorkflowService } from '../../../../shared/services/workflow.service';
import { SubjectDto } from '../../../../subjects/models/subject.model';
import { SubjectsService } from '../../../../subjects/services/subjects.service';
import { RequestDetails } from '../../../models/request-details';
import { FromUTCDateToShortDateString } from '../../../../payments-journal/helpers/date-helpers';
import {
  RequestPart,
  WorkflowBusinessService
} from '../../../services/workflow-business.service';

@Component({
  selector: 'app-request',
  templateUrl: './request.component.html',
  styleUrls: [
    './request.component.scss',
    '../request-detail/request-detail.component.scss'
  ]
})
export class RequestComponent
  implements OnInit, OnDestroy, OnChanges, AfterViewInit {
  workflows: any[];
  columns: Config[] = [
    new Config({ columnDef: 'routeNameRu', header: 'Маршрут', class: 'width-200' }),
    new Config({ columnDef: 'currentUserNameRu', header: 'Пользователь', class: 'width-200' }),
    new Config({ columnDef: 'dateCreate', header: 'Дата', class: 'width-200', format: row => FromUTCDateToShortDateString(row.dateCreate) }),
    new Config({ columnDef: 'currentStageNameRu', header: 'Этап', class: 'width-200' }),
    new Config({ columnDef: 'description', header: 'Описание', class: 'width-200' })
  ];
  formGroup: FormGroup;
  filteredPostAddresses: any;
  selectedAddresseeAddress: string;
  dicReceiveTypes: SelectOption[];
  dicPDTypes: SelectOption[];
  dicRequestStatuses: SelectOption[];
  dicDepartments: DicDepartment[];
  filteredDepartments: DicDepartment[];
  filteredObservableDepartments: Observable<DicDepartment[]>;
  dicDivisions: SelectOption[];
  editMode = false;
  editableControlsCreate: string[];
  editableControlsDefault: string[];
  editableControlsStageInitial: string[];
  editableControlsStageFormationAppData: string[];
  availableAtStage: any;
  selectedSubject: SubjectDto;
  isDisabled = false;
  unnecessaryDepartmentIds: number[] = [735, 736, 737, 739, 740, 741]; // TODO нужно удалить из базы
  divisionRgpNiisCode = '000001'; // РГП "НИИС"
  departmentCode_OLD_D_1_2 = 'OLD_D_1_2'; // Управление регистрации заявок и учета оплат
  departmentCode_D_3_1 = 'D_3_1'; // Управление предварительной экспертизы заявок на товарные знаки
  departmentCode_D_3_4 = 'D_3_4';//Управление экспертизы Промышленных образцов
  departmentCode_D_2_1 = 'D_2_1';//Управление формальной экспертизы заявок на изобретения и селекционные достижения
  departmentCode_D_3_3 = 'D_3_3';//Управление экспертизы международных товарных знаков
  protectionDocTypeContract = 'DK'; // Договор коммерциализации
  protectionDocTypeCode;

  get stagesinitial() {
    return RouteStageCodes.stagesinitial;
  }
  get stagesFormationAppData() {
    return RouteStageCodes.stagesFormationAppData;
  }

  @Input() requestDetails: RequestDetails;
  @Input() selectOptions: SelectOption[];
  @Input() disabled: boolean;
  @Output() submitData: EventEmitter<any> = new EventEmitter();
  @Output() edit: EventEmitter<boolean> = new EventEmitter();
  @Output() delete: EventEmitter<number> = new EventEmitter();
  @ViewChild(SubjectsSearchFormComponent)
  subjectSearch: SubjectsSearchFormComponent;

  private onDestroy = new Subject();

  constructor(
    private fb: FormBuilder,
    private dialog: MatDialog,
    private subjectsService: SubjectsService,
    private configService: ConfigService,
    private postKzService: PostKzService,
    private customerService: CustomerService,
    private workflowService: WorkflowService,
    private workflowBusinessService: WorkflowBusinessService,
    private snackbarHelper: SnackBarHelper,
    private dictionaryService: DictionaryService,
    private requestService: RequestService,
    private changeDetector: ChangeDetectorRef,
    private journalService: JournalService
  ) {
    this.buildForm();
  }

  ngOnInit() {
    Observable.combineLatest(
      this.dictionaryService.getBaseDictionary(DictionaryType.DicDepartment),
      this.dictionaryService.getSelectOptions(DictionaryType.DicRequestStatus)
    )
      .takeUntil(this.onDestroy)
      .subscribe(
        ([departments, requestStatuses]: [DicDepartment[], SelectOption[]]) => {
          this.dicDepartments = departments.filter(
            d => !this.unnecessaryDepartmentIds.includes(d.id)
          );
          this.filteredDepartments = departments;
          this.dicRequestStatuses = requestStatuses;
          this.formGroup
            .get('customerSearch')
            .setValue(this.requestDetails.addresseeInfo);
          this.formGroup.get('customerSearch').markAsPristine();

          if (!this.requestDetails.id) {
            const defaultDivisionId = this.dicDivisions.find(
              d => d.code === this.divisionRgpNiisCode
            ).id;
            this.formGroup.get('divisionId').setValue(defaultDivisionId);
            this.requestDetails.divisionId = defaultDivisionId;
            this.onDivisionChange(defaultDivisionId);

            let defaultDepartmentId = this.dicDepartments.find(
              d => d.code === this.departmentCode_OLD_D_1_2
            ).id;
            this.protectionDocTypeCode = this.dicPDTypes.find(
              pdt => pdt.id === this.requestDetails.protectionDocTypeId
            );
            if (this.protectionDocTypeCode.code === 'TM') {
              defaultDepartmentId = this.dicDepartments.find(
                d => d.code === this.departmentCode_D_3_1
              ).id;
            }
            else if (this.protectionDocTypeCode.code === 'S2') {
              defaultDepartmentId = this.dicDepartments.find(
                d => d.code === this.departmentCode_D_3_4
              ).id;
              this.formGroup.get('copyCount').setValue(1);
              this.requestDetails.copyCount = 1;
            }
            else if (this.protectionDocTypeCode.code === 'SA') {
              defaultDepartmentId = this.dicDepartments.find(
                d => d.code === this.departmentCode_D_2_1
              ).id;
              this.formGroup.get('copyCount').setValue(1);
              this.requestDetails.copyCount = 1;
            }
            else if (this.protectionDocTypeCode.code === 'TMI') {
              defaultDepartmentId = this.dicDepartments.find(
                d => d.code === this.departmentCode_D_3_3
              ).id;
              this.formGroup.get('copyCount').setValue(1);
              this.requestDetails.copyCount = 1;
            }
            this.formGroup.get('departmentId').setValue(defaultDepartmentId);
            this.requestDetails.departmentId = defaultDepartmentId;
          }
        }
      );
  }

  isMtzType() {
    return this.dicPDTypes.filter(d => d.id === this.requestDetails.protectionDocTypeId)[0].code === 'ITM';
  }

  ngOnDestroy(): void {
    this.formGroup.reset();
    this.toggleEditMode(false);
    this.onDestroy.next();
  }

  ngAfterViewInit(): void {
    this.changeDetector.detectChanges();

    if (this.requestDetails.protectionDocTypeId === 10) { // Проверка на МТЗ
      this.formGroup.get('customerSearch').clearValidators();
    }
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes.requestDetails && changes.requestDetails.currentValue) {
      this.formGroup.reset(this.requestDetails);
      this.formGroup
        .get('customerSearch')
        .setValue(this.requestDetails.addresseeInfo);
      this.formGroup.get('customerSearch').markAsPristine();
      this.selectedAddresseeAddress = this.requestDetails.addresseeAddress;
      this.availableAtStage = this.workflowBusinessService.availableAtStage(
        this.requestDetails,
        RequestPart.Request
      );
      const newRequest = this.requestDetails.id === 0;
      this.toggleEditMode(newRequest);
      if (!newRequest) {
        this.refreshWorkflows();
      }
    }

    if (changes.selectOptions && changes.selectOptions.currentValue) {
      this.initSelectOptions();
    }
    this.changeDetector.detectChanges();
  }

  onUndo() {
    if (this.formGroup.dirty) {
      this.openDialog();
      return false;
    } else {
      this.formGroup
        .get('customerSearch')
        .setValue(this.requestDetails.addresseeInfo);
      this.formGroup.reset(this.requestDetails);
      this.toggleEditMode(false);
    }
  }

  onEdit() {
    this.toggleEditMode(true);
  }
  getNumbersForRequest() {
    this.requestService
      .getRequestNumbers(this.requestDetails)
      .takeUntil(this.onDestroy)
      .subscribe(numbers => {
        this.requestDetails.barcode = numbers.barcode;
        this.requestDetails.dateCreate = numbers.dateCreate;
        this.formGroup.patchValue(numbers);

        this.getDataToSubmit();
      });
  }
  getDataToSubmit() {
    if (this.formGroup.invalid) {
      return;
    }

    this.formGroup.markAsPristine();

    const value = this.formGroup.getRawValue();
    const department = this.formGroup.get('departmentId').value;
    if (department) {
      value.departmentId = department.id;
    }
    this.applyAddresseeInfo(value);
    this.submitData.emit(value);
  }
  onSubmit() {
    if (!this.requestDetails.barcode) {
      this.getNumbersForRequest();
    } else {
      this.getDataToSubmit();
    }
  }

  onDivisionChange(divisionId: number) {
    this.filteredDepartments = this.dicDepartments
      .filter(st => st.divisionId === divisionId)
      .sort((c1, c2) => {
        if (c1.nameRu < c2.nameRu) {
          return -1;
        }
        if (c1.nameRu > c2.nameRu) {
          return 1;
        }
        return 0;
      });
    this.formGroup.get('departmentId').setValue('');
    this.formGroup.get('departmentId').enable();
    this.formGroup.get('departmentId').setErrors({ required: true });
    this.formGroup.get('departmentId').markAsDirty();
  }

  onDelete() {
    this.openDialogDelete();
  }

  refreshWorkflows() {
    this.workflowService
      .get(this.requestDetails.id, OwnerType.Request)
      .takeUntil(this.onDestroy)
      .subscribe(
        workflows =>
          (this.workflows = workflows.sort((w1, w2) => w2.id - w1.id))
      );
  }

  private applyAddresseeInfo(value: any) {
    const addresseeInfo = this.subjectSearch.value;
    value.addresseeId = addresseeInfo.addresseeId;
    value.addresseeNameRu = addresseeInfo.addresseeNameRu;
    value.addresseeXin = addresseeInfo.addresseeXin;
    value.addresseeAddress = addresseeInfo.addresseeAddress;
    value.addresseeShortAddress = addresseeInfo.addresseeShortAddress;
    value.apartment = addresseeInfo.apartment;
    value.republic = addresseeInfo.republic;
    value.oblast = addresseeInfo.oblast;
    value.region = addresseeInfo.region;
    value.city = addresseeInfo.city;
    value.street = addresseeInfo.street;
    value.contactInfos = addresseeInfo.contactInfos;

    delete value.dateCreate;
  }

  private buildForm() {
    this.formGroup = this.fb.group({
      customerSearch: [{ value: '' }, Validators.required],
      protectionDocTypeId: [{ value: '', disabled: true }, Validators.required],
      receiveTypeId: [{ value: '', disabled: true }, Validators.required],
      statusId: [''],
      divisionId: [{ value: '', disabled: true }, Validators.required],
      departmentId: [{ value: '', disabled: true }],
      incomingNumber: [{ value: '', disabled: true }],
      barcode: [{ value: '', disabled: true }],
      pageCount: [{ value: '', disabled: true }],
      copyCount: [{ value: '', disabled: true }],
      wasScanned: [{ value: '', disabled: true }],
      nameRu: [{ value: '', disabled: true }],
      nameKz: [{ value: '', disabled: true }],
      nameEn: [{ value: '', disabled: true }],
      description: [{ value: '', disabled: true }],
      dateCreate: [{ value: '', disabled: true }],
      outgoingNumber: [{ value: '', disabled: true }],
      outgoingDate: [{ value: '', disabled: true }],
      registerDateProtectionDoc: [{ value: '', disabled: true }],
      expectedValidDateProtectionDoc: [{ value: '', disabled: true }],
      outgoingNumberFilial: [{ value: '', disabled: true }]
    });

    this.editableControlsCreate = [
      'protectionDocTypeId',
      'receiveTypeId',
      'nameRu',
      'nameKz',
      'nameEn',
      'outgoingNumber',
      'outgoingDate',
      'registerDateProtectionDoc',
      'expectedValidDateProtectionDoc',
      'description',
      'pageCount',
      'copyCount'
    ];
    this.editableControlsDefault = [
      'receiveTypeId',
      'nameRu',
      'nameKz',
      'nameEn',
      'pageCount',
      'copyCount'
    ];
    this.editableControlsStageInitial = [
      'protectionDocTypeId',
      'receiveTypeId',
      'nameRu',
      'nameKz',
      'nameEn',
      'customerSearch',
      'copyCount',
      'outgoingNumber',
      'outgoingDate',
      'registerDateProtectionDoc',
      'expectedValidDateProtectionDoc',
      'description',
      'pageCount'
    ];
    this.editableControlsStageFormationAppData = [
      'receiveTypeId',
      'nameRu',
      'nameKz',
      'nameEn',
      'description',
      'pageCount'
    ];
  }

  private openDialog() {
    const dialogRef = this.dialog.open(UndoDialogComponent);

    dialogRef
      .afterClosed()
      .takeUntil(this.onDestroy)
      .subscribe(result => {
        if (result === 'true') {
          this.formGroup.reset(this.requestDetails);
          this.formGroup
            .get('customerSearch')
            .setValue(this.requestDetails.addresseeInfo);
          this.formGroup.get('customerSearch').markAsPristine();
          this.toggleEditMode(false);
        }
      });
  }

  private toggleEditMode(value: boolean) {
    this.editMode = value;
    this.edit.emit(value);

    value && this.formGroup.controls.divisionId !== null
      ? this.formGroup.get('departmentId').enable()
      : this.formGroup.get('departmentId').disable();

    this.filteredPostAddresses = [];
    this.selectedAddresseeAddress = this.requestDetails.addresseeAddress;

    let currentStageCode = '';
    if (this.requestDetails.currentWorkflow) {
      currentStageCode = this.requestDetails.currentWorkflow.currentStageCode;
    }
    // switch (true) {
    //   case !this.requestDetails.id: {
    //     // этап создания
    //     this.editableControlsCreate.forEach(c => {
    //       value
    //         ? this.formGroup.controls[c].enable()
    //         : this.formGroup.controls[c].disable();
    //     });
    //     break;
    //   }
    //   case this.stagesinitial.includes(currentStageCode): {
    //     this.editableControlsStageInitial.forEach(c => {
    //       value
    //         ? this.formGroup.controls[c].enable()
    //         : this.formGroup.controls[c].disable();
    //     });
    //     break;
    //   }
    //   case this.stagesFormationAppData.includes(currentStageCode): {
    //     this.editableControlsStageFormationAppData.forEach(c => {
    //       value
    //         ? this.formGroup.controls[c].enable()
    //         : this.formGroup.controls[c].disable();
    //     });
    //     break;
    //   }
    //   default: {
    //     this.editableControlsDefault.forEach(c => {
    //       value
    //         ? this.formGroup.controls[c].enable()
    //         : this.formGroup.controls[c].disable();
    //     });
    //     break;
    //   }
    // }
    const keys = [
      // 'protectionDocTypeId',
      // 'receiveTypeId',
      // 'divisionId',
      // 'departmentId',
      // 'incomingNumber',
      // 'barcode',
      // 'pageCount',
      // 'copyCount',
      // 'wasScanned',
      // 'nameRu',
      // 'nameKz',
      // 'nameEn',
      // 'description',
      // 'dateCreate',
      // 'outgoingNumber',
      // 'outgoingDate',
      // 'outgoingNumberFilial'
      'protectionDocTypeId',
      'receiveTypeId',
      'nameRu',
      'nameKz',
      'nameEn',
      'outgoingNumber',
      'outgoingDate',
      'registerDateProtectionDoc',
      'expectedValidDateProtectionDoc',
      'description',
      'pageCount',
      'copyCount',
      'customerSearch',
      'divisionId',
      'outgoingNumberFilial',
      'dateCreate'
    ];
    for (let key of keys) {
      if (value) {
        this.formGroup.controls[key].enable();
      } else {
        this.formGroup.controls[key].disable();
      }
    }

    this.formGroup.markAsPristine();
  }

  private openDialogDelete() {
    const dialogRef = this.dialog.open(DeleteDialogComponent);

    dialogRef
      .afterClosed()
      .takeUntil(this.onDestroy)
      .subscribe(result => {
        if (result === 'true') {
          this.delete.emit(this.requestDetails.id);
        }
      });
  }

  private initSelectOptions() {
    this.dicReceiveTypes = this.selectOptions.filter(
      so => so.dicType === DictionaryType.DicReceiveType
    );
    this.dicPDTypes = this.selectOptions.filter(
      so =>
        so.dicType === DictionaryType.DicProtectionDocType &&
        !so.code.endsWith('_PD')
    );
    this.dicDivisions = this.selectOptions.filter(
      so => so.dicType === DictionaryType.DicDivision
    );
  }

  isStageFormationAppData() {
    // if (this.requestDetails.currentWorkflow) {
    //   return this.stagesFormationAppData.includes(
    //     this.requestDetails.currentWorkflow.currentStageCode
    //   );
    // } else {
    //   return false;
    // }
    return false;
  }
  isStageInitial() {
    if (this.requestDetails.currentWorkflow) {
      return this.stagesinitial.includes(
        this.requestDetails.currentWorkflow.currentStageCode
      );
    } else {
      return false;
    }
  }

  doNamesHaveValue() {
    if (!this.formGroup) {
      return false;
    }
    const typeId = this.formGroup.get('protectionDocTypeId').value;
    const type = this.dicPDTypes.find(d => d.id === typeId);
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
  isDisabledButtonSave() {
    // return (
    //   this.formGroup.invalid ||
    //   this.formGroup.pristine ||
    //   (this.formGroup.dirty &&
    //     !this.formGroup.controls.departmentId.value.id) ||
    //   !this.doNamesHaveValue() ||
    //   !this.formGroup.controls.customerSearch.value.addresseeId
    // );
    return false;
  }

  onShowAddressClick() {
    this.requestService
      .generatePrintAddressee(this.requestDetails.id, OwnerType.Request)
      .takeUntil(this.onDestroy)
      .subscribe(blob => {
        window.open(window.URL.createObjectURL(blob, { oneTimeOnly: true }));
      });
  }

  onShowBarcodeClick() {
    this.dialog.open(BarcodeDialogComponent, {
      data: {
        barcode: this.requestDetails.barcode
      },
      width: '400px'
    });
  }

  getTitle(code: string) {
    switch (code) {
      case 'B':
        return 'Название изобретения';
      case 'U':
        return 'Название полезной модели';
      case 'TM':
        return 'Название товарного знака';
      case 'S2':
        return 'Название промышленного образца';
      case 'SA':
        return 'Название селекционного достижения';
      case 'PN':
        return 'Название наименования мест происхождения товаров';
    }
  }

  onPreviewClick() {
    this.journalService
      .getAttachment(OwnerType.Request, this.requestDetails.id)
      .takeUntil(this.onDestroy)
      .subscribe(blob => {
        window.open(window.URL.createObjectURL(blob, { oneTimeOnly: true }));
      });
  }
}
