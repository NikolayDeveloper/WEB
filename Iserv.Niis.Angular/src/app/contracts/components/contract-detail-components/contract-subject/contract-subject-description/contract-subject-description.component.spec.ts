import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ContractSubjectDescriptionComponent } from './contract-subject-description.component';

describe('ContractSubjectDescriptionComponent', () => {
  let component: ContractSubjectDescriptionComponent;
  let fixture: ComponentFixture<ContractSubjectDescriptionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ContractSubjectDescriptionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ContractSubjectDescriptionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
