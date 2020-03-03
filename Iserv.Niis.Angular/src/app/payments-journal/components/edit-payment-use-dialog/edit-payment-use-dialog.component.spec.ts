import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EditPaymentUseDialogComponent } from './edit-payment-use-dialog.component';

describe('EditPaymentUseDialogComponent', () => {
  let component: EditPaymentUseDialogComponent;
  let fixture: ComponentFixture<EditPaymentUseDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EditPaymentUseDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EditPaymentUseDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
