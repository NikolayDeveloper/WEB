import {
  addBooleanToQueryParams,
  addDateToQueryParams,
  addNumberToQueryParams,
  addStringToQueryParams,
  QueryParam
} from '../helpers/query-param';
import { DocumentCategory } from './document-category';
import { Moment } from 'moment';

export class DocumentsSearchParametersDto {
  public documentCategory: DocumentCategory;

  public docTypeId: number;

  public barcode: number;

  public receiveTypeId: number;

  public requestSubTypeId: number;

  public requestTypeId: number;

  public requestIncomingNumber: string;

  public requestRegNumber: string;

  public requestCreateDateFrom: Moment;

  public requestCreateDateTo: Moment;

  public requestRegDateFrom: Moment;

  public requestRegDateTo: Moment;

  public requestStatusId: number;

  public nameRu: string;

  public nameKz: string;

  public nameEn: string;

  public protectionDocNumber: string;

  public protectionDocMaintainYear: number;

  public protectionDocValidDate: Moment;

  public protectionDocExtensionDate: Moment;

  public protectionDocStatusId: number;

  public protectionDocDate: Moment;

  public protectionDocOutgoingDate: Moment;

  public icgsId: number;

  public selectionAchieveTypeId: number;

  public declarantName: string;

  public patentOwnerName: string;

  public authorName: string;

  public patentAttorneyName: string;

  public correspondenceName: string;

  public confidantName: string;

  public isNotMention: boolean;

  public confidantDateFrom: Moment;

  public confidantDateTo: Moment;

  public authorCertificateNumber: string;

  public bulletinNumber: string;

  public bulletinDate: Moment;

  public getQueryParams(): QueryParam[] {
    const result = [];

    if (this.documentCategory == null) {
      return result;
    }

    addNumberToQueryParams('documentCategory', this.documentCategory, result);
    addNumberToQueryParams('docTypeId', this.docTypeId, result);
    addNumberToQueryParams('barcode', this.barcode, result);
    addNumberToQueryParams('receiveTypeId', this.receiveTypeId, result);
    addNumberToQueryParams('requestSubTypeId', this.requestSubTypeId, result);
    addNumberToQueryParams('requestTypeId', this.requestTypeId, result);
    addStringToQueryParams('requestIncomingNumber', this.requestIncomingNumber, result);
    addStringToQueryParams('requestRegNumber', this.requestRegNumber, result);
    addDateToQueryParams('requestCreateDateFrom', this.requestCreateDateFrom, result);
    addDateToQueryParams('requestCreateDateTo', this.requestCreateDateTo, result);
    addDateToQueryParams('requestRegDateFrom', this.requestRegDateFrom, result);
    addDateToQueryParams('requestRegDateTo', this.requestRegDateTo, result);
    addNumberToQueryParams('requestStatusId', this.requestStatusId, result);
    addStringToQueryParams('nameRu', this.nameRu, result);
    addStringToQueryParams('nameKz', this.nameKz, result);
    addStringToQueryParams('nameEn', this.nameEn, result);
    addStringToQueryParams('protectionDocNumber', this.protectionDocNumber, result);
    addNumberToQueryParams('protectionDocMaintainYear', this.protectionDocMaintainYear, result);
    addDateToQueryParams('protectionDocValidDate', this.protectionDocValidDate, result);
    addDateToQueryParams('protectionDocExtensionDate', this.protectionDocExtensionDate, result);
    addNumberToQueryParams('protectionDocStatusId', this.protectionDocStatusId, result);
    addDateToQueryParams('protectionDocDate', this.protectionDocDate, result);
    addDateToQueryParams('protectionDocOutgoingDate', this.protectionDocOutgoingDate, result);
    addNumberToQueryParams('icgsId', this.icgsId, result);
    addNumberToQueryParams('selectionAchieveTypeId', this.selectionAchieveTypeId, result);
    addStringToQueryParams('declarantName', this.declarantName, result);
    addStringToQueryParams('patentOwnerName', this.patentOwnerName, result);
    addStringToQueryParams('authorName', this.authorName, result);
    addStringToQueryParams('patentAttorneyName', this.patentAttorneyName, result);
    addStringToQueryParams('correspondenceName', this.correspondenceName, result);
    addStringToQueryParams('confidantName', this.confidantName, result);
    addBooleanToQueryParams('isNotMention', this.isNotMention, result);
    addDateToQueryParams('confidantDateFrom', this.confidantDateFrom, result);
    addDateToQueryParams('confidantDateTo', this.confidantDateTo, result);
    addStringToQueryParams('authorCertificateNumber', this.authorCertificateNumber, result);
    addStringToQueryParams('bulletinNumber', this.bulletinNumber, result);
    addDateToQueryParams('bulletinDate', this.bulletinDate, result);

    return result;
  }
}
