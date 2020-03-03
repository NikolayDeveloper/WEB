import { Location } from '@angular/common';
import { Component, ElementRef, OnInit, AfterViewInit, ViewChild, EventEmitter } from '@angular/core';
import { OnDestroy } from '@angular/core/src/metadata/lifecycle_hooks';
import {
  AbstractControl,
  FormBuilder,
  FormControl,
  FormGroup,
  Validators
} from '@angular/forms';
import { MatDialog } from '@angular/material';
import { ActivatedRoute, Params, Router } from '@angular/router';
import {
  errorStatusCodes,
  NotificationStatus
} from 'app/shared/services/models/notification-status';
import { NotificationsService } from 'app/shared/services/notifications.service';
import { AddresseeInfo } from 'app/subjects/components/subjects-search-form/subjects-search-form.component';
import { Observable } from 'rxjs/Observable';
// tslint:disable-next-line:import-blacklist
import { Subject } from 'rxjs/Rx';
import { ContractService } from '../../../../contracts/contract.service';
import { ContractDetails } from '../../../../contracts/models/contract-details';
import { ProtectionDocDetails } from '../../../../protection-docs/models/protection-doc-details';
import { ProtectionDocsService } from '../../../../protection-docs/protection-docs.service';
import { RequestDetails } from '../../../../requests/models/request-details';
import { RequestService } from '../../../../requests/request.service';
import { SearchPaymentsComponent } from '../../../../shared/components/search-payments/search-payments.component';
import { Config } from '../../../../shared/components/table/config.model';
import { UndoDialogComponent } from '../../../../shared/components/undo-dialog/undo-dialog.component';
import { DocumentsService } from '../../../../shared/services/documents.service';
import {
  DicDocumentType,
  DicReceiveType
} from '../../../../shared/services/models/base-dictionary';
import { ChangeExecutorDialogComponent } from '../../../components/change-executor-dialog/change-executor-dialog.component';
import { MaterialWorkflow } from '../../../../shared/services/models/workflow-model';
import { AuthenticationService } from '../../../../shared/authentication/authentication.service';
import {
  getModuleName,
  OwnerType
} from '../../../../shared/services/models/owner-type.enum';
import { SelectOption } from '../../../../shared/services/models/select-option';
import { SubjectDto } from '../../../../subjects/models/subject.model';
import { SubjectsService } from '../../../../subjects/services/subjects.service';
import { DeleteDialogComponent } from '../../../components/delete-dialog/delete-dialog.component';
import { DocumentWorkflowDialogComponent } from '../../../components/workflow-dialog/workflow-dialog.component';
import {
  Attachment,
  DocumentType,
  getDocumentTypeRoute,
  MaterialDetail,
  MaterialOwnerDto,
  OutgoingDetail,
  UserInputDto,
  DocumentCommentDto,
  UserInputFieldConfig,
  MaterialTask,
  DocumentLinkDto
} from '../../../models/materials.model';
import { MaterialsService } from '../../../services/materials.service';
import { WorkflowBusinessService } from '../../../services/workflow-business/workflow-business.service';
import { DictionaryService } from '../../../../shared/services/dictionary.service';
import { DictionaryType } from '../../../../shared/services/models/dictionary-type.enum';
import { IncomingAnswerComponent } from '../../../../shared/components/incoming-answer/incoming-answer.component';
import { DicPositionTypeCodes } from 'app/shared/models/dic-position-type-codes.enum';
import { IServerStatus } from 'app/requests/interfaces/server-status.interface';
import { StatusCodes } from 'app/requests/enums/status-codes.enum';
import { SnackBarHelper } from 'app/core/snack-bar-helper.service';
import { StateService } from 'app/shared/services/state.service';
// import ImageResize from 'quill-image-resize-module';

// TODO: Временно отключен модуль imageResize
// (window as any).Quill.register('modules/imageResize', ImageResize)

@Component({
  selector: 'app-details',
  templateUrl: './details.component.html',
  styleUrls: ['./details.component.scss']
})
export class DetailsComponent implements OnInit, AfterViewInit, OnDestroy {
  workflows: MaterialWorkflow[];
  currentWorkflow: MaterialWorkflow;
  comments: DocumentCommentDto[];
  links: DocumentLinkDto[];
  parentLinks: DocumentLinkDto[];
  removeLinks: DocumentLinkDto[];
  columns: Config[] = [
    new Config({ columnDef: 'routeNameRu', header: 'Маршрут', class: 'width-200' }),
    new Config({ columnDef: 'currentUserNameRu', header: 'Пользователь', class: 'width-200' }),
    new Config({ columnDef: 'dateCreate', header: 'Дата', class: 'width-200' }),
    new Config({ columnDef: 'currentStageNameRu', header: 'Этап', class: 'width-200' }),
    new Config({ columnDef: 'description', header: 'Описание', class: 'width-200' }),
    new Config({ columnDef: 'isSigned', header: 'Подпись', class: 'width-100' })
  ];
  formGroup: FormGroup;
  ownerType: OwnerType;
  ownerId: number;
  protectionDocTypeId: number;
  filteredAvailableTypes: Observable<SelectOption[]>;
  userInputFormConfig: Array<UserInputFieldConfig> = [];
  ckeditorContent = '';
  quillModules: any = {};
  outgoingDetail: OutgoingDetail;
  docTypesCollection: SelectOption[];
  editMode = false;
  editableControls: string[];
  scanUnavailableControls: string[];
  scanAvailableControls: string[];
  initialUnavailableControls: string[];
  registerEditableControls: string[];
  id: number;
  availableAtStage: any;
  availableOfTransfer: any;
  isEditableStage: any;
  previewMode = false;
  statusCode: string;
  notificationStatuses: NotificationStatus[];
  sendTypes: SelectOption[];
  isReadOnly = false;
  typeCode: string;
  code = 'B;U;S2;TM;PN;SA;S1;A4;A;ITM;PR;DK;AP';
  registerCodes = [
    '006.014.0',
    '006.014.3'
  ];
  paymentInvoiceTypeCodes = ['TZPRED1.0', 'TZPOL4', 'NOT1_NEW'];
  ownertTypeMaterial = OwnerType.Material;
  documentTypeOutgoing = DocumentType.Outgoing;

