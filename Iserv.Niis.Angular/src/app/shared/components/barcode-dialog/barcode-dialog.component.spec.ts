import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BarcodeDialogComponent } from './barcode-dialog.component';

describe('BarcodeDialogComponent', () => {
  let component: BarcodeDialogComponent;
  let fixture: ComponentFixture<BarcodeDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BarcodeDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BarcodeDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should be created', () => {
    expect(component).toBeTruthy();
  });
});
