import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';

import { ConfigService } from '../core';
import { ErrorHandlerService } from '../core/error-handler.service';
import { ExpertInfo } from '../journal/models/expert-info.model';
import { BaseServiceWithPagination } from '../shared/base-service-with-pagination';
import { Request } from './models/request';

@Injectable()
export class AutoAllocationService extends BaseServiceWithPagination<Request>  {
  api = '/api/autoAllocation/';
  constructor(http: HttpClient,
    configService: ConfigService,
    errorHandlerService: ErrorHandlerService) {
    super(http, configService, errorHandlerService, '/api/autoAllocation/');
  }

  prepareAllocate(requestIds: string): Observable<Request[]> {
    return this.http.get(`${this.configService.apiUrl}${this.api}prepareAllocate/${requestIds}`)
      .catch(err => this.errorHandlerService.handleError.call(this.errorHandlerService, err))
      .map((data: Request[]) => data);
  }

  allocate(requests: Request[]) {
    return this.http.post(`${this.configService.apiUrl}${this.api}allocate`, requests)
      .catch(err => this.errorHandlerService.handleError.call(this.errorHandlerService, err))
      .map((data: any) => data);
  }

  getExperts(): Observable<ExpertInfo[]> {
    return this.http.get(`${this.configService.apiUrl}${this.api}getExperts`)
      .catch(err => this.errorHandlerService.handleError.call(this.errorHandlerService, err))
      .map((data: ExpertInfo[]) => data);
  }
}
