import { Component, OnInit, Inject, EventEmitter } from '@angular/core';
import { FormGroup, FormBuilder, FormControl, Validators } from '@angular/forms';
import { Subject } from 'rxjs';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { DeletePaymentUseRequestDto } from 'app/shared/models/delete-payment-use-request-dto';
import { PaymentUsesService } from 'app/payments-journal/services/payment-uses.service';
import { ModalService } from 'app/shared/services/modal.service';

@Component({
  selector: 'app-delete-payment-use-dialog',
  templateUrl: './delete-payment-use-dialog.component.html',
  styleUrls: ['./delete-payment-use-dialog.component.scss']
})
export class DeletePaymentUseDialogComponent implements OnInit {
  formGroup: FormGroup;
  private onDestroy = new Subject();

  public paymentUseDeleted: EventEmitter<any> = new EventEmitter<any>();

  constructor(
    private fb: FormBuilder,
    public dialogRef: MatDialogRef<DeletePaymentUseDialogComponent>,
    @Inject(MAT_DIALOG_DATA) private data: any,
    private paymentUsesService: PaymentUsesService,
    private modalService: ModalService
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
    const requestDto = new DeletePaymentUseRequestDto();
    requestDto.deletionReason = deletionReason;

    this.paymentUsesService.deletePaymentUse(this.data.PaymentUseId, requestDto)
      .takeUntil(this.onDestroy)
      .subscribe(() => {
        this.modalService.ok('Удалить зачтенную оплату.', 'Зачтенная оплата успешно удалена.')
          .subscribe();
        this.dialogRef.close();
        this.paymentUseDeleted.emit(true);
      });
  }

  ngOnDestroy(): void {
    this.onDestroy.next();
  }

}
