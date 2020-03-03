import { Component, Input, Inject, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialog } from '@angular/material';
import { OwnerType } from '../../../shared/services/models/owner-type.enum';
import { PaymentService } from '../../../payments/payment.service';
import { PaymentInvoice, PaymentUse } from '../../../payments/models/payment.model';
import { Config } from '../../../shared/components/table/config.model';
import { InvoiceDialogComponent } from '../../../payments/components/invoice-dialog/invoice-dialog.component';
import { Subject } from 'rxjs/Rx';

@Component({
  selector: 'app-search-payments',
  templateUrl: './search-payments.component.html',
  styleUrls: ['./search-payments.component.scss']
})
export class SearchPaymentsComponent implements OnDestroy {
  formGroup: FormGroup;
  selectRow: PaymentInvoice;
  isSelectedRow = false;
  private onDestroy = new Subject();
  invoiceDtos: PaymentInvoice[];
  @Input() ownerId: number;
  @Input() ownerType: OwnerType;

  columns: Config[] = [
    new Config({ columnDef: 'tariffCode', header: 'Код услуги', class: 'width-100' }),
    new Config({ columnDef: 'tariffNameRu', header: 'Наименование услуги', class: 'width-100' }),
    new Config({ columnDef: 'totalAmountNds', header: 'Сумма с НДС', class: 'width-100' }),
    new Config({ columnDef: 'amountUseSum', header: 'Фактическая сумма', class: 'width-100' }),
    new Config({ columnDef: 'totalAmount', header: 'Сумма выплаты', class: 'width-100' }),
  ];

  constructor(private fb: FormBuilder,
    private paymentService: PaymentService,
    private dialogRef: MatDialogRef<SearchPaymentsComponent>,
    private dialog: MatDialog,
    @Inject(MAT_DIALOG_DATA) private data: any) {
      this.buildForm();
      this.initForm();
    }

  onSubmit() {
    this.formGroup.markAsPristine();

    if (this.selectRow) {
      this.dialogRef.close(this.selectRow);
    }
  }

  initForm() {
    if (this.data.ownerId && this.data.ownerType) {
      this.paymentService.getInvoices(this.data.ownerId, this.data.ownerType)
        .takeUntil(this.onDestroy)
        .subscribe((invoices: any) => {
          this.invoiceDtos = invoices;
        });
    }
  }

  onCreate() {
    const dialogRef = this.dialog.open(InvoiceDialogComponent, {
      data: {
        ownerId: this.data.ownerId,
        ownerType: this.data.ownerType,
        protectionDocTypeId: this.data.protectionDocTypeId,
      },
      width: '1000px'
    });

    dialogRef.afterClosed()
      .takeUntil(this.onDestroy)
      .filter(invoice => !!invoice)
      .subscribe(invoice => {
        this.dialogRef.close(invoice);
      });
  }

  onSelect(row: PaymentInvoice) {
    this.selectRow = row;
    this.isSelectedRow = (this.selectRow !== null);
  }

  onCancel() {
    this.dialogRef.close();
  }

  ngOnDestroy(): void {
    this.onDestroy.next();
  }

  private buildForm() {
    this.formGroup = this.fb.group({
      ownerId: ['', Validators.required],
    });
  }
}
