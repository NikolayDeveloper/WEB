import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';
import { PaymentInvoiceDto } from '../../models/payment-invoice.dto';
import { PaymentInvoicesService } from '../../services/payment-invoices.service';
import { DocumentCategory } from '../../models/document-category';
import { PaymentUseDto } from '../../models/payment-use.dto';
import { AuthenticationService } from '../../../shared/authentication/authentication.service';
import { PaymentDto } from '../../models/payment.dto';

interface DialogData {
  documentId: number;
  documentCategory: DocumentCategory;
  payment: PaymentDto;
  paymentInvoice: PaymentInvoiceDto;
}

@Component({
  selector: 'app-bound-payment-dialog',
  templateUrl: './bound-payment-dialog.component.html',
  styleUrls: ['./bound-payment-dialog.component.scss']
})
export class BoundPaymentDialogComponent {
  public amount: number;
  public description: string;
  public force = false;
  public enableForce: boolean;
  public amountBiggerThanRemainder = false;

  constructor(
    public dialogRef: MatDialogRef<BoundPaymentDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DialogData,
    private paymentInvoicesService: PaymentInvoicesService,
    private auth: AuthenticationService
  ) {
    const availableAmount = data.paymentInvoice.remainder > data.payment.remainderAmount
      ? data.payment.remainderAmount
      : data.paymentInvoice.remainder;

    this.amount = Math.round(availableAmount * 100) / 100;
    this.enableForce = data.payment.isForeignCurrency;
  }

  onAmountChange(): void {
    if (this.amountBiggerThanRemainder) {
      this.amountBiggerThanRemainder = false;
    }
  }

  public onOkClick(): void {
    const paymentUseDto = new PaymentUseDto({
      paymentId: this.data.payment.id,
      paymentInvoiceId: this.data.paymentInvoice.id,
      amount: this.amount,
      createUserId: this.auth.userId,
      description: this.description,
    });

    this.paymentInvoicesService.boundPayment(paymentUseDto, this.data.documentCategory, this.force)
      .subscribe(
        () => this.dialogRef.close(true),
        () => {
          this.amountBiggerThanRemainder = true;
        }
      );
  }

  public onCancelClick(): void {
    this.dialogRef.close(false);
  }
}
