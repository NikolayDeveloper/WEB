import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SelectionFieldsComponent } from './selection-fields.component';

describe('SelectionFieldsComponent', () => {
  let component: SelectionFieldsComponent;
  let fixture: ComponentFixture<SelectionFieldsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SelectionFieldsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SelectionFieldsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
