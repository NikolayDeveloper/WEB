import { Workflow } from '../../shared/services/models/workflow-model';
import { IncomingItem, OutgoingItem, InternalItem } from '../../materials/models/materials.model';
import { SubjectDto, ContactInfoDto } from '../../subjects/models/subject.model';
import { AddresseeInfo } from '../../subjects/components/subjects-search-form/subjects-search-form.component';
import { PaymentInvoice } from '../../payments/models/payment.model';
import { OwnerType } from '../../shared/services/models/owner-type.enum';
import { IntellectualPropertyDetails } from '../../shared/models/intellectual-property-details';
import { ICGSRequestDto } from '../../bibliographic-data/models/icgs-request-dto';
import { RequestEarlyRegDto } from '../../bibliographic-data/models/request-early-reg-dto';
import { RequestConventionInfo } from '../../bibliographic-data/models/request-convention-info';


export class RequestDetails extends IntellectualPropertyDetails {
    id = 0;
    barcode: number;
    code: string;
    nameRu: string;
    nameKz: string;
    nameEn: string;
    description: string;
    incomingNumber: string;
    statusSending: string;
    isDocSendToEmail: boolean;
    incomingNumberFilial: string;
    requestNum: string;
    requestDate: Date;
    copyCount: number;
    pageCount: number;
    isComplete: boolean;
    templateDataFileId: number;
    scanFileId: number;
    dateCreate: Date;
    isRead: boolean;
    reviewDateAll: Date;
    reviewDateStage: Date;
    subjects: SubjectDto[] = [];
    parentRequestIds: number[] = [];
    childsRequestIds: number[] = [];
    icisRequestIds: number[] = [];
    ipcIds: number[] = [];
    mainIpcId: number;
    icfemIds: number[] = [];
    wasScanned: boolean;
    addresseeInfo: AddresseeInfo;
    outgoingNumber: string;
    outgoingDate: Date;
    registerDateProtectionDoc: Date;
    expectedValidDateProtectionDoc: Date;
    outgoingNumberFilial: string;
    hasRequiredOnCreate: boolean;
    // ** Специфические для типов заявки поля */
    // ** Общее поле */
    priority: string;
    // ** ТЗ */
    isExhibitPriority: boolean;
    transliteration: string;
    translation: string;
    isColorPerformance: boolean;
    colorTzIds: number[] = [];
    disclaimerRu: string;
    disclaimerKz: string;
    disclaimerEn: string;
    dateRecognizedKnown: Date;
    isRejected: boolean;
    rejectionReason: string;
    colectiveTrademarkParticipantsInfo: string;
    infoDecisionToRecognizedKnown: string;
    infoConfirmKnownTrademark: string;
    typeTrademarkId: number;
    // ** НМПТ */
    selectionFamily: string;
    productSpecialProp: string;
    productPlace: string;
    isHasMaterialExpertOpinionWithOugoingNumber: boolean;
    // ** Изобретения, полезные модели, промышленные образцы */
    referat: string;
    // ** Селекционные достижения */
    genus: string;
    breedingNumber: string;
    breedCountryId: number;
    breedCountryNameRu: string;

    // ** Referenced Keys */
    protectionDocTypeId: number;
    requestTypeId: number;
    receiveTypeId: number;
    protectionDocId: number;
    addresseeId: number;
    userId: number;
    divisionId: number;
    flDivisionId: number;
    departmentId: number;
    departmentNameRu: string;
    applicantTypeId: number;
    requestInfoId: number;
    currentWorkflowId: number;
    currentWorkflow: Workflow;
    invoiceDtos: PaymentInvoice[] = [];
    icgsRequestDtos: ICGSRequestDto[] = [];
    requestEarlyRegDtos: RequestEarlyRegDto[] = [];
    requestConventionInfoDtos: RequestConventionInfo[] = [];
    conventionTypeId: number;
    // ** Referenced Values */
    protectionDocTypeValue: string;
    protectionDocTypeCode: string;
    requestTypeCode: string;
    receiveTypeValue: string;
    protectionDocValue: string;
    addresseeXin: string;
    contactInfos: ContactInfoDto[];
    addresseeNameRu: string;
    addresseeAddress: string;
    addresseeShortAddress: string;
    republic: string;
    oblast: string;
    region: string;
    city: string;
    street: string;
    apartment: string;
    addresseeEmail: string;
    userValue: string;
    divisionValue: string;
    flDivisionValue: string;
    departmentValue: string;
    applicantTypeValue: string;
    requestInfoValue: string;
    isImageFromName: boolean;
    imageUrl: string;
    imageFile: File;
    beneficiaryTypeId: number;
    countIndependentItems: number;
    selectionAchieveTypeId: number;
    lastOnlineRequisitionStatusId: number;


    expertSearchKeywords: string;

    ownerType = OwnerType.Request;

    public constructor(init?: Partial<RequestDetails>) {
        super(init.id,
            init.currentWorkflowId,
            init.currentWorkflow,
            init.ownerType,
            init.protectionDocId,
            init.protectionDocTypeCode,
            init.invoiceDtos,
            init.subjects);
        Object.assign(this, init);
    }
}


