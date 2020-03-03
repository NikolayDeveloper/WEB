import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { IndustrialdesignFormComponent } from './industrialdesign-form.component';

describe('IndustrialdesignFormComponent', () => {
  let component: IndustrialdesignFormComponent;
  let fixture: ComponentFixture<IndustrialdesignFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ IndustrialdesignFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(IndustrialdesignFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
