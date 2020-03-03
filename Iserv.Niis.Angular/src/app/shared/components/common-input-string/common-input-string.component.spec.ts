import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CommonInputStringComponent } from './common-input-string.component';

describe('CommonInputStringComponent', () => {
  let component: CommonInputStringComponent;
  let fixture: ComponentFixture<CommonInputStringComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CommonInputStringComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CommonInputStringComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
