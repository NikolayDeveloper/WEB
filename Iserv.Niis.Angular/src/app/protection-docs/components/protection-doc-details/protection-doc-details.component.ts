import {
  Component,
  OnInit,
  OnChanges,
  OnDestroy,
  ViewChild
} from '@angular/core';
import { ProtectionDocDetails } from '../../models/protection-doc-details';
import { SelectOption } from '../../../shared/services/models/select-option';
import { Subject } from 'rxjs/Subject';
import { ProtectionDocComponent } from '../protection-doc/protection-doc.component';
import { ProtectionDocsService } from '../../protection-docs.service';
import { DictionaryService } from '../../../shared/services/dictionary.service';
import { ActivatedRoute, Router, Params } from '@angular/router';
import { AuthenticationService } from '../../../shared/authentication/authentication.service';
import { MatDialog } from '@angular/material';
import { Observable } from 'rxjs/Observable';
import { PaymentInvoice } from '../../../payments/models/payment.model';
import { OwnerType } from '../../../shared/services/models/owner-type.enum';
import { WorkflowSendType } from '../../../shared/services/models/workflow-model';
import { WorkflowService } from '../../../shared/services/workflow.service';
import { SubjectDto } from '../../../subjects/models/subject.model';
import { Location } from '@angular/common';
import { DictionaryType } from '../../../shared/services/models/dictionary-type.enum';
import { WorkflowBusinessService } from '../../services/workflow-business.service';
import { RequestPart } from '../../../requests/services/workflow-business.service';
import { WorkflowDialogComponent } from '../../../shared/components/workflow-dialog/workflow-dialog.component';
import { AuthorsCertificatesDialogComponent } from '../authors-certificates-dialog/authors-certificates-dialog.component';

@Component({
  selector: 'app-protection-doc-details',
  templateUrl: './protection-doc-details.component.html',
  styleUrls: ['./protection-doc-details.component.scss']
})
export class ProtectionDocDetailsComponent implements OnInit, OnDestroy {
  editMode: boolean;
  protectionDocDetails: ProtectionDocDetails;
  selectOptions: SelectOption[];
  availableOfTransfer: any;
  selectedIndex = 0;
  selectedSubIndex = 0;
  arePaymentsAvailable: boolean;
  isBibliographicDataAvailable: boolean;
  isCreatingAuthorsCertificatesAvailable: boolean;
  private onDestroy = new Subject();

  @ViewChild(ProtectionDocComponent) requestComponent: ProtectionDocComponent;

  get payingSubject(): SubjectDto {
    const declarants = this.protectionDocDetails.subjects.filter(
      s => s.roleCode === '3' || s.roleCode === 'OWNER'
    );
    if (declarants.length > 0) {
      return declarants[0];
    }
    const authors = this.protectionDocDetails.subjects.filter(
      s => s.roleCode === '2'
    );
    if (authors.length > 0) {
      return authors[0];
    }
    return null;
  }

  constructor(
    private protectionDocService: ProtectionDocsService,
    private dictionaryService: DictionaryService,
    private route: ActivatedRoute,
    private router: Router,
    private location: Location,
    private auth: AuthenticationService,
    private workflowService: WorkflowService,
    private workflowBusinessService: WorkflowBusinessService,
    private dialog: MatDialog
  ) {}

  ngOnInit() {
    Observable.combineLatest(
      this.getSelectOptions(),
      this.route.params.switchMap(
        (params: Params): Observable<ProtectionDocDetails> => {
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
            ? this.protectionDocService.get(parseInt(id, 10))
            : this.protectionDocService.createRawProtectionDoc(
                parseInt(params['typeId'].toString(), 10),
                this.auth.userId
              );
        }
      )
    )
      .takeUntil(this.onDestroy)
      .subscribe(([selectOptions, protectionDocDetails]) => {
        this.selectOptions = selectOptions;
        this.setProtectionDocDetails(protectionDocDetails);
      });
  }

  ngOnDestroy(): void {
    this.onDestroy.next();
  }

  onSubmitData(value: any) {
    Object.assign(this.protectionDocDetails, value);
    if (this.protectionDocDetails.id) {
      this.protectionDocService
        .update(this.protectionDocDetails)
        .takeUntil(this.onDestroy)
        .subscribe(processedRequestDetails => {
          this.setProtectionDocDetails(processedRequestDetails);
        }, console.log);
    } else {
      this.protectionDocService
        .create(this.protectionDocDetails, 2072)
        .takeUntil(this.onDestroy)
        .subscribe(protectionDocId => {
          this.router.navigate(['protectiondocs', protectionDocId]);
        }, console.log);
    }
  }

  onModifiedData(protectionDocDetails: ProtectionDocDetails) {
    this.setProtectionDocDetails(protectionDocDetails);
  }

