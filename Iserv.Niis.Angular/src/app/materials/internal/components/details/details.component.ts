import { MaterialWorkflow } from '../../../../shared/services/models/workflow-model';
import { Component, ElementRef, OnInit, ViewChild, EventEmitter } from '@angular/core';
import { Location } from '@angular/common';
import { OnDestroy } from '@angular/core/src/metadata/lifecycle_hooks';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  Validators
} from '@angular/forms';
import {
  MatDialog,
  MatSort
} from '@angular/material';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import { Subject } from 'rxjs/Subject';

import { UndoDialogComponent } from '../../../../shared/components/undo-dialog/undo-dialog.component';
import {
  Attachment,
  DocumentType,
  getDocumentTypeRoute,
  InternalDetail,
  MaterialDetail,
  UserInputDto,
  DocumentCommentDto,
  UserInputFieldConfig,
  DocumentLinkDto
} from '../../../models/materials.model';
import { DictionaryService } from '../../../../shared/services/dictionary.service';
import { DocumentsService } from '../../../../shared/services/documents.service';
import { MaterialsService } from '../../../services/materials.service';
import { DictionaryType } from '../../../../shared/services/models/dictionary-type.enum';
import { ChangeExecutorDialogComponent } from '../../../components/change-executor-dialog/change-executor-dialog.component';
import { AuthenticationService } from '../../../../shared/authentication/authentication.service';
import {
  OwnerType,
  getModuleName
} from '../../../../shared/services/models/owner-type.enum';
import { SelectOption } from '../../../../shared/services/models/select-option';
import { DeleteDialogComponent } from '../../../components/delete-dialog/delete-dialog.component';
import { DocumentWorkflowDialogComponent } from '../../../components/workflow-dialog/workflow-dialog.component';
import { WorkflowBusinessService } from '../../../services/workflow-business/workflow-business.service';
import { Config } from '../../../../shared/components/table/config.model';

// TODO: Временно отключен модуль imageResize
// (window as any).Quill.register('modules/imageResize', ImageResize)

// const journalCodes = [
//   'TZ-VIPISKA.1',
//   '001.001_SL',
//   'TZ-VIPISKA',
//   'GR_TZ_VYP',
//   'TZPOL555',
//   '001.024.1.1',
//   '001.001_SL',
//   '001.032',
//   'RE_PAYMENT',
//   '001.082',
//   '001.063',
//   '001.091'
// ];

const serchReportCode = 'IZ_OTCHET_POISK_IZ';

const registerCodes = [
  'REESTR_INPUT_KANC',
  'REESTR_OUT_KANC',
  'REESTR_POSTAGE-STAMP_KANC',
  'KANC_1',
  'KANC_2',
  'REESTR_INPUT_DEPART',
  'REESTR_OUTPUT_DEPART',
  'RE_PAYMENT]'
];

@Component({
  selector: 'app-details',
  templateUrl: './details.component.html',
  styleUrls: ['./details.component.scss']
})
export class DetailsComponent implements OnInit, OnDestroy {
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
  userInputFormConfig: Array<UserInputFieldConfig> = [];
  editableControls: string[];
  quillModules: any = {};
  editMode = false;
  previewMode = false;
  canScan = false;
  isReadOnly = false;

  id: number;
  internalDetail: InternalDetail;
  attachment: Attachment;

  docTypesCollection: SelectOption[];

  availableAtStage: any;
  availableOfTransfer: any;
  isEditableStage: any;

  private onDestroy = new Subject();
  private ownerId: number;
  private ownerType: OwnerType;
  private selectedTypeCode = '';

  noAttachments = false;

  @ViewChild(MatSort) sort: MatSort;
  @ViewChild('inputSingleFile') inputSingleFile: ElementRef;
  @ViewChild('inputDocxFile') inputDocxFile: ElementRef;
  @ViewChild('inputFileOnCreate') inputFileOnCreate: ElementRef;

  reloadAttachments = new EventEmitter();

