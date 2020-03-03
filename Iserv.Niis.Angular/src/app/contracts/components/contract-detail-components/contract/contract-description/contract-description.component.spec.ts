import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ContractDescriptionComponent } from './contract-description.component';

describe('DescriptionComponent', () => {
  let component: ContractDescriptionComponent;
  let fixture: ComponentFixture<ContractDescriptionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ContractDescriptionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ContractDescriptionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
