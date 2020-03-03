import { Operators } from '../../shared/filter/operators';

export class ProtectionDocSearchDto {
    id: number;
    statusId: number;
    statusNameRu: string;
    typeId: number;
    typeNameRu: string;
    currentStageId: number;
    currentStageNameRu: string;
    workflowDate: Date;
    publicDate: Date;
    gosNumber: string;
    gosDate: Date;
    name: string;
    validDate: Date;
    customerXin: string;
    customerNameRu: string;
    customerAddress: string;
    customerCountryId: number;
    customerCountryNameRu: string;
    receiveTypeId: number;
    receiveTypeNameRu: string;
}

export const OperatorFor = {
    statusId: Operators.equal,
    typeId: Operators.equal,
    currentStageId: Operators.equal,
    workflowDateFrom: Operators.greaterThanEqual,
    workflowDateTo: Operators.lessThan,
    publicDateFrom: Operators.greaterThanEqual,
    publicDateTo: Operators.lessThan,
    gosNumber: Operators.like,
    gosDateFrom: Operators.greaterThanEqual,
    gosDateTo: Operators.lessThan,
    name: Operators.like,
    validDateFrom: Operators.greaterThanEqual,
    validDateTo: Operators.lessThan,
    customerXin: Operators.like,
    customerNameRu: Operators.like,
    customerAddress: Operators.like,
    customerCountryId: Operators.equal,
    receiveTypeId: Operators.equal,
}
