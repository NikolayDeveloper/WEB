import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { ProtectionDocDetails } from './models/protection-doc-details';
import { ErrorHandlerService } from '../core/error-handler.service';
import { ConfigService } from '../core';
import { HttpClient } from '@angular/common/http';
import { OwnerType } from '../shared/services/models/owner-type.enum';
import { ICGSRequestDto } from '../bibliographic-data/models/icgs-request-dto';
import { SubjectDto } from '../subjects/models/subject.model';

@Injectable()
export class ProtectionDocsService {
  constructor(
    private http: HttpClient,
    private configService: ConfigService,
    private errorHandlerService: ErrorHandlerService
  ) {}
  api = '/api/ProtectionDocs/';

  get(id: number): Observable<ProtectionDocDetails> {
    return this.http
      .get(`${this.configService.apiUrl}${this.api}${id}`)
      .catch(err =>
        this.errorHandlerService.handleError.call(this.errorHandlerService, err)
      )
      .map((data: ProtectionDocDetails) => {
        data.ownerType = OwnerType.ProtectionDoc;
        return data;
      });
  }

  create(
    protectionDocDetails: ProtectionDocDetails,
    requestId: number
  ): Observable<ProtectionDocDetails> {
    return this.http
      .post(
        `${this.configService.apiUrl}${this.api}${requestId}`,
        protectionDocDetails
      )
      .catch(err =>
        this.errorHandlerService.handleError.call(this.errorHandlerService, err)
      )
      .map((data: ProtectionDocDetails) => data);
  }

  createRawProtectionDoc(
    pdTypeId: number,
    userId: number
  ): Observable<ProtectionDocDetails> {
    return Observable.of(new ProtectionDocDetails({ typeId: pdTypeId }));
  }

  update(
    protectionDocDetails: ProtectionDocDetails
  ): Observable<ProtectionDocDetails> {
    let url = '';
    let body;
    switch(protectionDocDetails.currentWorkflow.currentStageCode) {
      case 'OD04.3': {
        url = `${this.configService.apiUrl}${this.api}specialUpdate/${protectionDocDetails.id}`;
        body = [{Key: 'validDate', Value: protectionDocDetails.validDate}];
      }
      break;
      case 'OD01.6':{
        url = `${this.configService.apiUrl}${this.api}specialUpdate/${protectionDocDetails.id}`;
        body = [{Key: 'bulletinId', Value: protectionDocDetails.bulletinId}];
      }
        break;
      default:{
        const subjects = protectionDocDetails.subjects;
        url = `${this.configService.apiUrl}${this.api}${protectionDocDetails.id}`;
        body = protectionDocDetails;
      }
        break;
    }

    return this.http
      .put(url,body)
      .catch(err =>
        this.errorHandlerService.handleError.call(this.errorHandlerService, err)
      )
      .map((data: ProtectionDocDetails) => data);
  }
  
  splitProtectionDoc(
    protectionDocId: number,
    icgsRequests: ICGSRequestDto[]
  ): Observable<number> {
    return this.http
      .post(
        `${this.configService.apiUrl}${this.api}split/${protectionDocId}`,
        icgsRequests
      )
      .catch(err =>
        this.errorHandlerService.handleError.call(this.errorHandlerService, err)
      )
      .map((data: number) => data);
  }

  canSplit(protectionDocId: number): Observable<boolean> {
    return this.http
      .get(`${this.configService.apiUrl}${this.api}canSplit/${protectionDocId}`)
      .catch(err =>
        this.errorHandlerService.handleError.call(this.errorHandlerService, err)
      )
      .map((canSplit: boolean) => canSplit);
  }
  getImage(id) {
    return this.http
      .get(`${this.configService.apiUrl}Request/${id}/image/true`)
      .catch(err =>
        this.errorHandlerService.handleError.call(this.errorHandlerService, err)
      )
  }

  createAuthorsCertificates(protectionDocId: number, authors: SubjectDto[]) {
    return this.http
      .put(`${this.configService.apiUrl}${this.api}authorCertificate/${protectionDocId}`,authors)
      .catch(err =>
        this.errorHandlerService.handleError.call(this.errorHandlerService, err)
      )
      .map((result: boolean) => result);
  }
}
