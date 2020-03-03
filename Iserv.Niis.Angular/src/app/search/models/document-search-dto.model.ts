import { DocumentType } from '../../materials/models/materials.model';
import { Operators } from '../../shared/filter/operators';

export class DocumentSearchDto {
    id: number;
    documentType: DocumentType;
    typeId: number;
    typeNameRu: string;
    departmentId: number;
    departmentNameRu: string;
    userId: string;
    userNameRu: string;
    documentNum: string;
    documentDate: Date;
    name: string;
    customerXin: string;
    customerNameRu: string;
    customerAddress: string;
    customerCountryId: number;
    customerCountryNameRu: string;
    outgoingNumber: string;
    sendingDate: Date;
}

export const OperatorFor = {
    typeId: Operators.in,
    userId: Operators.in,
    documentNum: Operators.like,
    documentDateFrom: Operators.greaterThanEqual,
    documentDateTo: Operators.lessThan,
    name: Operators.like,
    customerXin: Operators.like,
    customerNameRu: Operators.like,
    customerAddress: Operators.like,
    customerCountryId: Operators.equal,
    outgoingNumber: Operators.like,
    sendingDateFrom: Operators.greaterThanEqual,
    sendingDateTo: Operators.lessThan,
    barcode: Operators.equal,
    receiveTypeId: Operators.equal,
    description: Operators.like
}
