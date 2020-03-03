import { Component, OnInit, EventEmitter, Inject } from '@angular/core';
import { FormGroup, FormBuilder, FormControl, Validators } from '@angular/forms';
import { Subject } from 'rxjs';
import { MatDialogRef, MAT_DIALOG_DATA, MatCheckboxChange } from '@angular/material';
import { ModalService } from 'app/shared/services/modal.service';
import { GetPaymentForReturnAmountResponseDto } from 'app/shared/models/get-payment-for-return-amount-response-dto';
import { PaymentReturnAmountRequestDto } from 'app/shared/models/payment-return-amount-request-dto';
import { PaymentService } from 'app/payments/payment.service';
import { PaymentReturnAmountResponseDto } from 'app/shared/models/payment-return-amount-response-dto';

@Component({
  selector: 'app-payment-return-amount-dialog',
  templateUrl: './payment-return-amount-dialog.component.html',
  styleUrls: ['./payment-return-amount-dialog.component.scss']
})
export class PaymentReturnAmountDialogComponent implements OnInit {
  private paymentForReturnAmount: GetPaymentForReturnAmountResponseDto;
  formGroup: FormGroup;
  private onDestroy = new Subject();

  public paymentAmountReturned: EventEmitter<any> = new EventEmitter<any>();

  constructor(
    private fb: FormBuilder,
    public dialogRef: MatDialogRef<PaymentReturnAmountDialogComponent>,
    @Inject(MAT_DIALOG_DATA) dialogData: any,
    private modalService: ModalService,
    private paymentService: PaymentService
  ) {
    this.paymentForReturnAmount = dialogData.PaymentForReturnAmount;
  }

  ngOnInit() {
    this.buildForm();
  }

  private buildForm() {
    this.formGroup = this.fb.group({
      returnFullAmount: new FormControl(true),
      returnAmount: new FormControl({ value: this.paymentForReturnAmount.paymentReminder, disabled: true }, Validators.min(0)),
      returnReason: new FormControl('', Validators.required)
    });
  }

  onReturnFullAmountChange($event: MatCheckboxChange): void {
    const returnAmountControl = this.formGroup.get('returnAmount');
    if ($event.checked) {
      returnAmountControl.disable();
    }
    else {
      returnAmountControl.enable();
    }
  }

  onCancelClick(): void {
    this.dialogRef.close();
  }

  onSubmit(): void {
    if (this.formGroup.invalid) {
      return;
    }

    const returnFullAmount = this.formGroup.get('returnFullAmount').value;
    const returnAmount = this.formGroup.get('returnAmount').value;
    const returnReason = this.formGroup.get('returnReason').value;

    const requestDto = new PaymentReturnAmountRequestDto();
    requestDto.returnFullAmount = returnFullAmount;
    requestDto.returnAmount = returnAmount;
    requestDto.returnReason = returnReason;

    this.paymentService.paymentReturnAmount(this.paymentForReturnAmount.id, requestDto)
      .takeUntil(this.onDestroy)
      .subscribe((responseDto: PaymentReturnAmountResponseDto) => {
        this.onPaymentReturnAmountCallback(requestDto, responseDto);
      });
  }

  onPaymentReturnAmountCallback(
    requestDto: PaymentReturnAmountRequestDto,
    responseDto: PaymentReturnAmountResponseDto
  ) {
    if (responseDto.success) {
      this.dialogRef.close();
      this.paymentAmountReturned.emit(true);

      this.modalService
        .ok('Возврат платежа.', 'Возврат платежа успешно выполнен.')
        .subscribe();

      return;
    }

    if (requestDto.returnFullAmount) {
      if (responseDto.paymentUsesExist) {
        this.modalService
          .ok('Возврат платежа.', 'Невозможно выполнить возврат платежа. Имеются зачтённые суммы.')
          .subscribe();
      }

      return;
    }

    if (responseDto.returnAmountIsGreaterThanPaymentReminder) {
      this.modalService
        .ok('Возврат платежа.', 'Сумма возврата должна быть меньше остаточной суммы платежа.')
        .subscribe();

      return;
    }
  }

  ngOnDestroy(): void {
    this.onDestroy.next();
  }

}
