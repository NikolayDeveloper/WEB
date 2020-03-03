import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DocumentWorkflowDialogComponent } from './workflow-dialog.component';

describe('DocumentWorkflowDialogComponent', () => {
  let component: DocumentWorkflowDialogComponent;
  let fixture: ComponentFixture<DocumentWorkflowDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [DocumentWorkflowDialogComponent]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DocumentWorkflowDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
