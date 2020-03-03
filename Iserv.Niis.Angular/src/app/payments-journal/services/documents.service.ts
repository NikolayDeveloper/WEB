import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { ConfigService } from '../../core';
import { ErrorHandlerService } from '../../core/error-handler.service';
import { BaseServiceWithPagination } from '../../shared/base-service-with-pagination';
import { DocumentDto } from '../models/document.dto';
import { SimpleSelectOption } from '../../shared/services/models/select-option';
import { DocumentCategory } from '../models/document-category';
import { Observable } from 'rxjs';

const PaymentsJournalDocumentsApiUrl = '/api/PaymentsJournal/Documents/';

@Injectable()
export class DocumentsService extends BaseServiceWithPagination<DocumentDto> {
  constructor(http: HttpClient,
              configService: ConfigService,
              errorHandlerService: ErrorHandlerService) {
    super(http, configService, errorHandlerService, PaymentsJournalDocumentsApiUrl);
  }

  public getDocCategories(): SimpleSelectOption[] {
    return [
      { id: DocumentCategory.Request, nameRu: 'Заявка' },
      { id: DocumentCategory.ProtectionDoc, nameRu: 'Охранные документ' },
      { id: DocumentCategory.Contract, nameRu: 'Договор' }
    ];
  }

  public getExcel(keyValuePairs: any[], type: number = null, id: number = null): Observable<any> {
    const params = this.buildQueryString(keyValuePairs);
    const url = type && id
      ? `${this.configService.apiUrl}/api/Payments/GetInvoicesExcel/${type}/${id}`
      : `${this.configService.apiUrl}/api/PaymentsJournal/Documents/GetExcel`;

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
