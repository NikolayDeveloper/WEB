import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ColorFieldsComponent } from './color-fields.component';

describe('ColorFieldsComponent', () => {
  let component: ColorFieldsComponent;
  let fixture: ComponentFixture<ColorFieldsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ColorFieldsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ColorFieldsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
