import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewCompareComponent } from './view-compare.component';

describe('ViewCompareComponent', () => {
  let component: ViewCompareComponent;
  let fixture: ComponentFixture<ViewCompareComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ViewCompareComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ViewCompareComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
