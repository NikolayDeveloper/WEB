import { RequestItemDto } from 'app/requests/models/request-item';
import { ICGSProtectionDocItemDto } from 'app/shared/services/models/ICGSProtectionDoc-short-info';
import { ICGSRequestsShortInfo } from 'app/shared/services/models/ICGSRequest-short-info';
import { OwnerType } from 'app/shared/services/models/owner-type.enum';
import { SubjectDto } from 'app/subjects/models/subject.model';
import {
  IncomingItem,
  MaterialOwnerDto,
  OutgoingItem
} from '../../materials/models/materials.model';
import { PaymentInvoice } from '../../payments/models/payment.model';
import { Workflow } from '../../shared/services/models/workflow-model';
import { AddresseeInfo } from '../../subjects/components/subjects-search-form/subjects-search-form.component';

export class ContractDetails {
  id = 0;
  protectionDocTypeId = 72;
  protectionDocTypeCode = 'DK';
  barcode: number;
  nameRu: string;
  nameKz: string;
  nameEn: string;
  description: string;
  statusId: number;
  validDate: string;
  categoryId: number;
  typeId: number;
  typeCode: string;
  contractTypeId: number;
  contractTypeCode: string;
  currentWorkflowId: number;
  currentWorkflow: Workflow;
  registrationPlace: string;
  invoiceDtos: PaymentInvoice[] = [];
  subjects: SubjectDto[] = [];
  changes: string;
  requestRelations: RequestRelationDto[] = [];
  owners: MaterialOwnerDto[] = [];
  protectionDocsOwners: MaterialOwnerDto[] = [];
  protectionDocRelations: ProtectionDocRelationDto[] = [];
  contractNum: string;
  regDate: Date;
  receiveTypeId: number;
  fullExpertiseExecutorId: number;
  outgoingDate: Date;
  outgoingNumber: string;
  copyCount: number;
  pageCount: number;
  wasScanned: boolean;
  addresseeInfo: AddresseeInfo;

  addresseeId: number;
  addresseeXin: string;
  addresseeNameRu: string;
  addresseeAddress: string;
  apartment: string;

  departmentId: number;
  departmentNameRu: string;
  divisionId: number;
  divisionNameRu: string;
  gosNumber: string;
  gosDate: Date;
  numberBulletin: string;
  bulletinDate: Date;
  terminateDate: Date;
  incomingNumber: string;
  incomingDate: Date;
  applicationDateCreate: Date;

  ownerType = OwnerType.Contract;

  incomingDocuments: IncomingItem[] = [];
  outgoingDocuments: OutgoingItem[] = [];

  constructor(init?: Partial<ContractDetails>) {
    Object.assign(this, init);
  }
}

export class RequestRelationDto {
  id = 0;
  contractId: number;
  icgsRequestRelations: ContractRequestICGSRequestRelationDto[];
  icgsRequestItemDtos: ICGSRequestsShortInfo[];
  constructor(public request: RequestItemDto) {}
}

export class ContractRequestICGSRequestRelationDto {
  id: number;
  descriptions: string[] = [];
  constructor(
    public contractRequestRelationId: number,
    public icgsRequest: ICGSRequestsShortInfo
  ) {}
}

export class ProtectionDocRelationDto {
  id = 0;
  contractId: number;
  protectionDoc: ProtectionDocItemDto;
  icgsProtectionDocs: ContractProtectionDocICGSProtectionDocRelationDto[];
}

export class ProtectionDocItemDto {
  id: number;
  protectionDocNum: string;
  protectionDocTypeName: string;
  protectionDocTypeCode: string;
  protectionDocDate: Date;
}

export class ContractProtectionDocICGSProtectionDocRelationDto {
  id: number;
  contractProtectionDocRelationId: number;
  icgsProtectionDoc: ICGSProtectionDocItemDto;
  description: string;
}
