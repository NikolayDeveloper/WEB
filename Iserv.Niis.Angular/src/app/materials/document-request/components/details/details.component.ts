import { Component, OnInit, OnDestroy, ViewChild, ElementRef, Output, EventEmitter } from '@angular/core';
import { MaterialWorkflow } from 'app/shared/services/models/workflow-model';
import {
  DocumentCommentDto,
  UserInputFieldConfig,
  DocumentRequestDetail,
  Attachment,
  MaterialDetail,
  DocumentType,
  getDocumentTypeRoute,
  UserInputDto,
  DocumentLinkDto
} from 'app/materials/models/materials.model';
import { Location } from '@angular/common';
import { FormGroup, FormBuilder, Validators, FormControl } from '@angular/forms';
import { SelectOption } from 'app/shared/services/models/select-option';
import { Observable } from 'rxjs/Observable';
import { Subject } from 'rxjs/Subject';
import { OwnerType, getModuleName } from 'app/shared/services/models/owner-type.enum';
import { MatSort, MatDialog } from '@angular/material';
import { DocumentsService } from 'app/shared/services/documents.service';
import { ActivatedRoute, Router, Params } from '@angular/router';
import { MaterialsService } from 'app/materials/services/materials.service';
import { WorkflowBusinessService } from '../../../services/workflow-business/workflow-business.service';
import { Config } from 'app/shared/components/table/config.model';
import { AuthenticationService } from 'app/shared/authentication/authentication.service';
import { DeleteDialogComponent } from 'app/materials/components/delete-dialog/delete-dialog.component';

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
    new Config({ columnDef: 'currentStageNameRu', header: 'Этап', class: 'width-200' })
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
  documentRequestDetail: DocumentRequestDetail;
  attachment: Attachment;

  docTypesCollection: SelectOption[];

  availableAtStage: any;
  isEditableStage: any;

  private onDestroy = new Subject();
  private ownerId: number;
  private ownerType: OwnerType;

  noAttachments = false;


  @Output() edit: EventEmitter<boolean> = new EventEmitter();

  @ViewChild(MatSort) sort: MatSort;
  @ViewChild('inputSingleFile') inputSingleFile: ElementRef;
  @ViewChild('inputDocxFile') inputDocxFile: ElementRef;
  @ViewChild('inputFileOnCreate') inputFileOnCreate: ElementRef;

  reloadAttachments = new EventEmitter();

  constructor(
    private fb: FormBuilder,
    private dialog: MatDialog,
    private documentsService: DocumentsService,
    private route: ActivatedRoute,
    private materialsService: MaterialsService,
    private workflowBusinessService: WorkflowBusinessService,
    private router: Router,
    private auth: AuthenticationService,
  ) {
    this.buildForm();
    this.quillModules = {
      toolbar: [
        ['bold', 'italic', 'underline', 'strike'],
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
            this.docTypesCollection = doctypes;
            if (!this.exists()) {
              this.documentRequestDetail = new DocumentRequestDetail();
              this.documentRequestDetail.owners = [];
              if (this.ownerId) {
                this.documentRequestDetail.owners.push({
                  ownerId: this.ownerId,
                  ownerType: this.ownerType,
                  protectionDocTypeId: null,
                });
                this.formGroup
                  .get('ownerNumbers')
                  .patchValue(this.documentRequestDetail.owners);
              }
              this.toggleEditMode(true);
              return Observable.of(null);
            } else {
              return this.materialsService.getSingleDocumentRequest(this.id);
            }
          });
        }
      )
      .takeUntil(this.onDestroy)
      .filter(documentRequestDetail => documentRequestDetail)
      .subscribe((documentRequestDetail: DocumentRequestDetail) => {
        this.documentRequestDetail = documentRequestDetail;
        this.InitializeFormGroupFromData();
        this.previewMode = false;
        this.toggleEditMode(false);
      });
  }

  onAttachmentsLoad(attachmentsCount) {
    this.noAttachments = attachmentsCount === 0;
  }

  private get dictionaries() {
    return Observable.combineLatest(
      this.documentsService.getDocumentsTypesByClassificationCode('05')
    );
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

  onTypeSelect(option) {
    if (option.code) {
      this.selectType(option.code);
    }
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
              this.documentRequestDetail.typeId ===
              this.formGroup.get('typeId').value.id
            ) {
              this.documentRequestDetail.userInput.fields.forEach(f => {
                this.userInputFields.controls[f.key].setValue(f.value);
              });
            } else {
              this.documentRequestDetail.userInput.fields = [];
              this.documentRequestDetail.userInput.code = '';
            }
          }
        }
      } else {
        this.canScan = true;
      }
    }, console.log);
  }

  onBack() {
    if (this.documentRequestDetail.owners.length > 0) {
      this.router.navigate([
        `${getModuleName(this.documentRequestDetail.ownerType)}`,
        this.documentRequestDetail.owners[0].ownerId
      ]);
    } else {
      this.router.navigate(['journal']);
    }
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

  InitializeFormGroupFromData() {
    this.comments = [...this.documentRequestDetail.commentDtos];
    this.parentLinks = [...this.documentRequestDetail.documentParentLinkDtos];
    this.links = [...this.documentRequestDetail.documentLinkDtos];
    this.removeLinks = [];

    this.formGroup.reset(this.documentRequestDetail);
    this.formGroup.get('ownerNumbers').patchValue(this.documentRequestDetail.owners);
    if (this.documentRequestDetail.attachment) {
      this.formGroup
        .get('pageCount')
        .setValue(this.documentRequestDetail.attachment.pageCount);
    }
    const docType = this.docTypesCollection.find(
      t => t.id === this.documentRequestDetail.typeId
    );
    this.formGroup.controls.typeId.patchValue(this.documentRequestDetail.typeId);
    if (docType) {
      this.selectType(docType.code);
    }
    this.workflows = this.documentRequestDetail.workflowDtos;
    this.currentWorkflow = this.workflows.filter(d => d.isCurent === true && d.currentUserId === this.auth.userId)[0];
    this.isEditableStage = this.workflowBusinessService.isEditStage(
      this.workflowBusinessService.getCurrentWorkflow(this.documentRequestDetail)
    );

    this.isReadOnly = this.documentRequestDetail.isReadOnly;
  }

  exists(): boolean {
    return !!this.id;
  }

  isInitialStage(): boolean {
    if (this.exists()) {
      return this.workflowBusinessService.isInitialStage(
        this.workflowBusinessService.getCurrentWorkflow(this.documentRequestDetail)
      );
    }
    return false;
  }


  onSingleFileSelect($event, index: number) {
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
      this.documentRequestDetail.attachment = newAttachment;
      this.materialsService
        .replaceAttachment(this.documentRequestDetail)
        .takeUntil(this.onDestroy)
        .subscribe((data: MaterialDetail) => {
          this.documentRequestDetail.workflowDtos = [
            ...this.documentRequestDetail.workflowDtos.map(d => {
              d.isCurent = false;
              return d;
            }),
            ...data.workflowDtos
          ];
          this.workflows = this.documentRequestDetail.workflowDtos;

          this.formGroup.patchValue({
            pageCount: data.attachment.pageCount,
            copyCount: data.attachment.copyCount
          });
          this.documentRequestDetail.wasScanned = data.wasScanned;
          this.formGroup.markAsPristine();
          this.InitializeFormGroupFromData();
          this.toggleEditMode(false);
          this.reloadAttachments.emit();
          this.router.navigate([`materials/document-request`, this.documentRequestDetail.id]);
          this.isReadOnly = !data.workflowDtos.some(
            d => d.currentUserId === this.auth.userId
          );
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
              this.documentRequestDetail.attachment = null;
              this.workflows = this.documentRequestDetail.workflowDtos;
              this.formGroup.patchValue({
                pageCount: 0,
                copyCount: 1
              });
              this.documentRequestDetail.wasScanned = false;
              this.formGroup.markAsPristine();
              this.InitializeFormGroupFromData();
              this.toggleEditMode(false);
              this.reloadAttachments.emit();
              this.router.navigate([`materials/document-request`, this.documentRequestDetail.id]);
              this.isReadOnly = !this.workflows.some(
                d => d.currentUserId === this.auth.userId
              );
            });
        }
      });
  }

  hasAttachment(): boolean {
    if (this.exists() && this.documentRequestDetail) {
      return !!this.documentRequestDetail.attachment;
    }
  }

  canEditData() {
    return !this.editMode;
  }

  onEdit() {
    this.toggleEditMode(true);
  }

  onUndo() {
    if (this.documentRequestDetail.owners.length > 0) {
      this.router.navigate([
        `${getModuleName(this.documentRequestDetail.owners[0].ownerType)}`,
        this.documentRequestDetail.owners[0].ownerId
      ]);
    } else {
      this.router.navigate(['journal']);
    }
  }

  onSubmit() {
    this.postData();
  }

  postData() {
    if (this.exists()) {
      this.materialsService
        .updateDocumentRequest(this.prepareFormValue())
        .takeUntil(this.onDestroy)
        .subscribe((documentRequestDetail: DocumentRequestDetail) => {
          this.documentRequestDetail = documentRequestDetail;
          if (this.attachment) {
            this.documentRequestDetail.attachment = this.attachment;
            this.materialsService
              .replaceAttachment(this.documentRequestDetail)
              .takeUntil(this.onDestroy);
          }
          this.InitializeFormGroupFromData();
          this.previewMode = false;
          this.toggleEditMode(false);
        });
    } else {
      this.materialsService
        .createDocumentRequest(this.prepareFormValue())
        .takeUntil(this.onDestroy)
        .subscribe(id => {
          this.router.navigate([
            getDocumentTypeRoute(DocumentType.DocumentRequest),
            id
          ]);
        }, console.log);
    }
  }

  private prepareFormValue() {
    const documentRequestDetail = new DocumentRequestDetail();
    if (this.exists()) {
      documentRequestDetail.id = this.id;
      if (this.attachment) {
        documentRequestDetail.attachment = this.attachment;
      }
    }
    const firstOwner = !!this.documentRequestDetail.owners.length
      ? this.documentRequestDetail.owners[0]
      : null;
    const input: UserInputDto = new UserInputDto(
      this.formGroup.get('code').value,
      !!firstOwner ? firstOwner.ownerId : 0,
      this.documentRequestDetail.id,
      'request',
      this.documentRequestDetail.owners.map(o => o.ownerId),
      !!firstOwner ? firstOwner.ownerType : OwnerType.None
    );
    input.pageCount = this.formGroup.get('pageCount').value;
    if (
      this.documentRequestDetail &&
      this.documentRequestDetail.userInput &&
      this.documentRequestDetail.userInput.index
    ) {
      input.index = this.documentRequestDetail.userInput.index;
    }
    const keys = Object.keys(this.userInputFields.value);
    keys.forEach(k => {
      input.fields.push({ key: k, value: this.userInputFields.value[k] });
    });
    documentRequestDetail.userInput = input;
    documentRequestDetail.nameEn = this.formGroup.get('nameEn').value;
    documentRequestDetail.nameRu = this.formGroup.get('nameRu').value;
    documentRequestDetail.nameKz = this.formGroup.get('nameKz').value;
    documentRequestDetail.typeId = this.formGroup.get('typeId').value.id;
    documentRequestDetail.owners = this.formGroup.get('ownerNumbers').value.owners;
    documentRequestDetail.pageCount = this.formGroup.get('pageCount').value;

    documentRequestDetail.commentDtos = this.comments;
    documentRequestDetail.documentLinkDtos = [...this.links, ...this.removeLinks];
    return documentRequestDetail;
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
      ownerNumbers: ['', Validators.required]
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
    this.edit.emit(value);

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
    if (this.documentRequestDetail && this.currentWorkflow) {
      comment.documentId = this.documentRequestDetail.id;
      comment.workflowId = this.currentWorkflow.id;
    }
    this.comments = [...this.comments, comment];
  }

  wasScanned(): boolean {
    if (this.exists() && this.documentRequestDetail) {
      return this.documentRequestDetail.wasScanned;
    }
  }

  hasSecondaryAttachment(): boolean {
    if (this.exists() && this.documentRequestDetail) {
      return this.documentRequestDetail.hasSecondaryAttachment;
    }
  }
}
