import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { OfficeWorkComponent } from './office-work.component';

describe('OfficeWorkComponent', () => {
  let component: OfficeWorkComponent;
  let fixture: ComponentFixture<OfficeWorkComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ OfficeWorkComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(OfficeWorkComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should be created', () => {
    expect(component).toBeTruthy();
  });
});
