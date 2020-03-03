import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateBulletinDialogComponent } from './create-bulletin-dialog.component';

describe('CreateBulletinDialogComponent', () => {
  let component: CreateBulletinDialogComponent;
  let fixture: ComponentFixture<CreateBulletinDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [CreateBulletinDialogComponent]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CreateBulletinDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
