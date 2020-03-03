import { DocumentSearchFormComponent } from '../document-search-form/document-search-form.component';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import {  } from './advanced-search-form.component';

describe('AdvancedSearchFormComponent', () => {
  let component: DocumentSearchFormComponent;
  let fixture: ComponentFixture<DocumentSearchFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DocumentSearchFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DocumentSearchFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
