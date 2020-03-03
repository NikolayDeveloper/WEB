import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { IcisFieldsComponent } from './icis-fields.component';

describe('IcisFieldsComponent', () => {
  let component: IcisFieldsComponent;
  let fixture: ComponentFixture<IcisFieldsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ IcisFieldsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(IcisFieldsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
