import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AttachToOwnerFormComponent } from './attach-to-owner-form.component';

describe('AttachToOwnerFormComponent', () => {
  let component: AttachToOwnerFormComponent;
  let fixture: ComponentFixture<AttachToOwnerFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AttachToOwnerFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AttachToOwnerFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
