import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EditBulletinSectionDialogComponent } from './edit-bulletin-section-dialog.component';

describe('EditBulletinSectionDialogComponent', () => {
  let component: EditBulletinSectionDialogComponent;
  let fixture: ComponentFixture<EditBulletinSectionDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EditBulletinSectionDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EditBulletinSectionDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