  onPaymentUsed(invoices: PaymentInvoice[]) {
    this.workflowService
      .get(this.protectionDocDetails.id, OwnerType.ProtectionDoc)
      .takeUntil(this.onDestroy)
      .subscribe(workflows => {
        const currentWorkflow = workflows.sortByDate(
          w => w.dateCreate,
          'desc'
        )[0];
        this.protectionDocDetails.currentWorkflowId = currentWorkflow.id;
        this.protectionDocDetails.currentWorkflow = currentWorkflow;
        this.protectionDocDetails.invoiceDtos = invoices;
        this.setProtectionDocDetails(
          Object.assign({}, this.protectionDocDetails)
        );
      });
  }

  onSubjectsChanged(subjects: SubjectDto[]) {
    this.protectionDocDetails.subjects = subjects;
  }

  onEdit(value: boolean) {
    setTimeout(() => (this.editMode = value));
  }

  onBack() {
    this.location.back();
  }

  setPaymentsAvailable(protectionDocDetails: ProtectionDocDetails) {
    this.workflowBusinessService
      .availableAtStage(protectionDocDetails, RequestPart.Payments)
      .takeUntil(this.onDestroy)
      .subscribe(available => {
        this.arePaymentsAvailable = available;
      });
  }

  setBibliographicDataAvailable(protectionDocDetails: ProtectionDocDetails) {
    this.workflowBusinessService
      .availableAtStageByWorkflow(
        this.protectionDocDetails.currentWorkflow,
        RequestPart.BibliographicData
      )
      .takeUntil(this.onDestroy)
      .subscribe(available => {
        this.isBibliographicDataAvailable = available;
      });
  }

  
  setCreatingAuthorsCertificatesAvailable(protectionDocDetails: ProtectionDocDetails) {
    if(protectionDocDetails.currentWorkflow.currentStageNameRu === "Печать охранного документа")
      this.isCreatingAuthorsCertificatesAvailable = true;
  }

  onOpenAuthorsCertificatesDialog() {
    const dialogRef = this.dialog.open(AuthorsCertificatesDialogComponent, {
      data: {
        authors: this.protectionDocDetails.subjects.filter(
          s => s.roleCode === '2'
        ),
        protectionDocId: this.protectionDocDetails.id
      },
      width: '700px'
    });

    dialogRef.afterClosed().subscribe(authors => {
      if (authors) {
        this.protectionDocService.createAuthorsCertificates(this.protectionDocDetails.id, authors)
          .subscribe(data =>
            data = data  
          );
      }
    });
  }

  onOpenWorkflowDialog() {
    const dialogRef = this.dialog.open(WorkflowDialogComponent, {
      data: {
        currentWorkflow: this.protectionDocDetails.currentWorkflow,
        ownerId: this.protectionDocDetails.id,
        ownerType: OwnerType.ProtectionDoc
      },
      width: '700px'
    });

    dialogRef.afterClosed().subscribe(newWorkflow => {
      if (newWorkflow) {
        if(newWorkflow.workflowSendType === WorkflowSendType.FinishParallelProcessing)
        {
          this.workflowService.finishParallelWorkflow(this.protectionDocDetails.currentWorkflow.id).subscribe(data =>
            data = data  
          );
          window.location.reload();
        }
        this.protectionDocDetails.currentWorkflowId = newWorkflow.id;
        this.protectionDocDetails.currentWorkflow = newWorkflow;
        if (newWorkflow.statusId) {
          this.protectionDocDetails.statusId = newWorkflow.statusId;
        }
        this.setProtectionDocDetails(
          Object.assign({}, this.protectionDocDetails)
        );
      }
    });
  }

  selectedIndexChange(index: number) {
    this.selectedIndex = index;
  }

  private getSelectOptions(): Observable<SelectOption[]> {
    try {
      return this.dictionaryService.getCombinedSelectOptions([
        DictionaryType.DicReceiveType,
        DictionaryType.DicProtectionDocType,
        DictionaryType.DicDivision,
        DictionaryType.DicRoute,
        DictionaryType.DicCustomerRole,
        DictionaryType.DicCustomerType,
        DictionaryType.DicSendType
      ]);
    } catch (error) {
      console.error(error);
    }
  }

  private setProtectionDocDetails(protectionDocDetails: ProtectionDocDetails) {
    this.protectionDocDetails = protectionDocDetails;
    this.setCreatingAuthorsCertificatesAvailable (protectionDocDetails);
    this.protectionDocDetails.ownerType = OwnerType.ProtectionDoc;
    this.setPaymentsAvailable(protectionDocDetails);
    this.setBibliographicDataAvailable(protectionDocDetails);
    this.availableOfTransfer = this.workflowBusinessService.availableOfTransfer(
      this.protectionDocDetails
    );
  }
}
