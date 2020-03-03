import { Component, OnInit, Input, Output, EventEmitter, SimpleChanges, OnChanges, OnDestroy } from '@angular/core';
import {
  ContractDetails,
  RequestRelationDto,
  ContractRequestICGSRequestRelationDto,
  ProtectionDocItemDto,
  ProtectionDocRelationDto
} from '../../../../models/contract-details';
import { SelectOption, SimpleSelectOption } from '../../../../../shared/services/models/select-option';
import { FormGroup, FormBuilder, Validators, FormArray, AbstractControl } from '@angular/forms';
import { Subject } from 'rxjs/Subject';
import { ICGSRequestsShortInfo } from '../../../../../shared/services/models/ICGSRequest-short-info';
import { MatDialog } from '@angular/material';
import { WorkflowBusinessService, ContractPart } from '../../../../services/workflow-business.service';
import { RequestService } from '../../../../../requests/request.service';
import { DictionaryType } from '../../../../../shared/services/models/dictionary-type.enum';
import { UndoDialogComponent } from '../../../../../shared/components/undo-dialog/undo-dialog.component';
import { RequestItemDto } from 'app/requests/models/request-item';
import { FormControl } from '@angular/forms/src/model';
import { ContractService } from 'app/contracts/contract.service';
import { ProtectionDocsComponent } from '../../../../../protection-docs/containers/protection-docs/protection-docs.component';

@Component({
  selector: 'app-contract-subject-description',
  templateUrl: './contract-subject-description.component.html',
  styleUrls: ['./contract-subject-description.component.scss']
})
export class ContractSubjectDescriptionComponent implements OnInit, OnChanges, OnDestroy {

  @Input() contractDetails: ContractDetails;
  @Input() selectOptions: SelectOption[];
  @Output() edit: EventEmitter<boolean> = new EventEmitter();
  @Output() submitData: EventEmitter<any> = new EventEmitter();

  formGroup: FormGroup;
  editableControls: { field: string, stageCodes: string[] }[];
  availableAtStage: any;
  editMode = false;
  dicCustomerTypes: SelectOption[];
  dicContractCategories: SelectOption[];

  private onDestroy = new Subject();
  requests: RequestItemDto[];
  icgsRequests: ICGSRequestsShortInfo[] = [];
  protectionDocs: ProtectionDocItemDto[];
  icgsProtectionDocs: SelectOption[];

  get requestRelations() { return this.formGroup.get('requestRelations') as FormArray; }
  get protectionDocRelations() { return this.formGroup.get('protectionDocRelations') as FormArray; }
  get licenseContractType(): boolean { return ['11', '12', '13'].includes(this.contractDetails.typeCode); }
  get agreementContractType(): boolean { return ['20'].includes(this.contractDetails.typeCode); }
  get currentStageCode(): string { return this.contractService.getCurrentStageCode(this.contractDetails); }

  constructor(
    private fb: FormBuilder,
    private dialog: MatDialog,
    private workflowBusinessService: WorkflowBusinessService,
    private requestService: RequestService,
    private contractService: ContractService
  ) {
    this.buildForm();
  }

  ngOnInit() {
  }


  ngOnChanges(changes: SimpleChanges): void {
    if (changes.contractDetails && changes.contractDetails.currentValue) {
      this.initializeIcgsRequests();
      this.formGroup.reset(this.contractDetails);
      this.availableAtStage = this.workflowBusinessService.availableAtStage(this.contractDetails, ContractPart.Contract);
      this.setRequestRelations(this.contractDetails.requestRelations);
      this.setProtectionDocRelations(this.contractDetails.protectionDocRelations);
      const newContract = this.contractDetails.id === 0;
      this.toggleEditMode(newContract);
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
      return false;
    } else {
      this.formGroup.reset(this.contractDetails);
      this.setRequestRelations(this.contractDetails.requestRelations);
      this.toggleEditMode(false);
    }
  }

  onSubmit() {
    if (this.formGroup.invalid) { return; }
    this.formGroup.markAsPristine();
    const value = this.formGroup.getRawValue();
    this.submitData.emit(value);
  }

  onEdit(value: boolean) {
    this.toggleEditMode(true);
  }

  private buildForm() {
    this.formGroup = this.fb.group({
      categoryId: [{ value: '', disabled: true }],
      registrationPlace: [{ value: '', disabled: true }],
      validDate: [{ value: '', disabled: false }],
      changes: [{ value: '', disabled: false }],
      terminateDate: [{ value: '', disabled: false }],
      requestRelations: this.fb.array([]),
      protectionDocId: [{ value: '', disabled: false }],
    });

    this.editableControls = [
      { field: 'categoryId', stageCodes: ['DK02.1'] },
      { field: 'registrationPlace', stageCodes: ['DK02.1'] },
      { field: 'validDate', stageCodes: ['DK02.1'] },
      { field: 'changes', stageCodes: ['DK02.1'] },
      { field: 'terminateDate', stageCodes: ['DK02.9.1'] },
      { field: 'protectionDocId', stageCodes: ['DK02.1'] }
    ];
  }
  private initializeIcgsRequests() {
    for (const requestRelation of this.contractDetails.requestRelations) {
      for (const icgsRequest of requestRelation.icgsRequestItemDtos) {
        const indexIcgs = this.icgsRequests.findIndex(i => i.id === icgsRequest.id);
        if (indexIcgs === -1) {
          this.icgsRequests.push(icgsRequest);
        }
      }
    }
  }
  private initDictionaries() {
    this.dicContractCategories = this.selectOptions.filter(so => so.dicType === DictionaryType.DicContractCategory && so.nameRu);
  }

