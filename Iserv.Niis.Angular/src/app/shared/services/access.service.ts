import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { DocumentKindDto } from '../models/document-kind-dto-model';
import { HttpClient } from '@angular/common/http';
import { ConfigService } from '../../core';
import { ErrorHandlerService } from '../../core/error-handler.service';
import { DocumentType } from '../../materials/models/materials.model';
import { SelectOption } from './models/select-option';
import { DictionaryType } from './models/dictionary-type.enum';

@Injectable()
export class AccessService {
  private readonly api: string = '/api/access/';

  constructor(
    private http: HttpClient, 
    private configService: ConfigService,
    private errorHandlerService: ErrorHandlerService) { }

  getDocumentKinds(ownerId : number) : Observable<DocumentKindDto[]> {
    return this.http.get(`${this.configService.apiUrl}${this.api}getDocumentKinds/${ownerId}/`)
      .catch(err => this.errorHandlerService.handleError.call(this.errorHandlerService, err))
      .map((data : DocumentType[]) => {
        var result : DocumentKindDto[] = [];
        data.forEach(documentType => {
          result.push(new DocumentKindDto(documentType));
        });
        return result;
      });
  }

  getProtectionDocTypesSelectOption(ownerId : number) : Observable<SelectOption[]> {
    return this.http.get(`${this.configService.apiUrl}${this.api}getAccessProtectionDocTypes`)
      .catch(err => this.errorHandlerService.handleError.call(this.errorHandlerService, err))
      .map((selectOprions : any) => {
        selectOprions.forEach((selectOption : SelectOption) => {
          selectOption.dicType = DictionaryType.DicProtectionDocType;
        });
        return selectOprions;
      });
  }
}
