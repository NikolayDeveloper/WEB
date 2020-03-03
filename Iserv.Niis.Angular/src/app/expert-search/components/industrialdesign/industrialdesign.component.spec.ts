import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { IndustrialdesignComponent } from './industrialdesign.component';

describe('IndustrialdesignComponent', () => {
  let component: IndustrialdesignComponent;
  let fixture: ComponentFixture<IndustrialdesignComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ IndustrialdesignComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(IndustrialdesignComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
