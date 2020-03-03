import { WorkflowService } from '../../../shared/services/workflow.service';
import { MaterialWorkflow, WorkflowSendType } from '../../../shared/services/models/workflow-model';
import { Component, Inject, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialog } from '@angular/material';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import { Observable } from 'rxjs/Observable';
import { Subject } from 'rxjs/Subject';
import { MaterialsService } from '../../services/materials.service';
import { DicRouteStage } from '../../../shared/services/models/base-dictionary';
import { DictionaryType } from '../../../shared/services/models/dictionary-type.enum';
import { SelectOption } from '../../../shared/services/models/select-option';
import { RouteStageService } from '../../../shared/services/route-stage.service';
import { WorkflowBusinessService } from '../../services/workflow-business/workflow-business.service';
import { DutyCertData, AlgorithmType, CertificatesType } from '../../../shared/models/certificate-model';
import { DocumentSign } from '../../models/document-sign';
import { SnackBarHelper } from '../../../core/snack-bar-helper.service';
import { NcaLayerApiService } from '../../../shared/services/nca-layer-api.service';
import { DialogForPasswordComponent } from '../../../login/dialog/dialog-for-password.component';
import { UsersService } from 'app/administration/users.service';
import { UserDetails } from 'app/administration/components/users/models/user.model';
import {map, startWith} from 'rxjs/operators';

let workflowSendTypes: Object[] = [
  { type: WorkflowSendType.ToNextByRoute, nameRu: 'Далее по маршруту' },
  { type: WorkflowSendType.ToCurrentStage, nameRu: 'По текущему этапу' },
  { type: WorkflowSendType.ReturnByRoute, nameRu: 'Возврат на предыдущий этап' }
];

const patentSpecificCodes: String[] = ['OP_PAT','OP_PAT_PM','OP_PAT_KZ','OP_PAT_PM_KZ','OP_PAT_ID','OP_PAT_ID_KZ','OP_PAT_SA','OP_PAT_SA_KZ'];

@Component({
  selector: 'app-document-workflow-dialog',
  templateUrl: './workflow-dialog.component.html',
  styleUrls: ['./workflow-dialog.component.scss']
})
export class DocumentWorkflowDialogComponent implements OnInit, OnDestroy {
  formGroup: FormGroup;
  WorkflowSendType = WorkflowSendType;
  workflowSendTypes = workflowSendTypes;
  stageUsers: SelectOption[];
  filteredStageUsers: Observable<SelectOption[]>;
  currentWorkflow: MaterialWorkflow;
  newWorkflow = new MaterialWorkflow();
  dicRouteStages: SelectOption[];
  filteredRouteStages: DicRouteStage[];
  descriptionControl: FormControl;
  sendType: WorkflowSendType;
  sendTypeSubject: BehaviorSubject<WorkflowSendType>;
  selectOptions: SelectOption[];
  dutyCertData: DutyCertData;
  username: string;
  password: string;
  isSpecificPatentCode = false;

  certStoragePath: string;

  private onDestroy = new Subject();

  constructor(
    private fb: FormBuilder,
    private materialsService: MaterialsService,
    private usersService: UsersService,
    private workflowBusinessService: WorkflowBusinessService,
    private routeStageService: RouteStageService,
    private workflowService: WorkflowService,
    private dialogRef: MatDialogRef<DocumentWorkflowDialogComponent>,
    private snackBarHelper: SnackBarHelper,
    private ncaLayerApiService: NcaLayerApiService,
    public dialog: MatDialog,
    @Inject(MAT_DIALOG_DATA) private data: any) {
    if(patentSpecificCodes.includes(data.detail.code))
    {
      this.isSpecificPatentCode = true;
      this.workflowSendTypes =  [
        { type: WorkflowSendType.FinishParallelProcessing, nameRu: 'Обработка документа' },
      ];
    }
    this.buildWorkflowForm();
    this.open();
  }

  /**
   * Фильтрует массив `stageUsers` по полю `nameRu`
   * @param name Что искать в поле `nameRu`
   * @return Отфильтрованный массив
   */
  private _filter(name: string): SelectOption[] {
    const filterValue = name.toLowerCase();

    return this.stageUsers.filter(option => option.nameRu.toLowerCase().includes(filterValue));
  }

