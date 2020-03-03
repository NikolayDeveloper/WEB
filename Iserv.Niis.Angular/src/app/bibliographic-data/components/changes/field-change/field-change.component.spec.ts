import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FieldChangeComponent } from './field-change.component';

describe('FieldChangeComponent', () => {
  let component: FieldChangeComponent;
  let fixture: ComponentFixture<FieldChangeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FieldChangeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FieldChangeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
