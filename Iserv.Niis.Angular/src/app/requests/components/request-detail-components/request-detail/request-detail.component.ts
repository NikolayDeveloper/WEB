import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material';
import { ActivatedRoute, Params, Router } from '@angular/router';
import {
  isStageCreation,
  isStageFormationAppData,
  isStagePayment
} from 'app/bibliographic-data/components/description/description.component';
import 'app/core/utils/array-extensions';
import { OwnerType } from 'app/shared/services/models/owner-type.enum';
import { AddresseeInfo } from 'app/subjects/components/subjects-search-form/subjects-search-form.component';
import { SubjectDto } from 'app/subjects/models/subject.model';
import { Observable } from 'rxjs/Observable';
import { Subject } from 'rxjs/Subject';
import { PaymentInvoice } from '../../../../payments/models/payment.model';
import { AuthenticationService } from '../../../../shared/authentication/authentication.service';
import { WorkflowDialogComponent } from '../../../../shared/components/workflow-dialog/workflow-dialog.component';
import { RouteStageCodes } from '../../../../shared/models/route-stage-codes';
import { DictionaryService } from '../../../../shared/services/dictionary.service';
import { DictionaryType } from '../../../../shared/services/models/dictionary-type.enum';
import { SelectOption } from '../../../../shared/services/models/select-option';
import { WorkflowService } from '../../../../shared/services/workflow.service';
import { RequestDetails } from '../../../models/request-details';
import { RequestService } from '../../../request.service';
import {
  RequestPart,
  WorkflowBusinessService
} from '../../../services/workflow-business.service';
import { RequestComponent } from '../request/request.component';
import { Workflow } from 'app/shared/services/models/workflow-model';
import { CustomerRole } from 'app/subjects/enums/customer-role.enum';
import { CustomerType } from 'app/subjects/enums/customer-type.enum';
import { TokenStorageService } from 'app/shared/authentication/token-storage.service';
import { ReceiveTypeCodes } from 'app/requests/enums/receive-type-codes.enum';
import { IServerStatus } from 'app/requests/interfaces/server-status.interface';
import { StatusCodes } from 'app/requests/enums/status-codes.enum';
import { SnackBarHelper } from 'app/core/snack-bar-helper.service';
import { YesNoCancelDialogComponent } from 'app/shared/components/yes-no-cancel-dialog/yes-no-cancel-dialog.component';
import { ModalButton } from 'app/shared/services/models/modal-button.enum';

@Component({
  selector: 'app-request-detail',
  templateUrl: './request-detail.component.html',
  styleUrls: ['./request-detail.component.scss']
})
export class RequestDetailComponent implements OnInit, OnDestroy {
  editMode: boolean;
  requestDetails: RequestDetails;
  selectOptions: SelectOption[];
  availableOfTransfer: any;
  selectedIndex = 0;
  selectedSubIndex = 0;
  arePaymentsAvailable: boolean;
  isBibliographicDataAvailable: boolean;
  needUpdateSubjects = false;
  isCreateStage = false;
  createStageCode = [
    'B01.1',
    'TM01.1',
    'SA01.1',
    'PO01.1',
    'U01.1',
    'NMPT01.1'
  ];
  private onDestroy = new Subject();
  roleCodes: CustomerRole[] = [
    CustomerRole.Declarant,
    CustomerRole.Author,
    CustomerRole.PatentAttorney,
    CustomerRole.Contact,
    CustomerRole.Confidant,
    CustomerRole.CorrespondingRecipient,
    CustomerRole.Owner
  ];
  private dialogRefChange = null;

  private compliteCreateStages = ['B01.1', 'TM01.1', 'PO01.1', 'SA01.1', 'U01.1'];

  private sendRequisitionCodes = ['TM03.3.7', 'PO03.8', 'NMPT03.7', 'B03.3.7.0', 'U03.7.0', 'SA03.3.7', 'B03.3.2.3', 'PO03.8'];

  @ViewChild(RequestComponent) requestComponent: RequestComponent;

