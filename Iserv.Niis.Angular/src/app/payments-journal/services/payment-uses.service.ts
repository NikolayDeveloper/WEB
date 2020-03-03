import { HttpClient, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { ConfigService } from '../../core';
import { ErrorHandlerService } from '../../core/error-handler.service';
import { Observable } from 'rxjs/Observable';
import { PaymentUseListDto } from '../models/payment-use-list.dto';
import { DeletePaymentUseRequestDto } from 'app/shared/models/delete-payment-use-request-dto';
import { EditPaymentUseRequestDto } from 'app/shared/models/edit-payment-use-request-dto';

const PaymentsJournalPaymentUsesApiUrl = '/api/PaymentsJournal/PaymentUses/';

@Injectable()
export class PaymentUsesService {
  constructor(private http: HttpClient,
    private configService: ConfigService,
    private errorHandlerService: ErrorHandlerService) {
  }


  public getByPaymentId(paymentId: number): Observable<PaymentUseListDto[]> {
    if (paymentId == null) {
      return Observable.of([]);
    }

    const url = `${this.configService.apiUrl}${PaymentsJournalPaymentUsesApiUrl}${paymentId}`;

    return this.http
      .get(url, { observe: 'response' })

      .map((resp: HttpResponse<PaymentUseListDto[]>) => {
        return resp.body;
      })
      .catch(err => this.errorHandlerService.handleError.call(this.errorHandlerService, err))

      .map((data: PaymentUseListDto[]) => data);
  }

  public isInvoiceCharged(paymentUseId: number) {
    const url = `${this.configService.apiUrl}${PaymentsJournalPaymentUsesApiUrl}${paymentUseId}/invoice/charged`;
    return this.http
      .get(url)
      .catch(err =>
        this.errorHandlerService.handleError.call(this.errorHandlerService, err)
      )
  }

  public deletePaymentUse(paymentUseId: number, request: DeletePaymentUseRequestDto) {
    const url = `${this.configService.apiUrl}${PaymentsJournalPaymentUsesApiUrl}${paymentUseId}/delete`;
    return this.http
      .post(url, request)
      .catch(err =>
        this.errorHandlerService.handleError.call(this.errorHandlerService, err)
      )
  }

  public getPaymentUseForEdit(paymentUseId: number) {
    const url = `${this.configService.apiUrl}${PaymentsJournalPaymentUsesApiUrl}${paymentUseId}/edit`;
    return this.http
      .get(url)
      .catch(err =>
        this.errorHandlerService.handleError.call(this.errorHandlerService, err)
      )
  }

  public editPaymentUse(paymentUseId: number, requestDto: EditPaymentUseRequestDto) {
    const url = `${this.configService.apiUrl}${PaymentsJournalPaymentUsesApiUrl}${paymentUseId}/edit`;
    return this.http
      .post(url, requestDto)
      .catch(err =>
        this.errorHandlerService.handleError.call(this.errorHandlerService, err)
      )
  }

  public getStatementFromBank(paymentUseId: number): Observable<any> {
    return this.http
      .get(`${this.configService.apiUrl}${PaymentsJournalPaymentUsesApiUrl}getStatementFromBank/${paymentUseId}`, { responseType: 'blob', observe: 'response' })
      .catch(err => this.errorHandlerService.handleError.call(this.errorHandlerService, err));
  }
}
