import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductFieldsComponent } from './product-fields.component';

describe('ProductFieldsComponent', () => {
  let component: ProductFieldsComponent;
  let fixture: ComponentFixture<ProductFieldsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProductFieldsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProductFieldsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
