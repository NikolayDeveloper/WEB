import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { InventionWithSimilarComponent } from './invention-with-similar.component';

describe('InventionWithSimilarComponent', () => {
  let component: InventionWithSimilarComponent;
  let fixture: ComponentFixture<InventionWithSimilarComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ InventionWithSimilarComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(InventionWithSimilarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