  get stagesFormationAppData() {
    return RouteStageCodes.stagesFormationAppData;
  }
  get pdTypeTMCodes() {
    return RouteStageCodes.pdTypeTMCodes;
  }
  get payingSubject(): SubjectDto {
    const declarants = this.requestDetails.subjects.filter(
      s => s.roleCode === '1'
    );
    if (declarants.length > 0) {
      return declarants[0];
    }
    const authors = this.requestDetails.subjects.filter(
      s => s.roleCode === '2'
    );
    if (authors.length > 0) {
      return authors[0];
    }
    return null;
  }

  constructor(
    private requestService: RequestService,
    private dictionaryService: DictionaryService,
    private workflowBusinessService: WorkflowBusinessService,
    private workflowService: WorkflowService,
    private route: ActivatedRoute,
    private router: Router,
    private auth: AuthenticationService,
    private dialog: MatDialog,
    private snackbarHelper: SnackBarHelper,
    private tokenStorageService: TokenStorageService
  ) {}

  ngOnInit() {
    Observable.combineLatest(
      this.getSelectOptions(),
      this.route.params.switchMap(
        (params: Params): Observable<RequestDetails> => {
          const selectedIndex = params['selectedIndex'];
          if (selectedIndex) {
            this.selectedIndex = +selectedIndex;
          }
          const selectedSubIndex = params['selectedSubIndex'];
          if (selectedSubIndex) {
            this.selectedSubIndex = +selectedSubIndex;
          }

          const id = params['id'];
          return id
            ? this.requestService.getRequestById(parseInt(id, 10))
            : this.requestService.createRawRequest(
                parseInt(params['typeId'].toString(), 10),
                this.auth.userId
              );
        }
      )
    )
      .takeUntil(this.onDestroy)
      .subscribe(([selectOptions, requestDetails]) => {
        this.selectOptions = selectOptions;
        this.setRequestDetails(requestDetails);
        if (['PN', 'TM'].includes(requestDetails.protectionDocTypeCode)) {
          if (requestDetails.isHasMaterialExpertOpinionWithOugoingNumber) {
            this.roleCodes = [
              CustomerRole.Declarant,
              CustomerRole.PatentAttorney,
              CustomerRole.Contact,
              CustomerRole.Confidant,
              CustomerRole.CorrespondingRecipient,
              CustomerRole.Owner
            ];
          } else {
            this.roleCodes = [
              CustomerRole.Declarant,
              CustomerRole.PatentAttorney,
              CustomerRole.Contact,
              CustomerRole.Confidant,
              CustomerRole.CorrespondingRecipient
            ];
          }
        } else if (requestDetails.protectionDocTypeCode === 'TM') {
          if (this.requestDetails.currentWorkflow.currentStageCode !== 'TM02.1') {
            this.roleCodes = [
              CustomerRole.Declarant,
              CustomerRole.PatentAttorney,
              CustomerRole.Contact,
              CustomerRole.Confidant,
              CustomerRole.CorrespondingRecipient,
              CustomerRole.Owner
            ];
          } else {
            this.roleCodes = [
              CustomerRole.Declarant,
              CustomerRole.PatentAttorney,
              CustomerRole.Contact,
              CustomerRole.Confidant,
              CustomerRole.CorrespondingRecipient
            ];
          }
        } else if (['U', 'B', 'S2'].includes(requestDetails.protectionDocTypeCode)) {
          if (this.requestDetails.currentWorkflow.currentStageCode !== 'TM02.1') {
            this.roleCodes = [
              CustomerRole.Declarant,
              CustomerRole.Author,
              CustomerRole.PatentAttorney,
              CustomerRole.Contact,
              CustomerRole.Confidant,
              CustomerRole.CorrespondingRecipient,
              CustomerRole.Owner
            ];
          } else {
            this.roleCodes = [
              CustomerRole.Declarant,
              CustomerRole.Author,
              CustomerRole.PatentAttorney,
              CustomerRole.Contact,
              CustomerRole.Confidant,
              CustomerRole.CorrespondingRecipient
            ];
          }
        } else if (requestDetails.protectionDocTypeCode === 'SA') {
          this.roleCodes = [
            CustomerRole.Declarant,
            CustomerRole.Author,
            CustomerRole.PatentAttorney,
            CustomerRole.Contact,
            CustomerRole.Confidant,
            CustomerRole.CorrespondingRecipient
          ];
        }

        if (this.requestDetails.currentWorkflow) {
          this.isCreateStage = this.createStageCode.includes(
            this.requestDetails.currentWorkflow.currentStageCode
          );
        }
      });
  }

