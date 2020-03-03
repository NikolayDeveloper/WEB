import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CommonAutocompleteComponent } from './common-autocomplete.component';

describe('InputWithAutocompleteComponent', () => {
  let component: CommonAutocompleteComponent;
  let fixture: ComponentFixture<CommonAutocompleteComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CommonAutocompleteComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CommonAutocompleteComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
