import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateBulletinSectionDialogComponent } from './create-bulletin-section-dialog.component';

describe('CreateBulletinSectionDialogComponent', () => {
  let component: CreateBulletinSectionDialogComponent;
  let fixture: ComponentFixture<CreateBulletinSectionDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [CreateBulletinSectionDialogComponent]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CreateBulletinSectionDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
