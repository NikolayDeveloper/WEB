import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ContractSearchListComponent } from './contract-search-list.component';

describe('AdvancedSearchListComponent', () => {
  let component: ContractSearchListComponent;
  let fixture: ComponentFixture<ContractSearchListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ContractSearchListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ContractSearchListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
