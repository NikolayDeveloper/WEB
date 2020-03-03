import { ContractSearchFormComponent } from '../contract-search-form/contract-search-form.component';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import {  } from './advanced-search-form.component';

describe('AdvancedSearchFormComponent', () => {
  let component: ContractSearchFormComponent;
  let fixture: ComponentFixture<ContractSearchFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ContractSearchFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ContractSearchFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
