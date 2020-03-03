import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SubjectsSearchFormComponent } from './subjects-search-form.component';

describe('SubjectsSearchFormComponent', () => {
  let component: SubjectsSearchFormComponent;
  let fixture: ComponentFixture<SubjectsSearchFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SubjectsSearchFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SubjectsSearchFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
