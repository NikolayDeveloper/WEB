import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TrademarkFieldsComponent } from './trademark-fields.component';

describe('TrademarkFieldsComponent', () => {
  let component: TrademarkFieldsComponent;
  let fixture: ComponentFixture<TrademarkFieldsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TrademarkFieldsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TrademarkFieldsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
