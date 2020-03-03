import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DialogForPasswordComponent } from './dialog-for-password.component';

describe('DialogForPasswordComponent', () => {
  let component: DialogForPasswordComponent;
  let fixture: ComponentFixture<DialogForPasswordComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DialogForPasswordComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DialogForPasswordComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
