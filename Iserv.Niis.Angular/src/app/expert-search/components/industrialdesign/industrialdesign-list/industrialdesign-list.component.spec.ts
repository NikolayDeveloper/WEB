import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { IndustrialdesignListComponent } from './industrialdesign-list.component';

describe('IndustrialdesignListComponent', () => {
  let component: IndustrialdesignListComponent;
  let fixture: ComponentFixture<IndustrialdesignListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ IndustrialdesignListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(IndustrialdesignListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
