import { RequestSearchFormComponent } from '../request-search-form/request-search-form.component';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import {  } from './advanced-search-form.component';

describe('AdvancedSearchFormComponent', () => {
  let component: RequestSearchFormComponent;
  let fixture: ComponentFixture<RequestSearchFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RequestSearchFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RequestSearchFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
