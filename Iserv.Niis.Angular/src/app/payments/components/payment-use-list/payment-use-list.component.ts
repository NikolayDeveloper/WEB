import { Component, EventEmitter, Input, OnDestroy, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Subject } from 'rxjs';
import { AuthenticationService } from '../../../shared/authentication/authentication.service';
import { Config } from '../../../shared/components/table/config.model';
import { DictionaryService } from '../../../shared/services/dictionary.service';
import { DictionaryType } from '../../../shared/services/models/dictionary-type.enum';
import { OwnerType } from '../../../shared/services/models/owner-type.enum';
import { SelectOption } from '../../../shared/services/models/select-option';
import { referencedMaxValueValidator } from '../../../shared/services/validator/custom-validators';
import { Payment, PaymentInvoice, PaymentUse } from '../../models/payment.model';
import { PaymentService } from '../../payment.service';

@Component({
  selector: 'app-payment-use-list',
  templateUrl: './payment-use-list.component.html',
  styleUrls: ['./payment-use-list.component.scss']
})
export class PaymentUseListComponent implements OnInit, OnDestroy {
  private onDestroy = new Subject();
  // payments: Payment[];
  columns: Config[] = [
    new Config({ columnDef: 'paymentNumber', header: 'Номер платежа', class: 'width-100' }),
    new Config({ columnDef: 'payment1CNumber', header: 'Номер 1С', class: 'width-100' }),
    new Config({ columnDef: 'purposeDescription', header: 'Описание', class: 'width-100' }),
    new Config({ columnDef: 'amount', header: 'Сумма', class: 'width-100' }),
    new Config({ columnDef: 'currencyType', header: 'Валюта', class: 'width-100' }),
    new Config({ columnDef: 'paymentDate', header: 'Дата', class: 'width-100' }),
    new Config({ columnDef: 'remainderAmount', header: 'Остаток', class: 'width-100' }),
    new Config({ columnDef: 'paymentUseAmountSum', header: 'Распределено', class: 'width-100' }),
    new Config({ columnDef: 'returnedAmount', header: 'Возвращено', class: 'width-100' }),
    new Config({ columnDef: 'blockedAmount', header: 'Заблокировано', class: 'width-100' })
  ];
  selectedPayment: any;
  formGroup: FormGroup;
  paymentStatuses: SelectOption[];
  roundedAvailableAmount;
  reset = new Subject();

  @Input() paymentInvoice: PaymentInvoice;
  @Input() ownerType: OwnerType;
  @Output() useCompleted = new EventEmitter<PaymentUse>();

  get source() { return this.paymentService; }

  constructor(
    private paymentService: PaymentService,
    private dictionaryService: DictionaryService,
    private auth: AuthenticationService,
    private fb: FormBuilder) {
    this.buildForm();
  }

  ngOnInit() {
    this.dictionaryService.getSelectOptions(DictionaryType.DicPaymentStatus)
      .takeUntil(this.onDestroy)
      .subscribe(paymentStatuses => this.paymentStatuses = paymentStatuses);

    this.reset
      .takeUntil(this.onDestroy)
      .subscribe(() => {
        this.formGroup.reset();
        this.formGroup.disable();
      });
  }

  onSubmit() {
    this.formGroup.markAsPristine();

    const insertedAmount = this.formGroup.value.amount;

    this.paymentService.addPaymentUse(new PaymentUse({
      paymentId: this.selectedPayment.id,
      paymentInvoiceId: this.paymentInvoice.id,
      amount: insertedAmount,
      createUserId: this.auth.userId,
      description: this.formGroup.value.description,
    }), this.ownerType)
      .subscribe(addedUse => {
        this.useCompleted.emit(addedUse);
      },
        console.log);
  }
  ngOnDestroy(): void {
    this.onDestroy.next();
  }

  onSelectPayment(selectedPayment: Payment) {
    if (selectedPayment.remainderAmount < this.paymentService.roundMaxGap) {
      // this.formGroup.controls['amount'].setValue('');
      // this.selectedPayment = null;
      // this.formGroup.disable();
      // return;
    }

    this.selectedPayment = selectedPayment;
    const remainder = this.paymentInvoice.remainder;
    if (Math.round(remainder) < 0) {
      // alert('Непогашенная сумма отрицательная, обратитесь к администратору!');
      // this.useCompleted.emit();
      // throw Error('Remainder is negative!');
    }
    const remainderAmount = selectedPayment.remainderAmount;
    const availableAmount = remainder > remainderAmount ? remainderAmount : remainder;
    this.roundedAvailableAmount = Math.round(availableAmount * 100) / 100;

    this.formGroup.controls['amount'].setValue(this.roundedAvailableAmount);
    this.formGroup.enable();
  }

  private buildForm() {
    this.formGroup = this.fb.group({
      // amount: ['', [Validators.required, Validators.min(0.01), referencedMaxValueValidator(this, 'roundedAvailableAmount')]],
      amount: ['', Validators.required],
      description: ['', Validators.required],
    });
    this.formGroup.disable();
  }
}
