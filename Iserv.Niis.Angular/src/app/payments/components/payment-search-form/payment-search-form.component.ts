import { Component, EventEmitter, Input, OnChanges, OnDestroy, OnInit, Output, SimpleChanges } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Subject } from 'rxjs';

import { Operators } from '../../../shared/filter/operators';
import { OwnerType } from '../../../shared/services/models/owner-type.enum';
import { moment, MY_FORMATS } from '../../../shared/shared.module';
import { OperatorFor } from '../../models/payment.model';
import { numberMask } from '../.././../shared/services/validator/custom-validators';
import { SubjectDto } from '../../../subjects/models/subject.model';

@Component({
  selector: 'app-payment-search-form',
  templateUrl: './payment-search-form.component.html',
  styleUrls: ['./payment-search-form.component.scss']
})
export class PaymentSearchFormComponent implements OnInit, OnChanges, OnDestroy {
  private onDestroy = new Subject();
  formGroup: FormGroup;
  numberMask = numberMask;

  @Input() payingSubject: SubjectDto;
  @Input() ownerType: OwnerType;
  @Output() cancel = new EventEmitter();
  @Output() search = new EventEmitter<any[]>();

  constructor(private fb: FormBuilder) {
    this.formGroup = fb.group({
      customerXin: ['', [Validators.minLength(3), Validators.maxLength(12)]],
      customerNameRu: [''],
      paymentNumber: [''],
      paymentDateFrom: [''],
      paymentDateTo: [''],
      amountFrom: [''],
      amountTo: [''],
    });
  }

  ngOnInit() { }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes.payingSubject && changes.payingSubject.currentValue) {
      this.formGroup.controls.customerXin.patchValue(this.payingSubject.xin);
      this.formGroup.controls.customerNameRu.patchValue(this.payingSubject.nameRu);
      setTimeout((xinQueryParams) => {
        this.search.emit(xinQueryParams);
      }, 200, this.buildQueryParamsFrom({ customerId: this.payingSubject.customerId }));
    }
  }

  onSubmit(): void {
    const value = this.formGroup.value;
    const queryParams = this.buildQueryParamsFrom(value);

    this.search.emit(queryParams);
  }

  onReset(): void {
    if (this.payingSubject) {
      this.formGroup.controls.customerXin.patchValue(this.payingSubject.xin);
      this.formGroup.controls.customerNameRu.patchValue(this.payingSubject.nameRu);
      this.formGroup.reset();
      this.search.emit(this.buildQueryParamsFrom({ customerId: this.payingSubject.customerId }));
    } else {
      this.formGroup.reset();
      this.search.emit([]);
    }
  }

  ngOnDestroy() {
    this.onDestroy.next();
  }

  toDate(value: any) {
    return moment(value, MY_FORMATS.parse.dateInput).toDate();
  }

  private buildQueryParamsFrom(values: any): any[] {
    const queryParams = Object
      .keys(values)
      .filter(key => values[key] && OperatorFor[key])
      .map(key =>
        ({ key: this.simplify(key) + OperatorFor[key], value: this.prepare(key, values[key]) }));

    return this.applySort(queryParams);

  }

  private applySort(queryParams: any[]) {
    queryParams.push(
      { key: Operators.sort, value: 'remainderAmount' },
      { key: Operators.order, value: 'desc' }
    );
    return queryParams;
  }

  private simplify(key: string) {
    if (key === 'paymentDateFrom' || key === 'paymentDateTo') {
      return 'paymentDate';
    }

    if (key === 'amountFrom' || key === 'amountTo') {
      return 'amount';
    }

    return key;
  }

  private prepare(key: string, value: any): any {
    if (moment.isMoment(value)) {
      value = new Date(value.toDate().toDateString());
      if (key === 'paymentDateTo') {
        value.setDate(value.getDate() + 1);
      }
      return value.toJSON();
    }

    return value;
  }
}
