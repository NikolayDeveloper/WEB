import 'rxjs/add/operator/takeUntil';

import { SelectionModel } from '@angular/cdk/collections';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig, MatPaginator, MatSort } from '@angular/material';
import { ActivatedRoute, Router } from '@angular/router';
import { Subject } from 'rxjs/Subject';

import { ConfigService } from '../../../core';
import { AutoAllocationService } from '../../../requests/auto-allocation.service';
import { Request } from '../../../requests/models/request';
import { BaseDataSource } from '../../../shared/base-data-source';
import {
  ResultAutoAllocationDialogComponent,
} from '../result-auto-allocation-dialog/result-auto-allocation-dialog.component';



@Component({
  selector: 'app-journal-auto-allocation',
  templateUrl: './journal-auto-allocation.component.html',
  styleUrls: ['./journal-auto-allocation.component.scss']
})
export class JournalAutoAllocationComponent implements OnInit {

  displayedColumns = ['select', 'id', 'protectionDocTypeValue', 'requestNum', 'dateCreate',
    'currentStageValue', 'ipcCodes'];
  dataSource: BaseDataSource<Request> | null;
  today: Date;
  selectionModel = new SelectionModel<Request>(true, []);
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild('filter') filter: ElementRef;

  private onDestroy = new Subject();


  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private autoAllocationService: AutoAllocationService,
    private configService: ConfigService,
    private dialog: MatDialog,
  ) { }

  ngOnInit() {
    this.paginator.pageIndex = 0;
    this.paginator.pageSize = this.configService.pageSize;
    this.paginator.pageSizeOptions = this.configService.pageSizeOptions;
    let source;
    source = new BaseDataSource<Request>(this.autoAllocationService, this.configService, this.paginator, this.sort, this.filter, false);
    this.dataSource = source;
  }
  onSelect(record: Request) {
    this.router.navigate([record.taskType + 's', record.id]);
  }

  isAllSelected() {
    const numSelected = this.selectionModel.selected.length;
    const numRows = this.dataSource.data.length;
    return numSelected === numRows;
  }

  masterToggle() {
    this.isAllSelected()
      ? this.selectionModel.clear()
      : this.dataSource.data.forEach(row => this.selectionModel.select(row));
  }

  isChecked(row: Request): boolean {
    return this.selectionModel.isSelected(row);
  }

  prepareAllocate() {
    if (this.selectionModel.selected.length > 0) {
      const requestIds = this.selectionModel.selected.map(r => r.id);
      const strRequestIds = requestIds.join(',');
      this.autoAllocationService.prepareAllocate(strRequestIds)
        .subscribe((result: Request[]) => {
          this.openDialogResultAutoAllocation(result);
        });
    }
  }

  openDialogResultAutoAllocation(requests: Request[]) {
    const config = new MatDialogConfig();
    config.disableClose = true;
    config.width = '80vw';
    config.height = '90vh';
    config.data = {
      requests: requests
    };
    const dialogRef = this.dialog.open(ResultAutoAllocationDialogComponent, config);
    dialogRef.afterClosed()
      .subscribe((result: any) => {

      });
  }
}