  constructor(
    private fb: FormBuilder,
    private documentsService: DocumentsService,
    private route: ActivatedRoute,
    private materialsService: MaterialsService,
    private dictionaryService: DictionaryService,
    private workflowBusinessService: WorkflowBusinessService,
    private dialog: MatDialog,
    private router: Router,
    private location: Location,
    private auth: AuthenticationService,
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
      .takeUntil(this.onDestroy)
      .switchMap(
        (params: Params): Observable<any> => {
          this.id = +params['id'];
          this.ownerId = +params['ownerId'];
          this.ownerType = +params['ownerType'];
          return this.dictionaries.switchMap(([doctypes]: [SelectOption[]]) => {
            if (this.ownerType !== OwnerType.None) {
              this.docTypesCollection = doctypes;
            } else {
              // this.docTypesCollection = doctypes.filter(dt =>
              //   journalCodes.includes(dt.code)
              // );
              this.docTypesCollection = doctypes;
            }
            if (!this.exists()) {
              this.internalDetail = new InternalDetail();
              this.internalDetail.owners = [];
              if (this.ownerId) {
                this.internalDetail.owners.push({
                  ownerId: this.ownerId,
                  ownerType: this.ownerType,
                  protectionDocTypeId: null,
                });
                this.formGroup
                  .get('ownerNumbers')
                  .patchValue(this.internalDetail.owners);
              }
              this.toggleEditMode(true);
              return Observable.of(null);
            } else {
              return this.materialsService.getSingleInternal(this.id);
            }
          });
        }
      )
      .takeUntil(this.onDestroy)
      .filter(internalDetail => internalDetail)
      .subscribe((internalDetail: InternalDetail) => {
        this.internalDetail = internalDetail;
        this.InitializeFormGroupFromData();
        this.previewMode = false;
        this.toggleEditMode(false);
        this.setAvailableOfTransfer();
      });
  }

  onAttachmentsLoad(attachmentsCount) {
    this.noAttachments = attachmentsCount === 0;
  }

  private get dictionaries() {
    return Observable.combineLatest(
      this.documentsService.getDocumentsTypesByClassificationCode('03')
    );
  }


  canChangeExecutor() {
    const curentPositionTypeCode = this.auth.positionTypeCode;
    const curentUserDepartmentId = this.auth.departmentId;
    if (curentPositionTypeCode === '31' || curentPositionTypeCode === '34' || curentPositionTypeCode === '62') {
      const curentWF = this.workflowBusinessService.getCurrentWorkflow(this.internalDetail);
      if (curentWF.currentUserDepartmentId.toString() === curentUserDepartmentId) {
        return false;
      }
    }

    return true;
  }

  ngOnDestroy(): void {
    this.onDestroy.next();
  }

