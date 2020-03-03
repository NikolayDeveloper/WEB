import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { UsefulmodelListComponent } from './usefulmodel-list.component';

describe('UsefulmodelListComponent', () => {
  let component: UsefulmodelListComponent;
  let fixture: ComponentFixture<UsefulmodelListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ UsefulmodelListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UsefulmodelListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
