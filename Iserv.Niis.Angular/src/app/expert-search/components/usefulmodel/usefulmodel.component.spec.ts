import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { UsefulmodelComponent } from './usefulmodel.component';

describe('UsefulmodelComponent', () => {
  let component: UsefulmodelComponent;
  let fixture: ComponentFixture<UsefulmodelComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ UsefulmodelComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UsefulmodelComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