  get userInputFields() {
    return this.formGroup.controls.userInputFields as FormGroup;
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

  onTypeSelect(option) {
    if (option.code) {
      this.selectType(option.code);
      this.selectedTypeCode = option.code;
    }
  }

  private setAvailableOfTransfer() {
    // if (!this.isRegister()) {
    this.workflowBusinessService
      .availableOfTransfer(this.internalDetail)
      .subscribe((value: boolean) => {
        this.availableOfTransfer = Observable.of(
          value && (this.wasScanned() || this.internalDetail.hasTemplate)
        );
        this.internalDetail = {...this.internalDetail}
      });
    // } else {
    //   this.availableOfTransfer = Observable.of(false);
    // }
  }

  private selectType(code: string) {
    this.formGroup.get('code').setValue(code);
    this.documentsService.getUserInputFields(code).subscribe(data => {
      if (data) {
        if (data.requireUserInput) {
          this.userInputFormConfig = data.fieldsConfig;
          this.buildUserInputFields();
          if (this.id) {
            if (
              this.internalDetail.typeId ===
              this.formGroup.get('typeId').value.id
            ) {
              this.internalDetail.userInput.fields.forEach(f => {
                this.userInputFields.controls[f.key].setValue(f.value);
              });
            } else {
              this.internalDetail.userInput.fields = [];
              this.internalDetail.userInput.code = '';
            }
          }
        }
      } else {
        this.canScan = true;
      }
    }, console.log);
  }


  changeExecutorDialog() {
    const tempDialogRef = this.dialog.open(ChangeExecutorDialogComponent, {
      data: {
        detail: this.internalDetail
      },
      width: '700px'
    });

    tempDialogRef.afterClosed().subscribe((newWorkflow: MaterialWorkflow) => {
      if (newWorkflow) {
        this.workflowBusinessService
          .afterUpdateWorkflow(this.internalDetail, newWorkflow)
          .takeUntil(this.onDestroy)
          .subscribe((detail: MaterialDetail) => {
            this.setWorkflowDetails(detail);
              this.workflows = Object.assign(
                  [],
                  this.internalDetail.workflowDtos
                );
              this.currentWorkflow = this.workflows.filter(d => d.isCurent === true && d.currentUserId === this.auth.userId)[0];
              this.isEditableStage = this.workflowBusinessService.isEditStage(
                this.workflowBusinessService.getCurrentWorkflow(
                  this.internalDetail
                )
            );
            this.setAvailableOfTransfer();
            this.toggleEditMode(false);
            this.isReadOnly = newWorkflow.currentUserId !== this.auth.userId;
          }, console.log);
      }
    });
  }

  onOpenWorkflowDialog() {
    const dialogRef = this.dialog.open(DocumentWorkflowDialogComponent, {
      data: {
        detail: this.internalDetail
      },
      width: '700px'
    });

    dialogRef
      .afterClosed()
      .takeUntil(this.onDestroy)
      .subscribe(newWorkflow => {
        if (newWorkflow) {
          this.workflowBusinessService
            .afterCreateWorkflow(this.internalDetail, newWorkflow)
            .takeUntil(this.onDestroy)
            .subscribe(detail => {
              this.setWorkflowDetails(detail);
              this.workflows = Object.assign(
                [],
                this.internalDetail.workflowDtos
              );
              this.currentWorkflow = this.workflows.filter(d => d.isCurent === true && d.currentUserId === this.auth.userId)[0];
              if (this.currentWorkflow.currentStageCode === 'IN01.1.3') {
                  // this.internalDetail.statusNameRu = 'Завершён';
                  this.internalDetail = {...this.internalDetail, statusNameRu: 'Завершён'};
              }
              this.isEditableStage = this.workflowBusinessService.isEditStage(
                this.workflowBusinessService.getCurrentWorkflow(
                  this.internalDetail
                )
              );
              this.setAvailableOfTransfer();
              this.toggleEditMode(false);
            }, console.log);
        }
      });
  }

  formatDate(param: Date): string {
    const date = new Date(param);
    return `${date.toLocaleDateString()} ${date.toLocaleTimeString()}`;
  }

  onBack() {
    if (this.internalDetail.owners.length > 0) {
      this.router.navigate([
        `${getModuleName(this.internalDetail.ownerType)}`,
        this.internalDetail.owners[0].ownerId
      ]);
    } else {
      this.router.navigate(['journal']);
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
              if (this.internalDetail.owners.length > 0) {
                this.router.navigate([
                  `${getModuleName(this.internalDetail.ownerType)}`,
                  this.internalDetail.owners[0].ownerId
                ]);
              } else {
                this.router.navigate(['journal']);
              }
            });
        }
      });
  }

  onPreview(wasScanned: boolean, isMain: boolean) {
    this.documentsService
      .getDocumentPdf(this.id, wasScanned, isMain)
      .takeUntil(this.onDestroy)
      .subscribe(blob => {
        window.open(
          window.URL.createObjectURL(blob /*, { oneTimeOnly: true }*/)
        );
        this.togglePreviewMode(false);
      });
  }

  togglePreviewMode(value: boolean) {
    this.previewMode = value;
  }

  onUndo() {
    if (this.formGroup.dirty) {
      this.openDialog();
      return false;
    } else {
      if (!this.exists()) {
        if (this.ownerId) {
          this.location.back();
        } else {
          this.router.navigate(['journal']);
        }
      } else {
        this.toggleEditMode(false);
      }
    }
  }

  get statusNameRu() {
    return this.internalDetail && this.internalDetail.statusId > 0
      ? this.internalDetail.statusNameRu
      : 'Создание';
      // : this.currentWorkflow.currentStageCode === 'IN01.1.3' ? 'Завершён' : 'Создание';
  }

  InitializeFormGroupFromData() {
    this.formGroup.reset(this.internalDetail);
    this.formGroup.get('ownerNumbers').patchValue(this.internalDetail.owners);
    if (this.internalDetail.attachment) {
      this.formGroup
        .get('pageCount')
        .setValue(this.internalDetail.attachment.pageCount);
    }
    const docType = this.docTypesCollection.find(
      t => t.id === this.internalDetail.typeId
    );
    this.formGroup.controls.typeId.patchValue(this.internalDetail.typeId);
    if (docType) {
      this.selectType(docType.code);
    }
    this.workflows = this.internalDetail.workflowDtos;
    this.currentWorkflow = this.workflows.filter(d => d.isCurent === true && d.currentUserId === this.auth.userId)[0];
    this.setAvailableOfTransfer();
    this.isEditableStage = this.workflowBusinessService.isEditStage(
      this.workflowBusinessService.getCurrentWorkflow(this.internalDetail)
    );

    this.comments = [...this.internalDetail.commentDtos];
    this.parentLinks = [...this.internalDetail.documentParentLinkDtos];
    this.links = [...this.internalDetail.documentLinkDtos];
    this.removeLinks = [];
    this.isReadOnly = this.internalDetail.isReadOnly;
  }

  exists(): boolean {
    return !!this.id;
  }

  isInitialStage(): boolean {
    if (this.exists()) {
      return this.workflowBusinessService.isInitialStage(
        this.workflowBusinessService.getCurrentWorkflow(this.internalDetail)
      );
    }
    return false;
  }

  onSingleFileSelect() {
    for (let i = 0; i < this.inputSingleFile.nativeElement.files.length; i++) {
      // TODO: интеграция со сканером
      const newAttachment = new Attachment(
        -1,
        1,
        1,
        this.inputSingleFile.nativeElement.files[i].name,
        this.inputSingleFile.nativeElement.files[i],
        this.inputSingleFile.nativeElement.files[i].type
      );
      newAttachment.isMain = true;
      this.internalDetail.attachment = newAttachment;
      this.materialsService
        .replaceAttachment(this.internalDetail)
        .takeUntil(this.onDestroy)
        .subscribe((data: MaterialDetail) => {
          this.formGroup.patchValue({
            pageCount: data.attachment.pageCount,
            copyCount: data.attachment.copyCount
          });
          // this.internalDetail.wasScanned = data.wasScanned;
          this.internalDetail.wasScanned = true;
          this.toggleEditMode(false);
          this.reloadAttachments.emit();
          this.formGroup.markAsPristine();
          this.setAvailableOfTransfer();
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
              this.internalDetail.wasScanned = false;
              this.formGroup.patchValue({
                pageCount: 0,
                copyCount: 1
              });
              this.toggleEditMode(false);
            });
        }
      });
  }

  hasAttachment(): boolean {
    if (this.exists() && this.internalDetail) {
      return !!this.internalDetail.attachment;
    }
  }

  onRecognize() {
    for (let i = 0; i < this.inputDocxFile.nativeElement.files.length; i++) {
      // TODO: интеграция со сканером
      const newAttachment = new Attachment(
        -1,
        1,
        1,
        this.inputDocxFile.nativeElement.files[i].name,
        this.inputDocxFile.nativeElement.files[i],
        this.inputDocxFile.nativeElement.files[i].type
      );
      newAttachment.isMain = false;
      this.internalDetail.attachment = newAttachment;
      this.materialsService
        .replaceAttachment(this.internalDetail)
        .takeUntil(this.onDestroy)
        .subscribe((data: MaterialDetail) => {
          this.internalDetail.attachment = data.attachment;
          this.internalDetail.hasSecondaryAttachment = true;
          this.internalDetail.pageCount = data.pageCount;
          this.formGroup.get('pageCount').setValue(data.pageCount);
          this.formGroup.get('wasScanned').patchValue(data.wasScanned);
          this.previewMode = false;
          this.toggleEditMode(false);
          this.reloadAttachments.emit();
          this.setAvailableOfTransfer();
        });
    }
  }

  isScanStage(): boolean {
    // if (this.exists() && !this.isJournal()) {
    if (this.exists()) {
      return this.workflowBusinessService.isScanStage(
        this.workflowBusinessService.getCurrentWorkflow(this.internalDetail)
      );
    }
  }

  isRegister(): boolean {
    return registerCodes.includes(this.formGroup.get('code').value);
  }

  // isJournal(): boolean {
  //   return journalCodes.includes(this.formGroup.get('code').value);
  // }

  onSubmit() {
    const values = this.formGroup.getRawValue();
    this.postData();
  }

  postData() {
    if (this.exists()) {
      this.materialsService
        .updateInternal(this.prepareFormValue())
        .takeUntil(this.onDestroy)
        .subscribe((internalDetail: InternalDetail) => {
          this.internalDetail = internalDetail;
          if (this.attachment) {
            this.internalDetail.attachment = this.attachment;
            this.materialsService
              .replaceAttachment(this.internalDetail)
              .takeUntil(this.onDestroy);
          }
          this.InitializeFormGroupFromData();
          this.previewMode = false;
          this.toggleEditMode(false);
        });
    } else {
      this.materialsService
        .createInternal(this.prepareFormValue())
        .takeUntil(this.onDestroy)
        .subscribe(id => {
          this.router.navigate([
            getDocumentTypeRoute(DocumentType.Internal),
            id
          ]);
        }, console.log);
    }
  }

  private prepareFormValue() {
    const internalDetail = new InternalDetail();
    if (this.exists()) {
      internalDetail.id = this.id;
      if (this.attachment) {
        internalDetail.attachment = this.attachment;
      }
    }
    const firstOwner = !!this.internalDetail.owners.length
      ? this.internalDetail.owners[0]
      : null;
    const input: UserInputDto = new UserInputDto(
      this.formGroup.get('code').value,
      !!firstOwner ? firstOwner.ownerId : 0,
      this.internalDetail.id,
      'request',
      this.internalDetail.owners.map(o => o.ownerId),
      !!firstOwner ? firstOwner.ownerType : OwnerType.None
    );
    input.pageCount = this.formGroup.get('pageCount').value;
    if (
      this.internalDetail &&
      this.internalDetail.userInput &&
      this.internalDetail.userInput.index
    ) {
      input.index = this.internalDetail.userInput.index;
    }
    const keys = Object.keys(this.userInputFields.value);
    keys.forEach(k => {
      input.fields.push({ key: k, value: this.userInputFields.value[k] });
    });
    internalDetail.userInput = input;
    internalDetail.nameEn = this.formGroup.get('nameEn').value;
    internalDetail.nameRu = this.formGroup.get('nameRu').value;
    internalDetail.nameKz = this.formGroup.get('nameKz').value;
    internalDetail.typeId = this.formGroup.get('typeId').value.id;
    internalDetail.owners = this.formGroup.get('ownerNumbers').value.owners;
    internalDetail.pageCount = this.formGroup.get('pageCount').value;

    internalDetail.commentDtos = this.comments;
    internalDetail.documentLinkDtos = [...this.links, ...this.removeLinks];
    return internalDetail;
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
              this.location.back();
            } else {
              this.router.navigate(['journal']);
            }
          }
        }
      });
  }

  private setWorkflowDetails(detail: MaterialDetail) {
    // this.internalDetail.currentWorkflowId = detail.currentWorkflowId;
    this.internalDetail.workflowDtos = detail.workflowDtos;
    // this.currentWorkflow = this.workflows.filter(d => d.isCurent === true && d.currentUserId === this.auth.userId)[0];
    this.setAvailableOfTransfer();
  }

  private buildForm() {
    this.formGroup = this.fb.group({
      typeId: ['', Validators.required],
      code: ['', Validators.required],
      barcode: [{ value: '', disabled: true }],
      pageCount: [{ value: '', disabled: true }],
      dateCreate: [{ value: '', disabled: true }],
      wasScanned: [{ value: '', disabled: true }],
      nameRu: [{ value: '', disabled: true }],
      nameEn: [{ value: '', disabled: true }],
      nameKz: [{ value: '', disabled: true }],
      userInputFields: this.fb.group([]),
      ownerNumbers: ['', Validators.required],
      statusNameRu: [{ value: '', disabled: true }]
    });

    this.comments = [];
    this.parentLinks = [];
    this.links = [];
    this.removeLinks = [];

    this.editableControls = ['nameRu', 'nameEn', 'nameKz', 'ownerNumbers', 'pageCount'];
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

  private buildUserInputFields() {
    this.formGroup.controls.userInputFields = this.fb.group([]);
    const group: any = {};

    if (this.userInputFormConfig.length > 0) {
      this.userInputFormConfig.forEach(fieldConfig => {
        group[fieldConfig.key] = fieldConfig.required
          ? new FormControl(fieldConfig.value || '', Validators.required)
          : new FormControl(fieldConfig.value || '');
      });
    }
    this.formGroup.controls.userInputFields = this.fb.group(group);
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
  }

  commentAdded(comment: DocumentCommentDto) {
    if (this.internalDetail && this.currentWorkflow) {
      comment.documentId = this.internalDetail.id;
      comment.workflowId = this.currentWorkflow.id;
    }
    this.comments = [...this.comments, comment];
  }

  wasScanned(): boolean {
    if (this.exists() && this.internalDetail) {
      return this.internalDetail.wasScanned;
    }
  }

  hasSecondaryAttachment(): boolean {
    if (this.exists() && this.internalDetail) {
      return this.internalDetail.hasSecondaryAttachment;
    }
  }

  canHaveName(): boolean {
    if (this.exists() && this.internalDetail) {
      return this.internalDetail.code !== serchReportCode;
    } else {
      return this.selectedTypeCode !== serchReportCode;
    }
  }
}
