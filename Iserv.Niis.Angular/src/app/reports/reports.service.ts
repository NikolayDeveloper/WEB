import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ConfigService } from 'app/core';
import { ErrorHandlerService } from 'app/core/error-handler.service';
import { BaseServiceWithPagination } from 'app/shared/base-service-with-pagination';
import { Observable } from 'rxjs/Observable';
import { ReportDto } from './models/reportdto';
import { ReportData, Row } from './models/reportdata';
import { ReportConditionData } from './models/report-condition-data';
import { ChartData, ChartDataset, ChartType, BarChartDataset } from './models/chart-data';

@Injectable()
export class ReportsService extends BaseServiceWithPagination<any> {
  private readonly api: string = '/api/Report/';
  private readonly apiUrl: string;
  constructor(http: HttpClient,
    configService: ConfigService,
    errorHandlerService: ErrorHandlerService) {
    super(http, configService, errorHandlerService, '/api/Report/');
    this.apiUrl = `${this.configService.apiUrl}${this.api}`;
  }

  getAllReports(): Observable<ReportDto[]> {
    return this.http
      .get(`${this.configService.apiUrl}${this.api}GetAllReports`)
      .catch(err => this.errorHandlerService.handleError.call(this.errorHandlerService, err))
      .map((data: ReportDto[]) => data);
  }

  getReportData(conditionData: ReportConditionData): Observable<ReportData> {
    return this.http
      .post(`${this.configService.apiUrl}${this.api}GetReportData`, conditionData)
      .catch(err => this.errorHandlerService.handleError.call(this.errorHandlerService, err))
      .map((data: ReportData) => {
        data.rows.forEach(r => {
          r.cells = r.cells.filter(c => c.value != null);
        });
        return data;
      });
  }

  getChartData(conditionData: ReportConditionData): Observable<ChartData> {
    return this.http
      .post(`${this.configService.apiUrl}${this.api}GetChartData`, conditionData)
      .catch(err => this.errorHandlerService.handleError.call(this.errorHandlerService, err))
      .map((data: ChartData) => data);
  }

  exportToExcel(conditionData: ReportConditionData): Observable<Blob> {
    return this.http
      .post(`${this.configService.apiUrl}${this.api}GetReportAsExcel`, conditionData, { responseType: 'blob' })
      .catch(err => this.errorHandlerService.handleError.call(this.errorHandlerService, err))
      .map((data: Blob) => data);
  }
}
