import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PaymentSearchFormComponent } from './payment-search-form.component';

describe('PaymentSearchFormComponent', () => {
  let component: PaymentSearchFormComponent;
  let fixture: ComponentFixture<PaymentSearchFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PaymentSearchFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PaymentSearchFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
