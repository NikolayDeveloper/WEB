import { Component, ElementRef, EventEmitter, OnInit, Output, ViewChild } from '@angular/core';
import { MatPaginator, MatSort } from '@angular/material';

import { ConfigService } from '../../../../core';
import { BaseDataSource } from '../../../../shared/base-data-source';
import { RolesService } from '../../../roles.service';
import { Role } from '../models/role.models';
import { Config } from '../../../../shared/components/table/config.model';


@Component({
  selector: 'app-roles-list',
  templateUrl: './roles-list.component.html',
  styleUrls: ['./roles-list.component.scss']
})
export class RolesListComponent implements OnInit {
  columns: Config[] = [
    new Config({ columnDef: 'id', header: 'ID', class: 'width-50' }),
    new Config({ columnDef: 'nameRu', header: 'Название', class: 'width-300' }),
    new Config({ columnDef: 'claimsTotal', header: 'Всего прав', class: 'width-200' }),
    new Config({ columnDef: 'stagesTotal', header: 'Всего этапов', class: 'width-200' }),
    new Config({ columnDef: 'code', header: 'Код', class: 'width-50' }),
  ];

  get source() { return this.rolesService; }

  @Output()
  select = new EventEmitter<number>();

  roleNames: string[] = [];

  constructor(private rolesService: RolesService) { }

  ngOnInit() { }

  onSelect(row: any) {
    this.select.emit(row.id);
  }
}
