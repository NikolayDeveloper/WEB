import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { FormBuilder, FormControl, Validators, FormGroup } from '@angular/forms';
import { Subject } from 'rxjs';
import { ChargePaymentInvoiceDto } from 'app/payments-journal/models/charge-payment-invoice-dto';
import { PaymentService } from 'app/payments/payment.service';
import { SnackBarHelper } from 'app/core/snack-bar-helper.service';

@Component({
  selector: 'app-charge-payment-invoice',
  templateUrl: './charge-payment-invoice.component.html',
  styleUrls: ['./charge-payment-invoice.component.scss']
})

export class ChargePaymentInvoiceComponent implements OnInit {
  formGroup: FormGroup;
  private onDestroy = new Subject();

  constructor(
    private fb: FormBuilder,
    public dialogRef: MatDialogRef<ChargePaymentInvoiceComponent>,
    private paymentService: PaymentService,
    private snackBarHelper: SnackBarHelper,
    @Inject(MAT_DIALOG_DATA) public data: any) {
  }

  ngOnInit() {
    this.buildForm();
  }

  private buildForm() {
    this.formGroup = this.fb.group({
      chargeDate: new FormControl(new Date(), Validators.required)
    });
  }

  onCancelClick(): void {
    this.dialogRef.close(false);
  }

  onChargePaymentClick(): void {
    const chargeDate = this.formGroup.get('chargeDate').value;
    const chargePaymentInvoiceDto = new ChargePaymentInvoiceDto();
    chargePaymentInvoiceDto.chargeDate = new Date(chargeDate).toLocaleString('en-US');
    chargePaymentInvoiceDto.paymentInvoiceId = this.data.paymentInvoice.id;
    chargePaymentInvoiceDto.ownerType = this.data.ownerType;
    this.paymentService.chargePaymentInvoice(chargePaymentInvoiceDto)
      .takeUntil(this.onDestroy)
      .subscribe((response: string) => {
        this.snackBarHelper.success(response);
        this.dialogRef.close(true);
      });
  }

  ngOnDestroy(): void {
    this.onDestroy.next();
  }
}
