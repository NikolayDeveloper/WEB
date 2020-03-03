import { Component, OnInit, OnDestroy, ViewChild } from '@angular/core';
import { Location } from '@angular/common';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { Subject } from 'rxjs/Subject';
import { Observable } from 'rxjs/Observable';

import { SelectOption } from '../../../../shared/services/models/select-option';
import { ContractDetails } from '../../../models/contract-details';
import { DictionaryService } from '../../../../shared/services/dictionary.service';
import { DictionaryType } from '../../../../shared/services/models/dictionary-type.enum';
import { ContractService } from '../../../contract.service';
import { WorkflowBusinessService, ContractPart } from 'app/contracts/services/workflow-business.service';
import { AuthenticationService } from '../../../../shared/authentication/authentication.service';
import { OwnerType } from 'app/shared/services/models/owner-type.enum';
import { WorkflowDialogComponent } from 'app/contracts/components/contract-detail-components/workflow-dialog/workflow-dialog.component';
import { MatDialog } from '@angular/material';
import { WorkflowService } from '../../../../shared/services/workflow.service';
import { PaymentInvoice } from '../../../../payments/models/payment.model';
import { SubjectDto } from '../../../../subjects/models/subject.model';
import { AddresseeInfo } from '../../../../subjects/components/subjects-search-form/subjects-search-form.component';

@Component({
  selector: 'app-contract-detail',
  templateUrl: './contract-detail.component.html',
  styleUrls: ['./contract-detail.component.scss']
})
export class ContractDetailComponent implements OnInit, OnDestroy {
  contractDetails: ContractDetails = new ContractDetails();
  selectedSubIndex: 0;
  editMode: boolean;
  availableOfTransfer: Observable<boolean>;
  contractId: number;
  requestSelectOptions: SelectOption[];
  subjectSelectOptions: SelectOption[];
  onDestroy = new Subject();
  arePaymentsAvailable: boolean;
  flagChangeForPayment = false;

  get payingSubject(): SubjectDto {
    const confidants = this.contractDetails.subjects.filter(s => s.roleCode === '6');
    if (confidants.length > 0) {
      return confidants[0];
    }

    return null;
  }

  constructor(
    private location: Location,
    private route: ActivatedRoute,
    private router: Router,
    private dictionaryService: DictionaryService,
    private contractService: ContractService,
    private workflowBusinessService: WorkflowBusinessService,
    private workflowService: WorkflowService,
    private auth: AuthenticationService,
    public dialog: MatDialog) { }

  ngOnInit() {
    this.route.params
      .switchMap((params: Params): Observable<ContractDetails> => {
        const id = +params['id'];
        return id
          ? this.contractService.getById(id)
          : this.contractService.createRawContract(+params['typeId']);
      })
      .takeUntil(this.onDestroy)
      .subscribe((contractDetails) => {
        this.setContractDetails(contractDetails);
      },
        console.log);
    this.fetchRequestDictionaries();
    this.fetchSubjectDictionaries();
  }

  setPaymentsAvailable(contractDetails: ContractDetails) {
    this.workflowBusinessService.availableAtStage(contractDetails, ContractPart.Payments)
      .takeUntil(this.onDestroy)
      .subscribe(available => {
        this.arePaymentsAvailable = available;
      });
  }

  onModifiedData(contractDetail: ContractDetails) {
    this.setContractDetails(contractDetail);
  }

  ngOnDestroy(): void {
    this.onDestroy.next();
  }

  onBack() {
    this.location.back();
  }

  onEdit(value: boolean) {
    setTimeout(() => this.editMode = value);
  }

  onDelete(contractId: number) {
    this.contractService.deleteContract(contractId)
      .takeUntil(this.onDestroy)
      .subscribe(() => {
        this.router.navigate(['journal']);
      });
  }

  onRegister(value: any) {
    Object.assign(this.contractDetails, value);
    this.contractService.registerContract(this.contractDetails)
      .takeUntil(this.onDestroy)
      .subscribe(processedContractDetails => {
        this.setContractDetails(processedContractDetails);
        this.router.navigate(['contracts', processedContractDetails.id]);
      },
        console.log);
  }

  onSubmitData(value: any) {
    Object.assign(this.contractDetails, value);
    if (this.contractDetails.id) {
      this.contractService
        .updateContract(this.contractDetails)
        .takeUntil(this.onDestroy)
        .subscribe(processedContractDetails => {
          this.setContractDetails(processedContractDetails);
          this.router.navigate(['contracts', processedContractDetails.id]);
        },
          console.log);
    } else {
      this.contractService
        .addContract(this.contractDetails)
        .takeUntil(this.onDestroy)
        .subscribe(processedContractDetails => {
          this.setContractDetails(processedContractDetails);
          this.router.navigate(['contracts', processedContractDetails.id]);
        },
          console.log);
    }
  }

