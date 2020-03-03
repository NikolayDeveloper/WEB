import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CtmParticipantsFieldDialogComponent } from './ctm-participants-field-dialog.component';

describe('CtmParticipantsFieldDialogComponent', () => {
  let component: CtmParticipantsFieldDialogComponent;
  let fixture: ComponentFixture<CtmParticipantsFieldDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CtmParticipantsFieldDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CtmParticipantsFieldDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
