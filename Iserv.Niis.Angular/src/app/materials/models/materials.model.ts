import { MaterialWorkflow } from '../../shared/services/models/workflow-model';
import { OwnerType } from '../../shared/services/models/owner-type.enum';
import { ContactInfoDto } from 'app/subjects/models/subject.model';

export interface MaterialItem {
  id: number;
  typeNameRu: string;
  dateCreate: Date;
  documentNum: string;
  copyCount: number;
  pageCount: number;
  name: string; // Все исполнители в текущих этапах, заполняется в MaterialProfile  CreateMap<Document, MaterialItemDto>()
  size: string;
  fileUrl: string;
  canDownload: boolean;
  hasTemplate: boolean;
  documentType: DocumentType;
  status: string; // Все статусы текущих этапов, заполняется в MaterialProfile  CreateMap<Document, MaterialItemDto>()
  initiator: string;
  executor: string;
}

export class DocumentCommentDto {
  public id: number;
  public documentId: number;
  public workflowId: number;
  public authorId: number;
  public authorInitials: string;
  public comment: string;
  public dateCreate: Date = new Date(Date.now());
  public isDeleted: boolean;
  public deletedDate: Date;
}

export class DocumentLinkDto {
  public id: number;
  public parentDocumentId: number;
  public parentDocumentTypeName: string;
  public parentDocumentNumber: string;
  public parentDocumentType: DocumentType;
  public childDocumentId: number;
  public childDocumentTypeName: string;
  public childDocumentNumber: string;
  public childDocumentType: DocumentType;
  public needRemove: boolean;
}

export abstract class MaterialDetail {
  public id: number;
  // public currentWorkflowId = 0;
  public workflowDtos: MaterialWorkflow[];
  public mainAttachmentId: number;
  public attachment: Attachment;
  public wasScanned: boolean;
  public pageCount: number;
  public outgoingNumber: string;
  public documentDate: Date;
  public owners: MaterialOwnerDto[];
  public ownerType: OwnerType;
  public addresseeXin: string;
  public addresseeCity: string;
  public addresseeOblast: string;
  public addresseeRepublic: string;
  public addresseeRegion: string;
  public addresseeStreet: string;
  public hasSecondaryAttachment: boolean;
  public code: string;
  public statusId: number;
  public statusCode: string;
  public statusNameRu: string;
  public isReadOnly: boolean;
  public commentDtos: DocumentCommentDto[];
  public documentLinkDtos: DocumentLinkDto[];
  public documentParentLinkDtos: DocumentLinkDto[];
}

export class MaterialOwnerDto {
  ownerId: number;
  ownerType: OwnerType;
  protectionDocTypeId: number;
  addressee?: object;
}

export class IncomingData {
  public parentId: number;
  public parentType: string;
  public attachments: Attachment[];
}

export class MaterialTask {
  public id: number;
  public displayNumber: number;
  public incomingNumber: number;
  public documentType: DocumentType;
  public typeNameRu: string;
  public barcode: number;
  public dateCreate: Date;
  public currentStageUser: string;
  public currentStageUserId: number;
  public creator: string;
  public wasScanned: boolean;
  public canDownload: boolean;
}

export class IncomingDetail extends MaterialDetail {
  public id = 0;
  public barcode: number;
  public attachment: Attachment;
  public addresseeId: number;
  public nameRu: string;
  public nameEn: string;
  public nameKz: string;
  public receiveTypeId: number;
  public typeId: number;
  public dateCreate: Date = new Date(Date.now());
  public documentDate: Date;
  public outgoingNumber: string;
  public incomingNumber: string;
  public incomingNumberFilial: string;
  public departmentId: number;
  public divisionId: number;
  public addresseeAddress: string;
  public addresseeShortAddress: string;
  public addresseeNameRu: string;
  public apartment: string;
  public controlMark: boolean;
  public outOfControl: boolean;
  public resolutionExtensionControlDate: string;
  public controlDate: Date;
  public dateOutOfControl: Date;
  public attachedPaymentsCount: number;
  public contactInfos: ContactInfoDto[];

  public constructor(fields?: {
    owners?: MaterialOwnerDto[]
  }) {
    super();
    if (fields) {
      Object.assign(this, fields);
    }
  }
}

export class IncomingShell {
  constructor(public detail: IncomingDetail[]) { }
}

export interface IncomingItem {
  id: number;
  typeNameRu: string;
  dateCreate: Date;
  documentNum: number;
  copyCount: number;
  pageCount: number;
  name: string;
  size: string;
  fileUrl: string;
  canDownload: boolean;
}

