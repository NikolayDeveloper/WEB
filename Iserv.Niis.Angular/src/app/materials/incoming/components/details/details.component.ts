import {
  Component,
  ElementRef,
  EventEmitter,
  Input,
  OnDestroy,
  OnInit,
  Output,
  ViewChild
} from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatCheckboxChange, MatDialog, MatSort } from '@angular/material';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { SettingsType } from 'app/shared/services/models/settings-type';
import { SystemSettingsService } from 'app/shared/services/system-settings.service';
import { AddresseeInfo } from 'app/subjects/components/subjects-search-form/subjects-search-form.component';
import { Observable } from 'rxjs/Observable';
import { Subject } from 'rxjs/Subject';
import { ContractService } from '../../../../contracts/contract.service';
import { ContractDetails } from '../../../../contracts/models/contract-details';
import { ProtectionDocDetails } from '../../../../protection-docs/models/protection-doc-details';
import { ProtectionDocsService } from '../../../../protection-docs/protection-docs.service';
import { RequestDetails } from '../../../../requests/models/request-details';
import { RequestService } from '../../../../requests/request.service';
import { AuthenticationService } from '../../../../shared/authentication/authentication.service';
import { Config } from '../../../../shared/components/table/config.model';
import { UndoDialogComponent } from '../../../../shared/components/undo-dialog/undo-dialog.component';
import { DictionaryService } from '../../../../shared/services/dictionary.service';
import { DocumentsService } from '../../../../shared/services/documents.service';
import {
  DicDepartment,
  DicDivision,
  DicDocumentType,
  DicProtectionDocType,
  DicReceiveType
} from '../../../../shared/services/models/base-dictionary';
import { DictionaryType } from '../../../../shared/services/models/dictionary-type.enum';
import {
  getModuleName,
  OwnerType
} from '../../../../shared/services/models/owner-type.enum';
import { SelectOption } from '../../../../shared/services/models/select-option';
import { MaterialWorkflow } from '../../../../shared/services/models/workflow-model';
import { moment } from '../../../../shared/shared.module';
import { ChangeExecutorDialogComponent } from '../../../components/change-executor-dialog/change-executor-dialog.component';
import { DeleteDialogComponent } from '../../../components/delete-dialog/delete-dialog.component';
import { DocumentWorkflowDialogComponent } from '../../../components/workflow-dialog/workflow-dialog.component';
import {
  Attachment,
  DocumentCommentDto,
  IncomingDetail,
  MaterialDetail,
  MaterialOwnerDto,
  DocumentLinkDto
} from '../../../models/materials.model';
import { MaterialsService } from '../../../services/materials.service';
import { WorkflowBusinessService } from '../../../services/workflow-business/workflow-business.service';
import { startWith, map } from 'rxjs/operators';
import { StateService } from 'app/shared/services/state.service';

@Component({
  selector: 'app-details',
  templateUrl: './details.component.html',
  styleUrls: ['./details.component.scss']
})
export class IncomingMaterialDetailsComponent implements OnInit, OnDestroy {
  workflows: MaterialWorkflow[];
  currentWorkflow: MaterialWorkflow;
  comments: DocumentCommentDto[];
  links: DocumentLinkDto[];
  parentLinks: DocumentLinkDto[];
  removeLinks: DocumentLinkDto[];
  currentStageCode: string;
  columns: Config[] = [
    new Config({ columnDef: 'routeNameRu', header: 'Маршрут', class: 'width-100' }),
    new Config({ columnDef: 'currentUserNameRu', header: 'Пользователь', class: 'width-100' }),
    new Config({ columnDef: 'dateCreate', header: 'Дата', class: 'width-200' }),
    new Config({ columnDef: 'currentStageNameRu', header: 'Этап', class: 'width-100' }),
    new Config({ columnDef: 'description', header: 'Описание', class: 'width-200' })
  ];
  formGroup: FormGroup;
  dicReceiveTypes: SelectOption[];
  dicDepartments: DicDepartment[];
  dicProtectionDocTypes: DicProtectionDocType[];
  departments: DicDepartment[];
  filteredDepartments: Observable<SelectOption[]>;
  docTypes: Observable<SelectOption[]>;
  dicDivisions: SelectOption[];
  editMode = false;
  editableControls: string[];
  blockedOnCreationControls: string[];
  blockedOnInitialControls: string[];
  availableAtStage: any;
  availableOfTransfer: any;
  docTypesCollection: SelectOption[];

  ownerDetailsList: MaterialOwnerDto[] = [];
  id: number;
  receiveTypeId:number;
  incomingDetail: IncomingDetail;
  isEditableStage: any;
  ownerType: OwnerType;
  ownerId: number;
  controlMarkChecked = false;
  outOfControlChecked = false;
  isReadOnly = false;

