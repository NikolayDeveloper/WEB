import { MaterialWorkflow } from '../../shared/services/models/workflow-model';
import {HttpClient, HttpHeaders, HttpParams, HttpResponse} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';

import { ConfigService } from '../../core';
import { ErrorHandlerService } from '../../core/error-handler.service';
import {
  MaterialDetail,
  IncomingDetail,
  IncomingItem,
  IncomingShell,
  InternalDetail,
  InternalItem,
  OutgoingDetail,
  OutgoingItem,
  TempFileItem,
  MaterialItem,
  DocumentRequestDetail,
  getDocumentTypeName
} from '../models/materials.model';
import { OwnerType } from 'app/shared/services/models/owner-type.enum';
import { BaseServiceWithPagination } from '../../shared/base-service-with-pagination';
import { DocumentSign } from '../models/document-sign';
import * as _moment from 'moment';
import { SelectOption } from 'app/shared/services/models/select-option';

@Injectable()
export class MaterialsService extends BaseServiceWithPagination<MaterialItem> {
  private api: string;
  constructor(http: HttpClient,
    configService: ConfigService,
    errorHandlerService: ErrorHandlerService, ) {
    super(http, configService, errorHandlerService, '/api/materials/listByOwner');
    this.api = `${configService.apiUrl}/api`;
  }

  getByOwner(ownerId: number, ownerType: OwnerType): Observable<MaterialItem[]> {
    return this.http
      .get(`${this.api}/materials/listByOwner/${ownerType}/${ownerId}`)
      .catch(err => this.errorHandlerService.handleError.call(this.errorHandlerService, err))
      .map((data: MaterialItem[]) => data);
  }

  createIncoming(incomingDetail: IncomingDetail): Observable<any> {
    return this.http
      .post(`${this.api}/materials/incoming`, incomingDetail)
      .catch(err => this.errorHandlerService.handleError.call(this.errorHandlerService, err))
      .map((data: number) => {
        return data;
      });
  }

  getSingleIncoming(id: number): Observable<IncomingDetail> {
    return this.http
      .get(`${this.api}/materials/incoming/${id}`)
      .catch(err => this.errorHandlerService.handleError.call(this.errorHandlerService, err))
      .map((data: IncomingDetail) => data);
  }

  doneMaterial(incomingDetail: IncomingDetail): Observable<IncomingDetail> {
    return this.http.get(`${this.api}/materials/doneMaterial/${incomingDetail.id}`)
      .catch(err => this.errorHandlerService.handleError.call(this.errorHandlerService, err))
      .map((data: IncomingDetail) => data);
  }

  getAllAttachments(id: number): Observable<any> {
    return this.http
      .get(`${this.api}/materials/attachments/${id}`)
      .catch(err => this.errorHandlerService.handleError.call(this.errorHandlerService, err))
      .map((data: any) => data);
  }

  getAttachment(id: number): Observable<Blob> {
    return this.http
      .get(`${this.api}/materials/attachment/${id}`, { responseType: 'blob', observe: 'response' })
      .catch(err => this.errorHandlerService.handleError.call(this.errorHandlerService, err))
      .map((data: HttpResponse<any>) => data.body);
  }

  updateIncoming(incomingDetail: IncomingDetail): Observable<IncomingDetail> {
    return this.http.put(`${this.api}/materials/incoming/${incomingDetail.id}`, incomingDetail)
      .catch(err => this.errorHandlerService.handleError.call(this.errorHandlerService, err))
      .map((data: IncomingDetail) => data);
  }

  getIncomingNumbers(incomingDetails: IncomingDetail): Observable<any> {
    return this.http
      .post(`${this.configService.apiUrl}${this.api}/materials/numbers`, incomingDetails)
      .catch(err =>
        this.errorHandlerService.handleError.call(this.errorHandlerService, err)
      )
      .map((data: any) => data);
  }

  replaceAttachment(detail: MaterialDetail): Observable<MaterialDetail> {
    const headers = new HttpHeaders();
    headers.delete('Content-Type');
    const formData = new FormData();
    formData.append(detail.attachment.attachment.name, detail.attachment.attachment);
    return this.http
      .post(`${this.api}/upload`, formData, { headers: headers })
      .catch(err => this.errorHandlerService.handleError.call(this.errorHandlerService, err))
      .switchMap((tmp: TempFileItem[]): Observable<any> => {
        detail.attachment.name = detail.attachment.attachment.name;
        delete detail.attachment.attachment;
        detail.attachment.tempName = tmp.filter(r => r.name === detail.attachment.name)[0].tempName;
        return this.http.post(`${this.api}/upload/completeMaterial`, [detail])
          .catch(err => this.errorHandlerService.handleError.call(this.errorHandlerService, err))
          .map((data: MaterialDetail[]) => data[0]);
      });
  }

  deleteAttachment(documentId: number, isMain: boolean): Observable<object> {
    return this.http
      .delete(`${this.api}/upload/${documentId}/${isMain}`)
      .catch(err => this.errorHandlerService.handleError.call(this.errorHandlerService, err))
      .map(() => {
        return null;
      });
  }

  createOutgoing(value: OutgoingDetail): Observable<any> {
    return this.http
      .post(`${this.api}/materials/outgoing`, value)
      .catch(err => this.errorHandlerService.handleError.call(this.errorHandlerService, err))
      .map((data: number) => {
        return data;
      });
  }

  getSingleOutgoing(id: number): Observable<OutgoingDetail> {
    return this.http
      .get(`${this.api}/materials/outgoing/${id}`)
      .catch(err => this.errorHandlerService.handleError.call(this.errorHandlerService, err))
      .map((data: OutgoingDetail) => data);
  }

