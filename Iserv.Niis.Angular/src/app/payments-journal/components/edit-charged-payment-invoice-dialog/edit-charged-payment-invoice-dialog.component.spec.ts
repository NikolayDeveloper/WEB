import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { EditChargedPaymentInvoiceDialogComponent } from './edit-charged-payment-invoice-dialog.component';

describe('EditChargedPaymentInvoiceDialogComponent', () => {
  let component: EditChargedPaymentInvoiceDialogComponent;
  let fixture: ComponentFixture<EditChargedPaymentInvoiceDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EditChargedPaymentInvoiceDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EditChargedPaymentInvoiceDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