  maxProlongationMonths: number;

  private onDestroy = new Subject();
  private incomingDetailFetched = new Subject();

  noAttachments = false;

  @Input() index: number;
  @Input() incomingAttachment: Attachment;
  @Output() edit: EventEmitter<boolean> = new EventEmitter();
  @Output() valid: EventEmitter<any> = new EventEmitter();
  @Output() submitData: EventEmitter<IncomingDetail> = new EventEmitter();

  @ViewChild(MatSort) sort: MatSort;
  @ViewChild('inputSingleFile') singleinputFile: ElementRef;

  reloadAttachments = new EventEmitter();

  constructor(
    private fb: FormBuilder,
    private dialog: MatDialog,
    private workflowBusinessService: WorkflowBusinessService,
    private dictionaryService: DictionaryService,
    private requestService: RequestService,
    private contractService: ContractService,
    private auth: AuthenticationService,
    private protectionDocService: ProtectionDocsService,
    private materialsService: MaterialsService,
    private route: ActivatedRoute,
    private router: Router,
    private documentsService: DocumentsService,
    private systemSettingsService: SystemSettingsService,
    private stateService: StateService
  ) {
    this.buildForm();
  }

  ngOnInit() {
    this.incomingDetailFetched.takeUntil(this.onDestroy).subscribe(() => {
      this.isEditableStage = this.workflowBusinessService.isEditStage(
        this.workflowBusinessService.getCurrentWorkflow(this.incomingDetail)
      );
    });

    this.systemSettingsService
      .getSettingsByType(SettingsType.MaxProlongationMonths)
      .takeUntil(this.onDestroy)
      .subscribe(data => (this.maxProlongationMonths = +data));

    this.route.params
      .switchMap(
        (params: Params): Observable<any> => {
          this.id = +params['id'];
          this.ownerType = +params['ownerType'];
          this.ownerId = +params['ownerId'];
          this.controlMarkChecked = false;

          return this.dictionaries.switchMap(
            ([
              departments,
              divisions,
              receivetypes,
              docTypes,
              incomingDocTypes
            ]: [
              DicDepartment[],
              DicDivision[],
              DicReceiveType[],
              DicProtectionDocType[],
              DicDocumentType[]
            ]) => {
              this.dicDepartments = departments;
              this.departments = departments;
              this.dicDivisions = divisions;
              this.dicReceiveTypes = receivetypes;
              this.dicProtectionDocTypes = docTypes;

              this.docTypesCollection = incomingDocTypes;

              this.filteredDepartments = this.formGroup.get('departmentId').valueChanges
                .pipe(
                  startWith(''),
                  map(value => typeof value === 'string' ? value : value ? (value as SelectOption).nameRu : null),
                  map(name => name ? this.filterDepartments(name) : this.departments ? this.departments.slice() : [])
                );

              if (!this.exists()) {
                const filteredDivisions = this.dicDivisions.filter(
                  d => d.code === '000001'
                );
                const filteredReceiveTypes = this.dicReceiveTypes.filter(
                  d => d.code === '4'
                );
                const incomingDetail = new IncomingDetail();
                this.formGroup.patchValue({
                  divisionId:
                    filteredDivisions.length > 0 ? filteredDivisions[0].id : '',
                  receiveTypeId:
                    filteredReceiveTypes.length > 0
                      ? filteredReceiveTypes[0].id
                      : ''
                });
                incomingDetail.divisionId =
                  filteredDivisions.length > 0 ? filteredDivisions[0].id : 0;
                this.initializeFormGroup();
                this.materialsService
                  .getIncomingNumbers(incomingDetail)
                  .takeUntil(this.onDestroy)
                  .subscribe(numbers => {
                    this.formGroup.patchValue(numbers);
                  });
                return Observable.of(null);
              } else {
                return this.materialsService.getSingleIncoming(this.id);
              }
            }
          );
        }
      )
      .takeUntil(this.onDestroy)
      .filter(incomingDetail => !!incomingDetail)
      .subscribe((incomingDetail: IncomingDetail) => {
        this.setIncomingData(incomingDetail);
        this.InitializeFormGroupFromData();
        if (this.incomingDetail.workflowDtos) {
          this.workflows = this.incomingDetail.workflowDtos;
          this.currentWorkflow = this.workflows.filter(
            d => d.isCurent === true && d.currentUserId === this.auth.userId
          )[0];
          this.currentStageCode = this.incomingDetail.workflowDtos.filter(
            d => d.id === this.currentWorkflow.id
          )[0].currentStageCode;
        }


        this.controlMarkChecked = incomingDetail.controlMark;
        this.incomingDetailFetched.next();
      });

    this.toggleEditMode(!this.id);
  }

