import { MaterialItem } from '../../materials/models/materials.model';
import { Workflow } from '../services/models/workflow-model';
import { PaymentInvoice } from '../../payments/models/payment.model';
import { SubjectDto } from '../../subjects/models/subject.model';
import { OwnerType } from '../services/models/owner-type.enum';
import { ICGSRequestDto } from '../../bibliographic-data/models/icgs-request-dto';
import { RequestEarlyRegDto } from '../../bibliographic-data/models/request-early-reg-dto';
import { RequestConventionInfo } from '../../bibliographic-data/models/request-convention-info';

export class IntellectualPropertyDetails {
  materials: MaterialItem[] = [];
  imageUrl: string;
  imageFile: File;
  mediaFileUrl: string;
  mediaFile: File;
  transliteration: string;
  colorTzIds: number[] = [];
  disclaimerRu: string;
  disclaimerKz: string;
  typeTrademarkId: number;
  speciesTrademarkCode: string;
  dateRecognizedKnown: Date;
  infoDecisionToRecognizedKnown: string;
  infoConfirmKnownTrademark: string;
  // поля НМПТ
  selectionFamily: string;
  productSpecialProp: string;
  productPlace: string;
  // поля изобретений, полезной модели, промышленного образца
  referat: string;
  conventionTypeId: number;
  // поля селекционных достижений
  genus: string;
  breedingNumber: string;
  breedCountryId: number;
  breedCountryNameRu: string;
  isImageFromName: boolean;
  icfemIds: number[] = [];
  ipcIds: number[] = [];
  mainIpcId: number;
  icisRequestIds: number[] = [];
  icgsRequestDtos: ICGSRequestDto[] = [];
  requestEarlyRegDtos: RequestEarlyRegDto[] = [];
  requestConventionInfoDtos: RequestConventionInfo[] = [];
  requestTypeId: number;
  requestNum: string;
  requestDate: Date;
  hasProxy: boolean;
  beneficiaryTypeId: number;
  nameRu: string;
  nameEn: string;
  nameKz: string;
  gosNumber: string;
  gosDate: Date;
  dateCreate: Date;
  selectionAchieveTypeId: number;
  lastOnlineRequisitionStatusId: number;
  countIndependentItems: number;
  statusId: number;
  validDate: Date;
  extensionDate: Date;
  yearMaintain: number;
  bulletinId: number;
  isHasMaterialExpertOpinionWithOugoingNumber: boolean;
  colectiveTrademarkParticipantsInfo: string;
  speciesTradeMarkId: number;
  isSyncRequestNum: boolean;
  isFromLk: boolean;
  receiveTypeId: number;

  public constructor(
    public id: number,
    public currentWorkflowId: number,
    public currentWorkflow: Workflow,
    public ownerType: OwnerType,
    public protectionDocTypeId: number,
    public protectionDocTypeCode: string,
    public invoiceDtos: PaymentInvoice[],
    public subjects: SubjectDto[]
  ) {
    this.materials = [];
  }
}
