import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { InventionFormComponent } from './invention-form.component';

describe('InventionFormComponent', () => {
  let component: InventionFormComponent;
  let fixture: ComponentFixture<InventionFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ InventionFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(InventionFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
