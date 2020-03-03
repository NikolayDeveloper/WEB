import { Operators } from '../../shared/filter/operators';
import { OwnerType } from '../../shared/services/models/owner-type.enum';
import { DocumentType } from '../../materials/models/materials.model';

export class SearchDto {
    ownerType: OwnerType;
    documentType: DocumentType;
    id: number;
    barcode: number;
    num: string;
    date: Date;
    description: string;
    xin: string;
    customer: string;
    address: string;
    countryId: number;
    countryNameRu: string;
    receiveTypeId: number;
    receiveTypeNameRu: string;
}

export const OperatorFor = {
    barcode: Operators.like,
    num: Operators.like,
    dateFrom: Operators.greaterThanEqual,
    dateTo: Operators.lessThan,
    description: Operators.like,
    xin: Operators.like,
    customer: Operators.like,
    address: Operators.like,
    countryId: Operators.equal,
    receiveTypeId: Operators.equal,
}
