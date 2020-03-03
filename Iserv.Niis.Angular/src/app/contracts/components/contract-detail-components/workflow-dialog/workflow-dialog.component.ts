import { Workflow, WorkflowSendType } from '../../../../shared/services/models/workflow-model';
import { WorkflowService } from '../../../../shared/services/workflow.service';
import { Component, Inject, Input, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import { Observable } from 'rxjs/Observable';
import { Subject } from 'rxjs/Subject';
import { Subscription } from 'rxjs/Subscription';

import { DictionaryService } from '../../../../shared/services/dictionary.service';
import { DicRouteStage } from '../../../../shared/services/models/base-dictionary';
import { DictionaryType } from '../../../../shared/services/models/dictionary-type.enum';
import { OwnerType } from '../../../../shared/services/models/owner-type.enum';
import { SelectOption } from '../../../../shared/services/models/select-option';
import { RouteStageService } from '../../../../shared/services/route-stage.service';
import { WorkflowBusinessService } from '../../../services/workflow-business.service';
import {map, startWith} from 'rxjs/operators';

const workflowSendTypes: Object[] = [
  { type: WorkflowSendType.ToNextByRoute, nameRu: 'Далее по маршруту' },
  { type: WorkflowSendType.ToCurrentStage, nameRu: 'По текущему этапу' },
  { type: WorkflowSendType.ReturnByRoute, nameRu: 'Возврат на предыдущий этап' }
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
  descriptionControl: FormControl;
  sendTypeSubject: BehaviorSubject<WorkflowSendType>;

  private onDestroy = new Subject();

  constructor(
    private fb: FormBuilder,
    private workflowService: WorkflowService,
    private dictionaryService: DictionaryService,
    private workflowBusinessService: WorkflowBusinessService,
    private routeStageService: RouteStageService,
    private dialogRef: MatDialogRef<WorkflowDialogComponent>,
    @Inject(MAT_DIALOG_DATA) private data: any) {
    this.buildWorkflowForm();
    if (this.data.details && this.data.details.currentWorkflow) {
      this.currentWorkflow = this.data.details.currentWorkflow;
      this.newWorkflow = this.workflowBusinessService.createRawWorkflow(this.currentWorkflow);
      this.formGroup.patchValue(this.newWorkflow);
      this.workflowService
        .get(this.data.details.id, OwnerType.Contract)
        .takeUntil(this.onDestroy)
        .subscribe(workflows => this.workflows =
          workflows.sort((w1, w2) => w2.id - w1.id));

      this.routeStageService
        .isFirst(this.currentWorkflow.currentStageId)
        .switchMap((first: boolean): Observable<Workflow> => {
          // При этапе создании заявки и при этапах генерации услуг блокировать направление возврата, по умолчанию - далее по маршруту
          if (first || (this.workflowBusinessService.isPaymentsStage(this.currentWorkflow.currentStageCode) &&
            this.workflowBusinessService.areInvoicesPayed(this.data.details))) {
            this.workflowSendTypes = this.workflowSendTypes.filter((t: any) => t.type !== WorkflowSendType.ReturnByRoute);
          }

          this.sendTypeSubject = new BehaviorSubject<WorkflowSendType>(WorkflowSendType.ToNextByRoute);
          return this.workflowBusinessService
            .generateWorkflowBy(this.sendTypeSubject, this.data.details, this.workflows);
        })
        .takeUntil(this.onDestroy)
        .subscribe(generatedWorkflow => {
          if (!generatedWorkflow) {
            throw Error('Transfer is unavailable!');
          }

          this.newWorkflow = generatedWorkflow;
          this.formGroup.patchValue(this.newWorkflow);
        },
          console.log);


      this.workflowService.getNextStagesByContractId(this.data.details.id)
        .takeUntil(this.onDestroy)
        .subscribe(stages => {
          this.filteredRouteStages = stages;
          const possibleStageCount = this.filteredRouteStages.length;
          if (possibleStageCount > 0) {
            this.onNextStageChange(stages[0].id);
          }
          if (possibleStageCount > 1) {
            this.formGroup.controls.currentStageId.enable();
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

  ngOnInit() {
    this.dictionaryService.getBaseDictionary(DictionaryType.DicRouteStage)
      .takeUntil(this.onDestroy)
      .subscribe((stages: DicRouteStage[]) => {
        this.dicRouteStages = stages;
      });

    this.formGroup.get('currentStageId').valueChanges
      .takeUntil(this.onDestroy)
      .startWith(null)
      .switchMap(stageId => this.workflowService.getStageUsers(stageId))
      .subscribe(data => {
        this.stageUsers = data;
        this.filteredStageUsers = this.formGroup.controls['currentUserId'].valueChanges
          .pipe(
              startWith(''),
              map(value => typeof value === 'string' ? value : value ? (value as SelectOption).nameRu : null),
              map(name => name ? this._filter(name) : this.stageUsers ? this.stageUsers.slice() : [])
          );
      });
  }

  onNextStageChange(stageId: number) {
    if (this.dicRouteStages) {
      const stage = this.dicRouteStages.filter(s => s.id === stageId)[0];
      if (stage) {
        this.formGroup.controls.isComplete.patchValue(stage.isLast);
        this.newWorkflow.currentStageId = stage.id;
        this.newWorkflow.currentStageCode = stage.code;
        this.newWorkflow.routeId = stage.routeId;
        this.newWorkflow.isComplete = stage.isLast;
        this.newWorkflow.isMain = stage.isMain;
      }
    }
  }

  ngOnDestroy(): void {
    this.onDestroy.next();
  }

  onRouteChange(sendType: WorkflowSendType) {
    switch (sendType) {
      case WorkflowSendType.ToNextByRoute:
        this.resetDescriptionField();
        break;
      case WorkflowSendType.ToCurrentStage:
        this.filteredRouteStages = this.dicRouteStages;
        this.resetDescriptionField();

        this.formGroup.controls['currentUserId'].setValue('');
        this.formGroup.controls['currentUserId'].enable();
        break;
      case WorkflowSendType.ReturnByRoute:
        this.filteredRouteStages = this.dicRouteStages;
        this.formGroup.controls['currentUserId'].disable();
        this.formGroup.controls['description'].setValidators(Validators.required);
        break;
      default:
        throw Error(`Unknown send type: ${sendType}`);
    }

    this.sendTypeSubject.next(sendType);
  }

  onSubmit() {
    if (this.formGroup.invalid) { return; }
    this.formGroup.markAsPristine();

    const values = this.formGroup.getRawValue();
    const currentUser = this.stageUsers.find(stageUser => (stageUser.nameRu === values.currentUserId));
    values.currentUserId = currentUser.id;

    Object.assign(this.newWorkflow, values);
    this.workflowService.add(this.newWorkflow, OwnerType.Contract)
      .takeUntil(this.onDestroy)
      .subscribe(workflow => {
        this.dialogRef.close(workflow);
      },
        console.log);
  }

  onWorkflowCancel() {
    this.dialogRef.close();
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
    });
  }

  private resetDescriptionField() {
    this.formGroup.controls['description'].clearValidators();
    this.formGroup.controls['description'].setValue('');
    this.formGroup.markAsPristine();
    this.formGroup.markAsUntouched();
    this.formGroup.updateValueAndValidity();
  }
}