  private toggleEditMode(value: boolean) {
    this.editMode = value;
    this.edit.emit(value);

    this.editableControls.forEach(c => {
      value && (c.stageCodes.length === 0 || c.stageCodes
        .includes(this.contractService.getCurrentStageCode(this.contractDetails)))
        ? this.formGroup.controls[c.field].enable()
        : this.formGroup.controls[c.field].disable();
    });
  }

  private openDialog() {
    const dialogRef = this.dialog.open(UndoDialogComponent);

    dialogRef.afterClosed()
      .takeUntil(this.onDestroy)
      .subscribe(result => {
        if (result === 'true') {
          this.toggleEditMode(false);
          this.formGroup.reset(this.contractDetails);
          this.setRequestRelations(this.contractDetails.requestRelations);
        }
      });
  }

  // Для RequestRelations НАЧАЛО
  getFilteredICGSRequests(index: number): ICGSRequestsShortInfo[] {
    if (index || index === 0) {
      const requestRelation = this.requestRelations.controls[index];
      const requestId = requestRelation.get('request').value.id;
      const result = this.icgsRequests
        .filter(ir => ir.requestId === requestId);
      return result;
    }
  }

  getFilteredICGSDescription(requestRelationIndex: number, icgsRequestRelationIndex: number): string[] {
    if (requestRelationIndex || requestRelationIndex === 0) {
      const icgsRelation = this.requestRelations.controls[requestRelationIndex].get('icgsRequestRelations') as FormArray;
      const icgsRequestId = icgsRelation.controls[icgsRequestRelationIndex].get('icgsRequestId').value;
      const icgsRequest = this.icgsRequests
        .filter(ir => ir.id === icgsRequestId)[0];
      if (icgsRequest && icgsRequest.description) {
        const result = icgsRequest.description.split(';');
        return result;
      }
      return [];
    }
  }

  isNeedToDisableAddICGS(requestRelationIndex: number): boolean {
    const icgsRequestRelations = this.requestRelations.controls[requestRelationIndex].get('icgsRequestRelations') as FormArray;
    const icgsRequestRelationsLength = icgsRequestRelations.length;
    return (icgsRequestRelationsLength >= this.getFilteredICGSRequests(requestRelationIndex).length);
  }

  isNeedToDisableIcgsRequestRelation(requestRelationIndex, icgsRequestRelationIndex): boolean {
    const icgsRequestRelations = this.requestRelations.controls[requestRelationIndex].get('icgsRequestRelations') as FormArray;
    const relationsLength: number = icgsRequestRelations.controls.length;
    if (icgsRequestRelationIndex < relationsLength - 1) {
      return true;
    }
    return false;
  }

  onIcgsRequestRelationDelete(requestRelationIndex, icgsRequestRelationIndex) {
    const icgsRequestRelations = this.requestRelations.controls[requestRelationIndex].get('icgsRequestRelations') as FormArray;
    icgsRequestRelations.removeAt(icgsRequestRelationIndex);
    this.formGroup.markAsDirty();
  }

  onIcgsRequestRelationAdd(requestRelationIndex, icgsRequestRelationIndex) {
    const requestRelation = this.requestRelations.controls[requestRelationIndex];
    const icgsFormGroup = this.fb.group({
      id: 0,
      contractRequestRelationId: [requestRelation.get('id').value],
      icgsRequestId: ['', Validators.required],
      descriptions: [[]]
    });
    const icgsRequestRelations = requestRelation.get('icgsRequestRelations') as FormArray;
    icgsRequestRelations.push(icgsFormGroup);
  }

  private initSelector() {
    this.requests = this.requestRelations.controls.map(c => c.value.request);
  }

  private initICGSRequests(requestRelationsFromDetails: RequestRelationDto[]) {
    this.requestService.getICGSRequestsByRequestIds(this.requests.map(r => r.id))
      .takeUntil(this.onDestroy)
      .subscribe(result => {
        this.icgsRequests = result;
      });
  }

  private setProtectionDocRelations(protectionDocs: ProtectionDocRelationDto[]) {
    if (protectionDocs && protectionDocs.length) {
        this.protectionDocs = protectionDocs.map(p => {
          let result = new ProtectionDocItemDto();
          result.id = p.protectionDoc.id,
          result.protectionDocDate = p.protectionDoc.protectionDocDate,
          result.protectionDocNum = p.protectionDoc.protectionDocNum,
          result.protectionDocTypeCode = p.protectionDoc.protectionDocTypeCode,
          result.protectionDocTypeName = p.protectionDoc.protectionDocTypeName
          return result;
        });
        this.formGroup.get('protectionDocId').patchValue(this.protectionDocs[0].id);
    }
  }

  private setRequestRelations(requestRelationsFromDetails: RequestRelationDto[]) {
    if (requestRelationsFromDetails) {
      const requestRelationFGs = requestRelationsFromDetails
        .map(rr => {
          const formGroup = this.fb.group({
            id: [rr.id, Validators.required],
            contractId: [rr.contractId],
            request: [rr.request, Validators.required],
            icgsRequestRelations: this.fb.array([])
          });
          const icgsFormGroup = rr.icgsRequestRelations.map(ir => {
            return this.fb.group({
              id: [ir.id],
              contractRequestRelationId: [rr.id],
              icgsRequestId: [ir.icgsRequest.id, Validators.required],
              descriptions: [ir.descriptions]
            });
          });
          const icgsRequestRelations = this.fb.array(icgsFormGroup);
          formGroup.setControl('icgsRequestRelations', icgsRequestRelations);
          return formGroup;
        });
      const requestRelations = this.fb.array(requestRelationFGs);
      this.formGroup.setControl('requestRelations', requestRelations);
      this.initSelector();
    }
  }
  // Для RequestRelations ОКОНЧАНИЕ
}
