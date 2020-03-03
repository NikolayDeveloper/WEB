import { OwnerType } from "app/shared/services/models/owner-type.enum";

export class IntellectualPropertySearchDto {
    id: number;
    barcode: string;
    dateCreate: Date;
    nameRu: string;
    number: string;
    type: OwnerType;
    typeId: number;
    protectionDocTypeId: number;
    requestTypeId: number;
    typeNameRu: string;
    isIndustrial: boolean;
    addressee?: object;
}
