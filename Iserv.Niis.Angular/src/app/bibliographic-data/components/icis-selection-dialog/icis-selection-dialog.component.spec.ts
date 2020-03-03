import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { IcisSelectionDialogComponent } from './icis-selection-dialog.component';

describe('IcisSelectionDialogComponent', () => {
  let component: IcisSelectionDialogComponent;
  let fixture: ComponentFixture<IcisSelectionDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ IcisSelectionDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(IcisSelectionDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
