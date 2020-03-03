import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DocumentsCompareComponent } from './documents-compare.component';

describe('DocumentsCompareComponent', () => {
  let component: DocumentsCompareComponent;
  let fixture: ComponentFixture<DocumentsCompareComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DocumentsCompareComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DocumentsCompareComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
