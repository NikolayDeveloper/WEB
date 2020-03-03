import { HttpClient, HttpResponse, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { ConfigService } from '../../core';
import { ErrorHandlerService } from '../../core/error-handler.service';
import { PaymentListDto } from '../models/payment-list.dto';
import { BaseServiceWithPagination } from '../../shared/base-service-with-pagination';
import { Observable } from 'rxjs/Observable';
import { PaymentDto } from '../models/payment.dto';

const PaymentsJournalPaymentsApiUrl = '/api/PaymentsJournal/Payments/';

@Injectable()
export class PaymentsService extends BaseServiceWithPagination<PaymentListDto> {
  constructor(http: HttpClient,
              configService: ConfigService,
              errorHandlerService: ErrorHandlerService) {
    super(http, configService, errorHandlerService, PaymentsJournalPaymentsApiUrl);
  }

  public getById(id: number): Observable<PaymentDto> {
    if (id == null) {
      return Observable.of(null);
    }

    const url = `${this.configService.apiUrl}/api/payments/${id}`;

    return this.http
      .get(url, { observe: 'response' })
      .map((resp: HttpResponse<PaymentDto>) => {
        return resp.body;
      })
      .catch(err => this.errorHandlerService.handleError.call(this.errorHandlerService, err))
      .map((data: PaymentDto) => data);
  }

  public getCurrencies(): Observable<string[]> {
    const url = `${this.configService.apiUrl}/api/PaymentsJournal/payments/currencies`;

    return this.http
      .get(url, { observe: 'response' })
      .map((resp: HttpResponse<string[]>) => {
        return resp.body;
      })
      .catch(err => this.errorHandlerService.handleError.call(this.errorHandlerService, err))
      .map((data: string[]) => data);
  }

  public getExcel(keyValuePairs: any[], id: number = null): Observable<any> {
    const params = this.buildQueryString(keyValuePairs);
    const url = id
      ? `${this.configService.apiUrl}/api/PaymentsJournal/PaymentUses/GetExcel/${id}`
      : `${this.configService.apiUrl}/api/PaymentsJournal/Payments/GetExcel`;

    return this.http
      .get(url, {
        observe: 'response',
        params: new HttpParams({
          fromString: params
        }),
        responseType: 'blob'
      })
      .catch(err => this.errorHandlerService.handleError.call(this.errorHandlerService, err));
  }

  private buildQueryString(keyValuePairs: any[] = []): string {
    return keyValuePairs
      .filter(item => item.value !== undefined && item.value.toString().length)
      .map(item => `${item.key}=${item.value}`)
      .join('&');
  }
}
