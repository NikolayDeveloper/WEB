import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Rx';

import { ErrorHandlerService } from '../../core/error-handler.service';
import { ConfigService } from '../../core/index';
import { SnackBarHelper } from '../../core/snack-bar-helper.service';
import { BaseServiceWithPagination } from '../../shared/base-service-with-pagination';
import { UsefulmodelDto } from '../models/usefulmodel-dto.model';


@Injectable()
export class UsefulmodelSearchService extends BaseServiceWithPagination<UsefulmodelDto> {
  private api = '/api/search/expert/usefulmodel/';

  constructor(http: HttpClient,
    configService: ConfigService,
    errorHandlerService: ErrorHandlerService,
    private snackBarHelper: SnackBarHelper) {
    super(http, configService, errorHandlerService, '/api/search/expert/usefulmodel/');
  }

  public get(): Observable<UsefulmodelDto[]> {
    return this.http.get(`${this.configService.apiUrl}${this.api}`)
      .catch(err => this.errorHandlerService.handleError.call(this.errorHandlerService, err))
      .map((data: UsefulmodelDto[]) => data);
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
