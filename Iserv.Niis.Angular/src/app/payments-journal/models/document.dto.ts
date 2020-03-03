import { DocumentCategory } from './document-category';

export class DocumentDto {
  public documentCategory: DocumentCategory;

  public id: number;

  public barcode: number;

  public protectionDocTypeId: number;

  public docTypeName: string;

  public requestSubTypeName: string;

  public requestTypeName: string;

  public incomingNumber: string;

  public dateCreate: Date;

  public regNumber: string;

  public receiveTypeName: string;

  public nameRu: string;

  public nameKz: string;

  public nameEn: string;

  public regDate: Date;

  public protectionDocNumber: string;

  public protectionDocMaintainYear: number;

  public protectionDocValidDate: Date;

  public protectionDocExtensionDate: Date;

  public requestStatusName: string;

  public protectionDocStatusName: string;

  public selectionAchieveTypeName: string;

  public breedingNumber: string;

  public protectionDocDate: Date;

  public protectionDocOutgoingDate: Date;

  public disclaimerRu: string;

  public disclaimerKz: string;

  public icisCodes: string;

  public icfemCodes: string;

  public ipcCodes: string;

  public icgsCodes: string;

  public declarantNames: string;

  public patentOwnerNames: string;

  public authorNames: string;

  public patentAttorneyNames: string;

  public correspondenceNames: string;

  public confidantNames: string;

  public authorsAreNotMentions: boolean;

  public authorsCertificateNumbers: string;

  public numberBulletin: string;

  public image: string;
}
