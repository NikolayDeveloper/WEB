import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ChargePaymentInvoiceComponent } from './charge-payment-invoice.component';

describe('ChargePaymentInvoiceDialogComponent', () => {
  let component: ChargePaymentInvoiceComponent;
  let fixture: ComponentFixture<ChargePaymentInvoiceComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ChargePaymentInvoiceComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ChargePaymentInvoiceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
