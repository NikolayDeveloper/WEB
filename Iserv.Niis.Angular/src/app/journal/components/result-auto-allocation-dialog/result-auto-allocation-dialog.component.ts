import { Component, Inject, OnInit, ViewChild } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialog, MatDialogConfig, MatDialogRef, MatTableDataSource, MatPaginator } from '@angular/material';

import { AutoAllocationService } from '../../../requests/auto-allocation.service';
import { Request } from '../../../requests/models/request';
import { ExpertInfo } from '../../models/expert-info.model';
import {
  ResultEmploymentExpertsDialogComponent,
} from '../result-employment-experts-dialog/result-employment-experts-dialog.component';
import { ConfigService } from '../../../core';

@Component({
  selector: 'app-result-auto-allocation-dialog',
  templateUrl: './result-auto-allocation-dialog.component.html',
  styleUrls: ['./result-auto-allocation-dialog.component.scss']
})
export class ResultAutoAllocationDialogComponent implements OnInit {

  displayedColumns = ['RequestNumber', 'CoefficientComplexity', 'IPC',
    'CountPages', 'CountIndependentItems', 'Expert'];

  @ViewChild(MatPaginator) paginator: MatPaginator;

  dataSource: MatTableDataSource<Request>;

  experts: ExpertInfo[] = [];
  constructor(public dialogRef: MatDialogRef<ResultAutoAllocationDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private autoAllocationService: AutoAllocationService,
    private dialog: MatDialog,
    private configService: ConfigService) { }

  ngOnInit() {
    this.dataSource = new MatTableDataSource<Request>(this.data.requests);

    this.paginator.pageIndex = 0;
    this.paginator.pageSize = this.configService.pageSize;
    this.paginator.pageSizeOptions = this.configService.pageSizeOptions;
    this.dataSource.paginator = this.paginator;

    this.autoAllocationService.getExperts()
      .subscribe((result: ExpertInfo[]) => {
        this.experts = result;
      });
  }
  onCancel(): void {
    this.dialogRef.close(null);
  }
  onSave() {
    this.autoAllocationService.allocate(this.dataSource.data)
      .subscribe((result: any) => {
        this.dialogRef.close(null);
        window.location.reload();
      });
  }

  openDialogResultEmploymentExperts() {
    const config = new MatDialogConfig();
    config.width = '70vw';
    config.height = '80vh';
    config.data = {
      experts: this.experts
    };
    const dialogRef = this.dialog.open(ResultEmploymentExpertsDialogComponent, config);
    dialogRef.afterClosed()
      .subscribe((result: any) => {

      });
  }
}
