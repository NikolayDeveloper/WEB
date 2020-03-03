import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PaymentBlockAmountDialogComponent } from './payment-block-amount-dialog.component';

describe('PaymentBlockAmountDialogComponent', () => {
  let component: PaymentBlockAmountDialogComponent;
  let fixture: ComponentFixture<PaymentBlockAmountDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [PaymentBlockAmountDialogComponent]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PaymentBlockAmountDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
