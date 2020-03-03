import { Component, EventEmitter, Input, OnInit, Output, ChangeDetectorRef, AfterViewInit } from '@angular/core';
import { UsersService } from 'app/administration/users.service';
import { Subject } from 'rxjs/Rx';

import { Config } from '../../../../shared/components/table/config.model';

@Component({
  selector: 'app-users-list',
  templateUrl: './users-list.component.html',
  styleUrls: ['./users-list.component.scss']
})
export class UsersListComponent implements AfterViewInit {
  columns: Config[] = [
    new Config({ columnDef: 'id', header: 'ID', class: 'width-50' }),
    new Config({ columnDef: 'nameRu', header: 'Ф.И.О', class: 'width-200 cell-small-text' }),
    new Config({ columnDef: 'roleNameRu', header: 'Роль', class: 'cell-small-text width-300' }),
    new Config({ columnDef: 'email', header: 'Email', class: 'width-200' }),
    new Config({ columnDef: 'positionNameRu', header: 'Должность', class: 'width-200 cell-small-text' }),
    new Config({ columnDef: 'departmentNameRu', header: 'Департамент', class: 'width-200 cell-small-text' }),
    new Config({ columnDef: 'divisionNameRu', header: 'Служба', class: 'width-200 cell-small-text' }),
  ];

  get source() { return this.usersService; }

  @Input() reset = new Subject();
  @Output()
  select = new EventEmitter<number>();

  constructor(private usersService: UsersService,
    private changeDetector: ChangeDetectorRef) { }

  ngAfterViewInit() {
    // TODO: workaround против этого бага https://github.com/angular/material2/issues/5593
    this.changeDetector.detectChanges();
  }

  onSelect(row: any) {
    this.select.emit(row.id);
  }
}
