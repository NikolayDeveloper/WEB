import { Component, OnInit, Inject, EventEmitter } from '@angular/core';
import { FormGroup, FormBuilder, FormControl, Validators } from '@angular/forms';
import { Subject } from 'rxjs';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { PaymentUsesService } from 'app/payments-journal/services/payment-uses.service';
import { EditPaymentUseRequestDto } from 'app/shared/models/edit-payment-use-request-dto';
import { GetPaymentUseForEditResponseDto } from 'app/shared/models/get-payment-use-for-edit-response-dto';
import { EditPaymentUseResponseDto } from 'app/shared/models/edit-payment-use-response-dto';
import { ModalService } from 'app/shared/services/modal.service';
import { ModalButton } from 'app/shared/services/models/modal-button.enum';

@Component({
  selector: 'app-edit-payment-use-dialog',
  templateUrl: './edit-payment-use-dialog.component.html',
  styleUrls: ['./edit-payment-use-dialog.component.scss']
})
export class EditPaymentUseDialogComponent implements OnInit {
  private paymentUseForEdit: GetPaymentUseForEditResponseDto;
  formGroup: FormGroup;
  private onDestroy = new Subject();

  public paymentUseEdited: EventEmitter<any> = new EventEmitter<any>();

  constructor(
    private fb: FormBuilder,
    public dialogRef: MatDialogRef<EditPaymentUseDialogComponent>,
    @Inject(MAT_DIALOG_DATA) dialogData: any,
    private paymentUsesService: PaymentUsesService,
    private modalService: ModalService
  ) {
    this.paymentUseForEdit = dialogData.PaymentUseForEdit;
  }

  ngOnInit() {
    this.buildForm();
  }

  private buildForm() {
    this.formGroup = this.fb.group({
      editReason: new FormControl('', Validators.required),
      amount: new FormControl(this.paymentUseForEdit.amount, Validators.min(0.01)),
      makeCredited: new FormControl({ value: false, disabled: false })
    });
  }

  onCancelClick(): void {
    this.dialogRef.close();
  }

  onSubmit(): void {
    if (this.formGroup.invalid) {
      return;
    }

    const amount = this.formGroup.get('amount').value;
    const editReason = this.formGroup.get('editReason').value;
    const makeCredited = this.formGroup.get('makeCredited').value;

    const requestDto = new EditPaymentUseRequestDto();
    requestDto.amount = amount;
    requestDto.editReason = editReason;
    requestDto.makeCredited = makeCredited;

    this.paymentUsesService.editPaymentUse(this.paymentUseForEdit.id, requestDto)
      .takeUntil(this.onDestroy)
      .subscribe((responseDto: EditPaymentUseResponseDto) => {
        this.onEditPaymentUseCallback(requestDto, responseDto);
      });
  }

  private onEditPaymentUseCallback(
    requestDto: EditPaymentUseRequestDto,
    responseDto: EditPaymentUseResponseDto): void {

    if (responseDto.success) {
      this.dialogRef.close();
      this.paymentUseEdited.emit(true);

      this.modalService
        .ok('Редактирование зачтенной оплаты.',
          'Зачтенная оплата была успешно отредактирована.')
        .subscribe();

      return;
    }

    if (responseDto.amountIsGreaterThanPaymentInvoiceReminder) {
      this.modalService
        .ok('Некорректная сумма.',
          `Заполненная сумма ${requestDto.amount} больше остатка суммы с НДС выставленной оплаты ${responseDto.paymentInvoiceReminder}`)
        .subscribe();

      return;
    }

    if (responseDto.amountIsGreaterThanPaymentReminder) {
      this.modalService
        .yesNoCancel('Некорректная сумма.',
          `Суммы платежа не достаточно для оплаты услуги. Списать остаток суммы платежа ${responseDto.paymentReminder} ?`)
        .subscribe((buttonClicked: ModalButton) => {
          if (buttonClicked == ModalButton.Yes) {
            requestDto.amount = responseDto.paymentReminder;
            this.formGroup.get('amount').patchValue(requestDto.amount);
            this.paymentUsesService.editPaymentUse(this.paymentUseForEdit.id, requestDto)
              .subscribe((responseDto: EditPaymentUseResponseDto) => {
                this.onEditPaymentUseCallback(requestDto, responseDto);
              });
          }
        });

      return;
    }

    if (responseDto.paymentInvoiceNewReminderIsGreaterThan100KZT) {
      this.modalService
        .ok('Некорректная сумма.',
          `Нельзя признать зачтенной так как заполненная сумма ${requestDto.amount} меньше остатка суммы с НДС выставленной оплаты ${responseDto.paymentInvoiceReminder} более чем на 100 тенге.`)
        .subscribe();

      return;
    }
  }

  ngOnDestroy(): void {
    this.onDestroy.next();
  }

}
