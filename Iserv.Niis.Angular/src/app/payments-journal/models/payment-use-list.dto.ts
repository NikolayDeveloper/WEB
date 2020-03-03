export class PaymentUseListDto {
  public id: number;

  public payerName: string;

  public payerXin: string;

  public payerRnn: string;

  public serviceCode: string;

  public serviceName: string;

  public protectionDocNumber: string;

  public requestNumber: string;

  public protectionDocTypeName: string;

  public protectionDocName: string;

  public contractNumber: string;

  public contractTypeName: string;

  public amount: number;

  public paymentUseDate: Date;

  public description: string;

  public issuingPaymentDate: Date;

  public employeeCheckoutPaymentName: string;

  public dateOfPayment: Date;

  public employeeWriteOffPaymentName: string;

  public deletionClearedPaymentDate: Date;

  public deletionClearedPaymentEmployeeName: string;

  public deletionClearedPaymentReason: string;

  public isDeleted: boolean;

  public editClearedPaymentDate: Date;

  public editClearedPaymentEmployeeName: string;

  public editClearedPaymentReason: string;
}
