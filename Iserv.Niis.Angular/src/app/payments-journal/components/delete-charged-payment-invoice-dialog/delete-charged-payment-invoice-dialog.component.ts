import { Component, OnInit, Inject, Output, EventEmitter } from '@angular/core';
import { FormGroup, FormBuilder, FormControl, Validators } from '@angular/forms';
import { Subject } from 'rxjs';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { SnackBarHelper } from 'app/core/snack-bar-helper.service';
import { DeleteChargedPaymentInvoiceDto } from 'app/payments-journal/models/delete-charged-payment-invoice-dto';
import { PaymentService } from 'app/payments/payment.service';


@Component({
  selector: 'app-delete-charged-payment-invoice-dialog',
  templateUrl: './delete-charged-payment-invoice-dialog.component.html',
  styleUrls: ['./delete-charged-payment-invoice-dialog.component.scss']
})
export class DeleteChargedPaymentInvoiceDialogComponent implements OnInit {
  formGroup: FormGroup;
  private onDestroy = new Subject();

  public сhargedPaymentInvoiceDeleted: EventEmitter<any> = new EventEmitter<any>();

  constructor(
    private fb: FormBuilder,
    public dialogRef: MatDialogRef<DeleteChargedPaymentInvoiceDialogComponent>,
    @Inject(MAT_DIALOG_DATA) private data: any,
    private paymentService: PaymentService,
    private snackBarHelper: SnackBarHelper
  ) {
  }

  ngOnInit() {
    this.buildForm();
  }

  private buildForm() {
    this.formGroup = this.fb.group({
      deletionReason: new FormControl('', Validators.required)
    });
  }

  onCancelClick(): void {
    this.dialogRef.close();
  }

  onSubmit(): void {
    if (this.formGroup.invalid) {
      return;
    }

    const deletionReason = this.formGroup.get('deletionReason').value;
    const requestDto = new DeleteChargedPaymentInvoiceDto();
    requestDto.deletionReason = deletionReason;
    requestDto.ownerType = this.data.ownerType;
    

    this.paymentService.deleteChargedPaymentInvoice(this.data.PaymentInvoiceId, requestDto)
      .takeUntil(this.onDestroy)
      .subscribe(() => {
        this.snackBarHelper.success('Списанная оплата успешно удалена.');
        this.dialogRef.close();
        this.сhargedPaymentInvoiceDeleted.emit(true);
      });
  }

  ngOnDestroy(): void {
    this.onDestroy.next();
  }
}
