import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { ConfigService } from '../../core';
import { ErrorHandlerService } from '../../core/error-handler.service';
import { DocumentCategory } from '../models/document-category';
import { PaymentInvoiceDto } from '../models/payment-invoice.dto';
import { PaymentUseDto } from '../models/payment-use.dto';
import { LinkedPaymentDto } from '../models/linked-payment.dto';

@Injectable()
export class PaymentInvoicesService {
  constructor(private http: HttpClient,
              private configService: ConfigService,
              private errorHandlerService: ErrorHandlerService) {
  }

  public getByDocument(documentId: number, documentCategory: DocumentCategory): Observable<PaymentInvoiceDto[]> {
    if (documentId == null || documentCategory == null) {
      return Observable.of([]);
    }

    const url = `${this.configService.apiUrl}/api/payments/invoices/${documentCategory}/${documentId}`;

    return this.http
      .get(url, { observe: 'response' })
      .map((resp: HttpResponse<PaymentInvoiceDto[]>) => {
        return resp.body;
      })
      .catch(err => this.errorHandlerService.handleError.call(this.errorHandlerService, err))
      .map((data: PaymentInvoiceDto[]) => data);
  }

  public boundPayment(dto: PaymentUseDto, documentCategory: DocumentCategory, force: boolean): Observable<void> {
    return this.http.post(`${this.configService.apiUrl}/api/payments/uses/${documentCategory}/${force}`, dto)
      .catch(err => err)
      .map(() => {});
  }

  public getLinkedPayments(paymentInvoiceId: number): Observable<LinkedPaymentDto[]> {
    if (paymentInvoiceId == null) {
      return Observable.of([]);
    }

    const url = `${this.configService.apiUrl}/api/PaymentsJournal/PaymentInvoices/LinkedPayments/${paymentInvoiceId}`;

    return this.http
      .get(url, { observe: 'response' })
      .map((resp: HttpResponse<LinkedPaymentDto[]>) => {
        return resp.body;
      })
      .catch(err => this.errorHandlerService.handleError.call(this.errorHandlerService, err))
      .map((data: LinkedPaymentDto[]) => data);
  }
}