export class Attachment {
  public id = 0;
  public tempName = '';
  public parentId: number;
  public parentType: string;
  public isMain: boolean;

  constructor(
    public typeId: number,
    public copyCount: number,
    public pageCount: number,
    public name: string,
    public attachment: File,
    public contentType: string
  ) { }
}

export class TempFileItem {
  public name: string;
  public tempName: string;
}


export interface OutgoingItem {
  id: number;
  typeNameRu: string;
  dateCreate: Date;
  documentNum: number;
  initiator: string;
  executor: string;
  status: string;
}

export class OutgoingDetail extends MaterialDetail {
  public id = 0;
  public barcode: number;
  public typeId: number;
  public sendTypeId: number;
  public dateCreate: Date;
  public documentDate: Date;
  public outgoingNumber: string;
  public addresseeId: number;
  public addresseeNameRu: string;
  public addresseeAddress: string;
  public addresseeShortAddress: string;
  public apartment: string;
  public userInput: UserInputDto;
  public pageCount: number;
  public nameRu: string;
  public nameEn: string;
  public nameKz: string;
  public documentNum: string;
  public addresseeEmail: string;
  public numberForPayment: string;
  public paymentInvoiceId: number;
  public paymentInvoiceCode: string;
  public paymentDate: Date;
  public incomingDocumentNumber: string;
  public trackNumber: string;
  public incomingAnswerId: number;
  public incomingAnswerNumber: string;
  public contactInfos: ContactInfoDto[];
}

export class InternalDetail extends MaterialDetail {
  public id = 0;
  public barcode: number;
  public typeId: number;
  public dateCreate: Date;
  public documentDate: Date;
  public userInput: UserInputDto;
  public pageCount: number;
  public nameRu: string;
  public nameEn: string;
  public nameKz: string;
  public hasTemplate: boolean;
}


export class DocumentRequestDetail extends MaterialDetail {
  public id = 0;
  public barcode: number;
  public typeId: number;
  public dateCreate: Date;
  public documentDate: Date;
  public userInput: UserInputDto;
  public pageCount: number;
  public nameRu: string;
  public nameEn: string;
  public nameKz: string;
  public hasTemplate: boolean;
}

export interface DocumentRequestItem {
  id: number;
  typeNameRu: string;
  dateCreate: Date;
  documentNum: number;
  initiator: string;
  executor: string;
  status: string;
  canDownload: boolean;
}

export interface InternalItem {
  id: number;
  typeNameRu: string;
  dateCreate: Date;
  documentNum: number;
  initiator: string;
  executor: string;
  status: string;
  canDownload: boolean;
}

export interface UserInputConfig {
  requireUserInput: boolean;
  fieldsConfig: UserInputFieldConfig[];
}

export interface UserInputFieldConfig {
  key: string;
  label: string;
  value: string;
  richInput: boolean;
  required: boolean;
  disabled: boolean;
}

export class UserInputDto {
  fields = [];
  pageCount: number;
  index: number;

  constructor(
    public code: string,
    public ownerId: number,
    public documentId: number,
    public type: string,
    public selectedRequestIds: number[],
    public ownerType: OwnerType) {
  }
}

export enum DocumentType {
  Incoming = 0,
  Outgoing = 1,
  Internal = 3,
  Request = 4,
  Contract = 5,
  ProtectionDoc = 6,
  DocumentRequest = 8
}

export function getDocumentTypeName(type: DocumentType) {
  switch (type) {
    case DocumentType.Incoming:
      return 'Входящий документ';
    case DocumentType.Outgoing:
      return 'Исходящий документ';
    case DocumentType.Internal:
      return 'Внутренний документ';
    case DocumentType.DocumentRequest:
      return 'Документ заявки';
    case DocumentType.Request:
      return 'Заявка';
    case DocumentType.Contract:
      return 'Заявка на регистрацию договора';
    case DocumentType.ProtectionDoc:
      return 'Охранный документ';
    default:
      return '';
  }
}

export function getDocumentTypeRoute(type: DocumentType) {
  switch (type) {
    case DocumentType.Incoming:
      return 'materials/incoming';
    case DocumentType.Outgoing:
      return 'materials/outgoing';
    case DocumentType.Internal:
      return 'materials/internal';
    case DocumentType.DocumentRequest:
      return 'materials/document-request';
    case DocumentType.Request:
      return 'requests';
    case DocumentType.Contract:
      return 'contracts';
    case DocumentType.ProtectionDoc:
      return 'protectiondocs';
    default:
      return '';
  }
}
