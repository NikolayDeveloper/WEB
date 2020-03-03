import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DocumentSearchListComponent } from './document-search-list.component';

describe('AdvancedSearchListComponent', () => {
  let component: DocumentSearchListComponent;
  let fixture: ComponentFixture<DocumentSearchListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DocumentSearchListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DocumentSearchListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
