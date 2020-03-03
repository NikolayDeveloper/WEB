import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SearchModeToggleComponent } from './search-mode-toggle.component';

describe('SearchModeToggleComponent', () => {
  let component: SearchModeToggleComponent;
  let fixture: ComponentFixture<SearchModeToggleComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SearchModeToggleComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SearchModeToggleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
