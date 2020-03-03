import { Location } from '@angular/common';
import { Component, EventEmitter, Input, OnDestroy, OnInit, Output, SimpleChanges, ViewChild } from '@angular/core';
import { OnChanges } from '@angular/core/src/metadata/lifecycle_hooks';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog, MatSort } from '@angular/material';
import { ContractPart, WorkflowBusinessService } from 'app/contracts/services/workflow-business.service';
import { RequestItemDto } from 'app/requests/models/request-item';
import { OwnerType } from 'app/shared/services/models/owner-type.enum';
import { Observable } from 'rxjs/Observable';
import { Subject } from 'rxjs/Subject';

import { User } from '../../../../../administration/components/users/models/user.model';
import { UsersService } from '../../../../../administration/users.service';
import { RequestService } from '../../../../../requests/request.service';
import { DeleteDialogComponent } from '../../../../../shared/components/delete-dialog/delete-dialog.component';
import { Config } from '../../../../../shared/components/table/config.model';
import { UndoDialogComponent } from '../../../../../shared/components/undo-dialog/undo-dialog.component';
import { DictionaryService } from '../../../../../shared/services/dictionary.service';
import { DicProtectionDocSubType, DicDepartment, DicContractType } from '../../../../../shared/services/models/base-dictionary';
import { DictionaryType } from '../../../../../shared/services/models/dictionary-type.enum';
import { SelectOption } from '../../../../../shared/services/models/select-option';
import { WorkflowService } from '../../../../../shared/services/workflow.service';
import { ContractService } from '../../../../contract.service';
import { ContractDetails, RequestRelationDto } from '../../../../models/contract-details';
import { Workflow } from '../../../../../shared/services/models/workflow-model';
import { positiveNumberValidator } from '../../.././../../shared/services/validator/custom-validators';
import { MaterialDetail } from '../../../../../materials/models/materials.model';
import { BarcodeDialogComponent } from '../../../../../shared/components/barcode-dialog/barcode-dialog.component';

@Component({
  selector: 'app-contract-description',
  templateUrl: './contract-description.component.html',
  styleUrls: ['./contract-description.component.scss']
})
export class ContractDescriptionComponent implements OnInit, OnChanges, OnDestroy {
  workflows: Workflow[];
  columns: Config[] = [
    new Config({ columnDef: 'routeNameRu', header: 'Маршрут', class: 'width-100' }),
    new Config({ columnDef: 'currentUserNameRu', header: 'Пользователь', class: 'width-100' }),
    new Config({ columnDef: 'dateCreate', header: 'Дата', class: 'width-100' }),
    new Config({ columnDef: 'currentStageNameRu', header: 'Этап', class: 'width-100' }),
    new Config({ columnDef: 'description', header: 'Описание', class: 'width-200' }),
  ];
  users: User[];
  formGroup: FormGroup;
  onDestroy = new Subject();
  editableControls: { field: string, stageCodes: string[] }[];
  onCreateEditableControls: string[];
  availableAtStage: any;
  editMode = false;

  requests: RequestItemDto[] = [];
  dicReceiveTypes: SelectOption[];
  dicContractStatuses: SelectOption[];
  dicProtectionDocType: SelectOption;
  dicProtectionDocTypes: SelectOption[];
  dicContractType: DicContractType[];
  selectedRequests: RequestItemDto[] = [];
  filteredRequests: Observable<RequestItemDto[]>;
  dicDepartments: DicDepartment[];
  filteredDepartments: DicDepartment[];
  dicDivisions: SelectOption[];

  divisionRgpNiisCode = '000001'; // РГП "НИИС"
  departmentContractExpertiseCode = 'D_4_2'; // Отдел экспертизы договоров
  requestOwnerType = OwnerType.Request;
  protectionDocOwnerType = OwnerType.ProtectionDoc;
  selectedProtectionDocTypeId: number;

