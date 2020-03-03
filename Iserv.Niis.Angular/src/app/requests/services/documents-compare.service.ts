import 'rxjs/add/observable/throw';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/map';

import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';

import { ErrorHandlerService } from '../../core/error-handler.service';
import { DocumentsInfoForCompare } from '../models/documents-compare.model';

@Injectable()
export class DocumentsCompareService {

    api = '/api/Documents/';
    constructor(private http: HttpClient,
        private errorHandlerService: ErrorHandlerService) {
    }

    getDocumentInfo(requestId: number): Observable<DocumentsInfoForCompare> {
        return this.http.get(`${this.api}getDocumetsInfoForCompare/${requestId}`)
            .catch(err => this.errorHandlerService.handleError.call(this.errorHandlerService, err))
            .map((data: DocumentsInfoForCompare) => data);
    }
    makeDocumentFinished(documentId: number) {
        return this.http.get(`${this.api}makeDocumentFinished/${documentId}`)
        .catch(err => this.errorHandlerService.handleError.call(this.errorHandlerService, err))
        .map((data: DocumentsInfoForCompare) => data);
    }
}
