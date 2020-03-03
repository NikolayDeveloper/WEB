import { Component, Inject, OnInit, ViewChild } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef, MatPaginator, MatTableDataSource } from '@angular/material';

import { ConfigService } from '../../../core';
import { ExpertInfo } from '../../models/expert-info.model';

@Component({
  selector: 'app-result-employment-experts-dialog',
  templateUrl: './result-employment-experts-dialog.component.html',
  styleUrls: ['./result-employment-experts-dialog.component.scss']
})
export class ResultEmploymentExpertsDialogComponent implements OnInit {
  displayedColumns = ['UserName', 'IpcCodes', 'RequestNumbers',
    'CountRequests', 'CountCompletedRequestsCurrentYear', 'EmploymentIndexExpert'];

  @ViewChild(MatPaginator) paginator: MatPaginator;

  dataSource: MatTableDataSource<ExpertInfo>;
  constructor(public dialogRef: MatDialogRef<ResultEmploymentExpertsDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private configService: ConfigService) { }

  ngOnInit() {
    this.dataSource = new MatTableDataSource(this.data.experts);

    this.paginator.pageIndex = 0;
    this.paginator.pageSize = this.configService.pageSize;
    this.paginator.pageSizeOptions = this.configService.pageSizeOptions;
    this.dataSource.paginator = this.paginator;
  }

  onCancel(): void {
    this.dialogRef.close(null);
  }
}
