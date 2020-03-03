import 'rxjs/add/operator/delay';

import { HttpClient, HttpResponse } from '@angular/common/http';
import { HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';

import { ConfigService } from '../core';
import { ErrorHandlerService } from '../core/error-handler.service';
import { PageResponse } from './page-response';

/**Базовый класс с методом для серверной пагинации. Испоьзуется в @class BaseDataSource<TEntity> */
export abstract class BaseServiceWithPagination<T> {
  constructor(protected http: HttpClient,
              protected configService: ConfigService,
              protected errorHandlerService: ErrorHandlerService,
              private readonly pagedApi: string) {

  }

  getPaged(params: string): Observable<PageResponse<T>> {
    return this.http
      .get(`${this.configService.apiUrl}${this.pagedApi}`, { observe: 'response', params: new HttpParams({ fromString: params }) })
      .map((resp: HttpResponse<T[]>) => {
        return new PageResponse<T>(+(resp.headers.get('x-total-count')), resp.body)
      })
      .catch(err => this.errorHandlerService.handleError.call(this.errorHandlerService, err))
      .map((data: PageResponse<T>) => data);
  }
}
