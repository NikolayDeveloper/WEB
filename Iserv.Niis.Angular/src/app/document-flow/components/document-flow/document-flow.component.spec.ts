import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DocumentFlowComponent } from './document-flow.component';

describe('DocumentFlowComponent', () => {
  let component: DocumentFlowComponent;
  let fixture: ComponentFixture<DocumentFlowComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DocumentFlowComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DocumentFlowComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });
  
  it('should be created', () => {
    expect(component).toBeTruthy();
  });
});
