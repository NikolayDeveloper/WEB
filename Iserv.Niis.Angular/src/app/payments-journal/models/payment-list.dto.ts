export class PaymentListDto {
  public id: number;

  public payerName: string;

  public payerXin: string;

  public payerRnn: string;

  public amount: number;

  public remainder: number;

  public distributed: number;

  public returnedAmount: number;

  public blockedAmount: number;

  public paymentPurpose: string;

  public paymentDate: Date;

  public paymentNumber: string;

  public paymentDocumentNumber: string;

  public isAdvancePayment: boolean;

  public paymentStatusName: string;

  public refundedEmployeeName: string;

  public blockedEmployeeName: string;

  public refundDate: Date;

  public blockedDate: Date;

  public isForeignCurrency: boolean;

  public currencyCode: string;

  public dateCreate: Date;

  public blockedReason: string;

  public returnedReason: string;
}
