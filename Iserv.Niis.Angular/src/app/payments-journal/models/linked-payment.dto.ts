export class LinkedPaymentDto {
  public payerName: string;

  public amount: number;

  public remainder: number;

  public distributed: number;

  public paymentUseAmount: number;

  public paymentUseDate: Date;

  public paymentUseEmployeeName: string;

  public paymentStatusName: string;

  public paymentPurpose: string;

  public paymentDate: Date;

  public paymentNumber: string;

  public payerXin: string;

  public paymentDateCreate: Date;

  public paymentId: number;

  public paymentDocumentNumber: string;

  public isAdvancePayment: boolean;

  public isForeignCurrency: boolean;

  public currencyCode: string;

  public deletionClearedPaymentDate: Date;

  public deletionClearedPaymentEmployeeName: string;

  public deletionClearedPaymentReason: string;
}
