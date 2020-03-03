import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DocumentRequestComponent } from './document-request.component';

describe('DocumentRequestComponent', () => {
  let component: DocumentRequestComponent;
  let fixture: ComponentFixture<DocumentRequestComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DocumentRequestComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DocumentRequestComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
