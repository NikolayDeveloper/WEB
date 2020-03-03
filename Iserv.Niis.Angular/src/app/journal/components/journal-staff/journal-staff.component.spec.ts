import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { JournalStaffComponent } from './journal-staff.component';

describe('JournalStaffComponent', () => {
  let component: JournalStaffComponent;
  let fixture: ComponentFixture<JournalStaffComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ JournalStaffComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(JournalStaffComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should be created', () => {
    expect(component).toBeTruthy();
  });
});
