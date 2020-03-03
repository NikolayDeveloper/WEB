import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RequestSearchListComponent } from './request-search-list.component';

describe('AdvancedSearchListComponent', () => {
  let component: RequestSearchListComponent;
  let fixture: ComponentFixture<RequestSearchListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RequestSearchListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RequestSearchListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