  private onDestroy = new Subject();
  private owners: MaterialOwnerDto[] = [];

  noAttachments = false;

  @ViewChild('inputSingleFile') singleinputFile: ElementRef;

  reloadAttachments = new EventEmitter();

  constructor(
    private fb: FormBuilder,
    private documentsService: DocumentsService,
    private route: ActivatedRoute,
    private requestService: RequestService,
    private contractService: ContractService,
    private dictionaryService: DictionaryService,
    private protectionDocService: ProtectionDocsService,
    private materialsService: MaterialsService,
    private workflowBusinessService: WorkflowBusinessService,
    private subjectsService: SubjectsService,
    private notificationsService: NotificationsService,
    private dialog: MatDialog,
    private router: Router,
    private location: Location,
    private auth: AuthenticationService,
    private snackbarHelper: SnackBarHelper,
    private stateService: StateService
  ) {
    this.buildForm();
    this.quillModules = {
      // TODO: Временно отключен модуль imageResize
      // imageResize: {},
      toolbar: [
        ['bold', 'italic', 'underline', 'strike'], // toggled buttons
        ['blockquote'],

        // custom button values
        [{ list: 'ordered' }, { list: 'bullet' }], // superscript/subscript
        [{ indent: '-1' }, { indent: '+1' }], // outdent/indent

        [{ size: ['small', false, 'large', 'huge'] }], // custom dropdown
        [{ header: [1, 2, 3, 4, 5, 6, false] }],

        [{ color: [] }, { background: [] }], // dropdown with defaults from theme
        [{ align: [] }],

        ['clean'], // remove formatting button

        ['link', 'image'] // link and image
      ]
    };
  }

  ngOnInit() {
    this.route.params
      .switchMap(
        (params: Params): Observable<any> => {
          this.id = +params['id'];
          this.ownerType = +params['ownerType'];
          this.ownerId = +params['ownerId'];
          this.protectionDocTypeId = +params['protectionDocTypeId'];
          return this.dictionaries.switchMap(
            ([docTypes]: [DicDocumentType[]]) => {
              this.docTypesCollection = docTypes;
              if (this.id) {
                return this.materialsService.getSingleOutgoing(this.id);
              } else {
                this.outgoingDetail = new OutgoingDetail();
                if (this.ownerId) {
                  this.owners.push({
                    ownerId: this.ownerId,
                    ownerType: this.ownerType,
                    protectionDocTypeId: this.protectionDocTypeId
                  });
                  this.setNumbers();
                  this.formGroup.get('ownerNumbers').patchValue(this.owners);
                }
                this.toggleEditMode(true);
                this.filterByIcgs();
                return Observable.of(null);
              }
            }
          );
        }
      )
      .takeUntil(this.onDestroy)
      .filter(outgoingDetail => !!outgoingDetail)
      .subscribe((outgoingDetail: OutgoingDetail) => {
        this.outgoingDetail = outgoingDetail;
        this.owners = outgoingDetail.owners;
        if (outgoingDetail.owners.length) {
          this.ownerId = outgoingDetail.owners[0].ownerId;
        }
        this.ownerType = outgoingDetail.ownerType;
        this.InitializeFormGroupFromData();
        this.previewMode = !outgoingDetail.wasScanned;
        this.toggleEditMode(false);
      });
    this.subscribeNotificationStatus();

    this.dictionaryService
      .getSelectOptions(DictionaryType.DicReceiveType)
      .takeUntil(this.onDestroy)
      .subscribe(data => (this.sendTypes = data));
  }

  ngAfterViewInit(): void {
    const state = this.stateService.loadState('incomingDetail', true);

    if (state) {
      this.formGroup.patchValue(state);
    }
  }

  onAttachmentsLoad(attachmentsCount) {
    this.noAttachments = attachmentsCount === 0;
  }

  hasIncomingAnswerNumber(): boolean {
    return !!this.outgoingDetail.incomingAnswerNumber || !!this.formGroup.get('incomingAnswerNumber').value;
  }

  getIncomingAnswerNumber() {
    if (this.outgoingDetail && this.outgoingDetail.incomingAnswerNumber) {
      return this.outgoingDetail.incomingAnswerNumber;
    } else {
      return this.formGroup.get('incomingAnswerNumber').value;
    }
  }

  private get dictionaries() {
    return Observable.combineLatest(
      this.documentsService.getDocumentsTypesByClassificationCode('02')
    );
  }

  private filterByIcgs() {
    const request = this.owners.find(o => o.ownerType === OwnerType.Request);
    if (!!request) {
      this.documentsService
        .getAreRequestIcgsPaidFor(request.ownerId)
        .takeUntil(this.onDestroy)
        .subscribe((data: boolean) => {
          if (!data) {
            this.docTypesCollection = this.docTypesCollection.filter(
              dt => dt.code !== 'TZPRED1'
            );
          }
        });
    }
  }

  isErrorStatus(statusCode) {
    return statusCode.code !== 'ES';
  }

  ngOnDestroy(): void {
    this.onDestroy.next();
  }

  onSubmit() {
    this.postData();
  }

  hasPayment() {
    return this.paymentInvoiceTypeCodes.includes(this.typeCode);
    /*if (this.typeCode === 'TZPRED1.0' || this.typeCode === 'TZPOL4') {
      return true;
    }
    return false;*/
  }

