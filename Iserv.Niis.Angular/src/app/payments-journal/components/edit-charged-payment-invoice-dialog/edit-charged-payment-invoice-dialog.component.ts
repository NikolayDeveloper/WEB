import { Component, OnInit, Inject, EventEmitter } from '@angular/core';
import { FormGroup, FormBuilder, FormControl, Validators } from '@angular/forms';
import { Subject } from 'rxjs';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

import { SnackBarHelper } from 'app/core/snack-bar-helper.service';
import { PaymentService } from 'app/payments/payment.service';
import { EditChargedPaymentInvoiceDto } from 'app/payments-journal/models/edit-charged-payment-invoice-dto';
import { fromDateToJsonString } from 'app/payments-journal/helpers/date-helpers';

@Component({
  selector: 'app-edit-charged-payment-invoice-dialog',
  templateUrl: './edit-charged-payment-invoice-dialog.component.html',
  styleUrls: ['./edit-charged-payment-invoice-dialog.component.scss']
})
export class EditChargedPaymentInvoiceDialogComponent implements OnInit {
  formGroup: FormGroup;
  private onDestroy = new Subject();

  public сhargedPaymentInvoiceEdited: EventEmitter<any> = new EventEmitter<any>();

  constructor(
    private fb: FormBuilder,
    public dialogRef: MatDialogRef<EditChargedPaymentInvoiceDialogComponent>,
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
      editReason: new FormControl('', Validators.required),
      editDate: new FormControl(new Date(), Validators.required)
    });
  }

  onCancelClick(): void {
    this.dialogRef.close();
  }

  onSubmit(): void {
    if (this.formGroup.invalid) {
      return;
    }

    const editReason = this.formGroup.get('editReason').value;
    const editDate = this.formGroup.get('editDate').value;
        
    const requestDto = new EditChargedPaymentInvoiceDto();
    requestDto.editReason = editReason;
    requestDto.editDate = new Date(editDate).toLocaleString('en-US');
    requestDto.ownerType = this.data.ownerType;

    this.paymentService.editChargedPaymentInvoice(this.data.PaymentInvoiceId, requestDto)
      .takeUntil(this.onDestroy)
      .subscribe(() => {
        this.snackBarHelper.success('Зачтенная оплата успешно иправленна.');
        this.dialogRef.close();
        this.сhargedPaymentInvoiceEdited.emit(true);
      });
  
  }

  ngOnDestroy(): void {
    this.onDestroy.next();
  }
}
