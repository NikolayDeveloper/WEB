import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ChangeTypeChooseComponent } from './change-type-choose.component';

describe('ChangeTypeChooseComponent', () => {
  let component: ChangeTypeChooseComponent;
  let fixture: ComponentFixture<ChangeTypeChooseComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ChangeTypeChooseComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ChangeTypeChooseComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
