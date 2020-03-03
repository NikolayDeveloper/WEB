import {Component, Inject, OnDestroy, OnInit, ChangeDetectorRef, QueryList} from '@angular/core';
import {
    FormBuilder,
    FormControl,
    FormGroup,
    Validators
} from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { BehaviorSubject, Observable, Subject } from 'rxjs';
import '../../../core/utils/array-extensions';
import { RequestDetails } from '../../../requests/models/request-details';
import { RouteStageCodes } from '../../models/route-stage-codes';
import { DicRouteStage } from '../../services/models/base-dictionary';
import { OwnerType } from '../../services/models/owner-type.enum';
import { SelectOption } from '../../services/models/select-option';
import {
    Workflow,
    WorkflowSendType
} from '../../services/models/workflow-model';
import { RouteStageService } from '../../services/route-stage.service';
import { WorkflowService } from '../../services/workflow.service';
import { BulletinService } from '../../services/bulletin.service';
import {map, startWith} from 'rxjs/operators';

let workflowSendTypes: Object[] = [
    { type: WorkflowSendType.ToNextByRoute, nameRu: 'Далее по маршруту' },
    { type: WorkflowSendType.ToCurrentStage, nameRu: 'По текущему этапу' },
    { type: WorkflowSendType.ReturnByRoute, nameRu: 'Возврат на предыдущий этап' }
];

const mistakeStageCodes: string[] = [
    'B04.0.0.1',
    'TM02.2.1',
    'U03.2.3'
];

@Component({
    selector: 'app-workflow-dialog',
    templateUrl: './workflow-dialog.component.html',
    styleUrls: ['./workflow-dialog.component.scss']
})
export class WorkflowDialogComponent implements OnInit, OnDestroy {
    formGroup: FormGroup;
    WorkflowSendType = WorkflowSendType;
    workflowSendTypes = workflowSendTypes;
    stageUsers: SelectOption[];
    filteredStageUsers: Observable<SelectOption[]>;
    currentWorkflow: Workflow;
    newWorkflow = new Workflow();
    workflows: Workflow[];
    dicRouteStages: DicRouteStage[];
    filteredRouteStages: DicRouteStage[];
    filteredStagesList: Observable<SelectOption[]>;
    descriptionControl: FormControl;
    sendTypeSubject: BehaviorSubject<WorkflowSendType>;
    hideUserInput = false;
    isBulletinRequired = false;
    parallelEnd = false;
    nextStageId: FormControl

    private onDestroy = new Subject();

