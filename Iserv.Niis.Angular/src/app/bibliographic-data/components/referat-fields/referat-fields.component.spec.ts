import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ReferatFieldsComponent } from './referat-fields.component';

describe('ReferatFieldsComponent', () => {
  let component: ReferatFieldsComponent;
  let fixture: ComponentFixture<ReferatFieldsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ReferatFieldsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ReferatFieldsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