  postData() {

    if (this.exists()) {
      this.materialsService
        .updateOutgoing(this.prepareFormValue(this.outgoingDetail))
        .takeUntil(this.onDestroy)
        .subscribe((outgoingDetail: OutgoingDetail) => {
          this.outgoingDetail = outgoingDetail;
          this.owners = outgoingDetail.owners;
          this.InitializeFormGroupFromData();
          this.previewMode = !outgoingDetail.wasScanned;
          this.toggleEditMode(false);
        });
    } else {
      this.materialsService
        .createOutgoing(this.prepareFormValue(new OutgoingDetail()))
        .takeUntil(this.onDestroy)
        .subscribe((id: number) => {
          this.router.navigate([
            getDocumentTypeRoute(DocumentType.Outgoing),
            id
          ]);
        }, console.log);
    }
  }

  /**
   * Проверяет, можно ли сменить исполнителя.
   */
  canChangeExecutor() {
    return true;

    const codes: string[] = [
      DicPositionTypeCodes.HeadOfAdministration,
      DicPositionTypeCodes.DeputyHeadOfAdministration,
      DicPositionTypeCodes.HeadOfDepartment
    ];

    const currentPositionTypeCode = this.auth.positionTypeCode;
    const currentUserDepartmentId = this.auth.departmentId;
    const currentUserId = this.auth.userId;

    const workflow = this.workflowBusinessService.getCurrentWorkflow(this.outgoingDetail);

    if (workflow === null) {
      return false;
    }

    if (currentUserId !== workflow.currentUserId) {
      return false;
    }

    if (codes.includes(currentPositionTypeCode)
    && workflow.currentUserDepartmentId.toString() === currentUserDepartmentId) {
      return true;
    }
    if (this.availableOfTransfer) {
      return true;
    }

    return false;
  }

  get userInputFields() {
    return this.formGroup.get('userInputFields') as FormGroup;
  }

  get validForm() {
    return this.formGroup.valid;
  }

  get dirtyForm() {
    return this.formGroup.dirty || this.userInputFields.dirty;
  }

  onEdit() {
    this.toggleEditMode(true);
  }

  filter(name: string): SelectOption[] {
    return this.docTypesCollection.filter(
      option =>
        option.nameRu.toLowerCase().indexOf(name.toString().toLowerCase()) > -1
    );
  }

  onTypeSelect(option) {
    if (option.code) {
      this.selectType(option.code);
    }
  }

  sendMessage(): void {
    // isFromRequest  this.outgoingDetail.owners.length > 0 если нет заявки то отсылать False
    this.documentsService.sendMessage(this.outgoingDetail.barcode, this.outgoingDetail.owners.length > 0)
      .subscribe((result: IServerStatus) => {
        this.processServerStatus(result);
      }, (error: any) => {
        this.snackbarHelper.error(`${error.message}. Просим обратиться к администратору системы.`);
      });
  }

