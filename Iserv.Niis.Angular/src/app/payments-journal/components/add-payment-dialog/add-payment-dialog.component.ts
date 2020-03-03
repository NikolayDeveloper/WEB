import 'rxjs/add/operator/takeUntil';

import { Component, Inject, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';

import { DictionaryService } from '../../../shared/services/dictionary.service';
import { DictionaryType } from '../../../shared/services/models/dictionary-type.enum';
import { SelectOption } from '../../../shared/services/models/select-option';
import { RequestService } from '../../../requests/request.service';
import { Config } from '../../../shared/components/table/config.model';
import { PaymentService } from '../../../payments/payment.service';
import { PaymentInvoice } from '../../../payments/models/payment.model';
import { Subject } from 'rxjs/Subject';
import { Subscription } from 'rxjs/Subscription';

@Component({
  selector: 'app-add-payment-dialog',
  templateUrl: './add-payment-dialog.component.html',
  styleUrls: ['./add-payment-dialog.component.scss']
})
export class AddPaymentDialogComponent implements OnDestroy {
  invoiceFormGroup: FormGroup;
  invoice: PaymentInvoice;
  tariffs: SelectOption[];
  subsription: Subscription;

  columns: Config[] = [
    new Config({ columnDef: 'id', header: 'ID', class: 'width-50' }),
    new Config({ columnDef: 'code', header: 'Код', class: 'width-100' }),
    new Config({ columnDef: 'nameRu', header: 'Название' }),
  ];

  private onDestroy = new Subject();

  constructor(private fb: FormBuilder,
              private requestService: RequestService,
              private paymentService: PaymentService,
              private dictionaryService: DictionaryService,
              private dialogRef: MatDialogRef<AddPaymentDialogComponent>,
              @Inject(MAT_DIALOG_DATA) private data: any) {
    this.buildForm();
    this.createInvoice();
    this.subsription = this.dictionaryService
      .getDicTariffs(this.data.protectionDocTypeId)
      .takeUntil(this.onDestroy)
      .subscribe(tariffs => {
        this.tariffs = tariffs;
      });
  }

  onSubmit() {
    this.invoiceFormGroup.markAsPristine();

    const values = this.invoiceFormGroup.getRawValue();
    const newInvoice = new PaymentInvoice();
    Object.assign(newInvoice, values);
    delete newInvoice.tariffNameRu;
    this.paymentService.addInvoice(newInvoice, this.data.ownerType)
      .subscribe(addedInvoice => {
          this.dialogRef.close(addedInvoice);
        },
        console.log);
  }

  onTariffSelect(tariffId: number) {
    this.invoiceFormGroup.get('penaltyPercent').markAsDirty();
    this.invoiceFormGroup.controls['tariffId'].setValue(tariffId);
    const tariffCountCmp = this.invoiceFormGroup.controls['tariffCount'];
    // Продление срока подачи возражения на решение экспертизы за каждый месяц
    // || Продление срока ответа на запрос за каждый месяц
    // || Продление и восстановление сроков представления ответа на запрос экспертизы и оплаты
    if (tariffId === 2549 || tariffId === 2547 || 3747) {
      tariffCountCmp.enable();
    } else {
      tariffCountCmp.disable();
      tariffCountCmp.setValue(1);
    }

  }

  onCancel() {
    this.dialogRef.close();
  }

  ngOnDestroy(): void {
    this.onDestroy.next();
  }

  createInvoice(): void {
    this.dictionaryService
      .getSelectOptions(DictionaryType.DicPaymentStatus)
      .takeUntil(this.onDestroy)
      .subscribe(statuses => {
        this.invoice = new PaymentInvoice({
          ownerId: this.data.ownerId,
          coefficient: this.paymentService.invoiceSettings.coefficient,
          tariffCount: this.paymentService.invoiceSettings.tariffCount,
          penaltyPercent: this.paymentService.invoiceSettings.penaltyPercent,
          nds: this.paymentService.invoiceSettings.nds,
          statusId: statuses.filter(s => s.code === this.paymentService.invoiceStatusCodes.notpaid)[0].id
        });

        this.invoiceFormGroup.reset(this.invoice);
      }, console.log);
  }

  private buildForm() {
    this.invoiceFormGroup = this.fb.group({
      id: [''],
      ownerId: ['', Validators.required],
      documentId: [''],
      tariffId: ['', Validators.required],
      coefficient: [{ value: '', disabled: true }, Validators.required],
      tariffCount: [{ value: '', disabled: true }, [Validators.required, Validators.min(1), Validators.max(1000)]],
      penaltyPercent: ['', Validators.required],
      nds: ['', Validators.required],
      statusId: [{ value: '', disabled: true }, Validators.required],
      // tariffPrice: [''],
      // totalAmount: [''],
      // amountUseSum: [''],
    });
  }
}
