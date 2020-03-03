import 'rxjs/add/operator/takeUntil';

import { Component, EventEmitter, Inject, OnChanges, OnDestroy, Output, SimpleChanges } from '@angular/core';
import {FormBuilder, FormControl, FormGroup, Validators} from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';
import { OwnerType } from 'app/shared/services/models/owner-type.enum';
import { Subject, Subscription } from 'rxjs/Rx';

import { DictionaryService } from '../../../shared/services/dictionary.service';
import { DictionaryType } from '../../../shared/services/models/dictionary-type.enum';
import { PaymentInvoice } from '../../models/payment.model';
import { SelectOption } from '../../../shared/services/models/select-option';
import { PaymentService } from '../../payment.service';
import { RequestService } from '../../../requests/request.service';
import { Config } from '../../../shared/components/table/config.model';
import {debounceTime, distinctUntilChanged, startWith} from 'rxjs/operators';

@Component({
  selector: 'app-invoice-dialog',
  templateUrl: './invoice-dialog.component.html',
  styleUrls: ['./invoice-dialog.component.scss']
})
export class InvoiceDialogComponent implements OnDestroy {
  invoiceFormGroup: FormGroup;
  filterForm: FormGroup;
  invoice: PaymentInvoice;
  tariffs: SelectOption[];
  dataSource: SelectOption[];
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
    private dialogRef: MatDialogRef<InvoiceDialogComponent>,
    @Inject(MAT_DIALOG_DATA) private data: any) {
    this.buildForm();
    this.createInvoice();
    this.subsription = this.dictionaryService
      .getDicTariffs(this.data.protectionDocTypeId)
      .takeUntil(this.onDestroy)
      .subscribe(tariffs => {
        this.dataSource = tariffs;
        this.tariffs = tariffs;
      });

    this.filterForm.valueChanges.pipe(
        startWith(''),
        debounceTime(500),
        distinctUntilChanged()
    ).subscribe((filter: any) => {
      if (this.tariffs && (filter.code || filter.title)) {
        const filteredTarrifs = this.tariffs.filter(tariff => {
          if (filter.code && filter.title) {
            return (
              String(tariff.code) === filter.code &&
              String(tariff.nameRu).toLowerCase().includes(filter.title.toLowerCase())
            );
          }

          if (filter.code) {
            return String(tariff.code) === filter.code;
          }

          if (filter.title) {
            return String(tariff.nameRu).toLowerCase().includes(filter.title.toLowerCase());
          }

          return true;
        });
        this.dataSource = [...filteredTarrifs];
      }
      if (!filter.code && !filter.title) {
        this.dataSource = this.tariffs;
      }
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
    if (tariffId === 2549 || tariffId === 2547 || tariffId === 3747) {
      tariffCountCmp.enable();
    } else {
      // tariffCountCmp.disable();
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
    this.filterForm = new FormGroup({
      code: new FormControl(),
      title: new FormControl()
    });
    this.invoiceFormGroup = this.fb.group({
      id: [''],
      ownerId: ['', Validators.required],
      documentId: [''],
      tariffId: ['', Validators.required],
      coefficient: [{ value: '', disabled: true }, Validators.required],
      tariffCount: [{ value: ''}, [Validators.required, Validators.min(1), Validators.max(1000)]],
      penaltyPercent: ['', Validators.required],
      nds: ['', Validators.required],
      statusId: [{ value: '', disabled: true }, Validators.required],
      // tariffPrice: [''],
      // totalAmount: [''],
      // amountUseSum: [''],
    });
  }
}
