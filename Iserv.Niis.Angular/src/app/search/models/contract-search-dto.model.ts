import { Operators } from '../../shared/filter/operators';

export class ContractSearchDto {
    id: number;
    statusId: number;
    statusNameRu: string;
    contractTypeId: number;
    contractTypeNameRu: string;
    categoryId: number;
    categoryNameRu: string;
    currentStageId: number;
    currentStageNameRu: string;
    workflowDate: Date;
    departmentId: number;
    departmentNameRu: string;
    userId: number;
    userNameRu: string;
    applicationNum: string;
    dateCreate: Date;
    contractNum: string;
    regDate: Date;
    protectionDocTypeId: number;
    protectionDocTypeNameRu: string;
    name: string;
    customerXin: string;
    customerNameRu: string;
    customerAddress: string;
    customerCountryId: number;
    customerCountryNameRu: string;
    registrationPlace: string;
    validDate: Date;
}

export const OperatorFor = {
    statusId: Operators.equal,
    contractTypeId: Operators.equal,
    categoryId: Operators.equal,
    currentStageId: Operators.equal,
    workflowDateFrom: Operators.greaterThanEqual,
    workflowDateTo: Operators.lessThan,
    userId: Operators.in,
    applicationNum: Operators.like,
    dateCreateFrom: Operators.greaterThanEqual,
    dateCreateTo: Operators.lessThan,
    contractNum: Operators.like,
    regDateFrom: Operators.greaterThanEqual,
    regDateTo: Operators.lessThan,
    protectionDocTypeId: Operators.equal,
    name: Operators.like,
    customerXin: Operators.like,
    customerNameRu: Operators.like,
    customerAddress: Operators.like,
    customerCountryId: Operators.equal,
    registrationPlace: Operators.like,
    validDateFrom: Operators.greaterThanEqual,
    validDateTo: Operators.lessThan,
}
