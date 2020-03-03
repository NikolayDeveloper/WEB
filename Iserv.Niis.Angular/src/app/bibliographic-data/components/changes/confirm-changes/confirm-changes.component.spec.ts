import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ConfirmChangesComponent } from './confirm-changes.component';

describe('ConfirmChangesComponent', () => {
  let component: ConfirmChangesComponent;
  let fixture: ComponentFixture<ConfirmChangesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ConfirmChangesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ConfirmChangesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
