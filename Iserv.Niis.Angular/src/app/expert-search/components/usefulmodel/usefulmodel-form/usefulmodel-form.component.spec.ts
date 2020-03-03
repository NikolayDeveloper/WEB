import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { UsefulmodelFormComponent } from './usefulmodel-form.component';

describe('UsefulmodelFormComponent', () => {
  let component: UsefulmodelFormComponent;
  let fixture: ComponentFixture<UsefulmodelFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ UsefulmodelFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UsefulmodelFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
