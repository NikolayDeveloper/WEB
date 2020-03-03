export class PaymentUseDto {
  public paymentId: number;
  public paymentInvoiceId: number;
  public amount: number;
  public createUserId: number;
  public description: string;

  constructor(init?: Partial<PaymentUseDto>) {
    Object.assign(this, init);
  }
}
