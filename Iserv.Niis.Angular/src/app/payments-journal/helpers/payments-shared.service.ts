import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

@Injectable()
export class PaymentsSharedService {

  private paymentUseEditedSubject = new Subject<void>();
  private paymentUseDeletedSubject = new Subject<void>();

  paymentUseEdited$ = this.paymentUseEditedSubject.asObservable();
  paymentUseDeleted$ = this.paymentUseDeletedSubject.asObservable();

  constructor() {
  }

  paymentUseEdited(): void {
    this.paymentUseEditedSubject.next();
  }

  paymentUseDeleted(): void {
    this.paymentUseDeletedSubject.next();
  }

}
