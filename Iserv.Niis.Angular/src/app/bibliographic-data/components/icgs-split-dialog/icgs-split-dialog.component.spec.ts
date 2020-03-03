import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { IcgsSplitDialogComponent } from './icgs-split-dialog.component';

describe('IcgsSplitDialogComponent', () => {
  let component: IcgsSplitDialogComponent;
  let fixture: ComponentFixture<IcgsSplitDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ IcgsSplitDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(IcgsSplitDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
