import { HttpClient, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { ConfigService } from '../core';
import { ErrorHandlerService } from '../core/error-handler.service';
import { Payment, PaymentInvoice, PaymentUse } from './models/payment.model';
import { OwnerType } from 'app/shared/services/models/owner-type.enum';
import { BaseServiceWithPagination } from '../shared/base-service-with-pagination';
import { ChargePaymentInvoiceDto } from 'app/payments-journal/models/charge-payment-invoice-dto';
import { DeleteChargedPaymentInvoiceDto } from 'app/payments-journal/models/delete-charged-payment-invoice-dto';
import { EditChargedPaymentInvoiceDto } from 'app/payments-journal/models/edit-charged-payment-invoice-dto';
import { DeletePaymentInvoiceDto } from 'app/payments-journal/models/delete-payment-invoice-dto';
import { PaymentReturnAmountRequestDto } from 'app/shared/models/payment-return-amount-request-dto';
import { PaymentBlockAmountRequestDto } from 'app/shared/models/payment-block-amount-request-dto';
import { PaymentListDto } from 'app/payments-journal/models/payment-list.dto';

const invoiceSettings: { coefficient: number, tariffCount: number, penaltyPercent: number, nds: number }
  = { coefficient: 1, tariffCount: 1, penaltyPercent: 0, nds: 0.12 };

const invoiceStatusCodes: { notpaid: string, credited: string, charged: string }
  = { notpaid: 'notpaid', credited: 'credited', charged: 'charged' };

// Максимальная погрешность округления
const roundMaxGap = 0.02;

@Injectable()
export class PaymentService extends BaseServiceWithPagination<Payment> {
  get invoiceSettings() { return invoiceSettings; }
  get invoiceStatusCodes() { return invoiceStatusCodes; }
  get roundMaxGap() { return roundMaxGap; }

  private readonly api: string = '/api/payments/';
  private readonly apiUrl: string;

  constructor(http: HttpClient,
    configService: ConfigService,
    errorHandlerService: ErrorHandlerService) {
    super(http, configService, errorHandlerService, '/api/payments/');
    this.apiUrl = `${this.configService.apiUrl}${this.api}`;
  }

  getInvoices(ownerId: number, ownerType: OwnerType): Observable<PaymentInvoice[]> {
    return this.http
      .get(`${this.apiUrl}invoices/${ownerType}/${ownerId}`)
      .catch(err => this.errorHandlerService.handleError.call(this.errorHandlerService, err))
      .map((data: PaymentInvoice[]) => data);
  }

  chargePaymentInvoice(chargePaymentInvoiceDto: ChargePaymentInvoiceDto) {
    return this.http
      .post(`${this.configService.apiUrl}${this.api}invoices/chargePaymentInvoice`, chargePaymentInvoiceDto)
      .catch(err =>
        this.errorHandlerService.handleError.call(this.errorHandlerService, err)
      );
  }
  
  public deleteChargedPaymentInvoice(invoice: PaymentInvoice, request: DeleteChargedPaymentInvoiceDto) {
    const url = `${this.configService.apiUrl}${this.api}invoices/${invoice}/deleteChargedPaymentInvoice`;
    return this.http
      .post(url, request)
      .catch(err =>
        this.errorHandlerService.handleError.call(this.errorHandlerService, err)
      )
  }

  public deletePaymentInvoice(invoice: PaymentInvoice, request: DeletePaymentInvoiceDto) {
    const url = `${this.configService.apiUrl}${this.api}invoices/${invoice.id}/deletePaymentInvoice`;
    return this.http
      .post(url, request)
      .catch(err =>
        this.errorHandlerService.handleError.call(this.errorHandlerService, err)
      )
  }

  public editChargedPaymentInvoice(invoice: PaymentInvoice, request: EditChargedPaymentInvoiceDto) {
    const url = `${this.configService.apiUrl}${this.api}invoices/${invoice}/editChargedPaymentInvoice`;
    return this.http
      .post(url, request)
      .catch(err =>
        this.errorHandlerService.handleError.call(this.errorHandlerService, err)
      )
  }

  addInvoice(invoice: PaymentInvoice, ownerType: OwnerType): Observable<PaymentInvoice> {
    return this.http.post(`${this.apiUrl}invoices/${ownerType}`, invoice)
      .catch(err => this.errorHandlerService.handleError.call(this.errorHandlerService, err))
      .map((data: PaymentInvoice) => data);
  }

  addRangeInvoice(invoices: PaymentInvoice[], ownerType: OwnerType): Observable<PaymentInvoice[]> {
    return this.http.post(`${this.apiUrl}addRangePaymentInvoice/${ownerType}`, invoices)
      .catch(err => this.errorHandlerService.handleError.call(this.errorHandlerService, err))
      .map((data: PaymentInvoice[]) => data);
  }

  addPaymentUse(use: PaymentUse, ownerType: OwnerType) {
    return this.http.post(`${this.apiUrl}uses/${ownerType}/${false}`, use)
      .catch(err => this.errorHandlerService.handleError.call(this.errorHandlerService, err))
      .map((data: PaymentUse) => data);
  }

  public getPaymentForReturnAmount(paymentId: number) {
    const url = `${this.configService.apiUrl}${this.api}${paymentId}/returnamount`;
    return this.http
      .get(url)
      .catch(err =>
        this.errorHandlerService.handleError.call(this.errorHandlerService, err)
      )
  }

  public paymentReturnAmount(paymentId: number, requestDto: PaymentReturnAmountRequestDto) {
    const url = `${this.configService.apiUrl}${this.api}${paymentId}/returnamount`;
    return this.http
      .post(url, requestDto)
      .catch(err =>
        this.errorHandlerService.handleError.call(this.errorHandlerService, err)
      )
  }

  public getPaymentForBlockAmount(paymentId: number) {
    const url = `${this.configService.apiUrl}${this.api}${paymentId}/blockamount`;
    return this.http
      .get(url)
      .catch(err =>
        this.errorHandlerService.handleError.call(this.errorHandlerService, err)
      )
  }


  public paymentBlockAmount(paymentId: number, requestDto: PaymentBlockAmountRequestDto) {
    const url = `${this.configService.apiUrl}${this.api}${paymentId}/blockamount`;
    return this.http
      .post(url, requestDto)
      .catch(err =>
        this.errorHandlerService.handleError.call(this.errorHandlerService, err)
      )
  }

  public getPaymentsByInvoiceId(paymentInvoiceId: number): Observable<PaymentListDto[]> {
    if (paymentInvoiceId == null) {
      return Observable.of([]);
    }

    const url = `${this.configService.apiUrl}${this.api}/getpaymentsbyinvoice/${paymentInvoiceId}`;
    
    return this.http
      .get(url, { observe: 'response' })
      .map((resp: HttpResponse<PaymentListDto[]>) => {
        return resp.body;  })
      .catch(err => this.errorHandlerService.handleError.call(this.errorHandlerService, err))
      .map((data: PaymentListDto[]) => data);      
  }

}
