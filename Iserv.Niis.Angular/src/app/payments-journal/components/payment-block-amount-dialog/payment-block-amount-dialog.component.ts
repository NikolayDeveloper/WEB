import { Component, OnInit, EventEmitter, Inject } from '@angular/core';
import { FormGroup, FormBuilder, FormControl, Validators } from '@angular/forms';
import { Subject } from 'rxjs';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { ModalService } from 'app/shared/services/modal.service';
import { PaymentService } from 'app/payments/payment.service';
import { GetPaymentForBlockAmountResponseDto } from 'app/shared/models/get-payment-for-block-amount-response-dto';
import { PaymentBlockAmountResponseDto } from 'app/shared/models/payment-block-amount-response-dto';
import { PaymentBlockAmountRequestDto } from 'app/shared/models/payment-block-amount-request-dto';

@Component({
  selector: 'app-payment-block-amount-dialog',
  templateUrl: './payment-block-amount-dialog.component.html',
  styleUrls: ['./payment-block-amount-dialog.component.scss']
})
export class PaymentBlockAmountDialogComponent implements OnInit {
  private paymentForBlockAmount: GetPaymentForBlockAmountResponseDto;
  formGroup: FormGroup;
  private onDestroy = new Subject();

  public paymentAmountBlocked: EventEmitter<any> = new EventEmitter<any>();

  constructor(
    private fb: FormBuilder,
    public dialogRef: MatDialogRef<PaymentBlockAmountDialogComponent>,
    @Inject(MAT_DIALOG_DATA) dialogData: any,
    private modalService: ModalService,
    private paymentService: PaymentService
  ) {
    this.paymentForBlockAmount = dialogData.PaymentForBlockAmount;
  }

  ngOnInit() {
    this.buildForm();
  }

  private buildForm() {
    this.formGroup = this.fb.group({
      blockAmount: new FormControl(this.paymentForBlockAmount.paymentReminder, Validators.min(0)),
      blockReason: new FormControl('', Validators.required)
    });
  }

  onCancelClick(): void {
    this.dialogRef.close();
  }

  onSubmit(): void {
    if (this.formGroup.invalid) {
      return;
    }

    const blockAmount = this.formGroup.get('blockAmount').value;
    const blockReason = this.formGroup.get('blockReason').value;

    const requestDto = new PaymentBlockAmountRequestDto();

    requestDto.blockAmount = blockAmount;
    requestDto.blockReason = blockReason;

    this.paymentService.paymentBlockAmount(this.paymentForBlockAmount.id, requestDto)
      .takeUntil(this.onDestroy)
      .subscribe((responseDto: PaymentBlockAmountResponseDto) => {
        this.onPaymentBlockAmountCallback(responseDto);
      });
  }

  onPaymentBlockAmountCallback(responseDto: PaymentBlockAmountResponseDto) {
    if (responseDto.success) {
      this.dialogRef.close();
      this.paymentAmountBlocked.emit(true);

      this.modalService
        .ok('Блокирование платежа.', 'Блокирование части суммы платежа успешно выполнено.')
        .subscribe();

      return;
    }

    if (responseDto.blockAmountIsGreaterThanPaymentReminder) {
      this.modalService
        .ok('Блокирование платежа.', 'Заблокированная сумма должна быть меньше остаточной суммы платежа.')
        .subscribe();

      return;
    }

  }

  ngOnDestroy(): void {
    this.onDestroy.next();
  }

}
