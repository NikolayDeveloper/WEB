import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BulletinSectionDetailsComponent } from './bulletin-section-details.component';

describe('BulletinSectionDetailsComponent', () => {
  let component: BulletinSectionDetailsComponent;
  let fixture: ComponentFixture<BulletinSectionDetailsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [BulletinSectionDetailsComponent]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BulletinSectionDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
