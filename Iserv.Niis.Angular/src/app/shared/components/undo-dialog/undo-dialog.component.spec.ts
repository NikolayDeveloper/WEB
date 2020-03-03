import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { UndoDialogComponent } from './undo-dialog.component';

describe('UndoDialogComponent', () => {
  let component: UndoDialogComponent;
  let fixture: ComponentFixture<UndoDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ UndoDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UndoDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
