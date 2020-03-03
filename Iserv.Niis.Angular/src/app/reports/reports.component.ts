import { Component, OnInit, OnDestroy, ViewChild } from '@angular/core';
import { ReportsService } from './reports.service';
import { ReportDto } from './models/reportdto';
import { ReportData, Row } from './models/reportdata';
import { Subject } from 'rxjs';
import { FormGroup, FormBuilder } from '@angular/forms';
import { ReportFilterComponent } from './components/report-filter/report-filter.component';
import { ChartData } from './models/chart-data';
import { saveAs } from 'file-saver';
import { ChargedPaymentInvoicesReportFilterComponent } from './components/charged-paymnet-invoices-report-filter/charged-paymnet-invoices-report-filter.component';

@Component({
  selector: 'app-reports',
  templateUrl: './reports.component.html',
  styleUrls: ['./reports.component.scss']
})
export class ReportsComponent implements OnInit, OnDestroy {
  formGroup: FormGroup;
  reports: ReportDto[];
  reportData: ReportData;
  chartDataType: number;
  chartData: ChartData;
  headers: Row[];
  body: Row[];
  reportCode: string;
  private onDestroy = new Subject();
  @ViewChild(ReportFilterComponent) ipoFilter: ReportFilterComponent;
  @ViewChild(ChargedPaymentInvoicesReportFilterComponent) cpiFilter: ChargedPaymentInvoicesReportFilterComponent;

  constructor(
    private reportsService: ReportsService,
    private fb: FormBuilder,
  ) {
    this.buildForm();
  }

  ngOnInit(): void {
    this.reportsService.getAllReports()
      .takeUntil(this.onDestroy)
      .subscribe(reports => this.reports = reports);
  }

  ngOnDestroy(): void {
    this.formGroup.reset();
    this.onDestroy.next();
  }

  buildForm() {
    this.formGroup = this.fb.group({
      reportCode: [''],
    });
  }

  onReset() {
    this.formGroup.reset();
    this.reportData = null;
    this.reportCode = null;
    this.chartData = null;
    this.chartDataType = null;
  }

  setReportCode(reportCode) {
    this.reportCode = reportCode;
    this.chartData = null;
    this.reportData = null;
  }

  onGenerate() {
    const condition = this.switchReportCode(this.reportCode);
    this.reportsService.getReportData(condition)
      .takeUntil(this.onDestroy)
      .subscribe(data => {
        this.reportData = data;
        this.headers = this.reportData.rows.filter(r => r.isHeader);
        this.body = this.reportData.rows.filter(r => !r.isHeader);
      });
    this.reportsService.getChartData(condition)
      .takeUntil(this.onDestroy)
      .subscribe(data => {
        this.chartData = data;
        this.chartDataType = data.chartType;
      });
  }
  
  onExportToExcel() {
    const condition = this.switchReportCode(this.reportCode);         
    this.reportsService.exportToExcel(condition)
      .takeUntil(this.onDestroy)
      .subscribe((data) => {
        saveAs(data, condition.reportCode + '.xlsx');
      });
  }
  switchReportCode(repotCode)
  {
    switch(repotCode)
    {
      case "ChargedPaymentInvoicesReport": {
        return  this.cpiFilter.getConditions();       
      }
      default: {
        return  this.ipoFilter.getConditions();     
      }
    }  
  }



}