import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { DeleteChargedPaymentInvoiceDialogComponent } from './delete-charged-payment-invoice-dialog.component';

describe('DeleteChargedPaymentInvoiceDialogComponent', () => {
  let component: DeleteChargedPaymentInvoiceDialogComponent;
  let fixture: ComponentFixture<DeleteChargedPaymentInvoiceDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DeleteChargedPaymentInvoiceDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DeleteChargedPaymentInvoiceDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
