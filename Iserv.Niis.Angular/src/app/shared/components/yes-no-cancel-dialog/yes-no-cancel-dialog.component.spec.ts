import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { YesNoCancelDialogComponent } from './yes-no-cancel-dialog.component';

describe('YesNoCancelDialogComponent', () => {
  let component: YesNoCancelDialogComponent;
  let fixture: ComponentFixture<YesNoCancelDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ YesNoCancelDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(YesNoCancelDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
