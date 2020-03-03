import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';

import { ConfigService } from '../../core';
import { ErrorHandlerService } from '../../core/error-handler.service';
import { CustomerShortInfo } from './models/customer-short-info';
import { OwnerType } from './models/owner-type.enum';

@Injectable()
export class CustomerService {
  private readonly api: string = '/api/customer/';
  constructor(
    private http: HttpClient,
    private configService: ConfigService,
    private errorHandlerService: ErrorHandlerService
  ) { }

  getByXin(
    xin: string,
    isPatentAttorney?: boolean
  ): Observable<CustomerShortInfo> {
    return this.http
      .get(
        `${this.configService.apiUrl}${
        this.api
        }getbyxin/${xin}/${isPatentAttorney}`
      )
      .catch(
        (error: HttpErrorResponse | any): Observable<any> =>
          error instanceof HttpErrorResponse && error.status === 404
            ? Observable.throw({
              status: 404,
              message: 'Не найден контрагент с таким ИИН!',
            })
            : error instanceof HttpErrorResponse && error.status === 422
              ? Observable.throw({
                status: 422,
                message:
                  'Более одного контрагента с таким ИИН, целостность данных нарушена!',
              })
              : this.errorHandlerService.handleError.call(
                this.errorHandlerService,
                error
              )
      )
      .map((data: CustomerShortInfo) => data);
  }
  getAddresseeByOwnerId(ownerType: OwnerType, id: number): Observable<CustomerShortInfo> {
    return this.http
      .get(`${this.api}getAddresseeByOwnerId/${ownerType}/${id}`)
      .catch(err => this.errorHandlerService.handleError.call(this.errorHandlerService, err))
      .map((data: CustomerShortInfo) => data);
  }
}
