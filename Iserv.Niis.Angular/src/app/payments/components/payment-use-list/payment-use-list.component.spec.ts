import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PaymentUseListComponent } from './payment-use-list.component';

describe('PaymentUseListComponent', () => {
  let component: PaymentUseListComponent;
  let fixture: ComponentFixture<PaymentUseListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PaymentUseListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PaymentUseListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
