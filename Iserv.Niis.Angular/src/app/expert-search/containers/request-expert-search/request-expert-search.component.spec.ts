import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RequestExpertSearchComponent } from './request-expert-search.component';

describe('ExpertSearchComponent', () => {
  let component: RequestExpertSearchComponent;
  let fixture: ComponentFixture<RequestExpertSearchComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RequestExpertSearchComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RequestExpertSearchComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