  ngOnInit() {
    this.formGroup.get('currentStageId').valueChanges
      .takeUntil(this.onDestroy)
      .filter(value => !!value)
      .switchMap(stageId => this.workflowService.getStageUsers(stageId))
      .subscribe(data => {
        this.stageUsers = data;
        this.filteredStageUsers = this.formGroup.controls['currentUserId'].valueChanges
          .pipe(
              startWith(''),
              map(value => typeof value === 'string' ? value : (value as SelectOption).nameRu),
              map(name => name ? this._filter(name) : this.stageUsers.slice())
          );
      });

    this.formGroup.get('workflowSendType').patchValue(WorkflowSendType.ToNextByRoute);

    this.dutyCertData = new DutyCertData(this.getPlainData(), AlgorithmType.GOST);
  }

  open() {
    if (this.data.detail) {
      const currentWorkflow = this.workflowBusinessService.getCurrentWorkflow(this.data.detail);
      if (currentWorkflow) {
        this.currentWorkflow = currentWorkflow;
        this.newWorkflow = this.workflowBusinessService.createRawWorkflow(currentWorkflow);
        if(this.isSpecificPatentCode)
        {
          this.workflowService.getRouteStageByCode('IN01.1.3')
          .takeUntil(this.onDestroy)
          .subscribe( (nextStage : DicRouteStage)  => {
            if(this.filteredRouteStages === undefined)
              this.filteredRouteStages = [];
            this.filteredRouteStages.push(nextStage);
            this.formGroup.get('currentStageId').setValue(nextStage.id);
            this.formGroup.get('currentStageId').disable();
          });
        }
        else{
          this.workflowService.getNextStagesByWorkflow(this.currentWorkflow.id)
          // this.workflowService.getNextStages(this.currentWorkflow.currentStageId)
          .takeUntil(this.onDestroy)
          .subscribe(nextStages => {
            if (!nextStages.length) {
              throw Error('Transfer is unavailable!');
            }
            this.filteredRouteStages = nextStages;
            if (this.currentWorkflow.currentStageCode === 'IN01.1.4') {
              const defaultValue = nextStages.find(stage => stage.code === 'IN01.1.0');
              this.formGroup.get('currentStageId').setValue(defaultValue.id);
            } else if (this.currentWorkflow.currentStageCode === 'IN01.1.1') {
              const defaultValue = nextStages.find(stage => stage.code === 'IN01.1.3');
              this.formGroup.get('currentStageId').setValue(defaultValue.id);
            } else {
              this.formGroup.get('currentStageId').setValue(nextStages[0].id);
              if (this.filteredRouteStages.length > 1) {
                this.formGroup.get('currentStageId').enable();
              }
            }
            // if (possibleStageCount > 1) {
            //   this.formGroup.get('currentStageId').enable();
            // }
          });
        }
        this.formGroup.patchValue(this.currentWorkflow);
        this.formGroup.controls['currentUserId'].setValue(this.currentWorkflow.currentUserNameRu);
        this.formGroup.get('description').reset();
        this.routeStageService
          .isFirst(this.currentWorkflow.currentStageId)
          .takeUntil(this.onDestroy)
          .subscribe((first: boolean) => {
            if(!this.isSpecificPatentCode){
              if (first) {
                // При этапе создании заявки и при этапах генерации услуг блокировать направление возврата, по умолчанию - далее по маршруту
                this.workflowSendTypes = this.workflowSendTypes.filter((t: any) => t.type !== WorkflowSendType.ReturnByRoute);
              }
              this.sendTypeSubject = new BehaviorSubject<WorkflowSendType>(WorkflowSendType.ToNextByRoute);
            }
          },
            console.log);

        this.workflowService.getRouteStageById(this.currentWorkflow.currentStageId)
          .takeUntil(this.onDestroy)
          .subscribe((currentStage: DicRouteStage) => {
            this.dicRouteStages = [];
            this.dicRouteStages.push(currentStage);
            this.formGroup.get('fromStageId').setValue(currentStage.id);
            this.formGroup.markAsDirty();
          });
      }
    }

    this.usersService.getCurrent()
      .takeUntil(this.onDestroy)
      .subscribe((data: UserDetails) => {
        this.password = data.certPassword;
        this.certStoragePath = data.certStoragePath;
      });

    this.formGroup.controls.currentStageId.disable();
  }

  ngOnDestroy(): void {
    this.onDestroy.next();
  }

