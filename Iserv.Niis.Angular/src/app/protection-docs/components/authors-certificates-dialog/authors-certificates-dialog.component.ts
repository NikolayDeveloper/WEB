import { Component, Inject, OnDestroy, OnInit, ChangeDetectorRef } from '@angular/core';
import {
    FormBuilder,
    FormControl,
    FormGroup,
    Validators
} from '@angular/forms';
import { MatDialogRef, MatPaginator, MAT_DIALOG_DATA } from '@angular/material';
import { BehaviorSubject, Observable, Subject } from 'rxjs';
import '../../../core/utils/array-extensions';
import { SubjectDto } from '../../../subjects/models/subject.model';
import { PaymentInvoicesService } from '../../../payments-journal/services/payment-invoices.service';
import { PaymentInvoiceDto } from '../../../payments-journal/models/payment-invoice.dto';
import { DocumentCategory } from '../../../payments-journal/models/document-category';

const validTariffss: string[] = [
    '702',   // Подготовка документа к выдаче удостоверения автора (за каждое удостоверение)  (для юридических лиц)
    '702.1', // Подготовка документа к выдаче удостоверения автора (за каждое удостоверение) (Для субъектов малого и среднего бизнеса – резидентов) 
    '702.2', // Подготовка документа к выдаче удостоверения автора (за каждое удостоверение) (для физических лиц)
    '702.3'  // Подготовка документа к выдаче удостоверения автора (за каждое удостоверение) (для участников Великой отечественной войны, инвалидов, учащихся общеобразовательных школ и колледжей, студентов высших учебных заведений)
];

@Component({
    selector: 'app-authors-certificates-dialog',
    templateUrl: './authors-certificates-dialog.component.html',
    styleUrls: ['./authors-certificates-dialog.component.scss']
})
export class AuthorsCertificatesDialogComponent implements OnInit, OnDestroy {
    displayedColumns = [
        'select',
        'name'
    ];
    formGroup: FormGroup;
    descriptionControl: FormControl;
    sendTypeSubject: BehaviorSubject<number[]>;
    dataSource: Row[] = [];
    selectedAuthors: SubjectDto[] = [];
    invoicesFiltered: PaymentInvoiceDto[];
    counter = 0;

    private onDestroy = new Subject();

    constructor(
        private fb: FormBuilder,
        private dialogRef: MatDialogRef<AuthorsCertificatesDialogComponent>,
        private changeDetectorRef: ChangeDetectorRef,
        private paymentInvoicesService: PaymentInvoicesService,
        @Inject(MAT_DIALOG_DATA) private data: any
    ) {
        this.buildAuthorsCertificatesForm();
        for (let index = 0; index < data.authors.length; index++) {
            const row = new Row();
            row.position = index;
            row.author = data.authors[index];
            this.dataSource.push(row);
        }
        
        this.paymentInvoicesService
        .getByDocument(data.protectionDocId, DocumentCategory.ProtectionDoc)
        .subscribe((invoices: PaymentInvoiceDto[]) =>{
            if(invoices != undefined && invoices.length != undefined && invoices.length > 0){
                this.invoicesFiltered = invoices.filter(invoice => invoice.statusCode === 'credited' && validTariffss.includes(invoice.tariffCode));
                if(this.invoicesFiltered.length != undefined && this.invoicesFiltered.length > 0){
                    this.counter = this.invoicesFiltered.length;
                }
            }
        });
    }


    ngOnInit() {
        this.changeDetectorRef.detectChanges();
    }


    ngOnDestroy(): void {
        this.onDestroy.next();
    }


    onSubmit() {
        for (let index = 0; index < this.dataSource.length; index++) {
            if(this.dataSource[index].isChecked === true)
            this.selectedAuthors.push(this.dataSource[index].author);
        }

        if(this.selectedAuthors.length > 0)
        {
            const result = this.selectedAuthors;
            
            this.dialogRef.close(result);
            return Observable.of(result);
        }
        this.dialogRef.close();

    }

    onWorkflowCancel() {
        this.dialogRef.close();
    }

    cantPrint(): boolean {
        return this.dataSource.filter(r => r.isChecked).length < 1;
    }

    onSelect(record: Row) {
        if(record.isChecked){
            this.counter = this.counter + 1;
            record.isChecked = false;
        }
        else if(this.counter > 0){
            this.counter = this.counter - 1;
            record.isChecked = true;
        }
        else
            return;
    }


    private buildAuthorsCertificatesForm() {
        this.formGroup = this.fb.group({
        });
    }

}

export class Row {
    author: SubjectDto;
    isChecked = false;
    position: number;
  }