  updateOutgoing(outgoingDetail: OutgoingDetail): Observable<OutgoingDetail> {
    return this.http.put(`${this.api}/materials/outgoing/${outgoingDetail.id}`, outgoingDetail)
      .catch(err => this.errorHandlerService.handleError.call(this.errorHandlerService, err))
      .map((data: OutgoingDetail) => data);
  }

  createDocumentRequest(value: DocumentRequestDetail): Observable<any> {
    return this.http
      .post(`${this.api}/materials/documentRequest`, value)
      .catch(err => this.errorHandlerService.handleError.call(this.errorHandlerService, err))
      .map((data: DocumentRequestDetail) => {
        return data;
      });
  }

  createInternal(value: InternalDetail): Observable<any> {
    return this.http
      .post(`${this.api}/materials/internal`, value)
      .catch(err => this.errorHandlerService.handleError.call(this.errorHandlerService, err))
      .map((data: InternalDetail) => {
        return data;
      });
  }

  getSingleDocumentRequest(id: number): Observable<DocumentRequestDetail> {
    return this.http
      .get(`${this.api}/materials/documentRequest/${id}`)
      .catch(err => this.errorHandlerService.handleError.call(this.errorHandlerService, err))
      .map((data: DocumentRequestDetail) => data);
  }

  getSingleInternal(id: number): Observable<InternalDetail> {
    return this.http
      .get(`${this.api}/materials/internal/${id}`)
      .catch(err => this.errorHandlerService.handleError.call(this.errorHandlerService, err))
      .map((data: InternalDetail) => data);
  }

  updateDocumentRequest(documentRequestDetail: DocumentRequestDetail): Observable<DocumentRequestDetail> {
    return this.http.put(`${this.api}/materials/documentRequest/${documentRequestDetail.id}`, documentRequestDetail)
      .catch(err => this.errorHandlerService.handleError.call(this.errorHandlerService, err))
      .map((data: DocumentRequestDetail) => data);
  }

  updateInternal(internalDetail: InternalDetail): Observable<InternalDetail> {
    return this.http.put(`${this.api}/materials/internal/${internalDetail.id}`, internalDetail)
      .catch(err => this.errorHandlerService.handleError.call(this.errorHandlerService, err))
      .map((data: InternalDetail) => data);
  }

  addPreviousWorkflow(materialWorkflow: MaterialWorkflow): Observable<MaterialWorkflow> {
    return this.http.post(`${this.api}/materials/previousWorkflows`, materialWorkflow)
      .catch(err => this.errorHandlerService.handleError.call(this.errorHandlerService, err))
      .map((data: MaterialWorkflow) => data);
  }

  addWorkflow(materialWorkflow: MaterialWorkflow): Observable<MaterialWorkflow> {
    return this.http.post(`${this.api}/materials/workflows`, materialWorkflow)
      .catch(err => this.errorHandlerService.handleError.call(this.errorHandlerService, err))
      .map((data: MaterialWorkflow) => data);
  }

  updateWorkflow(materialWorkflow: MaterialWorkflow): Observable<MaterialWorkflow> {
    return this.http.put(`${this.api}/materials/updateWorkflows`, materialWorkflow)
      .catch(err => this.errorHandlerService.handleError.call(this.errorHandlerService, err))
      .map((data: MaterialWorkflow) => data);
  }


  deleteDocument(id: number): Observable<any> {
    return this.http.delete(`${this.api}/materials/${id}`)
      .catch(err => this.errorHandlerService.handleError.call(this.errorHandlerService, err));
  }

  sign(documentSign: DocumentSign): Observable<any> {
    return this.http
      .post(`${this.api}/materials/sign`, documentSign)
      .catch(err => this.errorHandlerService.handleError.call(this.errorHandlerService, err))
      .map(data => data);
  }

  generateOutgoingNumber(id: number): Observable<any> {
    return this.http
      .get(`${this.api}/materials/generateOutgoingNumber/${id}`)
      .catch(err => this.errorHandlerService.handleError.call(this.errorHandlerService, err))
      .map(data => data);
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

  generateInternalRegister(ownerId: number, ownerType: OwnerType): Observable<Blob> {
    return this.http
      .get(`${this.api}/materials/internalRegister/${ownerId}/${ownerType}`, { responseType: 'blob', observe: 'response' })
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

  getAllMaterials() {
    return this.http
      .get(`${this.api}/Materials`)
      .catch(err => this.errorHandlerService.handleError.call(this.errorHandlerService, err))
      .map((material: any[]) => {
        return material.map(data => {
          data.dateCreate = _moment(data.dateCreate).utc().format('DD-MM-YYYY');
          data.documentType = getDocumentTypeName(data.documentType);
          return data;
        });
      });
  }
    getFilteredMaterials(params) {
    return this.http
      .get(`${this.api}/Materials`, { observe: 'response', params: new HttpParams({ fromString: params }) })
      .catch(err => this.errorHandlerService.handleError.call(this.errorHandlerService, err))
      .map((material: any) => {
        return material.body.map(data => {
          data.dateCreate = _moment(data.dateCreate).utc().format('DD-MM-YYYY');
          data.documentType = getDocumentTypeName(data.documentType);
          return data;
        });
      });
  }

  /**
   * Получает статус документа по его идентификатору.
   *
   * @param documentId Идентификатор документа.
   */
  getDocumentStatus(documentId: number): Observable<SelectOption> {
      return this.http
                    .get(`${this.api}/materials/getDocumentStatus/${documentId}`)
                    .catch(err => this.errorHandlerService.handleError.call(this.errorHandlerService, err))
                    .map((data: SelectOption) => data);
  }
}
