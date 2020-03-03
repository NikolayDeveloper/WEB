import { ProtectionDocSearchFormComponent } from '../protectiondoc-search-form/protectiondoc-search-form.component';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import {  } from './advanced-search-form.component';

describe('AdvancedSearchFormComponent', () => {
  let component: ProtectionDocSearchFormComponent;
  let fixture: ComponentFixture<ProtectionDocSearchFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProtectionDocSearchFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProtectionDocSearchFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
