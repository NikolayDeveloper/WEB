import { Injectable } from '@angular/core';
import { ImportPaymentsRequestDto } from '../models/import-payments-request-dto';
import { HttpClient } from '@angular/common/http';
import { ConfigService } from 'app/core';
import { ErrorHandlerService } from 'app/core/error-handler.service';

@Injectable()
export class IntegrationWith1cService {
  private readonly api: string = '/api/IntegrationWith1C/';
  constructor(
    private http: HttpClient,
    private configService: ConfigService,
    private errorHandlerService: ErrorHandlerService
  ) {
  }

  importPayments(requestDto: ImportPaymentsRequestDto) {
    return this.http
      .post(`${this.configService.apiUrl}${this.api}importpayments`, requestDto)
      .catch(err =>
        this.errorHandlerService.handleError.call(this.errorHandlerService, err)
      );
  }
}
  