  ngOnDestroy(): void {
    this.onDestroy.next();
  }

  canEdit(): boolean {
    if (this.requestDetails && this.requestDetails.currentWorkflow) {
      return this.requestDetails.currentWorkflow.currentUserId === this.tokenStorageService.getCurrentUserID();
    } else {
      return false;
    }
  }

  onSubmitData(value: any) {
    Object.assign(this.requestDetails, value);
    if (this.requestDetails.id) {
      this.requestService
        .updateRequest(this.requestDetails)
        .switchMap(updatedRequestDetails =>
          this.workflowBusinessService.afterUpdateRequest(updatedRequestDetails)
        )
        .takeUntil(this.onDestroy)
        .subscribe(processedRequestDetails => {
          this.uploadImage(value.imageFile);
          this.uploadMedia(value.mediaFile);
          this.setRequestDetails(processedRequestDetails);
        }, console.log);
    } else {
      this.requestService
        .addRequest(this.requestDetails)
        .switchMap(addedRequestDetails =>
          this.workflowBusinessService.afterCreateRequest(addedRequestDetails)
        )
        .takeUntil(this.onDestroy)
        .subscribe(processedRequestDetails => {
          this.uploadImage(value.imageFile);
          this.uploadMedia(value.mediaFile);
          this.setRequestDetails(processedRequestDetails);
          this.router.navigate(['requests', processedRequestDetails.id]);
        }, console.log);
    }
  }

  onDelete(requestId: number) {
    this.requestService
      .delete(requestId)
      .takeUntil(this.onDestroy)
      .subscribe(() => {
        this.router.navigate(['journal']);
      });
  }

  private uploadImage(file: File) {
    if (file && !this.requestDetails.isImageFromName) {
      this.requestService
        .uploadImage(file, this.requestDetails.id)
        .subscribe(data => {
          this.requestDetails.imageUrl = data.url;
          this.requestDetails = {...this.requestDetails};
        });
    }
  }

  private uploadMedia(file: File) {
    if (file) {
      this.requestService
        .uploadMedia(file, this.requestDetails.id)
        .subscribe(data => {
          this.requestDetails.mediaFileUrl = data;
          this.requestDetails = {...this.requestDetails};
        });
    }
  }

  onModifiedData(requestDetails: RequestDetails) {
    this.setRequestDetails(requestDetails);
  }

  onPaymentUsed(invoices: PaymentInvoice[]) {
    Observable.combineLatest(
      this.workflowService.get(this.requestDetails.id, OwnerType.Request),
      this.requestService.getRequestById(this.requestDetails.id)
    )
      .takeUntil(this.onDestroy)
      .subscribe(([workflows, requestDetails]) => {
        const currentWorkflow = workflows.sortByDate(
          w => w.dateCreate,
          'desc'
        )[0];
        this.requestDetails.currentWorkflowId = currentWorkflow.id;
        this.requestDetails.currentWorkflow = currentWorkflow;
        this.requestDetails.invoiceDtos = invoices;
        this.requestDetails.statusId = requestDetails.statusId;
        this.setRequestDetails(Object.assign({}, this.requestDetails));
      });
  }

  isSendButtonDisabled(): boolean {
    return (this.requestDetails.isFromLk);
  }

  requisitionSend(): void {
    this.requestService.requisitionSend(this.requestDetails.id)
      .subscribe((status: IServerStatus) => {
        this.processSendStatus(status);
      }, (error: any) => {
        this.snackbarHelper.error(`${error.message}. Просим обратиться к администратору системы.`);
      });
  }

  private processSendStatus(status: IServerStatus) {
    if (status) {
      switch (status.code) {
        case StatusCodes.Successfully:
          this.snackbarHelper.success(status.message);
          this.requestDetails.isFromLk = true;
          break;
        case StatusCodes.NotFound:
        case StatusCodes.UnknownType:
        case StatusCodes.BadRequest:
        case StatusCodes.ExistNumber:
          this.snackbarHelper.error(status.message);
          break;
        default:
          this.snackbarHelper.error(status.message);
          break;
      }
    }
  }

