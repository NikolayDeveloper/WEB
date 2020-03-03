import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AuthorsCertificatesDialogComponent } from './authors-certificates-dialog.component';

describe('AuthorsCertificatesDialogComponent', () => {
  let component: AuthorsCertificatesDialogComponent;
  let fixture: ComponentFixture<AuthorsCertificatesDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AuthorsCertificatesDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AuthorsCertificatesDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
