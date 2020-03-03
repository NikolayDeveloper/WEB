import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProtectionDocSearchListComponent } from './protectionDoc-search-list.component';

describe('AdvancedSearchListComponent', () => {
  let component: ProtectionDocSearchListComponent;
  let fixture: ComponentFixture<ProtectionDocSearchListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProtectionDocSearchListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProtectionDocSearchListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
