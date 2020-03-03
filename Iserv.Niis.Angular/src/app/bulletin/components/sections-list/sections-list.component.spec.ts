import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SectionsListComponent } from './sections-list.component';

describe('SectionsListComponent', () => {
  let component: SectionsListComponent;
  let fixture: ComponentFixture<SectionsListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [SectionsListComponent]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SectionsListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