  onRouteChange(sendType: WorkflowSendType) {
    if(WorkflowSendType.FinishParallelProcessing === sendType){
      this.formGroup.get('currentUserId').disable();
      this.formGroup.markAsPristine();
    }
    else{
      this.sendTypeSubject.next(sendType);
      this.sendType = sendType;
      switch (sendType) {
        case WorkflowSendType.ToNextByRoute:
        case WorkflowSendType.ToCurrentStage:

          this.formGroup.get('currentUserId').setValue('');
          this.formGroup.get('currentUserId').enable();
          this.resetDescriptionField();
          this.workflowService.getNextStagesByWorkflow(this.currentWorkflow.id)
            // this.workflowService.getNextStages(this.currentWorkflow.currentStageId)
            .takeUntil(this.onDestroy)
            .subscribe(nextStages => {
              if (!nextStages.length) {
                throw Error('Transfer is unavailable!');
              }
              this.filteredRouteStages = nextStages;
              this.formGroup.get('currentStageId').setValue(nextStages[0].id);
            });

          break;
        case WorkflowSendType.ReturnByRoute:
          this.formGroup.get('currentUserId').setValue('');
          this.formGroup.get('currentUserId').enable();
          // this.formGroup.get('currentUserId').setValue(this.currentWorkflow.currentUserNameRu);
          this.formGroup.get('description').setValidators(Validators.required);
          const previousWorkflow = this.workflowBusinessService.getPreviousWorkflow(this.data.detail);
          if (!previousWorkflow) {
            this.workflowService.getPreviousStages(this.currentWorkflow.currentStageId)
              .takeUntil(this.onDestroy)
              .subscribe(previousStages => {
                if (!previousStages.length) {
                  throw Error('Transfer is unavailable!');
                }
                this.filteredRouteStages = previousStages;
                this.formGroup.get('currentStageId').setValue(previousStages[0].id);
                this.formGroup.markAsPristine();
              });
          } else {
            const filteredRouteStageItems: DicRouteStage[] = [
              {
                dicType: DictionaryType.DicRouteStage,
                nameRu: previousWorkflow.currentStageNameRu,
                id: previousWorkflow.currentStageId,
                code: previousWorkflow.currentStageCode,
                dateCreate: null,
                description: previousWorkflow.currentStageNameRu,
                interval: null,
                isAuto: null,
                isFirst: null,
                isLast: null,
                isMain: null,
                isMultiUser: null,
                isReturnable: null,
                isSystem: null,
                nameEn: previousWorkflow.currentStageNameRu,
                nameKz: previousWorkflow.currentStageNameRu,
                onlineRequisitionStatusId: null,
                routeId: null,
              }
            ];

            this.filteredRouteStages = filteredRouteStageItems;
            this.formGroup.get('currentStageId').setValue(previousWorkflow.currentStageId);
            this.formGroup.markAsPristine();
          }
          break;
        default:
          throw Error(`Unknown send type: ${sendType}`);
      }
    }

    const possibleStageCount = this.filteredRouteStages.length;
    if (possibleStageCount > 1) {
      this.formGroup.get('currentStageId').enable();
      this.formGroup.get('currentStageId').markAsDirty();
    }
  }

  onSubmit() {
    if (this.formGroup.invalid || this.filteredRouteStages.length === 0) { return; }
    this.formGroup.markAsPristine();
    const values = this.formGroup.getRawValue();
    const currentUser = this.stageUsers.find(stageUser => (stageUser.nameRu === values.currentUserId));
    values.currentUserId = currentUser.id;
    const rawWorkflow = new MaterialWorkflow();
    Object.assign(rawWorkflow, values);
    Observable.combineLatest(this.workflowService.getRouteStageById(rawWorkflow.fromStageId),
      this.workflowService.getRouteStageById(rawWorkflow.currentStageId),
      this.workflowBusinessService.isNextSendStage(rawWorkflow))
      .switchMap(([fromStage, currentStage, isSend]): Observable<MaterialWorkflow> => {
        rawWorkflow.currentStageCode = currentStage.code;
        rawWorkflow.fromStageCode = fromStage.code;
        rawWorkflow.isComplete = currentStage.isLast;
        rawWorkflow.isMain = currentStage.isMain;
        rawWorkflow.routeId = currentStage.routeId;
        rawWorkflow.isSend = isSend;
        return Observable.of(rawWorkflow);
      })
      .subscribe(newWorkflow => {
        if (this.sendType === WorkflowSendType.ReturnByRoute) {
          this.materialsService.addPreviousWorkflow(newWorkflow)
            .takeUntil(this.onDestroy)
            .subscribe(workflow => {
              this.dialogRef.close(workflow);
            }, console.log);
        } else {
          this.materialsService.addWorkflow(newWorkflow)
            .takeUntil(this.onDestroy)
            .subscribe(workflow => {
              this.dialogRef.close(workflow);
            }, console.log);
        }
      });
  }

