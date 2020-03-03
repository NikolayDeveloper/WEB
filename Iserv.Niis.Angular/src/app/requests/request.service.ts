import 'rxjs/add/observable/throw';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/map';

import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { Injectable, ComponentFactoryResolver } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { HttpErrorResponse } from '@angular/common/http';

import { ConfigService } from '../core';
import { ErrorHandlerService } from '../core/error-handler.service';
import { SnackBarHelper } from '../core/snack-bar-helper.service';
import { BaseServiceWithPagination } from '../shared/base-service-with-pagination';
import { Request } from './models/request';
import { RequestDetails } from './models/request-details';
import { RequestItemDto } from './models/request-item';
import {
  SelectOption,
  SimpleSelectOption
} from 'app/shared/services/models/select-option';
import { ICGSRequestsShortInfo } from '../shared/services/models/ICGSRequest-short-info';
import { IntellectualPropertySearchDto } from 'app/search/models/intellectual-property-search-dto';
import { OwnerType } from '../shared/services/models/owner-type.enum';
import { ICGSRequestDto } from '../bibliographic-data/models/icgs-request-dto';
import {
  ConvertDto,
  ConvertResponseDto
} from 'app/bibliographic-data/models/convert-dto';
import { Workflow } from 'app/shared/services/models/workflow-model';
import {PageResponse} from '../shared/page-response';
import { IGenerateNumberResponse } from './interfaces/generate-number-response.interface';
import { IServerStatus } from './interfaces/server-status.interface';

@Injectable()
export class RequestService extends BaseServiceWithPagination<Request> {
  api = '/api/requests/';
  constructor(
    http: HttpClient,
    configService: ConfigService,
    errorHandlerService: ErrorHandlerService
  ) {
    super(http, configService, errorHandlerService, '/api/requests/');
  }

  getRequests(): Observable<Request[]> {
    return this.http
      .get(`${this.configService.apiUrl}${this.api}`)
      .catch(err =>
        this.errorHandlerService.handleError.call(this.errorHandlerService, err)
      )
      .map((data: Request[]) => data);
  }
  getRequestsWithParams(params): Observable<any> {
    return this.http
      .get(`${this.configService.apiUrl}${this.api}`, { observe: 'response', params: new HttpParams({ fromString: params }) })
      .catch(err =>
        this.errorHandlerService.handleError.call(this.errorHandlerService, err)
      )
      .map((data: HttpResponse<[any]>) => new PageResponse<any>(+(data.headers.get('x-total-count')), data.body));
  }

  getRequestsForSelector(
    protectionDocTypeCode: string
  ): Observable<RequestItemDto[]> {
    return this.http
      .get(
        `${this.configService.apiUrl}${
          this.api
        }shortInformation/${protectionDocTypeCode}`
      )
      .catch(err =>
        this.errorHandlerService.handleError.call(this.errorHandlerService, err)
      )
      .map((data: RequestItemDto[]) => data);
  }

  getImage(ownerId: number, ownerType: number): Observable<any> {
    return this.http
    .get(`${this.configService.apiUrl}${this.api}${ownerId}/${ownerType}/imageBase64`)
    .catch(err =>
      this.errorHandlerService.handleError.call(this.errorHandlerService, err)
    )
    .map((data) => data);
  }

  getRequestsByCode(code: string): Observable<IntellectualPropertySearchDto[]> {
    return this.http
      .get(`${this.configService.apiUrl}${this.api}/bycode/${code}`)
      .catch(err =>
        this.errorHandlerService.handleError.call(this.errorHandlerService, err)
      )
      .map((data: IntellectualPropertySearchDto[]) => data);
  }

