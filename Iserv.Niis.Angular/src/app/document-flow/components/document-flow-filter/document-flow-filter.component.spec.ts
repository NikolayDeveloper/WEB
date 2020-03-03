import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DocumentFlowFilterComponent } from './document-flow-filter.component';

describe('DocumentFlowFilterComponent', () => {
  let component: DocumentFlowFilterComponent;
  let fixture: ComponentFixture<DocumentFlowFilterComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DocumentFlowFilterComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DocumentFlowFilterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