  private processServerStatus(status: IServerStatus) {
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

  isSendButtonDisabled(): boolean {
    return (!this.outgoingDetail.outgoingNumber);
  }

  changeExecutorDialog() {
    const tempDialogRef = this.dialog.open(ChangeExecutorDialogComponent, {
      data: {
        detail: this.outgoingDetail
      },
      width: '700px'
    });

    tempDialogRef.afterClosed().subscribe((newWorkflow: MaterialWorkflow) => {
      if (newWorkflow) {
        this.workflowBusinessService
          .afterUpdateWorkflow(this.outgoingDetail, newWorkflow)
          .takeUntil(this.onDestroy)
          .subscribe((detail: MaterialDetail) => {
            this.setWorkflowDetails(detail);
            this.outgoingDetail.outgoingNumber = detail.outgoingNumber;
            this.outgoingDetail.documentDate = detail.documentDate;
            this.formGroup
              .get('outgoingNumber')
              .setValue(detail.outgoingNumber);
            this.formGroup.get('documentDate').setValue(detail.documentDate);
            this.workflows = Object.assign(
              [],
              this.outgoingDetail.workflowDtos
            );
            this.currentWorkflow = this.workflows.filter(
              d => d.isCurent === true && d.currentUserId === this.auth.userId
            )[0];
            this.isReadOnly = newWorkflow.currentUserId !== this.auth.userId;
            this.setEditableStage();
            this.toggleEditMode(false);
          }, console.log);
      }
    });
  }

  onOpenWorkflowDialog() {
    this.toggleEditMode(false);
    const dialogRef = this.dialog.open(DocumentWorkflowDialogComponent, {
      data: {
        detail: this.outgoingDetail
      },
      width: '700px'
    });

    dialogRef
      .afterClosed()
      .takeUntil(this.onDestroy)
      .subscribe(newWorkflow => {
        if (newWorkflow) {
          this.workflowBusinessService
            .afterCreateWorkflow(this.outgoingDetail, newWorkflow)
            .takeUntil(this.onDestroy)
            .subscribe(detail => {
              this.setWorkflowDetails(detail);
              this.outgoingDetail.outgoingNumber = detail.outgoingNumber;
              this.outgoingDetail.documentDate = detail.documentDate;
              this.formGroup
                .get('outgoingNumber')
                .setValue(detail.outgoingNumber);
              this.formGroup.get('documentDate').setValue(detail.documentDate);
              this.workflows = Object.assign(
                [],
                this.outgoingDetail.workflowDtos
              );
              this.currentWorkflow = this.workflows.filter(
                d => d.isCurent === true && d.currentUserId === this.auth.userId
              )[0];
              this.setEditableStage();
              this.toggleEditMode(false);
            }, console.log);
        }
      });
  }

  private setEditableStage(): void {
    let isEditStage: boolean;
    this.workflowBusinessService
      .isEditStage(
        this.workflowBusinessService.getCurrentWorkflow(this.outgoingDetail)
      )
      .takeUntil(this.onDestroy)
      .subscribe((data: boolean) => {
        isEditStage = data;
        this.isEditableStage = Observable.of(isEditStage);
      });
  }

  formatDate(param: Date): string {
    const date = new Date(param);
    return `${date.toLocaleDateString()} ${date.toLocaleTimeString()}`;
  }

  onBack() {
    if (this.owners.length > 0) {
      this.router.navigate([
        `${getModuleName(this.owners[0].ownerType)}`,
        this.owners[0].ownerId
      ]);
    } else {
      this.location.back();
    }
  }

  onDelete() {
    const dialogRef = this.dialog.open(DeleteDialogComponent);
    dialogRef
      .afterClosed()
      .takeUntil(this.onDestroy)
      .subscribe(result => {
        if (result === 'true') {
          this.materialsService
            .deleteDocument(this.id)
            .takeUntil(this.onDestroy)
            .subscribe(() => {
              this.router.navigate([
                `${getModuleName(this.ownerType)}`,
                this.ownerId
              ]);
            });
        }
      });
  }

  onPreview(wasScanned: boolean) {
    this.documentsService
      .getDocumentPdf(this.id, wasScanned, true)
      .takeUntil(this.onDestroy)
      .subscribe(blob => {
        window.open(window.URL.createObjectURL(blob));
        this.togglePreviewMode(!this.outgoingDetail.wasScanned);
      });
  }

  togglePreviewMode(value: boolean) {
    this.previewMode = value;
  }

  onUndo() {
    if (this.dirtyForm) {
      this.openDialog();
      return false;
    } else {
      if (this.exists()) {
        this.toggleEditMode(false);
      } else {
        if (this.isRegister) {
          this.router.navigate(['journal']);
        } else {
          this.location.back();
        }
      }
    }
  }

  InitializeFormGroupFromData() {
    this.formGroup.reset(this.outgoingDetail);
    this.statusCode = this.outgoingDetail.statusCode;
    const addresseeInfo = new AddresseeInfo();
    addresseeInfo.addresseeId = this.outgoingDetail.addresseeId;

    addresseeInfo.addresseeAddress = this.outgoingDetail.addresseeAddress;
    addresseeInfo.addresseeShortAddress = this.outgoingDetail.addresseeShortAddress;

    addresseeInfo.city = this.outgoingDetail.addresseeCity;
    addresseeInfo.oblast = this.outgoingDetail.addresseeOblast;
    addresseeInfo.republic = this.outgoingDetail.addresseeRepublic;
    addresseeInfo.region = this.outgoingDetail.addresseeRegion;
    addresseeInfo.street = this.outgoingDetail.addresseeStreet;

    addresseeInfo.addresseeXin = this.outgoingDetail.addresseeXin;
    addresseeInfo.addresseeNameRu = this.outgoingDetail.addresseeNameRu;
    addresseeInfo.apartment = this.outgoingDetail.apartment;
    addresseeInfo.email = this.outgoingDetail.addresseeEmail;
    addresseeInfo.contactInfos = this.outgoingDetail.contactInfos;

    if (this.owners.length > 0) {
      this.ownerId = this.owners[0].ownerId;
    }
    this.formGroup.patchValue({
      nameRu: this.outgoingDetail.nameRu,
      nameEn: this.outgoingDetail.nameEn,
      nameKz: this.outgoingDetail.nameKz,
      barcode: this.outgoingDetail.barcode,
      dateCreate: this.outgoingDetail.dateCreate,
      documentDate: this.outgoingDetail.documentDate,
      outgoingNumber: this.outgoingDetail.outgoingNumber,
      wasScanned: this.outgoingDetail.wasScanned,
      pageCount: this.outgoingDetail.pageCount,
      customerSearch: addresseeInfo,
      documentNum: this.outgoingDetail.documentNum,
      ownerNumbers: this.owners,
      sendTypeId: this.outgoingDetail.sendTypeId,
      statusNameRu: this.outgoingDetail.statusNameRu,
      incomingAnswerNumber: this.outgoingDetail.incomingAnswerNumber
    });
    const docType = this.docTypesCollection.find(
      t => t.id === this.outgoingDetail.typeId
    );
    this.formGroup.get('typeId').patchValue(this.outgoingDetail.typeId);

    if (docType) {
      this.selectType(docType.code);
    }
    this.workflows = this.outgoingDetail.workflowDtos || this.workflows;
    this.currentWorkflow = this.workflows.filter(
      d => d.isCurent === true && d.currentUserId === this.auth.userId
    )[0];
    this.availableOfTransfer = this.workflowBusinessService.availableOfTransfer(
      this.outgoingDetail
    );
    this.setEditableStage();

    this.setNumbers();

    this.comments = [...this.outgoingDetail.commentDtos];
    this.parentLinks = [...this.outgoingDetail.documentParentLinkDtos];
    this.links = [...this.outgoingDetail.documentLinkDtos];
    this.removeLinks = [];
    this.isReadOnly = this.outgoingDetail.isReadOnly;
  }

  exists(): boolean {
    return !!this.id;
  }

  onShowAddressClick() {
    this.requestService
      .generatePrintAddressee(this.outgoingDetail.id, OwnerType.Material)
      .takeUntil(this.onDestroy)
      .subscribe(blob => {
        window.open(window.URL.createObjectURL(blob));
      });
  }

  isInitialStage(): boolean {
    if (this.exists() && this.outgoingDetail) {
      return this.workflowBusinessService.isInitialStage(
        this.workflowBusinessService.getCurrentWorkflow(this.outgoingDetail)
      );
    }
    return false;
  }

  isSendStage(): boolean {
    if (this.exists() && this.outgoingDetail) {
      return this.workflowBusinessService.isSendStage(
        this.workflowBusinessService.getCurrentWorkflow(this.outgoingDetail)
      );
    }
    return false;
  }

  canAttach(): boolean {
    if (this.exists() && this.outgoingDetail) {
      return this.workflowBusinessService.isInitialStage(
        this.workflowBusinessService.getCurrentWorkflow(this.outgoingDetail)
      );
    }
    return this.isRegister();
  }

  hasAttachment(): boolean {
    if (this.exists() && this.outgoingDetail) {
      return !!this.outgoingDetail.attachment;
    }
  }

  onSingleFileSelect() {
    for (let i = 0; i < this.singleinputFile.nativeElement.files.length; i++) {
      // TODO: интеграция со сканером
      const newAttachment = new Attachment(
        -1,
        1,
        1,
        this.singleinputFile.nativeElement.files[i].name,
        this.singleinputFile.nativeElement.files[i],
        this.singleinputFile.nativeElement.files[i].type
      );
      newAttachment.isMain = true;
      this.outgoingDetail.attachment = newAttachment;
      this.materialsService
        .replaceAttachment(this.outgoingDetail)
        .takeUntil(this.onDestroy)
        .subscribe((data: MaterialDetail) => {
          this.formGroup.patchValue({
            pageCount: data.attachment.pageCount,
            copyCount: data.attachment.copyCount
          });
          this.outgoingDetail.wasScanned = data.wasScanned;
          this.toggleEditMode(false);
          this.reloadAttachments.emit();
          this.formGroup.markAsPristine();
        });
    }
  }

  onUnpinClick() {
    const dialogRef = this.dialog.open(DeleteDialogComponent);
    dialogRef
      .afterClosed()
      .takeUntil(this.onDestroy)
      .subscribe(result => {
        if (result === 'true') {
          this.materialsService
            .deleteAttachment(this.id, true)
            .takeUntil(this.onDestroy)
            .subscribe(() => {
              this.outgoingDetail.wasScanned = false;
              this.formGroup.patchValue({
                pageCount: 0,
                copyCount: 1,
              });
              this.toggleEditMode(false);
              this.reloadAttachments.emit();
            });
        }
      });
  }

  isScanStage(): boolean {
    if (this.exists()) {
      return this.workflowBusinessService.isScanStage(
        this.workflowBusinessService.getCurrentWorkflow(this.outgoingDetail)
      );
    }
  }

  isGenerateNumberDisabled(): boolean {
    if (!this.formGroup) {
      return true;
    }
    if (this.formGroup.get('outgoingNumber').value) {
      return true;
    }
    return !this.isSendStage();
  }

  /**
   * Генерирует номер для исходящего документа и изменяет его статус.
   */
  onGenerateOutgoingNumberClick() {
    this.materialsService
      .generateOutgoingNumber(this.id)
      .takeUntil(this.onDestroy)
      .subscribe(result => {
        this.outgoingDetail.outgoingNumber = result.outgoingNumber;
        this.formGroup.get('outgoingNumber').setValue(result.outgoingNumber);
        this.outgoingDetail.documentDate = result.sendingDate;
        this.formGroup.get('documentDate').setValue(result.sendingDate);
        if (this.formGroup.pristine) {
          this.toggleEditMode(false);
        }
        this.subscribeNotificationStatus();

        this.materialsService
                .getDocumentStatus(this.id)
                .takeUntil(this.onDestroy)
                .subscribe(status => {
                  this.formGroup.patchValue({ statusNameRu: status.nameRu });
                  this.outgoingDetail.statusNameRu = status.nameRu;
                });
      });
  }

  isRegister(): boolean {
    if (!this.formGroup) {
      return false;
    }
    if (!this.docTypesCollection) {
      return false;
    }
    const type = this.formGroup.get('typeId').value;
    if (!type) {
      return false;
    }
    return this.registerCodes.includes(type.code);
  }

  commentAdded(comment: DocumentCommentDto) {
    if (this.outgoingDetail && this.currentWorkflow) {
      comment.documentId = this.outgoingDetail.id;
      comment.workflowId = this.currentWorkflow.id;
    }
    this.comments = [...this.comments, comment];
  }

  private openDialog() {
    const dialogRef = this.dialog.open(UndoDialogComponent);

    dialogRef
      .afterClosed()
      .takeUntil(this.onDestroy)
      .subscribe(result => {
        if (result === 'true') {
          if (this.exists()) {
            this.toggleEditMode(false);
            this.InitializeFormGroupFromData();
          } else {
            if (this.isRegister) {
              this.router.navigate(['journal']);
            } else {
              this.location.back();
            }
          }
        }
      });
  }

  private setWorkflowDetails(detail: MaterialDetail) {
    // this.outgoingDetail.currentWorkflowId = detail.currentWorkflowId;
    this.outgoingDetail.workflowDtos = detail.workflowDtos;
    this.availableOfTransfer = this.workflowBusinessService.availableOfTransfer(
      this.outgoingDetail
    );
  }

  private updateMandatoryOf(control: AbstractControl, clearMandatory: boolean) {
    if (this.getControlName(control) === 'nameRu') {
      control.clearValidators();
      control.reset(control.value);
      return;
    }

    clearMandatory
      ? control.clearValidators()
      : control.setValidators(Validators.required);
    control.reset(control.value);
  }

  private getControlName(control: AbstractControl): string {
    let controlName = null;
    const parent = control.parent;
    Object.keys(parent.controls).forEach(name => {
      if (control === parent.controls[name]) {
        controlName = name;
      }
    });
    return controlName;
  }

  private selectType(code: string) {
    this.typeCode = code;
    this.formGroup.get('code').setValue(code);
    this.documentsService.getUserInputFields(code).subscribe(data => {
      if (data.requireUserInput) {
        this.userInputFormConfig = data.fieldsConfig;
        this.buildUserInputFields();
        if (this.exists() && this.outgoingDetail.userInput) {
          if (
            this.outgoingDetail.typeId === this.formGroup.get('typeId').value.id
          ) {
            this.outgoingDetail.userInput.fields.forEach(f => {
              this.userInputFields.controls[f.key].setValue(f.value);
            });
          } else {
            this.outgoingDetail.userInput.fields = [];
            this.outgoingDetail.userInput.code = '';
          }
        }
      }
    }, console.log);
  }

  private buildForm() {
    this.formGroup = this.fb.group({
      code: ['', Validators.required],
      userInputFields: this.fb.group([]),
      typeId: ['', Validators.required],
      ownerNumbers: [''],
      customerSearch: [''],
      barcode: [{ value: '', disabled: true }],
      documentDate: [{ value: '', disabled: true }],
      pageCount: [{ value: '', disabled: true }],
      dateCreate: [{ value: '', disabled: true }],
      outgoingNumber: [{ value: '', disabled: true }],
      wasScanned: [{ value: '', disabled: true }],
      nameRu: [{ value: '', disabled: true }],
      nameEn: [{ value: '', disabled: true }],
      nameKz: [{ value: '', disabled: true }],
      documentNum: [{ value: '', disabled: true }],
      sendTypeId: ['', Validators.required],
      numberForPayment: [{ value: '', disabled: true }],
      paymentDate: [{ value: '', disabled: true }],
      paymentInvoiceId: [{ value: '', disabled: true }],
      paymentInvoiceCode: [{ value: '', disabled: true }],
      incomingDocumentNumber: [{ value: '', disabled: true }],
      trackNumber: [{ value: '', disabled: true }],
      incomingAnswerNumber: [{ value: '', disabled: true }],
      incomingAnswerId: [{ value: '', disabled: true }],
      statusNameRu: [{ value: '', disabled: true }]
    });

    this.editableControls = [
      'typeId',
      'nameRu',
      'nameEn',
      'nameKz',
      'ownerNumbers',
      'pageCount',
      'incomingDocumentNumber',
      'trackNumber'
    ];
    this.scanUnavailableControls = ['typeId', 'nameRu', 'nameEn', 'nameKz'];
    this.scanAvailableControls = [];
    this.comments = [];
    this.parentLinks = [];
    this.links = [];
    this.removeLinks = [];
    this.initialUnavailableControls = ['typeId'];
    this.registerEditableControls = ['customerSearch', 'sendTypeId'];

    const nameRu = this.formGroup.get('nameRu');
    const nameKz = this.formGroup.get('nameKz');
    const nameEn = this.formGroup.get('nameEn');
    Observable.merge(
      nameKz.valueChanges,
      nameEn.valueChanges,
      nameRu.valueChanges
    )
      .takeUntil(this.onDestroy)
      .filter(
        value =>
          nameRu.dirty ||
          nameRu.touched ||
          nameKz.dirty ||
          nameKz.touched ||
          nameEn.dirty ||
          nameEn.touched
      )
      .switchMap(
        (triggered): Observable<boolean> =>
          Observable.of(!!nameKz.value || !!nameEn.value || !!nameRu.value)
      )
      .distinctUntilChanged()
      .subscribe(atLeastOneHasValue => {
        this.updateMandatoryOf(nameRu, atLeastOneHasValue);
      });
  }

  linkAdded(comment: DocumentLinkDto) {
    this.links = [...this.links, comment];
  }

  linkRemove(comment: DocumentLinkDto) {
    this.links = [...this.links.filter(d => d.childDocumentId !== comment.childDocumentId)];
    comment.needRemove = true;
    if (comment.id > 0) {
      this.removeLinks = [...this.removeLinks, comment];
    }
  }

  onSelectAnswerNumberClick() {
    const dialogRef = this.dialog.open(IncomingAnswerComponent, {
      width: '880px'
    });

    dialogRef.afterClosed().subscribe((materialTask: MaterialTask) => {
      if (materialTask) {
        this.formGroup.get('incomingAnswerId').setValue(materialTask.id);
        const incomingAnswerNumber =
          materialTask.incomingNumber + ' ' + materialTask.typeNameRu;
        this.outgoingDetail.incomingAnswerNumber = incomingAnswerNumber;
        this.formGroup
          .get('incomingAnswerNumber')
          .setValue(incomingAnswerNumber);
      }
    });
  }

  get statusNameRu() {
    return this.outgoingDetail ? this.outgoingDetail.statusNameRu : 'Создание';
  }

  incomingAnswerNumberOnClick() {
    const incomingId = this.formGroup.get('incomingAnswerId').value;
    if (incomingId > 0) {
      this.router.navigate([
        getDocumentTypeRoute(DocumentType.Incoming),
        incomingId
      ]);
    }
  }

  removeIncomingAnswerNumber() {
    this.formGroup.get('incomingAnswerId').setValue(null);
    this.outgoingDetail.incomingAnswerNumber = '';
    this.formGroup.get('incomingAnswerNumber').setValue('');
  }

  onAddPaymentInvoiceClick() {
    const ownerNumbers = this.formGroup.get('ownerNumbers').value;
    if (!ownerNumbers) {
      return;
    }
    const selectedOwners = ownerNumbers.owners;
    if (selectedOwners.length !== 1) {
      return;
    }

    const selectedOwner = selectedOwners[0];
    const dialogRef = this.dialog.open(SearchPaymentsComponent, {
      data: {
        ownerId: selectedOwner.ownerId,
        ownerType: selectedOwner.ownerType,
        protectionDocTypeId: selectedOwner.protectionDocTypeId
      },
      width: '1000px'
    });

    dialogRef
      .afterClosed()
      .takeUntil(this.onDestroy)
      .filter(invoice => !!invoice)
      .subscribe(invoice => {
        this.formGroup.get('paymentInvoiceId').setValue(invoice.id);
        this.formGroup.get('paymentInvoiceCode').setValue(invoice.tariffCode);
      });
  }

  private buildUserInputFields() {
    this.formGroup.controls['userInputFields'] = this.fb.group([]);
    const group: any = {};

    if (this.userInputFormConfig.length > 0) {
      this.userInputFormConfig.forEach(fieldConfig => {
        group[fieldConfig.key] = fieldConfig.required
          ? new FormControl(fieldConfig.value || '', Validators.required)
          : new FormControl(fieldConfig.value || '');
      });
    }
    this.formGroup.controls['userInputFields'] = this.fb.group(group);
  }

  private prepareFormValue(outgoingDetail: OutgoingDetail) {
    const addresseeInfo = this.formGroup.get('customerSearch').value;
    this.owners = this.formGroup.get('ownerNumbers').value.owners || [];
    if (this.exists()) {
      outgoingDetail.id = this.id;
    }
    const firstOwner = !!this.owners.length ? this.owners[0] : null;
    const input: UserInputDto = new UserInputDto(
      this.formGroup.get('code').value,
      !!firstOwner ? firstOwner.ownerId : 0,
      this.outgoingDetail.id,
      'request',
      this.owners.map(o => o.ownerId),
      !!firstOwner ? firstOwner.ownerType : OwnerType.None
    );
    if (
      this.outgoingDetail &&
      this.outgoingDetail.userInput &&
      this.outgoingDetail.userInput.index
    ) {
      input.index = this.outgoingDetail.userInput.index;
    }
    const keys = Object.keys(this.userInputFields.value);
    keys.forEach(k => {
      input.fields.push({ key: k, value: this.userInputFields.value[k] });
    });
    Object.assign(outgoingDetail, this.formGroup.getRawValue());
    outgoingDetail.addresseeShortAddress = addresseeInfo.addresseeShortAddress;
    outgoingDetail.addresseeAddress = addresseeInfo.addresseeAddress;
    outgoingDetail.apartment = addresseeInfo.apartment;
    outgoingDetail.addresseeId = addresseeInfo.addresseeId;
    outgoingDetail.contactInfos = addresseeInfo.contactInfos;
    outgoingDetail.typeId = this.formGroup.get('typeId').value.id;
    outgoingDetail.userInput = input;
    outgoingDetail.owners = this.owners;

    outgoingDetail.commentDtos = this.comments;
    outgoingDetail.documentLinkDtos = [...this.links, ...this.removeLinks];

    const addres = this.formGroup.get('customerSearch').value;
    outgoingDetail.contactInfos = addres.contactInfos;

    return outgoingDetail;
  }

  private toggleEditMode(value: boolean) {
    this.editMode = value;

    this.userInputFormConfig.forEach(config => {
      value
        ? this.isInitialStage()
          ? (config.disabled = false)
          : (config.disabled = true)
        : (config.disabled = true);
    });

    value
      ? this.isInitialStage()
        ? this.userInputFields.enable()
        : this.userInputFields.disable()
      : this.userInputFields.disable();

    this.editableControls.forEach(c => {
      value
        ? this.formGroup.controls[c].enable()
        : this.formGroup.controls[c].disable();
    });
    if (this.isScanStage()) {
      this.scanAvailableControls.forEach(c => {
        value
          ? this.formGroup.controls[c].enable()
          : this.formGroup.controls[c].disable();
      });
      this.scanUnavailableControls.forEach(c => {
        this.formGroup.controls[c].disable();
      });
    }
    if (this.isInitialStage()) {
      this.initialUnavailableControls.forEach(c => {
        this.formGroup.controls[c].disable();
      });
    }
    if (this.isRegister) {
      this.registerEditableControls.forEach(c => {
        value
          ? this.formGroup.controls[c].enable()
          : this.formGroup.controls[c].disable();
      });
    }
    if (this.ownerType === OwnerType.Contract) {
      value
        ? this.formGroup.get('customerSearch').enable()
        : this.formGroup.get('customerSearch').disable();
    }

    this.updateMandatoryOf(
      this.formGroup.get('nameRu'),
      !!this.outgoingDetail.nameKz || !!this.outgoingDetail.nameEn
    );
  }

  private setNumbers() {
    let ownerType;
    if (this.ownerType) {
      ownerType = this.ownerType;
    } else if (this.outgoingDetail.ownerType) {
      ownerType = this.outgoingDetail.ownerType;
    } else {
      return;
    }
    switch (ownerType) {
      case OwnerType.Request: {
        this.setRequestNumbersAndSendType();
        break;
      }
      case OwnerType.Contract: {
        this.setContractNumbersAndSendType();
        break;
      }
      case OwnerType.ProtectionDoc: {
        this.setProtectionDocNumbers();
        break;
      }
      default: {
        break;
      }
    }
  }

  private setRequestNumbersAndSendType() {
    const requests$ = this.owners
      .filter(o => o.ownerType === OwnerType.Request)
      .map(owner => this.requestService.getRequestById(owner.ownerId));
    Observable.combineLatest(requests$)
      .takeUntil(this.onDestroy)
      .subscribe(
        (requests: RequestDetails[]) => {
          if (requests.length > 0) {
            this.subjectsService
              .get(this.owners[0].ownerId, OwnerType.Request)
              .takeUntil(this.onDestroy)
              .subscribe((subjects: SubjectDto[]) => {
                const customer =
                  subjects.findIndex(s => s.roleCode === 'CORRESPONDENCE') > -1
                    ? subjects.find(s => s.roleCode === 'CORRESPONDENCE')
                    : subjects.find(s => s.roleCode === '1');
                if (customer) {
                  if (!this.exists()) {
                    const addresseeInfo = new AddresseeInfo();
                    addresseeInfo.addresseeId = customer.customerId;
                    addresseeInfo.addresseeShortAddress = customer.shortAddress;
                    addresseeInfo.addresseeAddress = customer.address;
                    addresseeInfo.addresseeXin = customer.xin;
                    addresseeInfo.addresseeNameRu = customer.nameRu;
                    addresseeInfo.apartment = customer.apartment;
                    addresseeInfo.email = customer.email;
                    addresseeInfo.city = customer.city;
                    addresseeInfo.oblast = customer.oblast;
                    addresseeInfo.republic = customer.republic;
                    addresseeInfo.region = customer.region;
                    addresseeInfo.street = customer.street;
                    addresseeInfo.contactInfos = customer.contactInfos;
                    this.formGroup
                      .get('customerSearch')
                      .patchValue(addresseeInfo);
                  }
                } else {
                  if (!this.exists()) {
                    const addresseeInfo = new AddresseeInfo();
                    addresseeInfo.addresseeId = requests[0].addresseeId;
                    addresseeInfo.addresseeShortAddress = requests[0].addresseeShortAddress;
                    addresseeInfo.addresseeAddress = requests[0].addresseeAddress;
                    addresseeInfo.addresseeXin = requests[0].addresseeXin;
                    addresseeInfo.addresseeNameRu = requests[0].addresseeNameRu;
                    addresseeInfo.apartment = requests[0].apartment;
                    addresseeInfo.email = requests[0].addresseeEmail;
                    this.formGroup
                      .get('customerSearch')
                      .patchValue(addresseeInfo);
                  }
                }
              });

            if (!this.exists()) {
              this.formGroup
                .get('sendTypeId')
                .patchValue(requests[0].receiveTypeId);
            }
          }
        },
        err => {
          this.formGroup.get('ownerNumbers').setValue(null);
          this.formGroup.get('customerSearch').patchValue('');
        }
      );
  }

  private setContractNumbersAndSendType() {
    const requests$ = this.owners
      .filter(o => o.ownerType === OwnerType.Contract)
      .map(owner => this.contractService.getById(owner.ownerId));
    Observable.combineLatest(requests$)
      .takeUntil(this.onDestroy)
      .subscribe(
        (contracts: ContractDetails[]) => {
          if (contracts.length > 0) {
            this.subjectsService
              .get(this.owners[0].ownerId, OwnerType.Contract)
              .takeUntil(this.onDestroy)
              .subscribe((subjects: SubjectDto[]) => {
                const customer =
                  subjects.findIndex(s => s.roleCode === 'CORRESPONDENCE') > -1
                    ? subjects.find(s => s.roleCode === 'CORRESPONDENCE')
                    : subjects.find(s => s.roleCode === '1');
                if (customer && !this.exists()) {
                  const addresseeInfo = new AddresseeInfo();
                  addresseeInfo.addresseeId = customer.customerId;
                  addresseeInfo.addresseeShortAddress = customer.shortAddress;
                  addresseeInfo.addresseeAddress = customer.address;
                  addresseeInfo.addresseeXin = customer.xin;
                  addresseeInfo.addresseeNameRu = customer.nameRu;
                  addresseeInfo.apartment = customer.apartment;
                  addresseeInfo.apartment = customer.email;
                  addresseeInfo.city = customer.city;
                  addresseeInfo.oblast = customer.oblast;
                  addresseeInfo.republic = customer.republic;
                  addresseeInfo.region = customer.region;
                  addresseeInfo.street = customer.street;
                  addresseeInfo.contactInfos = customer.contactInfos;
                  this.formGroup
                    .get('customerSearch')
                    .patchValue(addresseeInfo);
                }
              });

            if (!this.exists()) {
              this.formGroup
                .get('sendTypeId')
                .patchValue(contracts[0].receiveTypeId);
            }
          }
        },
        err => {
          this.formGroup.get('ownerNumbers').setValue(null);
          this.formGroup.get('customerSearch').patchValue('');
        }
      );
  }

  private setProtectionDocNumbers() {
    const protectionDocs$ = this.owners
      .filter(o => o.ownerType === OwnerType.ProtectionDoc)
      .map(owner => this.protectionDocService.get(owner.ownerId));
    Observable.combineLatest(protectionDocs$)
      .takeUntil(this.onDestroy)
      .subscribe(
        (protectionDoc: ProtectionDocDetails[]) => {
          if (protectionDoc.length > 0) {
            this.subjectsService
              .get(this.owners[0].ownerId, OwnerType.ProtectionDoc)
              .takeUntil(this.onDestroy)
              .subscribe((subjects: SubjectDto[]) => {
                const customer =
                  subjects.findIndex(s => s.roleCode === 'CORRESPONDENCE') > -1
                    ? subjects.find(s => s.roleCode === 'CORRESPONDENCE')
                    : subjects.find(s => s.roleCode === '2');
                if (customer && !this.exists()) {
                  const addresseeInfo = new AddresseeInfo();
                  addresseeInfo.addresseeId = customer.customerId;
                  addresseeInfo.addresseeShortAddress = customer.shortAddress;
                  addresseeInfo.addresseeAddress = customer.address;
                  addresseeInfo.addresseeXin = customer.xin;
                  addresseeInfo.addresseeNameRu = customer.nameRu;
                  addresseeInfo.apartment = customer.apartment;
                  addresseeInfo.email = customer.email;
                  addresseeInfo.city = customer.city;
                  addresseeInfo.oblast = customer.oblast;
                  addresseeInfo.republic = customer.republic;
                  addresseeInfo.region = customer.region;
                  addresseeInfo.street = customer.street;
                  addresseeInfo.contactInfos = customer.contactInfos;
                  this.formGroup
                    .get('customerSearch')
                    .patchValue(addresseeInfo);
                }
              });
          }
        },
        err => {
          this.formGroup.get('ownerNumbers').setValue(null);
          this.formGroup.get('customerSearch').patchValue('');
        }
      );
  }

  private subscribeNotificationStatus() {
    this.notificationsService
      .getStatusList(OwnerType.Material, this.id)
      .subscribe(data => (this.notificationStatuses = data));
  }

  haveOutgoingNumber(): boolean {
    if (this.exists() && !!this.outgoingDetail) {
      return this.outgoingDetail.code !== 'DK_ZAKLUCHENIE';
    } else {
      return true;
    }
  }
}