  generatePrintAddressee(ownerId: number, ownerType: OwnerType): Observable<Blob> {
    return this.http
      .get(`${this.configService.apiUrl}${this.api}/printAddressee/${ownerId}/${ownerType}`, { responseType: 'blob', observe: 'response' })
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

  getRequestById(id: number): Observable<RequestDetails> {
    return this.http
      .get(`${this.configService.apiUrl}${this.api}${id}`)
      .catch(err =>
        this.errorHandlerService.handleError.call(this.errorHandlerService, err)
      )
      .map((data: RequestDetails) => {
        data.ownerType = OwnerType.Request;
        return data;
      });
  }

  getICGSRequestsByRequestIds(
    requestIds: number[]
  ): Observable<ICGSRequestsShortInfo[]> {
    return this.http
      .post(`${this.configService.apiUrl}${this.api}icgsRequests`, requestIds)
      .catch(err =>
        this.errorHandlerService.handleError.call(this.errorHandlerService, err)
      )
      .map((data: ICGSRequestsShortInfo[]) => data);
  }

  createRawRequest(
    pdTypeId: number,
    userId: number
  ): Observable<RequestDetails> {
    return Observable.of(
      new RequestDetails({
        protectionDocTypeId: pdTypeId,
        userId: userId,
        receiveTypeId: 4
      })
    );
  }

  getRequestByNumberAndDocType(
    number: string,
    docType: number
  ): Observable<RequestDetails> {
    return this.http
      .get(`${this.configService.apiUrl}${this.api}/bynumber`, {
        params: new HttpParams({ fromString: `${number}$${docType}` })
      })
      .catch(
        (error: HttpErrorResponse | any): Observable<any> =>
          error instanceof HttpErrorResponse && error.status === 404
            ? Observable.throw(
                'Не найдена заявка с таким номером и/или типом ОД!!!'
              )
            : this.errorHandlerService.handleError.call(
                this.errorHandlerService,
                error
              )
      )
      .map((data: RequestDetails) => data);
  }

  updateRequest(requestDetails: RequestDetails): Observable<RequestDetails> {
    const subjects = requestDetails.subjects;
    return this.http
      .put(
        `${this.configService.apiUrl}${this.api}${requestDetails.id}`,
        requestDetails
      )
      .catch(err =>
        this.errorHandlerService.handleError.call(this.errorHandlerService, err)
      )
      .map((data: RequestDetails) =>
        this.transferIndependentArrayProperties(requestDetails, data)
      );
  }

  addRequest(requestDetails: RequestDetails): Observable<RequestDetails> {
    return this.http
      .post(`${this.configService.apiUrl}${this.api}`, requestDetails)
      .catch(err =>
        this.errorHandlerService.handleError.call(this.errorHandlerService, err)
      )
      .map((data: RequestDetails) => data);
  }

  getRequestNumbers(requestDetails: RequestDetails): Observable<any> {
    return this.http
      .post(`${this.configService.apiUrl}${this.api}numbers`, requestDetails)
      .catch(err =>
        this.errorHandlerService.handleError.call(this.errorHandlerService, err)
      )
      .map((data: any) => data);
  }

  delete(requestId: number): Observable<any> {
    return this.http.delete(
      `${this.configService.apiUrl}${this.api}${requestId}`
    );
  }

  generateNotification(documentCodes: string[], requestId: number): Observable<any> {
    return this.http
      .post(
        `${this.configService.apiUrl}${this.api}generateNotification/${requestId}`,
        documentCodes
      )
      .catch(err =>
        this.errorHandlerService.handleError.call(this.errorHandlerService, err)
      );
  }

  uploadImage(file: File, requestId: number): Observable<any> {
    const formData: FormData = new FormData();
    formData.append(file.name, file);

    return this.http
      .post(
        `${this.configService.apiUrl}${this.api}${requestId}/upload`,
        formData
      )
      .map(data => data);
  }
  generateRequestNumber(
    requestId: number,
    subtypeId: number
  ): Observable<IGenerateNumberResponse> {
    return this.http
      .get(`${this.configService.apiUrl}${this.api}generateNumber/${requestId}/${subtypeId}`)
      .catch((error: HttpErrorResponse | any): Observable<any> => {
        if (error instanceof HttpErrorResponse && error.status === 422) {
          return Observable.throw({
            status: 422,
            message: 'Тип заявки должен быть либо "Животноводство" либо "Растениеводство"'
          });
        } else {
          this.errorHandlerService.handleError.call(this.errorHandlerService, error);
          return Observable.of(null);
        }
      })
      .map((data: IGenerateNumberResponse) => data);
  }

  sendRegNumber(requestId: number): Observable<IServerStatus> {
    return this.http
      .get(`${this.configService.apiUrl}${this.api}sendRegNumber/${requestId}`)
      .catch((error: any) => Observable.throw(error))
      .map((data: IServerStatus) => data);
  }

  requisitionSend(requestId: number): Observable<IServerStatus> {
    return this.http
      .get(`${this.configService.apiUrl}${this.api}requisitionSend/${requestId}`)
      .catch((error: any) => Observable.throw(error))
      .map((data: IServerStatus) => data);
  }

  sendStatus(requestId: number, statusId: number): Observable<IServerStatus> {
    return this.http
      .post(`${this.configService.apiUrl}${this.api}sendStatus/${requestId}/${statusId}`, {})
      .catch((error: any) => Observable.throw(error))
      .map((data: IServerStatus) => data);
  }

  splitRequest(
    requestId: number,
    icgsRequests: ICGSRequestDto[]
  ): Observable<number> {
    return this.http
      .post(
        `${this.configService.apiUrl}${this.api}split/${requestId}`,
        icgsRequests
      )
      .catch(err =>
        this.errorHandlerService.handleError.call(this.errorHandlerService, err)
      )
      .map((data: number) => data);
  }

  canSplit(requestId: number): Observable<boolean> {
    return this.http
      .get(`${this.configService.apiUrl}${this.api}canSplit/${requestId}`)
      .catch(err =>
        this.errorHandlerService.handleError.call(this.errorHandlerService, err)
      )
      .map((canSplit: boolean) => canSplit);
  }

  convertRequest(convertDto: ConvertDto): Observable<ConvertResponseDto> {
    return this.http
      .post(`${this.configService.apiUrl}${this.api}convert`, convertDto)
      .catch(err =>
        this.errorHandlerService.handleError.call(this.errorHandlerService, err)
      )
      .map((data: ConvertResponseDto) => data);
  }

  uploadMedia(file: File, requestId: number): Observable<string> {
    const formData: FormData = new FormData();
    formData.append(file.name, file);

    return this.http
      .post(
        `${this.configService.apiUrl}${this.api}${requestId}/uploadmedia`,
        formData
      )
      .map((data: string) => data);
  }

  completeCreate(requestId: number): Observable<Workflow> {
    return this.http
      .get(`${this.configService.apiUrl}${this.api}completeCreate/${requestId}`)
      .catch(err =>
        this.errorHandlerService.handleError.call(this.errorHandlerService, err)
      )
      .map((result: Workflow) => result);
  }

  importRequest(number: string): Observable<number> {
    return this.http
      .get(`${this.configService.apiUrl}${this.api}importRequest/${number}`)
      .catch(err =>
        this.errorHandlerService.handleError.call(this.errorHandlerService, err)
      )
      .map((result: number) => result);
  }


  arePDAccelerated(protectionDocIds: number[]): Observable<boolean> {
    return this.http
      .post(`${this.configService.apiUrl}${this.api}arePDAccelerated`, protectionDocIds)
      .catch(err =>
        this.errorHandlerService.handleError.call(this.errorHandlerService, err)
      )
      .map((data: boolean) => data);
  }

  private transferIndependentArrayProperties(
    oldRequestDetails: RequestDetails,
    newRequestDetails: RequestDetails
  ): RequestDetails {
    newRequestDetails.subjects = oldRequestDetails.subjects;
    return newRequestDetails;
  }
}