  /**
   * Фильтрует массив `departments` по полю `nameRu`
   * @param name Что искать в поле `nameRu`
   * @return Отфильтрованный массив
   */
  private filterDepartments(name: string): SelectOption[] {
    const filterValue = name.toLowerCase();

    return this.departments.filter(option => option.nameRu.toLowerCase().includes(filterValue));
  }

  getDisplayDepartment() {
    return (value) => this.displayDepartment(value);
  }

  displayDepartment(departmentId): string | undefined {
    if (this.departments) {
      const department = this.departments.find(entry => entry.id === departmentId);

      return department ? department.nameRu : undefined;
    }

    return undefined;
  }

  createOutgoing() {
    const typeId = this.formGroup.get('typeId').value;

    this.stateService.saveState('incomingDetail', {
      customerSearch: this.formGroup.get('customerSearch').value,
      sendTypeId: this.formGroup.get('receiveTypeId').value,
      incomingAnswerId: this.id,
      incomingAnswerNumber: `${this.formGroup.get('incomingNumber').value} ${typeId.nameRu}`
    });

    this.router.navigate(['materials', 'outgoing', 'create', '0']);
  }

  onAttachmentsLoad(attachmentsCount) {
    this.noAttachments = attachmentsCount === 0;
  }

  private get dictionaries() {
    return Observable.combineLatest(
      this.dictionaryService.getBaseDictionary(DictionaryType.DicDepartment),
      this.dictionaryService.getBaseDictionary(DictionaryType.DicDivision),
      this.dictionaryService.getBaseDictionary(DictionaryType.DicReceiveType),
      this.dictionaryService.getBaseDictionary(
        DictionaryType.DicProtectionDocType
      ),
      this.documentsService.getDocumentsTypesByClassificationCode('01')
    );
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
      if (!this.exists()) {
        if (this.ownerId) {
          this.router.navigate([
            `${getModuleName(this.ownerType)}`,
            this.ownerId
          ]);
        } else {
          this.router.navigate(['journal']);
        }
      } else {
        this.toggleEditMode(false);
      }
    }
  }

  done() {
    this.materialsService
      .doneMaterial(this.incomingDetail)
      .takeUntil(this.onDestroy)
      .subscribe(data => {
        this.incomingDetail = data;
        this.InitializeFormGroupFromData();
        this.toggleEditMode(false);
      });
  }

  onEdit() {
    this.toggleEditMode(true);
  }

  onAttach(data) {
    const mapper = (data) => {
      const result: AddresseeInfo = {
        addresseeId: data.id,
        addresseeXin: data.xin,
        addresseeNameRu: data.nameRu,
        addresseeAddress: data.address,
        addresseeShortAddress: data.shortAddress,
        apartment: data.apartment,
        email: data.email,
        republic: data.republic,
        oblast: data.oblast,
        region: data.region,
        city: data.city,
        street: data.street,
        contactInfos: data.contactInfos
      };
      return result;
    };

    const isEmpty = Object.values(this.formGroup.get('customerSearch').value).every((entry) => !entry);
    if (data && data.length === 1) {
      if (isEmpty) {
        this.formGroup.get('customerSearch').patchValue(mapper(data[0]));
      }
    } else {
      this.formGroup.get('customerSearch').patchValue({});
    }
  }

  onSubmit() {
    if (this.exists()) {
      this.materialsService
        .updateIncoming(this.getIncomingData(this.incomingDetail))
        .takeUntil(this.onDestroy)
        .subscribe(data => {
          this.incomingDetail = data;
          this.InitializeFormGroupFromData();
          this.toggleEditMode(false);
        });
    } else {
      const incomingDetail = this.getIncomingData(new IncomingDetail());
      incomingDetail.ownerType = this.ownerType;
      this.materialsService
        .createIncoming(incomingDetail)
        .takeUntil(this.onDestroy)
        .subscribe((id: number) => {
          if (id) {
            this.router.navigate([`materials/incoming`, id]);
          }
        });
    }
  }

  isProlongationPetition(): boolean {
    let result = false;
    if (this.formGroup && this.docTypesCollection) {
      const typeId = this.formGroup.get('typeId').value.id;
      const type = this.docTypesCollection.find(doc => doc.id === typeId);
      if (type) {
        result = ['001.004A_4', '001.004G_3', '001.004A_5'].includes(type.code);
      }
    }
    if (result) {
      this.formGroup
        .get('attachedPaymentsCount')
        .setValidators(Validators.required);
    } else {
      this.formGroup.get('attachedPaymentsCount').clearValidators();
    }
    this.formGroup.get('attachedPaymentsCount').updateValueAndValidity();
    return result;
  }

  private getIncomingData(incomingDetail: IncomingDetail): IncomingDetail {
    const addresseeInfo = this.formGroup.get('customerSearch').value;
    incomingDetail.addresseeId = addresseeInfo.addresseeId;
    incomingDetail.addresseeShortAddress = addresseeInfo.addresseeShortAddress;
    incomingDetail.addresseeAddress = addresseeInfo.addresseeAddress;
    incomingDetail.apartment = addresseeInfo.apartment;
    incomingDetail.addresseeCity = addresseeInfo.city;
    incomingDetail.addresseeOblast = addresseeInfo.oblast;
    incomingDetail.addresseeRepublic = addresseeInfo.republic;
    incomingDetail.addresseeStreet = addresseeInfo.street;
    incomingDetail.contactInfos = addresseeInfo.contactInfos;

    Object.assign(incomingDetail, this.formGroup.getRawValue());
    incomingDetail.typeId = this.formGroup.get('typeId').value.id;
    if (this.incomingAttachment) {
      const attachment = new Attachment(
        this.formGroup.get('typeId').value.id,
        this.formGroup.get('copyCount').value,
        this.formGroup.get('pageCount').value,
        this.formGroup.get('nameRu').value,
        this.incomingAttachment.attachment,
        this.incomingAttachment.attachment.type
      );
      incomingDetail.attachment = attachment;
    }
    incomingDetail.commentDtos = this.comments;
    incomingDetail.documentLinkDtos = [...this.links, ...this.removeLinks];

    incomingDetail.owners = this.formGroup.get('ownerNumbers').value.owners;
    return incomingDetail;
  }

  typeIdChanged(event) {
    const depatrment = this.dicDepartments.filter(d => d.code === 'D_3_2')[0];
    if (depatrment) {
      this.formGroup.get('departmentId').setValue(depatrment.id);
    }
  }

  onDivisionChange(divisionId: number) {
    this.departments = this.dicDepartments
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
    this.formGroup.get('departmentId').enable();
  }

  formatDate(param: Date): string {
    const date = new Date(param);
    return `${date.toLocaleDateString()} ${date.toLocaleTimeString()}`;
  }

  exists(): boolean {
    return !!this.id;
  }

  isAttached(): boolean {
    // if (this.incomingDetail && this.incomingDetail.owners) {
    //   if (this.incomingDetail.owners.length > 0) {
    //     return true;
    //   }
    //   return false;
    // }
    // return false;
    return true;
  }

  changeExecutorDialog() {
    const tempDialogRef = this.dialog.open(ChangeExecutorDialogComponent, {
      data: {
        detail: this.incomingDetail
      },
      width: '700px'
    });

    tempDialogRef.afterClosed().subscribe((newWorkflow: MaterialWorkflow) => {
      if (newWorkflow) {
        this.workflowBusinessService
          .afterUpdateWorkflow(this.incomingDetail, newWorkflow)
          .takeUntil(this.onDestroy)
          .subscribe((detail: MaterialDetail) => {
            this.setWorkflowData(detail);
            this.workflows = Object.assign(
              [],
              this.incomingDetail.workflowDtos
            );
            this.currentWorkflow = this.workflows.filter(
              d => d.isCurent === true && d.currentUserId === this.auth.userId
            )[0];
            this.isEditableStage = this.workflowBusinessService.isEditStage(
              this.workflowBusinessService.getCurrentWorkflow(
                this.incomingDetail
              )
            );
            this.isReadOnly = newWorkflow.currentUserId !== this.auth.userId;
            this.setAvailableOfTransfer();
            this.toggleEditMode(false);
          }, console.log);
      }
    });
  }

  onOpenWorkflowDialog() {
    const dialogRef = this.dialog.open(DocumentWorkflowDialogComponent, {
      data: {
        detail: this.incomingDetail
      },
      width: '700px'
    });

    dialogRef.afterClosed().subscribe(newWorkflow => {
      if (newWorkflow) {
        this.workflowBusinessService
          .afterCreateWorkflow(this.incomingDetail, newWorkflow)
          .takeUntil(this.onDestroy)
          .subscribe((detail: MaterialDetail) => {
            this.setWorkflowData(detail);
            this.workflows = Object.assign(
              [],
              this.incomingDetail.workflowDtos
            );
            this.currentWorkflow = this.workflows.filter(
              d => d.isCurent === true && d.currentUserId === this.auth.userId
            )[0];
            this.isEditableStage = this.workflowBusinessService.isEditStage(
              this.workflowBusinessService.getCurrentWorkflow(
                this.incomingDetail
              )
            );
            this.setAvailableOfTransfer();
            this.toggleEditMode(false);
            this.currentWorkflow = {...this.currentWorkflow};
            this.incomingDetail = {...this.incomingDetail};
          }, console.log);
      }
    });
  }

  displayFn(option: SelectOption): string {
    return option ? option.nameRu : '';
  }

  filter(name: string): SelectOption[] {
    return this.docTypesCollection.filter(
      option =>
        option.nameRu.toLowerCase().indexOf(name.toString().toLowerCase()) > -1
    );
  }

  onPreview() {
    this.documentsService
      .getDocumentPdf(this.id, true, true)
      .takeUntil(this.onDestroy)
      .subscribe(blob => {
        window.open(window.URL.createObjectURL(blob ));
      });
  }

  isInitialStage(): boolean {
    if (this.incomingDetail) {
      return this.workflowBusinessService.isInitialStage(
        this.workflowBusinessService.getCurrentWorkflow(this.incomingDetail)
      );
    }
    return false;
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
              if (this.incomingDetail.owners.length > 0) {
                this.router.navigate([
                  `${getModuleName(this.incomingDetail.ownerType)}`,
                  this.incomingDetail.owners[0].ownerId
                ]);
              } else {
                this.router.navigate(['journal']);
              }
            });
        }
      });
  }

  onSingleFileSelect($event, index: number) {
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
      this.incomingDetail.attachment = newAttachment;
      this.materialsService
        .replaceAttachment(this.incomingDetail)
        .takeUntil(this.onDestroy)
        .subscribe((data: MaterialDetail) => {
          this.incomingDetail.workflowDtos = [
            ...this.incomingDetail.workflowDtos.map(d => {
              d.isCurent = false;
              return d;
            }),
            ...data.workflowDtos
          ];
          this.workflows = this.incomingDetail.workflowDtos;

          this.formGroup.patchValue({
            pageCount: data.attachment.pageCount,
            copyCount: data.attachment.copyCount
          });
          this.incomingDetail.wasScanned = data.wasScanned;
          this.setAvailableOfTransfer();
          this.formGroup.markAsPristine();
          this.InitializeFormGroupFromData();
          this.toggleEditMode(false);
          this.reloadAttachments.emit();
          this.router.navigate([`materials/incoming`, this.incomingDetail.id]);
          this.isReadOnly = !data.workflowDtos.some(
            d => d.currentUserId === this.auth.userId
          );
        });
    }
  }

  onBackClick() {
    this.router.navigate([
      `${getModuleName(this.incomingDetail.ownerType)}`,
      this.incomingDetail.owners[0].ownerId
    ]);
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
              this.incomingDetail.wasScanned = false;
              this.incomingDetail.attachment = null;
              this.formGroup.patchValue({
                pageCount: 0,
                copyCount: 1
              });
              this.InitializeFormGroupFromData();
              this.toggleEditMode(false);
            });
        }
      });
  }

  hasAttachment(): boolean {
    if (this.exists() && this.incomingDetail) {
      return !!this.incomingDetail.attachment;
    }
  }

  areOwnersProtectionDoc(): boolean {
    return (
      !!this.ownerDetailsList &&
      this.ownerDetailsList.every(o => o.ownerType === OwnerType.ProtectionDoc)
    );
  }

  areOwnersRequest(): boolean {
    return (
      !!this.ownerDetailsList &&
      this.ownerDetailsList.every(o => o.ownerType === OwnerType.Request)
    );
  }

  areOwnersContract(): boolean {
    return (
      !!this.ownerDetailsList &&
      this.ownerDetailsList.every(o => o.ownerType === OwnerType.Contract)
    );
  }

  private setIncomingData(incomingDetail: IncomingDetail) {
    this.incomingDetail = incomingDetail;
    this.setAvailableOfTransfer();
  }

  canChangeExecutor() {
    const curentPositionTypeCode = this.auth.positionTypeCode;
    const curentUserDepartmentId = this.auth.departmentId;
    if (curentPositionTypeCode === '31' || curentPositionTypeCode === '34' || curentPositionTypeCode === '62') {
      const curentWF = this.workflowBusinessService.getCurrentWorkflow(
        this.incomingDetail
      );
      if (
        curentWF.currentUserDepartmentId &&
        curentWF.currentUserDepartmentId.toString() === curentUserDepartmentId
      ) {
        return false;
      }
    }

    return true;
  }

  canEditData() {
    // return (
    //   !this.editMode &&
    //   (
    //     this.currentStageCode === 'IN1.1' ||
    //     this.currentStageCode === 'IN3' ||
    //     this.currentStageCode === 'IN1.2.1' ||
    //     this.currentStageCode === 'IN2.2'
    //   )
    // );
    return !this.editMode;
  }

  showOutOfControl() {
    const curentWF = this.workflowBusinessService.getCurrentWorkflow(
      this.incomingDetail
    );
    if (curentWF && curentWF.currentStageCode === 'IN3') {
      return true;
    }
    return false;
  }

  canByDone() {
    if (!this.incomingDetail || !this.currentWorkflow) {
      return false;
    }

    const codes = ['004.1.1', '004.1.1.1', '006.02.01', '001.002', '001.002.1'];

    const curentType = this.docTypesCollection.filter(
      d => d.id === this.incomingDetail.typeId
    )[0];

    if (
      this.currentWorkflow.isComplete && curentType &&
      codes.some(c => c === curentType.code) &&
      this.incomingDetail.statusCode !== '2'
    ) {
      return true;
    }
  }

  private setAvailableOfTransfer() {

    // Боль, но иначе никак =(
    const codes = ['006.03', '006.05', '001.003.1'];

    this.workflowBusinessService
      .availableOfTransfer(this.incomingDetail)
      .subscribe((value: boolean) => {

        const curentType = this.docTypesCollection.filter(
          d => d.id === this.incomingDetail.typeId
        )[0];

        const cwf = this.workflowBusinessService.getCurrentWorkflow(this.incomingDetail);

        this.availableOfTransfer = cwf
          && Observable.of((value || (codes.some(c => c === curentType.code)))
          && this.hasAttachment);
      });
  }

  private setWorkflowData(detail: MaterialDetail) {
    this.incomingDetail.workflowDtos = detail.workflowDtos;
    this.currentWorkflow = this.workflows.filter(
      d => d.isCurent === true && d.currentUserId === this.auth.userId
    )[0];
    this.incomingDetail.wasScanned = detail.wasScanned;
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
            if (this.ownerId) {
              this.router.navigate([
                `${getModuleName(this.ownerType)}`,
                this.ownerId
              ]);
            } else {
              this.router.navigate(['journal']);
            }
          }
        }
      });
  }

  private toggleEditMode(value: boolean) {
    this.editMode = value;
    this.edit.emit(value);

    this.editableControls.forEach(c => {
      if (c === 'controlDate') {
        value && this.controlMarkChecked
          ? this.formGroup.controls[c].enable()
          : this.formGroup.controls[c].disable();
      } else if (c === 'outOfControl') {
        value && this.controlMarkChecked
          ? this.formGroup.controls[c].enable()
          : this.formGroup.controls[c].disable();
      } else if (c === 'dateOutOfControl') {
        value && this.outOfControlChecked
          ? this.formGroup.controls[c].enable()
          : this.formGroup.controls[c].disable();
      } else if (c === 'resolutionExtensionControlDate') {
        value && this.controlMarkChecked
          ? this.formGroup.controls[c].enable()
          : this.formGroup.controls[c].disable();
      } else {
        value
          ? this.formGroup.controls[c].enable()
          : this.formGroup.controls[c].disable();
      }
    });

    if (!this.exists()) {
      this.blockedOnCreationControls.forEach(c => {
        this.formGroup.controls[c].disable();
      });
    }

    if (this.isInitialStage()) {
      this.blockedOnInitialControls.forEach(c => {
        this.formGroup.controls[c].disable();
      });
    }
  }

  private initializeFormGroup() {
    const addresseeInfo = new AddresseeInfo();
    if (this.ownerId) {
      switch (this.ownerType) {
        case OwnerType.Request:
          this.requestService
            .getRequestById(this.ownerId)
            .takeUntil(this.onDestroy)
            .subscribe((data: RequestDetails) => {
              addresseeInfo.addresseeId = data.addresseeId;
              addresseeInfo.addresseeShortAddress = data.addresseeShortAddress;
              addresseeInfo.addresseeAddress = data.addresseeAddress;
              addresseeInfo.addresseeNameRu = data.addresseeNameRu;
              addresseeInfo.addresseeXin = data.addresseeXin;
              addresseeInfo.apartment = data.apartment;

              const requestOwner = new MaterialOwnerDto();
              requestOwner.ownerId = data.id;
              requestOwner.ownerType = OwnerType.Request;
              if (!this.ownerDetailsList.includes(requestOwner)) {
                this.ownerDetailsList.push(requestOwner);
              }
              this.formGroup.patchValue({
                customerSearch: addresseeInfo,
                ownerNumbers: this.ownerDetailsList,
                receiveTypeId: data.receiveTypeId,
                divisionId: data.divisionId,
                departmentId: data.departmentId
              });
            });
          break;
        case OwnerType.Contract:
          this.contractService
            .getById(this.ownerId)
            .takeUntil(this.onDestroy)
            .subscribe((data: ContractDetails) => {
              const contractOwner = new MaterialOwnerDto();
              contractOwner.ownerId = data.id;
              contractOwner.ownerType = OwnerType.Contract;
              if (!this.ownerDetailsList.includes(contractOwner)) {
                this.ownerDetailsList.push(contractOwner);
              }
              this.formGroup.patchValue({
                ownerNumbers: this.ownerDetailsList,
                receiveTypeId: data.receiveTypeId,
                divisionId: this.dicDivisions.find(d => d.code === '000001').id,
                departmentId: this.dicDepartments.find(d => d.code === 'OLD_0-11')
                  .id
              });
            });
          break;
        case OwnerType.ProtectionDoc:
          this.protectionDocService
            .get(this.ownerId)
            .takeUntil(this.onDestroy)
            .subscribe((protectionDocDetails: ProtectionDocDetails) => {
              if (protectionDocDetails.addressee) {
                addresseeInfo.addresseeId = protectionDocDetails.addressee.id;
                addresseeInfo.addresseeAddress = protectionDocDetails.addressee.address;
                addresseeInfo.addresseeShortAddress = protectionDocDetails.addressee.shortAddress;
                addresseeInfo.addresseeNameRu =
                  protectionDocDetails.addressee.nameRu;
                addresseeInfo.addresseeXin = protectionDocDetails.addressee.xin;
                addresseeInfo.apartment =
                  protectionDocDetails.addressee.apartment;
                addresseeInfo.city = protectionDocDetails.addressee.city;
                addresseeInfo.oblast = protectionDocDetails.addressee.oblast;
                addresseeInfo.republic =
                  protectionDocDetails.addressee.republic;
                addresseeInfo.region = protectionDocDetails.addressee.region;
                addresseeInfo.street = protectionDocDetails.addressee.street;
              }
              const protectionDocOwner = new MaterialOwnerDto();
              protectionDocOwner.ownerId = protectionDocDetails.id;
              protectionDocOwner.ownerType = OwnerType.ProtectionDoc;
              if (!this.ownerDetailsList.includes(protectionDocOwner)) {
                this.ownerDetailsList.push(protectionDocOwner);
              }
              this.formGroup.patchValue({
                customerSearch: addresseeInfo,
                ownerNumbers: this.ownerDetailsList,
                divisionId: this.dicDivisions.find(d => d.code === '000001').id,
                departmentId: this.dicDepartments.find(d => d.code === 'OLD_0-11')
                  .id
              });
            });
          break;
      }
    }
  }

  private InitializeFormGroupFromData() {
    this.formGroup.reset(this.incomingDetail);

    const addresseeInfo = new AddresseeInfo();
    addresseeInfo.addresseeShortAddress = this.incomingDetail.addresseeShortAddress;
    addresseeInfo.addresseeAddress = this.incomingDetail.addresseeAddress;
    addresseeInfo.addresseeId = this.incomingDetail.addresseeId;
    addresseeInfo.addresseeNameRu = this.incomingDetail.addresseeNameRu;
    addresseeInfo.addresseeXin = this.incomingDetail.addresseeXin;
    addresseeInfo.apartment = this.incomingDetail.apartment;

    addresseeInfo.city = this.incomingDetail.addresseeCity;
    addresseeInfo.oblast = this.incomingDetail.addresseeOblast;
    addresseeInfo.republic = this.incomingDetail.addresseeRepublic;
    addresseeInfo.street = this.incomingDetail.addresseeStreet;
    addresseeInfo.region = this.incomingDetail.addresseeRegion;

    const departments = this.dicDepartments.filter(
      d => d.id === this.incomingDetail.departmentId
    );
    this.formGroup.patchValue({
      divisionId:
        departments.length > 0 ? departments[0].divisionId : '',
      departmentId: this.incomingDetail.departmentId,
      pageCount: this.incomingDetail.pageCount ?
      this.incomingDetail.pageCount
      :(this.incomingDetail.attachment
        ? this.incomingDetail.attachment.pageCount
        : ''),
      copyCount: this.incomingDetail.attachment
        ? this.incomingDetail.attachment.copyCount
        : 1,
      customerSearch: addresseeInfo,
      ownerNumbers: this.incomingDetail.owners,
      statusNameRu: this.incomingDetail.statusNameRu,
      wasScanned: this.incomingDetail.wasScanned
    });

    this.comments = [...this.incomingDetail.commentDtos];
    this.parentLinks = [...this.incomingDetail.documentParentLinkDtos];
    this.links = [...this.incomingDetail.documentLinkDtos];
    this.removeLinks = [];
    this.isReadOnly = this.incomingDetail.isReadOnly;
  }

  get statusNameRu() {
    return this.incomingDetail ? this.incomingDetail.statusNameRu : 'Создание';
  }

  private buildForm() {
    this.formGroup = this.fb.group({
      typeId: ['', Validators.required],
      ownerNumbers: [''],
      receiveTypeId: [{ value: '', disabled: true }, Validators.required],
      customerSearch: [''],
      divisionId: [{ value: '', disabled: true }],
      departmentId: [{ value: '', disabled: true }, Validators.required],
      barcode: [{ value: '', disabled: true }],
      documentDate: [{ value: '', disabled: true }],
      pageCount: [{ value: '', disabled: true }],
      copyCount: [{ value: '1', disabled: true }],
      wasScanned: [{ value: '', disabled: true }],
      nameRu: [{ value: '', disabled: true }],
      nameKz: [{ value: '', disabled: true }],
      nameEn: [{ value: '', disabled: true }],
      dateCreate: [{ value: '', disabled: true }],
      outgoingNumber: [{ value: '', disabled: true }],
      incomingNumber: [{ value: '', disabled: true }],
      incomingNumberFilial: [{ value: '', disabled: true }],
      controlMark: [{ value: '', disabled: true }],
      controlDate: [{ value: '', disabled: true }],
      resolutionExtensionControlDate: [{ value: '', disabled: true }],
      outOfControl: [{ value: '', disabled: true }],
      dateOutOfControl: [{ value: '', disabled: true }],
      isHasPaymentDocument: [{ value: '', disabled: true }],
      attachedPaymentsCount: [{ value: '', disabled: true }],
      statusNameRu: [{ value: '', disabled: true }]
    });

    this.editableControls = [
      'typeId',
      'receiveTypeId',
      'ownerNumbers',
      'nameRu',
      'nameKz',
      'nameEn',
      'documentDate',
      'outgoingNumber',
      'copyCount',
      'departmentId',
      'customerSearch',
      'pageCount',
      'controlMark',
      'controlDate',
      'resolutionExtensionControlDate',
      'outOfControl',
      'dateOutOfControl',
      'isHasPaymentDocument',
      'attachedPaymentsCount'
    ];

    this.blockedOnCreationControls = [];
    this.comments = [];
    this.parentLinks = [];
    this.links = [];
    this.removeLinks = [];

    this.blockedOnInitialControls = ['divisionId'];
  }

  commentAdded(comment: DocumentCommentDto) {
    if (this.incomingDetail && this.currentWorkflow) {
      comment.documentId = this.incomingDetail.id;
      comment.workflowId = this.currentWorkflow.id;
    }
    this.comments = [...this.comments, comment];
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

  outOfControlOnChange(value: MatCheckboxChange) {
    this.outOfControlChecked = value.checked;

    if (this.outOfControlChecked) {
      this.formGroup.get('dateOutOfControl').setValue(moment().format());
    } else {
      this.formGroup.get('dateOutOfControl').setValue(null);
    }
  }

  controlMarkOnChange(value: MatCheckboxChange) {
    this.controlMarkChecked = value.checked;
    if (this.controlMarkChecked) {
      this.formGroup.get('controlDate').setValue(moment().format());
      this.formGroup.get('resolutionExtensionControlDate').enable();
    } else {
      this.formGroup.get('controlDate').setValue(null);
      this.formGroup.get('resolutionExtensionControlDate').disable();
    }
  }

  isDisabledButtonSave() {
    // return (
    //   this.formGroup.invalid ||
    //   this.formGroup.pristine ||
    //   !(
    //     this.formGroup.controls.typeId.value &&
    //     this.formGroup.controls.typeId.value.id
    //   ) ||
    //   (this.formGroup.controls.controlMark.value === true &&
    //     !this.formGroup.controls.controlDate.value) ||
    //   (this.formGroup.controls.outOfControl.value === true &&
    //     !this.formGroup.controls.dateOutOfControl.value) ||
    //   (this.formGroup.controls.controlMark.value === true &&
    //     this.formGroup.controls.outOfControl.value === true &&
    //     !this.formGroup.controls.resolutionExtensionControlDate.value)
    // );
    return false;
  }
}
