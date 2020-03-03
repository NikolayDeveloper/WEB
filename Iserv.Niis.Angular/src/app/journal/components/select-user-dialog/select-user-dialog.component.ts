import { Component, Inject, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { Observable } from 'rxjs';
import { Subject } from 'rxjs/Subject';
import { DicRouteStage } from '../../../shared/services/models/base-dictionary';
import { SelectOption } from '../../../shared/services/models/select-option';
import { WorkflowService } from '../../../shared/services/workflow.service';
import { map, startWith } from 'rxjs/operators';

@Component({
  selector: 'app-select-user-dialog',
  templateUrl: './select-user-dialog.component.html',
  styleUrls: ['./select-user-dialog.component.scss']
})
export class SelectUserDialogComponent implements OnInit, OnDestroy {
  formGroup: FormGroup;
  currentStage: DicRouteStage;
  stageUsers: SelectOption[];
  filteredStageUsers: Observable<SelectOption[]>;
  stageUsersPrint: SelectOption[];
  filteredStageUsersPrint: Observable<SelectOption[]>;
  stageUsersMaintenance: SelectOption[];
  filteredStageUsersMaintenance: Observable<SelectOption[]>;
  stageUsersDescriptions: SelectOption[];
  filteredStageUsersDescriptions: Observable<SelectOption[]>;
  bulletinUsers: SelectOption[];
  filteredBulletinUsers: Observable<SelectOption[]>;
  supportUsers: SelectOption[];
  filteredSupportUsers: Observable<SelectOption[]>;
  areAllSelectedTrademarks = false;
  nextStageCode: string;
  areSelectedRequests = false;
  areAccelerated = false;
  nextStageIsParalel = false;

  private onDestroy = new Subject();

  constructor(
    private workflowService: WorkflowService,
    private fb: FormBuilder,
    private dialogRef: MatDialogRef<SelectUserDialogComponent>,
    @Inject(MAT_DIALOG_DATA) private data: any
  ) {
    this.areAllSelectedTrademarks = data.areAllSelectedTrademarks;
    this.areSelectedRequests = data.areSelectedRequests;
    this.nextStageCode = data.nextStageCode;
    this.nextStageIsParalel = data.nextStageIsParalel;

    // data.areAccelerated
    //   .takeUntil(this.onDestroy)
    //   .subscribe(x =>
    //     this.areAccelerated = x === true ? true : false
    //   );
    this.areAccelerated = Boolean(data.areAccelerated);
    this.buildForm();
  }

  /**
   * Фильтрует массив `data` по полю `nameRu`
   * @param name Что искать в поле `nameRu`
   * @return Отфильтрованный массив
   */
  private _filter(data, name: string): SelectOption[] {
    const filterValue = name.toLowerCase();

    return data.filter(option => option.nameRu.toLowerCase().includes(filterValue));
  }

  ngOnInit() {
    this.workflowService
      .getRouteStageByCode(this.nextStageCode)
      .takeUntil(this.onDestroy)
      .switchMap(
        (stage): Observable<DicRouteStage> => {
          this.currentStage = stage;
          return Observable.of(this.currentStage);
        }
      )
      .subscribe(stage => {
        this.workflowService
          .getStageUsers(stage.id)
          .takeUntil(this.onDestroy)
          .subscribe(stageUsers => {
            this.stageUsers = stageUsers;
            this.filteredStageUsers = this.formGroup.controls['currentUserId'].valueChanges
              .pipe(
                  startWith(''),
                  map(value => typeof value === 'string' ? value : (value as SelectOption).nameRu),
                  map(name => name ? this._filter(this.stageUsers, name) : this.stageUsers.slice())
              );
          });
      });
    this.workflowService
      .getRouteStageByCode('OD01.3')
      .takeUntil(this.onDestroy)
      .switchMap(
        (stage): Observable<DicRouteStage> => {
          return Observable.of(stage);
        }
      )
      .subscribe(stage => {
        this.workflowService
          .getStageUsers(stage.id)
          .takeUntil(this.onDestroy)
          .subscribe(stageUsers => {
            this.stageUsersPrint = stageUsers;
            this.filteredStageUsersPrint = this.formGroup.controls['nextUserForPrintId'].valueChanges
              .pipe(
                  startWith(''),
                  map(value => typeof value === 'string' ? value : (value as SelectOption).nameRu),
                  map(name => name ? this._filter(this.stageUsersPrint, name) : this.stageUsersPrint.slice())
              );
          });
      });
    this.workflowService
        .getRouteStageByCode('OD03.1')
        .takeUntil(this.onDestroy)
        .switchMap(
          (stage): Observable<DicRouteStage> => {
            return Observable.of(stage);
          }
        )
        .subscribe(stage => {
          this.workflowService
            .getStageUsers(stage.id)
            .takeUntil(this.onDestroy)
            .subscribe(stageUsers => {
              this.stageUsersMaintenance = stageUsers;
              this.filteredStageUsersMaintenance = this.formGroup.controls['nextUserForMaintenanceId'].valueChanges
                .pipe(
                    startWith(''),
                    map(value => typeof value === 'string' ? value : (value as SelectOption).nameRu),
                    map(name => name ? this._filter(this.stageUsersMaintenance, name) : this.stageUsersMaintenance.slice())
                );
            });
      });
      this.workflowService
          .getRouteStageByCode('OD01.2.2')
          .takeUntil(this.onDestroy)
          .switchMap(
            (stage): Observable<DicRouteStage> => {
              return Observable.of(stage);
            }
          )
          .subscribe(stage => {
            this.workflowService
              .getStageUsers(stage.id)
              .takeUntil(this.onDestroy)
              .subscribe(stageUsers => {
                this.stageUsersDescriptions = stageUsers;
                this.filteredStageUsersDescriptions = this.formGroup.controls['nextUserForDescriptionsId'].valueChanges
                  .pipe(
                      startWith(''),
                      map(value => typeof value === 'string' ? value : (value as SelectOption).nameRu),
                      map(name => name ? this._filter(this.stageUsersDescriptions, name) : this.stageUsersDescriptions.slice())
                  );
              });
        });
    this.workflowService
      .getBulletinUsers()
      .takeUntil(this.onDestroy)
      .subscribe(bulletinUsers => {
        this.bulletinUsers = bulletinUsers;
        this.filteredBulletinUsers = this.formGroup.controls['bulletinUserId'].valueChanges
          .pipe(
              startWith(''),
              map(value => typeof value === 'string' ? value : (value as SelectOption).nameRu),
              map(name => name ? this._filter(this.bulletinUsers, name) : this.bulletinUsers.slice())
          );
      });
    this.workflowService
      .getSupportUsers()
      .takeUntil(this.onDestroy)
      .subscribe(supportUsers => {
        this.supportUsers = supportUsers;
        this.filteredSupportUsers = this.formGroup.controls['supportUserId'].valueChanges
          .pipe(
              startWith(''),
              map(value => typeof value === 'string' ? value : (value as SelectOption).nameRu),
              map(name => name ? this._filter(this.supportUsers, name) : this.supportUsers.slice())
          );
      });
    if (this.areAllSelectedTrademarks && !this.areSelectedRequests) {
      this.formGroup.get('bulletinUserId').clearValidators();
      this.formGroup.get('supportUserId').clearValidators();
      this.formGroup.get('bulletinUserId').updateValueAndValidity();
      this.formGroup.get('supportUserId').updateValueAndValidity();
    }
  }

  ngOnDestroy(): void {
    this.onDestroy.next();
  }

  onSubmit() {
    if (this.formGroup.invalid) {
      return;
    }
    this.formGroup.markAsPristine();

    const values = this.formGroup.getRawValue();
    const users = {
      currentUserId: this.stageUsers,
      nextUserForPrintId: this.stageUsersPrint,
      nextUserForDescriptionsId: this.stageUsersDescriptions,
      nextUserForMaintenanceId: this.stageUsersMaintenance,
      bulletinUserId: this.bulletinUsers,
      supportUserId: this.supportUsers
    };
    for (let key in users) {
      if (values[key]) {
        const currentUser = users[key].find(user => (user.nameRu === values[key]));
        values[key] = currentUser.id;
      }
    }
    this.dialogRef.close(values);
  }

  onCancel() {
    this.dialogRef.close();
  }

  private buildForm() {
    this.formGroup = this.fb.group({
      currentUserId: [''],
      nextUserForPrintId: [''],
      nextUserForDescriptionsId: [''],
      nextUserForMaintenanceId: [''],
      bulletinUserId: [''],
      supportUserId: [''],
      bulletinId: ['']
    });
  }

  private getTime(date?: Date) {
    return date != null ? date.getTime() : 0;
  }
}
