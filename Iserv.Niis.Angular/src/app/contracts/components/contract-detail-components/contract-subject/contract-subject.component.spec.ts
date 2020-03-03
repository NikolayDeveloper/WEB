import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ContractSubjectComponent } from './contract-subject.component';

describe('ContractSubjectComponent', () => {
  let component: ContractSubjectComponent;
  let fixture: ComponentFixture<ContractSubjectComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ContractSubjectComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ContractSubjectComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
