import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ChangeExecutorDialogComponent } from './change-executor-dialog.component';

describe('ChangeExecutorDialogComponent', () => {
  let component: ChangeExecutorDialogComponent;
  let fixture: ComponentFixture<ChangeExecutorDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ChangeExecutorDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ChangeExecutorDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
