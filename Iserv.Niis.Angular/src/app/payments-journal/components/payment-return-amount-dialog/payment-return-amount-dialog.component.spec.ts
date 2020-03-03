import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PaymentReturnAmountDialogComponent } from './payment-return-amount-dialog.component';

describe('PaymentReturnAmountDialogComponent', () => {
  let component: PaymentReturnAmountDialogComponent;
  let fixture: ComponentFixture<PaymentReturnAmountDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [PaymentReturnAmountDialogComponent]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PaymentReturnAmountDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
