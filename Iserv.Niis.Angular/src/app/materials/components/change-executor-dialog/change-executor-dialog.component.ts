import { Component, Inject, OnInit, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialog } from '@angular/material';
import { MaterialsService } from '../../services/materials.service';
import { WorkflowBusinessService } from '../../services/workflow-business/workflow-business.service';
import { WorkflowService } from '../../../shared/services/workflow.service';
import { SelectOption } from '../../../shared/services/models/select-option';
import { MaterialWorkflow } from '../../../shared/services/models/workflow-model';
import { Subject } from 'rxjs/Subject';
import { Observable } from 'rxjs/Observable';
import { map, startWith } from 'rxjs/operators';

@Component({
  selector: 'app-change-executor-dialog',
  templateUrl: './change-executor-dialog.component.html',
  styleUrls: ['./change-executor-dialog.component.scss']
})
export class ChangeExecutorDialogComponent implements OnInit, OnDestroy {
  formGroup: FormGroup;
  stageUsers: SelectOption[];
  filteredStageUsers: Observable<SelectOption[]>;
  currentWorkflow: MaterialWorkflow;

  private onDestroy = new Subject();

  constructor(
    private fb: FormBuilder,
    private materialsService: MaterialsService,
    private workflowService: WorkflowService,
    private workflowBusinessService: WorkflowBusinessService,
    private dialogRef: MatDialogRef<ChangeExecutorDialogComponent>,
    public dialog: MatDialog,
    @Inject(MAT_DIALOG_DATA) private data: any
  ) {
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

  private buildWorkflowForm() {
    this.formGroup = this.fb.group({
      currentUserId: '',
    });
  }

  open() {
    if (this.data.detail) {
      const currentWorkflow = this.workflowBusinessService.getCurrentWorkflow(this.data.detail);
      if (currentWorkflow) {
        this.currentWorkflow = currentWorkflow;
      }
    }
  }

  ngOnInit() {
      const currentWorkflow = this.workflowBusinessService.getCurrentWorkflow(this.data.detail);
      this.workflowService.getStageUsers(currentWorkflow.currentStageId)
        .takeUntil(this.onDestroy)
        .subscribe(data => {
          this.stageUsers = [...data];
          this.filteredStageUsers = this.formGroup.controls['currentUserId'].valueChanges
            .pipe(
                startWith(''),
                map(value => typeof value === 'string' ? value : (value as SelectOption).nameRu),
                map(name => name ? this._filter(name) : this.stageUsers.slice())
            );
        });
  }

  ngOnDestroy(): void {
    this.onDestroy.next();
  }

  onSubmit() {
    this.formGroup.markAsPristine();
    const values = this.formGroup.getRawValue();
    const currentUser = this.stageUsers.find(stageUser => (stageUser.nameRu === values.currentUserId));
    values.currentUserId = currentUser.id;

    Object.assign(this.currentWorkflow, values);

    this.materialsService.updateWorkflow(this.currentWorkflow)
            .takeUntil(this.onDestroy)
            .subscribe(workflow => {
              this.dialogRef.close(workflow);
            }, console.log);
  }

  isDisabledButtonSend(): boolean {
    return !this.formGroup.get('currentUserId').value;
  }

  onCancel() {
    this.dialogRef.close();
  }
}
