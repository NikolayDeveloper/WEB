export class ContractItem {
  id: number;
  contractNum: string;
  regDate: Date;
  gosDate: Date;
  gosNumber: string;
  initiator: string;
  executor: string;
  statusNameRu: string;
  currentStageNameRu: string;
  validDate: Date;
  categoryNameRu: string;
  typeNameRu: string;
  sideOneNameRu: string;
  sideTwoNameRu: string;

  constructor(init?: Partial<ContractItem>) {
    Object.assign(this, init);
  }
}