  @Input() contractDetails: ContractDetails;
  @Input() selectOptions: SelectOption[];
  @Output() submitData: EventEmitter<any> = new EventEmitter();
  @Output() register: EventEmitter<any> = new EventEmitter();
  @Output() edit: EventEmitter<boolean> = new EventEmitter();
  @Output() delete: EventEmitter<number> = new EventEmitter();

  get pdTypeTMCodes() { return this.workflowBusinessService.pdTypeTMCodes; }
  get requestRelations() { return this.formGroup.get('requestRelations').value as RequestRelationDto[]; }
  get registerAvailable(): boolean {
    return !this.contractDetails.gosNumber && this.workflowBusinessService
      && this.workflowBusinessService.isFormalExamCompleteStage(this.contractDetails);
  }
  get isPublication(): boolean {
    if (this.contractDetails.id > 0 && this.workflowBusinessService && this.contractDetails.currentWorkflow) {
      return this.workflowBusinessService.isPublicationStage(this.contractDetails.currentWorkflow.currentStageCode);
    }
    return false;
  }

  constructor(
    private fb: FormBuilder,
    private dialog: MatDialog,
    private dictionaryService: DictionaryService,
    private workflowBusinessService: WorkflowBusinessService,
    private workflowService: WorkflowService,
    private usersService: UsersService,
    private requestService: RequestService,
    private contractService: ContractService,
    private location: Location
  ) {
    this.buildForm();
    this.initRequestRelationControl();
  }

  ngOnInit() {
    this.usersService.get()
      .takeUntil(this.onDestroy)
      .subscribe(users => this.users = users);
    this.formGroup.get('customerSearch').setValue(this.contractDetails.addresseeInfo);
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes.contractDetails && changes.contractDetails.currentValue) {
      this.formGroup.reset(this.contractDetails);
      this.initRequestsSelector();
      this.availableAtStage = this.workflowBusinessService.availableAtStage(this.contractDetails, ContractPart.Contract);
      const newContract = this.contractDetails.id === 0;
      this.formGroup.get('customerSearch').setValue(this.contractDetails.addresseeInfo);
      this.toggleEditMode(newContract);
      if (!newContract) {
        this.refreshWorkflows();
      }
    }

