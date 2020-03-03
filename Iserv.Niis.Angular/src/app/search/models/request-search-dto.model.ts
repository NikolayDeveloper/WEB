import { Operators } from '../../shared/filter/operators';

export class RequestSearchDto {
  id: number;
  statusId: number;
  statusNameRu: string;
  protectionDocTypeId: number;
  protectionDocTypeNameRu: string;
  requestTypeId: number;
  requestTypeNameRu: string;
  currentStageId: number;
  currentStageNameRu: string;
  workflowDate: Date;
  departmentId: number;
  departmentNameRu: string;
  userId: string;
  userNameRu: string;
  requestNum: string;
  requestDate: Date;
  name: string;
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
  protectionDocTypeId: Operators.equal,
  requestTypeId: Operators.equal,
  currentStageId: Operators.equal,
  workflowDateFrom: Operators.greaterThanEqual,
  workflowDateTo: Operators.lessThan,
  userId: Operators.in,
  requestNum: Operators.like,
  barcode: Operators.equal,
  incomingNumber: Operators.like,
  requestDateFrom: Operators.greaterThanEqual,
  requestDateTo: Operators.lessThan,
  name: Operators.like,
  customerXin: Operators.like,
  customerNameRu: Operators.like,
  customerAddress: Operators.like,
  customerCountryId: Operators.equal,
  receiveTypeId: Operators.equal
};