  onOpenWorkflowDialog() {
    const dialogRef = this.dialog.open(WorkflowDialogComponent, {
      data: {
        details: this.contractDetails
      }
    });

    dialogRef.afterClosed()
      .subscribe(newWorkflow => {
        if (newWorkflow) {
          this.contractDetails.currentWorkflowId = newWorkflow.id;
          this.contractDetails.currentWorkflow = newWorkflow;
          this.contractDetails.statusId = newWorkflow.statusId;
          this.contractDetails.gosDate = newWorkflow.contractGosDate;
          this.contractDetails.fullExpertiseExecutorId = newWorkflow.fullExpertiseExecutorId;
          this.contractDetails.contractNum = newWorkflow.contractNum;
          this.contractDetails.applicationDateCreate = newWorkflow.applicationDateCreate;

          this.workflowBusinessService.doPaymentLogic(Object.assign({}, this.contractDetails))
            .takeUntil(this.onDestroy)
            .subscribe(contractDetails => {
              this.setContractDetails(contractDetails);
              if (this.flagChangeForPayment) {
                this.flagChangeForPayment = false;
              } else {
                this.flagChangeForPayment = true;
              }
            },
              console.log);
        }
      });
  }

  onPaymentUsed(invoices: PaymentInvoice[]) {
    this.workflowService.get(this.contractDetails.id, OwnerType.Contract)
      .takeUntil(this.onDestroy)
      .subscribe(workflows => {
        const currentWorkflow = workflows.sort((w1, w2) => w2.id - w1.id)[0];
        this.contractDetails.currentWorkflowId = currentWorkflow.id;
        this.contractDetails.currentWorkflow = currentWorkflow;
        this.contractDetails.invoiceDtos = invoices;
        this.setContractDetails(Object.assign({}, this.contractDetails));
      });
  }
  isInitialStage() {
    if (this.contractDetails && this.contractDetails.currentWorkflow) {
      return this.workflowBusinessService.isInitialStage(this.contractDetails.currentWorkflow.currentStageCode);
    } else {
      return false;
    }
  }
  private fetchRequestDictionaries() {
    this.dictionaryService.getCombinedSelectOptions([
      DictionaryType.DicReceiveType,
      DictionaryType.DicContractStatus,
      DictionaryType.DicProtectionDocType,
      DictionaryType.DicCustomerRole,
      DictionaryType.DicCustomerType,
      DictionaryType.DicDepartment,
      DictionaryType.DicDivision])
      .takeUntil(this.onDestroy)
      .subscribe(
        selectOptions => this.requestSelectOptions = selectOptions,
        console.log);
  }

  private fetchSubjectDictionaries() {
    this.dictionaryService.getCombinedSelectOptions([
      DictionaryType.DicContractCategory,
      DictionaryType.DicCustomerRole,
      DictionaryType.DicCustomerType])
      .takeUntil(this.onDestroy)
      .subscribe(
        selectOptions => this.subjectSelectOptions = selectOptions,
        console.log);
  }

  private setContractDetails(contractDetails: ContractDetails) {
    contractDetails.subjects = this.contractDetails.subjects;
    contractDetails.invoiceDtos = this.contractDetails.invoiceDtos;
    this.contractDetails = contractDetails;
    this.contractDetails.ownerType = OwnerType.Contract;
    this.contractDetails.contractNum = contractDetails.contractNum;

    this.contractDetails.addresseeInfo = new AddresseeInfo();
    this.contractDetails.addresseeInfo.addresseeId = this.contractDetails.addresseeId;
    this.contractDetails.addresseeInfo.addresseeAddress = this.contractDetails.addresseeAddress;
    this.contractDetails.addresseeInfo.addresseeNameRu = this.contractDetails.addresseeNameRu;
    this.contractDetails.addresseeInfo.addresseeXin = this.contractDetails.addresseeXin;
    this.contractDetails.addresseeInfo.apartment = this.contractDetails.apartment;
    this.setPaymentsAvailable(contractDetails);
    this.contractDetails.owners = contractDetails.owners;
    this.availableOfTransfer = this.workflowBusinessService.availableOfTransfer(this.contractDetails);
  }
}