    if (changes.selectOptions && changes.selectOptions.currentValue) {
      this.initDictionaries();
    }
  }

  ngOnDestroy(): void {
    this.formGroup.reset();
    this.toggleEditMode(false);
    this.onDestroy.next();
  }

  onUndo() {
    if (this.formGroup.dirty) {
      this.openDialog();
      return;
    }

    if (!this.contractDetails.id) {
      this.location.back();
      return;
    }

    this.formGroup.reset(this.contractDetails);
    this.formGroup.get('customerSearch').setValue(this.contractDetails.addresseeInfo);
    this.getProtectionDocTypeCode();
    this.selectedProtectionDocTypeId = null;
    this.toggleEditMode(false);
  }

  onEdit() {
    this.toggleEditMode(true);
  }

  onSubmit() {
    if (this.formGroup.invalid) { return; }
    this.formGroup.markAsPristine();
    const value = this.formGroup.getRawValue();
    this.preapreSubmitValue(value);
    value.protectionDocTypeId = this.dicProtectionDocTypes.filter(d => d.code === 'DK')[0].id;
    this.submitData.emit(value);
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

    if (this.contractDetails.id) {
      this.formGroup.get('departmentId').setValue('');
    }
    this.formGroup.get('departmentId').enable();
    this.formGroup.get('departmentId').setErrors({ 'required': true });
    this.formGroup.get('departmentId').markAsDirty();
  }

  onDelete() {
    this.openDialogDelete();
  }

  onRegister() {
    if (this.formGroup.invalid) { return; }
    this.formGroup.markAsPristine();
    const value = this.formGroup.getRawValue();
    this.preapreSubmitValue(value);
    this.register.emit(value);
  }

  onShowBarcodeClick() {
    this.dialog.open(BarcodeDialogComponent, {
      data: {
        barcode: this.contractDetails.barcode
      },
      width: '400px'
    });
  }

  exists(): boolean {
    return !!this.contractDetails.id;
  }

  onFilterRequestsAndPD(event: any) {
    this.formGroup.get('relationProtectionDocTypeCode').setValue(event.value);
    this.initRequestsSelector();
    this.requestRelations.splice(0, this.requestRelations.length);
  }

  onRemoveRequestRelation(request: any): void {
    const index = this.requestRelations.indexOf(request);

    if (index >= 0) {
      this.requestRelations.splice(index, 1);
      this.validateRequireOfRequestRelations();
      this.formGroup.markAsDirty();
    }
  }

  isRequestAlreadySelected(id: number): boolean {
    if (id) {
      return this.requestRelations.map(i => i.request.id).includes(id);
    }
    return false;
  }

  doNamesHaveValue(): boolean {
    if (!this.formGroup) {
      return false;
    }
    return this.formGroup.controls.nameRu.value
      || this.formGroup.controls.nameKz.value
      || this.formGroup.controls.nameEn.value;
  }

  isDisabledSaveButton() {
    return this.formGroup.invalid || (this.formGroup.dirty && !this.formGroup.controls.departmentId.value.id)
      || !this.doNamesHaveValue();
  }

  private buildForm() {
    this.formGroup = this.fb.group({
      barcode: [{ value: '', disabled: true }],
      receiveTypeId: [{ value: '', disabled: true }],
      outgoingNumber: [{ value: '', disabled: true }],
      outgoingDate: [{ value: '', disabled: true }],
      contractNum: [{ value: '', disabled: true }],
      statusId: [{ value: '', disabled: true }],
      protectionDocTypeId: [{ value: '', disabled: true }],
      typeId: [{ value: '', disabled: true }, Validators.required],
      gosNumber: [{ value: '', disabled: true }],
      gosDate: [{ value: '', disabled: true }],
      numberBulletin: [{ value: '', disabled: true }],
      bulletinDate: [{ value: '', disabled: true }],
      requestRelations: [{ value: '', disabled: true }],
      relationProtectionDocTypeCode: [{ value: '', disabled: true }],
      requestSearchText: [{ value: '', disabled: true }],
      customerSearch: [{ value: '' }],
      addresseeId: [{ value: '', disabled: true }],
      pageCount: [{ value: '', disabled: true }, positiveNumberValidator],
      copyCount: [{ value: '', disabled: true }, positiveNumberValidator],
      divisionId: [{ value: '', disabled: true }, Validators.required],
      departmentId: [{ value: '', disabled: true }, Validators.required],
      owners: [{ value: '', disabled: true }],
      protectionDocsOwners: [{ value: '', disabled: true }],
      incomingNumber: [{ value: '', disabled: true }],
      incomingDate: [{ value: '', disabled: true }],
      applicationDateCreate: [{ value: '', disabled: true }],
      nameRu: [{ value: '', disabled: true }],
      nameKz: [{ value: '', disabled: true }],
      nameEn: [{ value: '', disabled: true }],
      description: [{ value: '', disabled: true }],
    });

    this.editableControls = [
      { field: 'typeId', stageCodes: ['DK01.1', 'DK02.1'] },
      { field: 'relationProtectionDocTypeCode', stageCodes: ['DK02.1'] },
      { field: 'requestSearchText', stageCodes: ['DK02.1'] },
      { field: 'requestRelations', stageCodes: ['DK02.1'] },
      { field: 'receiveTypeId', stageCodes: ['DK01.1'] },
      { field: 'outgoingNumber', stageCodes: ['DK01.1'] },
      { field: 'outgoingDate', stageCodes: ['DK01.1'] },
      { field: 'addresseeId', stageCodes: ['DK01.1'] },
      { field: 'pageCount', stageCodes: ['DK01.1', 'DK02.1'] },
      { field: 'copyCount', stageCodes: ['DK01.1', 'DK02.1'] },
      { field: 'divisionId', stageCodes: ['DK01.1'] },
      { field: 'gosDate', stageCodes: ['DK03.00', 'DK02.2'] },
      { field: 'applicationDateCreate', stageCodes: ['DK02.1'] },
      { field: 'numberBulletin', stageCodes: ['DK03.00', 'DK03.03', 'DK02.9.1', 'DK03.2', 'DK02.2'] },
      { field: 'bulletinDate', stageCodes: ['DK03.00', 'DK03.03', 'DK02.9.1', 'DK03.2', 'DK02.2'] },
      { field: 'nameRu', stageCodes: ['DK01.1'] },
      { field: 'nameKz', stageCodes: ['DK01.1'] },
      { field: 'nameEn', stageCodes: ['DK01.1'] },
      { field: 'description', stageCodes: ['DK01.1'] },
    ];
  }

  private validateRequireOfRequestRelations() {
    this.formGroup.get('requestRelations').setErrors(this.requestRelations.length > 0 ? null : { 'incorrect': true });
  }

  private initDictionaries() {
    this.dicReceiveTypes = this.selectOptions.filter(so => so.dicType === DictionaryType.DicReceiveType);
    this.dicContractStatuses = this.selectOptions.filter(so => so.dicType === DictionaryType.DicContractStatus);
    this.dicProtectionDocType = this.selectOptions.filter(so =>
      so.dicType === DictionaryType.DicProtectionDocType
      && so.code === this.contractDetails.protectionDocTypeCode)[0];
    this.dicProtectionDocTypes = this.selectOptions.filter(so => so.dicType === DictionaryType.DicProtectionDocType);
    this.dicDivisions = this.selectOptions.filter(so => so.dicType === DictionaryType.DicDivision);
    this.dictionaryService.getBaseDictionary(DictionaryType.DicContractType)
      .takeUntil(this.onDestroy)
      .subscribe((subTypes: DicContractType[]) => {
        this.dicContractType = subTypes;
      });
    this.dictionaryService.getBaseDictionary(DictionaryType.DicDepartment)
      .takeUntil(this.onDestroy)
      .subscribe((dicDepartments) => {
        this.dicDepartments = dicDepartments;
        this.filteredDepartments = this.dicDepartments;
        if (!this.contractDetails.id) {
          const defaultDivisionId = this.dicDivisions.find(d => d.code === this.divisionRgpNiisCode).id;
          this.formGroup.get('divisionId').setValue(defaultDivisionId);
          const defaultDepartamentId = this.dicDepartments.find(d => d.code === this.departmentContractExpertiseCode).id;
          this.formGroup.get('departmentId').setValue(defaultDepartamentId);
          this.onDivisionChange(defaultDivisionId);
        }
      });
    this.formGroup.controls.protectionDocTypeId.setValue(this.dicProtectionDocType ? this.dicProtectionDocType.id : null);
  }

  private initRequestRelationControl() {
    this.filteredRequests = this.formGroup.get('requestSearchText').valueChanges
      .startWith('')
      .takeUntil(this.onDestroy)
      .map(value => value ? this.filterRequestList(value) : this.requests.slice());
  }

  private initRequestsSelector() {
    // const protectionDocTypeCode = this.getProtectionDocTypeCode();
    // this.requestService.getRequestsForSelector(protectionDocTypeCode)
    //   .takeUntil(this.onDestroy)
    //   .subscribe(result => {
    //     this.requests = result;
    //   });
  }

  private getProtectionDocTypeCode(): string {
    let protectionDocTypeCode: string = this.formGroup.get('relationProtectionDocTypeCode').value;
    if (!protectionDocTypeCode) {
      if (this.requestRelations.length > 0) {
        protectionDocTypeCode = this.requestRelations[0].request.protectionDocTypeCode;
        this.formGroup.get('relationProtectionDocTypeCode').setValue(protectionDocTypeCode);
      } else {
        protectionDocTypeCode = undefined;
      }
    }
    return protectionDocTypeCode;
  }

  private refreshWorkflows() {
    this.workflowService
      .get(this.contractDetails.id, OwnerType.Contract)
      .takeUntil(this.onDestroy)
      .subscribe(workflows => this.workflows =
        workflows.sort((w1, w2) => w2.id - w1.id));
  }

  private filterRequestList(text: string) {
    return this.requests.filter(user =>
      (user.requestNum != null
        && user.requestNum.toLowerCase().indexOf(text.toString().toLowerCase()) >= 0)
      || (user.protectionDocTypeName != null
        && user.protectionDocTypeName.toLowerCase().indexOf(text.toString().toLowerCase()) >= 0));
  }

  private openDialogDelete() {
    const dialogRef = this.dialog.open(DeleteDialogComponent);

    dialogRef.afterClosed()
      .takeUntil(this.onDestroy)
      .subscribe(result => {
        if (result === 'true') {
          this.delete.emit(this.contractDetails.id);
        }
      });
  }

  private toggleEditMode(value: boolean) {
    this.editMode = value;
    this.edit.emit(value);

    this.editableControls.forEach(c => {
      value && (c.stageCodes.length === 0 || c.stageCodes
        .includes(this.contractService.getCurrentStageCode(this.contractDetails)))
        ? this.formGroup.controls[c.field].enable()
        : this.formGroup.controls[c.field].disable();

      this.formGroup.controls[c.field].markAsTouched();
    });
  }

  private openDialog() {
    const dialogRef = this.dialog.open(UndoDialogComponent);

    dialogRef.afterClosed()
      .takeUntil(this.onDestroy)
      .subscribe(result => {
        if (result === 'true') {
          if (!this.contractDetails.id) {
            this.location.back();
            return;
          }

          this.toggleEditMode(false);
          this.formGroup.get('customerSearch').setValue(this.contractDetails.addresseeInfo);
          this.formGroup.get('customerSearch').markAsPristine();
          this.formGroup.reset(this.contractDetails);
          this.getProtectionDocTypeCode();
        }
      });
  }

  onSelectProtectionDocTypeId(pdTypeId: number) {
    this.selectedProtectionDocTypeId = pdTypeId;
  }

  private preapreSubmitValue(value: any) {
    const addresseeInfo = this.formGroup.get('customerSearch').value;
    value.addresseeId = addresseeInfo.addresseeId;
    value.addresseeNameRu = addresseeInfo.addresseeNameRu;
    value.addresseeXin = addresseeInfo.addresseeXin;
    value.addresseeAddress = addresseeInfo.addresseeAddress;
    value.apartment = addresseeInfo.apartment;
    value.owners = this.formGroup.get('owners').value.owners;
    value.protectionDocsOwners = this.formGroup.get('protectionDocsOwners').value.owners;
    const department = this.formGroup.get('departmentId').value;
    if (department) {
      value.departmentId = department.id;
    }
    if (!value.protectionDocTypeId) {
      value.protectionDocTypeId = this.dicProtectionDocTypes.filter(d => d.code === 'DK')[0].id;
    }
  }
  isDisabledGenerateContractNum(): boolean {
    if (!this.formGroup) {
      return true;
    }
    const contractNum = this.formGroup.controls.contractNum.value;
    if (contractNum) {
      return true;
    }
    return this.editMode || !this.isStageRegistrationAppData();
  }
  setContractNum(): void {
    this.contractService.generateContractNum(this.contractDetails.id)
      .subscribe((data: any) => {
        if (data) {
          this.formGroup.controls.contractNum.setValue(data.contractNum);
          this.contractDetails.contractNum = data.contractNum;
        }
      });
  }
  isStageFormationAppData(): boolean {
    if (!!this.contractDetails && this.contractDetails.currentWorkflow) {
      return this.workflowBusinessService.isFormationStage(this.contractDetails.currentWorkflow.currentStageCode);
    } else {
      return false;
    }
  }

  isStageRegistrationAppData(): boolean {
    if (!!this.contractDetails && this.contractDetails.currentWorkflow) {
      return this.workflowBusinessService.isRegistrationStage(this.contractDetails.currentWorkflow.currentStageCode);
    } else {
      return false;
    }
  }
}
