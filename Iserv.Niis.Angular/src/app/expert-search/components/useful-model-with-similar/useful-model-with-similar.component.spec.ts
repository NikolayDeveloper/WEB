import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { UsefulModelWithSimilarComponent } from './useful-model-with-similar.component';

describe('UsefulModelWithSimilarComponent', () => {
  let component: UsefulModelWithSimilarComponent;
  let fixture: ComponentFixture<UsefulModelWithSimilarComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ UsefulModelWithSimilarComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UsefulModelWithSimilarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
