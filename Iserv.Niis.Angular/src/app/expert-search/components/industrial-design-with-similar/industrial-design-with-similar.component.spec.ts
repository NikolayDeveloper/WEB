import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { IndustrialDesignWithSimilarComponent } from './industrial-design-with-similar.component';

describe('InventionWithSimilarComponent', () => {
  let component: IndustrialDesignWithSimilarComponent;
  let fixture: ComponentFixture<IndustrialDesignWithSimilarComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ IndustrialDesignWithSimilarComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(IndustrialDesignWithSimilarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
