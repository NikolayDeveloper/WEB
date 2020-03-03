import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { JournalTasksComponent } from './journal-tasks.component';

describe('JournalTasksComponent', () => {
  let component: JournalTasksComponent;
  let fixture: ComponentFixture<JournalTasksComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ JournalTasksComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(JournalTasksComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should be created', () => {
    expect(component).toBeTruthy();
  });
});
