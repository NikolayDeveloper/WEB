import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { SubjectChangeComponent } from './subject-change.component';

describe('SubjectChangeComponent', () => {
  let component: SubjectChangeComponent;
  let fixture: ComponentFixture<SubjectChangeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SubjectChangeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SubjectChangeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
