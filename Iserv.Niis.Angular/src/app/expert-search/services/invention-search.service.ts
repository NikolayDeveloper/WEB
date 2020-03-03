import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Rx';
import { ErrorHandlerService } from '../../core/error-handler.service';
import { ConfigService } from '../../core/index';
import { BaseServiceWithPagination } from '../../shared/base-service-with-pagination';
import { InventionSearchDto } from '../models/invention-dto.model';

@Injectable()
export class InventionSearchService extends BaseServiceWithPagination<InventionSearchDto> {
  private api = '/api/search/expert/invention/';

  constructor(http: HttpClient,
    configService: ConfigService,
    errorHandlerService: ErrorHandlerService) {
    super(http, configService, errorHandlerService, '/api/search/expert/invention/');
  }

  public get(): Observable<InventionSearchDto[]> {
    return this.http.get(`${this.configService.apiUrl}${this.api}`)
      .catch(err => this.errorHandlerService.handleError.call(this.errorHandlerService, err))
      .map((data: InventionSearchDto[]) => data);
  }

  public getExcel(keyValuePairs: any[]): Observable<any> {
    const params = this.buildQueryString(keyValuePairs);

    return this.http
      .get(`${this.configService.apiUrl}${this.api}/getexcel`,
        { observe: 'response', params: new HttpParams({ fromString: params }), responseType: 'blob' })
      .catch(err => this.errorHandlerService.handleError.call(this.errorHandlerService, err));
  }

  private buildQueryString(keyValuePairs: any[] = []): string {
    return keyValuePairs
      .filter(item => item.value !== undefined && item.value.toString().length)
      .map(item => `${item.key}=${item.value}`)
      .join('&');
  }
}
