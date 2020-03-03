import { Component, OnInit, AfterViewInit, ChangeDetectorRef, OnDestroy } from '@angular/core';

import { Config } from '../../../shared/components/table/config.model';
import { JournalService } from '../../journal.service';
import { RequestService } from '../../../requests/request.service';
import { MaterialsService } from '../../../materials/services/materials.service';
import { UsersService } from '../../../administration/users.service';
import { FormGroup, FormControl } from '@angular/forms';
import {Subject} from 'rxjs/Subject';
import {Observable} from 'rxjs/Observable';
import { Operators } from '../../../shared/filter/operators';
import {debounceTime, distinctUntilChanged} from 'rxjs/operators';

@Component({
  selector: 'app-journal-staff',
  templateUrl: './journal-staff.component.html',
  styleUrls: ['./journal-staff.component.scss'],
  providers: [UsersService]
})
export class JournalStaffComponent implements OnInit, AfterViewInit, OnDestroy {
  private onDestroy: Subject<any> = new Subject<any>();
  columns: Config[] = [
    new Config({ columnDef: 'id', header: 'ID', class: 'width-50' }),
    new Config({ columnDef: 'fullName', header: 'ФИО', class: 'width-100' }),
    new Config({ columnDef: 'incoming', header: 'Входящие', class: 'width-100' }),
    new Config({ columnDef: 'executed', header: 'Исполненные', class: 'width-100' }),
    new Config({ columnDef: 'onJob', header: 'В работе', class: 'width-100' }),
    new Config({ columnDef: 'notOnJob', header: 'Не в работе', class: 'width-100' }),
    new Config({ columnDef: 'overdue', header: 'Просроченные', class: 'width-100' }),
    new Config({ columnDef: 'outgoing', header: 'Исходящие', class: 'width-100' }),
  ];
  columns2: Config[];
  usersSearch: FormGroup;
  usersList: any[];

  opened = true;
  tableSort: any;
  get source() { return this.journalService; }
  private activeTask = 0;
  public tableState = false;
  private overdueTask = 0;
  private finishedTask = 0;
  private documents = 0;
  private finishedDocuments = 0;
  private overdueDocuments = 0;
  public showMainTable = false;
  private dataSet: any[] = [];
  private users: any[];
  private materials: any[];
  private requests: any[];
  public currentUser;
  public unfilteredUsers;
  public queryParams: any[];
  constructor(private journalService: JournalService,
              private changeDetector: ChangeDetectorRef,
              private requestService: RequestService,
              private materialService: MaterialsService,
              private userService: UsersService) {
  }

  ngOnInit() {
    this.queryParams = [
      {key: 'users' + Operators.equal, value: ''},
      {key: 'fromdate' + Operators.equal, value: ''},
      {key: 'todate' + Operators.equal, value: ''},
    ];
    Observable.combineLatest(
      this.userService.getCurrent(),
      this.userService.get()
    ).takeUntil(this.onDestroy)
      .subscribe(([currentUser, users]) => {
        this.currentUser = currentUser;
        this.unfilteredUsers = users
        this.getUsers(currentUser, users);
      });
    this.usersSearch = new FormGroup({
      users: new FormControl(),
      createDate_from: new FormControl(),
      createDate_to: new FormControl()
    });
    this.usersSearch.valueChanges.pipe(
      debounceTime(2000),
      distinctUntilChanged()
    ).subscribe(data => {
      const queryId = data.users.filter(user => user);
      if (queryId.length) {
        this.recursionTask(queryId);
        this.tableState = true;
        this.opened = false;
        this.dataSet = [];
      } else {
        this.dataSet = [];
        this.getUsers(this.currentUser, this.unfilteredUsers);
        this.tableState = false;
      }
    });
  }
  getUsers(currentUser, users) {
    this.users = users.filter(user => user.departmentId === currentUser.departmentId && currentUser.id !== user.id);
    this.usersList = this.users.map(user => {
      return {key: user.id, name: user.nameRu};
    });
    this.tableState = true;
    const queryId = this.users.map(user => {
      return user.id;
    });
   this.recursionTask(queryId);
  }
  buildUsersQuery(queryId: Array<string>): string {
    return `users=${queryId.join(',')}`;
  }

  recursionTask(ids) {
    const queries = ids.splice(0, 5);
    this.getUsersTask(queries);
    return ids.length ? this.recursionTask(ids) :  null;
  }
  getUsersTask(queryId) {
    this.journalService.getUsersTaskCount(this.buildUsersQuery(queryId))
      .takeUntil(this.onDestroy)
      .subscribe(data => {
        this.showMainTable = false;
        const dataSet = [];
        data.forEach((user, index) => {
          this.dataSet .push({
            id: user.id,
            userName: this.users.find(allUser => allUser.id === user.id).nameRu,
            requestsActive: user.activeTasks,
            requestsCompleted: user.completedTasks,
            requestsOverdue: user.expiredTasks,
            documents: user.documents,
            completedDocuments: user.completedDocuments,
            overdueDocuments: user.expiredDocuments
          });
          this.activeTask += user.activeTasks;
          this.finishedTask += user.completedTasks;
          this.overdueTask += user.expiredTasks;
          this.documents += user.documents;
          this.finishedDocuments += user.completedDocuments;
          this.overdueDocuments += user.expiredDocuments;
          if (index === data.length - 1) {
            this.renderColumns();
          }
        });
        this.dataSet = this.dataSet.concat(dataSet);
      });
  }
  renderColumns() {
    this.columns2 = [
      new Config({ columnDef: 'userName', header: '',
        class: 'user-name-header'}),
      new Config({ columnDef: 'requestsActive', header: `${this.activeTask}`,
        secondHeader: 'Активные задачи',
        class: 'active-table-item'}),
      new Config({ columnDef: 'requestsCompleted', header: `${this.finishedTask}`,
        secondHeader: 'Завершенные задачи',
        class: 'active-table-item'}),
      new Config({ columnDef: 'requestsOverdue', header: `${this.overdueTask}`,
        secondHeader: 'Просроченные задачи',
        class: 'overdue-table-item'}),
      new Config({ columnDef: 'documents', header: `${this.documents}`,
        secondHeader: 'Документы',
        class: 'active-table-item'}),
      new Config({ columnDef: 'completedDocuments', header: `${this.finishedDocuments}`,
        secondHeader: 'Завершенные документы',
        class: 'active-table-item'}),
      new Config({ columnDef: 'overdueDocuments', header: `${this.overdueDocuments}`,
        secondHeader: 'Просроченные документы',
        class: 'overdue-table-item'}),
    ];
    this.showMainTable = true;
  }


  ngAfterViewInit(): void {
    // TODO: workaround против этого бага https://github.com/angular/material2/issues/5593
    this.changeDetector.detectChanges();
  }
  ngOnDestroy(): void {
    this.onDestroy.next(null);
  }
}


