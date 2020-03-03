import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { InventionListComponent } from './invention-list.component';

describe('InventionListComponent', () => {
  let component: InventionListComponent;
  let fixture: ComponentFixture<InventionListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ InventionListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(InventionListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
