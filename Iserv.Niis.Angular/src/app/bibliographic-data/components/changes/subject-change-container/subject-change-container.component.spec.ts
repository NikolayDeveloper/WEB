import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SubjectChangeContainerComponent } from './subject-change-container.component';

describe('SubjectChangeContainerComponent', () => {
  let component: SubjectChangeContainerComponent;
  let fixture: ComponentFixture<SubjectChangeContainerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SubjectChangeContainerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SubjectChangeContainerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
