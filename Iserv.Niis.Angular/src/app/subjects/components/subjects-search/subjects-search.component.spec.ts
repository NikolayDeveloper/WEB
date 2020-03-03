import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SubjectsSearchComponent } from './subjects-search.component';

describe('SubjectsSearchComponent', () => {
  let component: SubjectsSearchComponent;
  let fixture: ComponentFixture<SubjectsSearchComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SubjectsSearchComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SubjectsSearchComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