  checkSubjects(): void {
    const declarants = this.requestDetails.subjects.filter(subject => (subject.roleCode === CustomerRole.Declarant));
    if (declarants.some(subject => (
      subject.typeId !== CustomerType.Individual &&
      subject.typeId !== CustomerType.LegalEntity
    ))) {
      this.isBibliographicDataAvailable = true;
      this.needUpdateSubjects = true;
    }
  }

  onSubjectsChanged(subjects: SubjectDto[]) {
    this.requestService
      .getRequestById(this.requestDetails.id)
      .takeUntil(this.onDestroy)
      .subscribe(requestDetails => {
        this.requestDetails = requestDetails;
        this.requestDetails.subjects = subjects;
        this.setRequestDetails(Object.assign({}, this.requestDetails));
        this.checkSubjects();
      });
  }
  onRequestNumberChanged(requestNum: string) {
    this.requestDetails.requestNum = requestNum;
    this.setRequestDetails(Object.assign({}, this.requestDetails));
  }
  onEdit(value: boolean) {
    setTimeout(() => (this.editMode = value));
  }

  onBack() {
    if (this.selectedIndex === 0) {
      this.router.navigate(['journal/tasks/active']);
    } else {
      this.selectedIndex = 0;
    }
  }

  setPaymentsAvailable(requestDetails: RequestDetails) {
    this.workflowBusinessService
      .availableAtStage(requestDetails, RequestPart.Payments)
      .takeUntil(this.onDestroy)
      .subscribe(available => {
        // this.arePaymentsAvailable = available;
        this.arePaymentsAvailable = true;
      });
  }

  setBibliographicDataAvailable(requestDetails: RequestDetails) {
    this.workflowBusinessService
      .availableAtStageByWorkflow(
        this.requestDetails.currentWorkflow,
        RequestPart.BibliographicData
      )
      .takeUntil(this.onDestroy)
      .subscribe(available => {
        // this.isBibliographicDataAvailable = available;
        this.isBibliographicDataAvailable = true;
      });
  }

  onOpenWorkflowDialog() {
    const dialogRef = this.dialog.open(WorkflowDialogComponent, {
      data: {
        currentWorkflow: this.requestDetails.currentWorkflow,
        ownerId: this.requestDetails.id,
        ownerType: OwnerType.Request,
        requestDetails: this.requestDetails
      },
      width: '700px'
    });

    dialogRef.afterClosed().subscribe(newWorkflow => {
      if (newWorkflow) {
        this.requestDetails.currentWorkflowId = newWorkflow.id;
        this.requestDetails.currentWorkflow = newWorkflow;
        if (newWorkflow.statusId) {
          this.requestDetails.statusId = newWorkflow.statusId;
        }
        this.workflowBusinessService
          .doPaymentLogic(Object.assign({}, this.requestDetails))
          .takeUntil(this.onDestroy)
          .subscribe(requestDetails => {
            this.setRequestDetails(requestDetails);
          }, console.log);
      }

      if (this.sendRequisitionCodes.some(d => d === this.requestDetails.currentWorkflow.currentStageCode)
            && !this.requestDetails.isFromLk
            && this.requestDetails.receiveTypeId === ReceiveTypeCodes.ElectronicFeed) {
        this.dialog
          .open(YesNoCancelDialogComponent, {
              data: {
                  title: 'Заявка будет отправлена в Личный кабинет заявителю!',
                  message: 'При выборе опции «Позже» заявку можно будет отправить только посредством опции «Отправить в ЛК»',
                  buttons: [ModalButton.Yes, ModalButton.No],
                  labels: {
                    [ModalButton.Yes]: 'Отправить',
                    [ModalButton.No]: 'Позже'
                  },
              }
          })
          .afterClosed()
          .subscribe((result: ModalButton) => {
              if (result === ModalButton.Yes) {
                this.requisitionSend();
              }
          });
      }
    });
  }

  selectedIndexChange(index: number) {
    this.selectedIndex = index;
  }

  isCurrentStageInventionFormationRequestData(): boolean {
    if (this.requestDetails && this.requestDetails.currentWorkflow) {
      return this.requestDetails.currentWorkflow.currentStageCode === 'B02.1';
    } else {
      return false;
    }
  }
  isCurrentStageCreateStage(): boolean {
    if (this.requestDetails && this.requestDetails.currentWorkflow) {
      return this.createStageCode.includes(
        this.requestDetails.currentWorkflow.currentStageCode
      );
    } else {
      return false;
    }
  }
  isVisibleExpertize() {
    if (!this.requestDetails) {
      return true;
    }
    return this.requestDetails.protectionDocTypeCode !== 'SA';
  }

