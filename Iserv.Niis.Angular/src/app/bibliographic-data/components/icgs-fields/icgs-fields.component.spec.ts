import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { IcgsFieldsComponent } from './icgs-fields.component';

describe('IcgsFieldsComponent', () => {
  let component: IcgsFieldsComponent;
  let fixture: ComponentFixture<IcgsFieldsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ IcgsFieldsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(IcgsFieldsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