  onWorkflowCancel() {
    this.dialogRef.close();
  }

  isSignStage(): boolean {
    if (this.currentWorkflow) {
      return this.workflowBusinessService.isSignStage(this.currentWorkflow);
    }
    return false;
  }

  isDisabledButtonSend(): boolean {
    const sendType = this.formGroup.get('workflowSendType').value;
    return ((sendType === WorkflowSendType.ToNextByRoute || sendType === WorkflowSendType.ToCurrentStage || sendType === WorkflowSendType.FinishParallelProcessing)
      && !this.currentWorkflow.isSigned && this.isSignStage())
      || this.formGroup.invalid || this.formGroup.pristine || this.filteredRouteStages === undefined;
  }

  private buildWorkflowForm() {
    this.formGroup = this.fb.group({
      workflowSendType: ['', Validators.required],
      ownerId: ['', Validators.required],
      fromStageId: [{ value: '', disabled: true }],
      fromUserId: [{ value: '', disabled: true }],
      currentStageId: [{ value: '', disabled: true }],
      currentUserId: [''],
      routeId: [''],
      description: [''],
      isComplete: [''],
      userName: [''],
      password: [''],
    });
  }

  private resetDescriptionField() {
    this.formGroup.get('description').clearValidators();
    this.formGroup.get('description').setValue('');
    this.formGroup.markAsPristine();
    this.formGroup.markAsUntouched();
    this.formGroup.updateValueAndValidity();
  }

  private getPlainData(): string {
    return `<xml><documentId>${this.currentWorkflow.ownerId}</documentId><workflowId>${this.currentWorkflow.id}</workflowId></xml>`;
  }

  // авто подписание документа
  public autoSign() {
    this.dutyCertData.storagePath = this.certStoragePath;
    this.openDialog();
  }

  // подписание документа
  public browseCertificate() {
    if (this.certStoragePath) {
      this.autoSign();
      return;
    }

    this.ncaLayerApiService.browseKeyStore(this.dutyCertData.storageName, this.dutyCertData.fileExtension, '');

    this.ncaLayerApiService.NCALayer.subscribe(result => {
      if (result.errorCode !== 'NONE') {
        this.snackBarHelper.error('Ошибка');
        return;
      }
      if (!result.result) {
        return;
      }

      this.dutyCertData.storagePath = result.result;
      this.certStoragePath = result.result;
      this.openDialog();
    });
  }

  private openDialog(): void {
    const dialogRef = this.dialog.open(DialogForPasswordComponent, {
      width: '300px',
      data: {
        dutyCertData: this.dutyCertData,
        certificatesType: CertificatesType.SIGN,
        certPassword: this.password,
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {

        if (result.LOAD_KEYSTORE_ERROR) {
          this.certStoragePath = null;
          this.browseCertificate();
          return;
        }

        this.dutyCertData = result.certData;
        this.username = result.username;
        this.password = result.password;
        this.signXml();
      }
    });
  }

  private signXml() {
    this.ncaLayerApiService.signXml(this.dutyCertData.storageName, this.dutyCertData.storagePath,
      this.dutyCertData.keyAlias, this.dutyCertData.password, this.dutyCertData.data);
    this.ncaLayerApiService.NCALayer.subscribe(result => {
      this.dutyCertData.signedData = result.result;
      this.signDocument();
    });
  }

  private signDocument() {
    const docSign = this.getDocumentSignModel();
    if (docSign && docSign.signerCertificate &&
      docSign.plainData && docSign.signedData) {
      this.materialsService.sign(docSign)
        .takeUntil(this.onDestroy)
        .subscribe((result: any) => {
          if (result) {
            this.currentWorkflow.isSigned = true;
            this.onSubmit();
          }
        });
    } else {
      this.snackBarHelper.error('Не удалось извлечь данные из сертификата. Попробуйте позже');
    }
  }

  private getDocumentSignModel(): DocumentSign {
    const documentSign = new DocumentSign();
    documentSign.workflowId = this.currentWorkflow.id;
    documentSign.plainData = this.dutyCertData.data;
    documentSign.signedData = this.dutyCertData.signedData;
    documentSign.signerCertificate = this.dutyCertData.certData;
    documentSign.password = this.password;
    documentSign.certStoragePath = this.certStoragePath;
    return documentSign;
  }
}