  IsBiblioDisabled(): boolean {
    // if (this.needUpdateSubjects) {
    //   return false;
    // } else {
    //   return (
    //     isStageCreation(this.requestDetails) ||
    //     isStagePayment(this.requestDetails)
    //   );
    // }
    return false;
  }

  isPaymentDisabled(): boolean {
    // return (
    //   isStageCreation(this.requestDetails) ||
    //   isStageFormationAppData(this.requestDetails)
    // );
    return false;
  }

  isContractsDisabled(): boolean {
    // return (
    //   isStageCreation(this.requestDetails) ||
    //   isStagePayment(this.requestDetails) ||
    //   isStageFormationAppData(this.requestDetails)
    // );
    return false;
  }

  isExpertizeDisabled(): boolean {
  //   return (
  //     isStageCreation(this.requestDetails) ||
  //     isStagePayment(this.requestDetails) ||
  //     isStageFormationAppData(this.requestDetails)
  //   );
  return false;
  }

  isSendToNextStageVisible(): boolean {
    // return (
    //   this.requestDetails &&
    //   this.requestDetails.currentWorkflow &&
    //   !this.compliteCreateStages.includes( this.requestDetails.currentWorkflow.currentStageCode)
    // );
    return true;
  }

  isCompleteCreateDisabled(): boolean {
    // return !this.requestDetails || !this.requestDetails.hasRequiredOnCreate;
    return false;
  }

  isCompleteCreateVisible(): boolean {
    // return (
    //   this.requestDetails &&
    //   this.requestDetails.currentWorkflow &&
    //   this.compliteCreateStages.includes(this.requestDetails.currentWorkflow.currentStageCode)
    // );
    return false;
  }

  onCompleteCreateClick() {
    this.requestService
      .completeCreate(this.requestDetails.id)
      .takeUntil(this.onDestroy)
      .subscribe((workflow: Workflow) => {
        this.requestDetails.currentWorkflowId = workflow.id;
        this.requestDetails.currentWorkflow = workflow;
        this.requestDetails = Object.assign({}, this.requestDetails);
      });
  }

  private getSelectOptions(): Observable<SelectOption[]> {
    return this.dictionaryService.getCombinedSelectOptions([
      DictionaryType.DicReceiveType,
      DictionaryType.DicProtectionDocType,
      DictionaryType.DicDivision,
      DictionaryType.DicRoute,
      DictionaryType.DicCustomerRole,
      DictionaryType.DicCustomerType
    ]);
  }

  private setRequestDetails(requestDetails: RequestDetails) {
    this.requestDetails = requestDetails;
    this.requestDetails.addresseeInfo = new AddresseeInfo();
    this.requestDetails.addresseeInfo.addresseeShortAddress = this.requestDetails.addresseeShortAddress;
    this.requestDetails.addresseeInfo.addresseeAddress = this.requestDetails.addresseeAddress;
    this.requestDetails.addresseeInfo.addresseeId = this.requestDetails.addresseeId;
    this.requestDetails.addresseeInfo.addresseeNameRu = this.requestDetails.addresseeNameRu;
    this.requestDetails.addresseeInfo.addresseeXin = this.requestDetails.addresseeXin;
    this.requestDetails.addresseeInfo.apartment = this.requestDetails.apartment;
    this.requestDetails.addresseeInfo.republic = this.requestDetails.republic;
    this.requestDetails.addresseeInfo.oblast = this.requestDetails.oblast;
    this.requestDetails.addresseeInfo.region = this.requestDetails.region;
    this.requestDetails.addresseeInfo.city = this.requestDetails.city;
    this.requestDetails.addresseeInfo.street = this.requestDetails.street;
    this.requestDetails.ownerType = OwnerType.Request;
    this.setPaymentsAvailable(requestDetails);
    this.setBibliographicDataAvailable(requestDetails);
    this.availableOfTransfer = this.workflowBusinessService.availableOfTransfer(
      this.requestDetails
    );
  }
}
