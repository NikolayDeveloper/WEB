import { Component } from '@angular/core';

enum PaymentsJournalMode {
  Payments = 1,

  Documents = 2
}

@Component({
  selector: 'app-payments-journal',
  templateUrl: './payments-journal.component.html'
})
export class PaymentsJournalComponent {
  public mode: PaymentsJournalMode = PaymentsJournalMode.Payments;
  public selectedPaymentId: number;

  public get isPaymentsMode(): boolean {
    return this.mode === PaymentsJournalMode.Payments;
  }

  public get isDocumentsMode(): boolean {
    return this.mode === PaymentsJournalMode.Documents;
  }

  public onBoundPayment(paymentId: number): void {
    this.selectedPaymentId = paymentId;
    this.mode = PaymentsJournalMode.Documents;
  }

  public onReturnToPayments(): void {
    this.selectedPaymentId = null;
    this.mode = PaymentsJournalMode.Payments;
  }
}
