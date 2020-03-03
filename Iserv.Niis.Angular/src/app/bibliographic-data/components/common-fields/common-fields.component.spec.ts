import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CommonFieldsComponent } from './common-fields.component';

describe('CommonFieldsComponent', () => {
  let component: CommonFieldsComponent;
  let fixture: ComponentFixture<CommonFieldsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CommonFieldsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CommonFieldsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
