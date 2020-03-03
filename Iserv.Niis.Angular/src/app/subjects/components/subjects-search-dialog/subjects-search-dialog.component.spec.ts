import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SubjectsSearchDialogComponent } from './subjects-search-dialog.component';

describe('SubjectsSearchDialogComponent', () => {
  let component: SubjectsSearchDialogComponent;
  let fixture: ComponentFixture<SubjectsSearchDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SubjectsSearchDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SubjectsSearchDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
