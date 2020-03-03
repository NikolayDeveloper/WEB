import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { InventionComponent } from './invention.component';

describe('InventionComponent', () => {
  let component: InventionComponent;
  let fixture: ComponentFixture<InventionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ InventionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(InventionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
