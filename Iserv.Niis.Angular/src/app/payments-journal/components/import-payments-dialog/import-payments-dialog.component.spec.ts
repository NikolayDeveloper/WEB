import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ImportPaymentsDialogComponent } from './import-payments-dialog.component';

describe('ImportPaymentsDialogComponent', () => {
  let component: ImportPaymentsDialogComponent;
  let fixture: ComponentFixture<ImportPaymentsDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ImportPaymentsDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ImportPaymentsDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
