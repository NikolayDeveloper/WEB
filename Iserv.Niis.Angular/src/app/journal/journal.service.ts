import { HttpClient, HttpResponse, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { ConfigService } from '../core';
import { ErrorHandlerService } from '../core/error-handler.service';
import { BaseServiceWithPagination } from '../shared/base-service-with-pagination';
import { StaffTask } from './models/journal.model';
import { OwnerType } from '../shared/services/models/owner-type.enum';
import { Observable } from 'rxjs/Observable';
import { SelectionMode } from './components/journal-tasks/journal-tasks.component';
import {PageResponse} from '../shared/page-response';

@Injectable()
export class JournalService extends BaseServiceWithPagination<StaffTask> {
  private readonly api: string = '/api/journal/';

  constructor(
    http: HttpClient,
    configService: ConfigService,
    errorHandlerService: ErrorHandlerService
  ) {
    super(http, configService, errorHandlerService, '/api/journal/staff/');
  }

  getAttachment(ownerType: OwnerType, ownerId: number): Observable<Blob> {
    return this.http
      .get(`${this.api}/getAttachment/${ownerType}/${ownerId}`, {
        responseType: 'blob',
        observe: 'response'
      })
      .catch(err =>
        this.errorHandlerService.handleError.call(this.errorHandlerService, err)
      )
      .map((data: HttpResponse<any>) => {
        const fileNameMatch = data.headers.get('content-disposition')
          ? data.headers
              .get('content-disposition')
              .match(/filename\*=UTF-8''(.+)/)
          : [];
        if (fileNameMatch.length === 2) {
          data.body.name = decodeURIComponent(fileNameMatch[1]);
        }
        return data.body;
      });
  }

  getUsersTaskCount(params): Observable<any> {
    return this.http
      .get(`${this.api}userstaskscounts/`, { observe: 'response', params: new HttpParams({ fromString: params })})
      .map((resp: HttpResponse<any[]>) => {
        return resp.body;
      })
      .catch(err => this.errorHandlerService.handleError.call(this.errorHandlerService, err))
      .map((data: PageResponse<any>) => data);
  }

  getUserDocuments(id): Observable<any> {
    return this.http
      .get(`/api/materials/users/${id}`)
      .map((resp: HttpResponse<any[]>) => {
        return resp.body;
      })
      .catch(err => this.errorHandlerService.handleError.call(this.errorHandlerService, err))
      .map((data: PageResponse<any>) => data);
  }


  sendProtectionDocsToStage(
    mainUserId: number,
    nextUserForPrintId: number,
    nextUserForDescriptionsId: number,
    nextUserForMaintenanceId: number,
    bulletinUserId: number,
    bulletinId: number,
    supportUserId: number,
    ids: number[],
    keyValuePairs: any[]
  ) {
    const params = this.buildQueryString(keyValuePairs);
    return this.http
      .post(
        `${
          this.configService.apiUrl
        }/api/ProtectionDocs/workflow/
        ${mainUserId}/${bulletinUserId}/${supportUserId}/${bulletinId}/
        ${nextUserForPrintId}/${nextUserForDescriptionsId}/${nextUserForMaintenanceId}`,
        ids,
        { params: new HttpParams({ fromString: params }) }
      )
      .catch(err =>
        this.errorHandlerService.handleError.call(this.errorHandlerService, err)
      );
  }

  sendRequestsToStage(userId: number, ids: number[], keyValuePairs: any[]) {
    const params = this.buildQueryString(keyValuePairs);
    return this.http
      .post(
        `${this.configService.apiUrl}/api/Requests/workflow/${userId}`,
        ids,
        { params: new HttpParams({ fromString: params }) }
      )
      .catch(err =>
        this.errorHandlerService.handleError.call(this.errorHandlerService, err)
      );
  }

  private buildQueryString(keyValuePairs: any[] = []): string {
    return keyValuePairs
      .filter(item => item.value !== undefined && item.value.toString().length)
      .map(item => `${item.key}=${item.value}`)
      .join('&');
  }
}
