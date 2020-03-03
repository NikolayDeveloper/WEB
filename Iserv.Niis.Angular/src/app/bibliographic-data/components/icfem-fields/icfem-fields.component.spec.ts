import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { IcfemFieldsComponent } from './icfem-fields.component';

describe('IcfemFieldsComponent', () => {
  let component: IcfemFieldsComponent;
  let fixture: ComponentFixture<IcfemFieldsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ IcfemFieldsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(IcfemFieldsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
