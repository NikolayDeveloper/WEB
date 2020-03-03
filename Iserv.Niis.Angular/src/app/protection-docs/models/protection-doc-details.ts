import { OwnerType } from '../../shared/services/models/owner-type.enum';
import { IntellectualPropertyDetails } from '../../shared/models/intellectual-property-details';
import { SubjectDto } from '../../subjects/models/subject.model';

export class ProtectionDocDetails extends IntellectualPropertyDetails {
    id = 0;
    barcode: number;
    gosNumber: string;
    gosDate: Date;
    outgoingNumber: string;
    outgoingDate: Date;
    bulletinDate: Date;
    numberBulletin: string;
    extensionDate: Date;
    validDate: Date;
    statusId: number;
    typeId: number;
    subTypeId: number;
    typeCode: string;
    addressee: SubjectDto;
    addresseeId: number;
    sendTypeId: number;
    pageCount: number;
    wasScanned: boolean;

    ownerType = OwnerType.ProtectionDoc;

    public constructor(init?: Partial<ProtectionDocDetails>) {
        super(init.id,
            init.currentWorkflowId,
            init.currentWorkflow,
            init.ownerType,
            init.typeId,
            init.typeCode,
            init.invoiceDtos,
            init.subjects);
        Object.assign(this, init);
    }
}
