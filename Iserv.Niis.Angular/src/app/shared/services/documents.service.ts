import { OutgoingDetail, UserInputDto, UserInputConfig } from '../../materials/models/materials.model';
import { SelectOption } from './models/select-option';
import { Observable } from 'rxjs/Observable';
import { HttpClient, HttpHeaders, HttpErrorResponse, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { ConfigService } from '../../core';
import { ErrorHandlerService } from '../../core/error-handler.service';
import { IServerStatus } from 'app/requests/interfaces/server-status.interface';

@Injectable()
export class DocumentsService {

  private api: string;
  constructor(private http: HttpClient,
    private errorHandlerService: ErrorHandlerService,
    configService: ConfigService) {
    this.api = `${configService.apiUrl}/api`;
  }

  getDocumentPdf(id: number, wasScanned: boolean, isMain: boolean): Observable<Blob> {
    return this.http
      .get(`${this.api}/documents/${id}/${wasScanned}/${isMain}`, { responseType: 'blob', observe: 'response' })
      .catch(err => this.errorHandlerService.handleError.call(this.errorHandlerService, err))
      .map((data: HttpResponse<any>) => {
        const fileNameMatch = data.headers.get('content-disposition')
          ? data.headers.get('content-disposition').match(/filename\*=UTF-8''(.+)/)
          : [];
        if (fileNameMatch.length === 2) {
          data.body.name = decodeURIComponent(fileNameMatch[1]);
        }
        return data.body;
      });
  }

  getUserInputFields(documentCode: string): Observable<UserInputConfig> {
    return this.http
      .get(`${this.api}/documents/getUserInputFields/${documentCode}`)
      .catch((error: HttpErrorResponse | any): Observable<any> => error instanceof HttpErrorResponse && error.status === 404
        ? Observable.throw(error)
        : this.errorHandlerService.handleError.call(this.errorHandlerService, error))
      .map((data: UserInputConfig) => data);
  }

  getAvailableTypes(requestId: number): Observable<SelectOption[]> {
    return this.http
      .get(`${this.api}/documents/availableTypes/${requestId}`)
      .catch(err => this.errorHandlerService.handleError.call(this.errorHandlerService, err))
      .map((data: SelectOption[]) => data);
  }

  getDocumentsTypesByClassificationCode(code: string): Observable<SelectOption[]> {
    return this.http
      .get(`${this.api}/documents/types/bycode/${code}`)
      .catch(err => this.errorHandlerService.handleError.call(this.errorHandlerService, err))
      .map((data: SelectOption[]) => data);
  }

  getAreRequestIcgsPaidFor(requestId: number): Observable<boolean> {
    return this.http
      .get(`${this.api}/documents/checkIcgs/${requestId}`)
      .catch(err => this.errorHandlerService.handleError.call(this.errorHandlerService, err))
      .map((data: boolean) => data);
  }

  sendMessage(barcode: number, isFromRequest: boolean): Observable<IServerStatus> {
    return this.http
      .post(`${this.api}/documents/sendMessage/${barcode}/${isFromRequest}`, null)
      .catch(error => Observable.throw(error))
      .map((data: IServerStatus) => data);
  }
}
