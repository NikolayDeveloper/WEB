import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PaymentUseDialogComponent } from './payment-use-dialog.component';

describe('PaymentUseDialogComponent', () => {
  let component: PaymentUseDialogComponent;
  let fixture: ComponentFixture<PaymentUseDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PaymentUseDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PaymentUseDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should be created', () => {
    expect(component).toBeTruthy();
  });
});
