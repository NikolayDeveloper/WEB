import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ChargedPaymentInvoicesReportFilterComponent } from './charged-paymnet-invoices-report-filter.component';

describe('ReportFilterComponent', () => {
  let component: ChargedPaymentInvoicesReportFilterComponent;
  let fixture: ComponentFixture<ChargedPaymentInvoicesReportFilterComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ChargedPaymentInvoicesReportFilterComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ChargedPaymentInvoicesReportFilterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
