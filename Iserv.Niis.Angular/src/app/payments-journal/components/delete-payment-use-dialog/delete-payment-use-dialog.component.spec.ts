import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DeletePaymentUseDialogComponent } from './delete-payment-use-dialog.component';

describe('DeletePaymentUseDialogComponent', () => {
  let component: DeletePaymentUseDialogComponent;
  let fixture: ComponentFixture<DeletePaymentUseDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DeletePaymentUseDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DeletePaymentUseDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