    constructor(
        private fb: FormBuilder,
        private workflowService: WorkflowService,
        private bulletinService: BulletinService,
        private routeStageService: RouteStageService,
        private dialogRef: MatDialogRef<WorkflowDialogComponent>,
        private changeDetectorRef: ChangeDetectorRef,
        @Inject(MAT_DIALOG_DATA) private data: any
    ) {
        this.nextStageId = new FormControl();

        this.buildWorkflowForm();

        if (this.data.currentWorkflow && this.data.ownerId && this.data.ownerType) {
            this.newWorkflow = this.createRawWorkflow(this.data.currentWorkflow);
            this.currentWorkflow = data.currentWorkflow;

            this.workflowService
                .get(this.data.ownerId, this.data.ownerType)
                .takeUntil(this.onDestroy)
                .subscribe(
                    workflows =>
                        (this.workflows = workflows.sort((w1, w2) => w2.id - w1.id))
                );

            this.workflowService
                .getRouteStageById(this.data.currentWorkflow.currentStageId)
                .takeUntil(this.onDestroy)
                .subscribe((currentStage: DicRouteStage) => {
                    this.dicRouteStages = [];
                    this.dicRouteStages.push(currentStage);
                    this.formGroup.controls['fromStageId'].setValue(currentStage.id);
                });

            this.workflowService
                .isParallelWorkflow(this.data.currentWorkflow.id)
                .takeUntil(this.onDestroy)
                .subscribe(res =>{
                    if(res === true){
                        this.parallelEnd = true;
                        workflowSendTypes.push({ type: WorkflowSendType.FinishParallelProcessing, nameRu: 'Завершить паралельный этап' });
                        this.workflowSendTypes = workflowSendTypes;
                        
                        this.formGroup.controls['workflowSendType'].setValue(WorkflowSendType.FinishParallelProcessing);
                    }
                });
        }
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

  private _filterStage(name: string): SelectOption[] {
    const filterValue = name.toLowerCase();

    return this.filteredRouteStages.filter(option => option.nameRu.toLowerCase().includes(filterValue));
  }

  displayStageFn(stageId?: any): string | undefined {
      return stageId ? stageId.nameRu : undefined;
  }

  getDisplayUser() {
    return (value: number) => this.displayUser(value);
  }

  displayUser(userId: number): string | null {
    if (this.stageUsers) {
      const stageUser = this.stageUsers.find(entry => entry.id === userId);

      return stageUser ? stageUser.nameRu : null;
    }

    return null;
  }

  ngOnInit() {
        this.formGroup
            .get('currentStageId')
            .valueChanges.takeUntil(this.onDestroy)
            .filter(value => !!value)
            .switchMap(stageId => this.workflowService.getStageUsers(stageId))
            .subscribe(data => {
                this.stageUsers = data;
                this.filteredStageUsers = this.formGroup.controls['currentUserId'].valueChanges
                    .pipe(
                        startWith(''),
                        map(value => typeof value === 'string' ? value : value ? (value as SelectOption).nameRu : null),
                        map(name => name ? this._filter(name) : this.stageUsers.slice())
                    );
            });

        if (!this.filteredRouteStages) {
          this.filteredRouteStages = [];
        }
      this.filteredStagesList = this.nextStageId.valueChanges
        .pipe(
          startWith(''),
          map(value => typeof value === 'string' ? value : value ? (value as SelectOption).nameRu : null),
          map(name => name ? this._filterStage(name) : this.filteredRouteStages.slice())
        );

    let th = this
    this.nextStageId.valueChanges.subscribe(ns => {
      if (ns && ns.id) {
        th.formGroup.get('currentStageId').setValue(ns.id);
        th.onNextStageChange(ns.id);
      }
    });


        this.sendTypeSubject = new BehaviorSubject<WorkflowSendType>(
            WorkflowSendType.ToNextByRoute
        );
        this.formGroup.patchValue(this.newWorkflow);
        this.updateCurrentUserState(this.data.currentWorkflow.currentStageCode);

        if (this.newWorkflow.isAuto) {
            this.workflowSendTypes = this.workflowSendTypes.filter(
                w => w !== WorkflowSendType.ToNextByRoute
            );
            this.formGroup
                .get('workflowSendType')
                .patchValue(WorkflowSendType.ReturnByRoute);
            this.onRouteChange(WorkflowSendType.ReturnByRoute);
        } else {
            this.formGroup
                .get('workflowSendType')
                .patchValue(WorkflowSendType.ToNextByRoute);
            this.onRouteChange(WorkflowSendType.ToNextByRoute);
        }

        this.routeStageService
            .isFirst(this.data.currentWorkflow.currentStageId)
            .switchMap(
                (first: boolean): Observable<Object> => {
                    // При этапе создании заявки и при этапах генерации услуг блокировать направление возврата, по умолчанию - далее по маршруту
                    switch (this.data.ownerType) {
                        case OwnerType.Request:
                            if (
                                first ||
                                (this.isPaymentsStage(
                                    this.data.currentWorkflow.currentStageCode
                                ) &&
                                    this.areInvoicesPayed(this.data.requestDetails)
                                ) 
                            ) {
                                this.workflowSendTypes = this.workflowSendTypes.filter(
                                    (t: any) => t.type !== WorkflowSendType.ReturnByRoute
                                );
                            }
                            return null;
                        default:
                            return null;
                    }
                }
            );
        this.changeDetectorRef.detectChanges();
    }

    onNextStageChange(stageId: number) {

        const stage = this.filteredRouteStages.filter(s => s.id === stageId)[0];
            if (['OD04.6', 'OD04.4', 'OD03.2', 'OD04.5'].includes(stage.code)) {
                this.isBulletinRequired = true;
                this.formGroup.get('bulletinId').setValidators(Validators.required);
            } else {
                this.isBulletinRequired = false;
                this.formGroup.get('bulletinId').clearValidators();
            }
            this.formGroup.get('bulletinId').updateValueAndValidity();
            this.formGroup.controls.isComplete.patchValue(stage.isLast);
            this.updateCurrentUserState(stage.code);
            if (!this.hideUserInput) {
                this.formGroup.controls.currentUserId.reset();
            }
    }

    private updateCurrentUserState(stageCode: string) {
        // this.hideUserInput = mistakeStageCodes.includes(stageCode);
        if (this.hideUserInput) {
            this.formGroup.markAsDirty();
        }
    }

    ngOnDestroy(): void {
        this.onDestroy.next();
    }

    onRouteChange(sendType: WorkflowSendType) {
        if(this.parallelEnd){
            this.formGroup.controls['workflowSendType'].setValue(WorkflowSendType.FinishParallelProcessing);
            this.formGroup.controls['currentUserId'].disable();
            this.formGroup.controls['workflowSendType'].disable();
            this.formGroup.controls['description'].disable();
            this.formGroup.clearValidators();
        }
        else
        switch (sendType) {
            case WorkflowSendType.ToNextByRoute:   
                    this.resetDescriptionField();
                    this.workflowService
                    .getNextStagesByOwnerId(this.data.ownerId, this.data.ownerType)
                    .takeUntil(this.onDestroy)
                    .subscribe(nextStages => {
                        if (!nextStages.length) {
                            this.formGroup
                                .get('workflowSendType')
                                .patchValue(WorkflowSendType.ReturnByRoute);
                            this.onRouteChange(WorkflowSendType.ReturnByRoute);
                            return;
                        }
                        this.filteredRouteStages = nextStages;
                        if (this.filteredRouteStages.length > 1) {
                            this.formGroup.get('currentStageId').enable();
                        } else {
                            this.formGroup.get('currentStageId').disable();
                        }
                        // this.formGroup.get('currentStageId').setValue(nextStages[0].id);
                        // this.onNextStageChange(nextStages[0].id);
                        // для фикса
                        // this.nextStageId.patchValue(nextStages[0]);
                    });

                this.formGroup.controls['currentUserId'].setValue('');
                this.formGroup.controls['currentUserId'].enable();
                break;
            case WorkflowSendType.ToCurrentStage:
                    this.formGroup.controls['description'].setValidators(Validators.required);
                    this.workflowService
                    .getRouteStageById(this.currentWorkflow.currentStageId)
                    .takeUntil(this.onDestroy)
                    .subscribe(data => {
                        this.filteredRouteStages = [data];
                        this.hideUserInput = false;
                        this.formGroup
                            .get('fromStageId')
                            .setValue(this.currentWorkflow.currentStageId);
                        // this.formGroup
                        //     .get('currentStageId')
                        //     .setValue(this.currentWorkflow.currentStageId);
                        this.nextStageId.patchValue(data);
                        this.formGroup.get('currentUserId').setValue('');
                        this.formGroup.get('currentUserId').enable();
                    });
                break;
            case WorkflowSendType.ReturnByRoute:
                    this.formGroup.controls['currentUserId'].disable();
                    this.formGroup.controls['currentUserId'].setValue(
                        this.data.currentWorkflow.fromUserNameRu
                    );
                    this.formGroup.controls['description'].setValidators(Validators.required);
                    this.workflowService
                        .getPreviousStagesByOwnerId(this.data.ownerId, this.data.ownerType)
                        .takeUntil(this.onDestroy)
                        .subscribe(previousStage => {
                            if (!previousStage) {
                                throw Error('Transfer is unavailable!');
                            }
                            this.filteredRouteStages = [];
                            this.filteredRouteStages.push(previousStage);
                            // this.formGroup.get('currentStageId').setValue(previousStage.id);
                            this.nextStageId.patchValue(previousStage);
                        });
                break;
            default:
                throw Error(`Unknown send type: ${sendType}`);
        }

        const possibleStageCount = !!this.filteredRouteStages
            ? this.filteredRouteStages.length
            : 0;
        if (possibleStageCount > 1) {
            this.formGroup.controls.currentStageId.enable();
        }
        if (possibleStageCount > 0) {
        }

        this.sendTypeSubject.next(sendType);
    }

    onSubmit() {
        if(this.parallelEnd)
        {
            let parallelWorkflow = new Workflow();
            parallelWorkflow.workflowSendType = WorkflowSendType.FinishParallelProcessing;
            this.dialogRef.close(parallelWorkflow);
            return Observable.of(parallelWorkflow);
        }
        if (this.formGroup.invalid || this.filteredRouteStages.length === 0) {
            return;
        }
        this.formGroup.markAsPristine();
        const values = this.formGroup.getRawValue();

        const bulletinId = !!values.bulletinId.id
            ? values.bulletinId.id
            : values.bulletinId.bulletinId;
        if (this.isBulletinRequired) {
            const bulletinData = {
                bulletinId: bulletinId,
                protectionDocId: values.ownerId
            };
            this.bulletinService
                .attachProtectionDocToBulletin(bulletinData)
                .takeUntil(this.onDestroy)
                .subscribe();
        }
        const rawWorkflow = new Workflow();
        Object.assign(rawWorkflow, values);
        Observable.combineLatest(
            this.workflowService.getRouteStageById(rawWorkflow.fromStageId),
            this.workflowService.getRouteStageById(rawWorkflow.currentStageId)
        )
            .switchMap(
                ([fromStage, currentStage]): Observable<Workflow> => {
                    rawWorkflow.currentStageCode = currentStage.code;
                    rawWorkflow.fromStageCode = fromStage.code;
                    rawWorkflow.isComplete = currentStage.isLast;
                    rawWorkflow.isMain = currentStage.isMain;
                    rawWorkflow.routeId = currentStage.routeId;
                    return Observable.of(rawWorkflow);
                }
            )
            .subscribe(newWorkflow => {
                this.workflowService
                    .add(newWorkflow, this.data.ownerType)
                    .takeUntil(this.onDestroy)
                    .subscribe(workflow => {
                        this.dialogRef.close(workflow);
                    }, console.log);
            });
    }

    onWorkflowCancel() {
        this.dialogRef.close();
    }

    private buildWorkflowForm() {
        this.formGroup = this.fb.group({
            workflowSendType: ['', Validators.required],
            ownerId: ['', Validators.required],
            bulletinId: [''],
            fromStageId: [{ value: '', disabled: true }],
            fromUserId: [{ value: '', disabled: true }],
            currentStageId: [{ value: '', disabled: true }],
            currentUserId: ['', Validators.required],
            routeId: [''],
            description: [''],
            isComplete: [''],
        });
    }

    /**
     * Action for reset and close dialog window
     */

    private resetDescriptionField() {
        this.formGroup.controls['description'].clearValidators();
        this.formGroup.controls['description'].setValue('');
        this.formGroup.markAsPristine();
        this.formGroup.markAsUntouched();
        this.formGroup.updateValueAndValidity();
    }

    private createRawWorkflow(currentWorkflow: Workflow): Workflow {
        return new Workflow({
            ownerId: currentWorkflow.ownerId,
            fromStageId: currentWorkflow.currentStageId,
            fromUserId: currentWorkflow.currentUserId,
            fromUserNameRu: currentWorkflow.currentUserNameRu
        });
    }

    /**
     * Предикат проверки этапа, является ли этапом оплат
     * Возвращает положительный результат при этапах применения оплат
     * @param {string} stageCode публичный код этапа
     * @returns {boolean}
     * @memberof WorkflowBusinessService
     */
    private isPaymentsStage(stageCode: string): boolean {
        return (
            RouteStageCodes.stageCodes.payment.allCodes().includes(stageCode) ||
            RouteStageCodes.stageCodes.formalExamComplete
                .allCodes()
                .includes(stageCode) ||
            RouteStageCodes.stageCodes.prepareSendToGosReestr
                .allCodes()
                .includes(stageCode)
        );
    }

    private areInvoicesPayed(requestDetails: RequestDetails): boolean {
        return (
            requestDetails.invoiceDtos.filter(i => i.statusCode === 'notpaid')
                .length === 0
        );
    }
}
