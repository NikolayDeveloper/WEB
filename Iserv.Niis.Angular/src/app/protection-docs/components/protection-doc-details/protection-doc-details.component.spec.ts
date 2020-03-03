import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProtectionDocDetailsComponent } from './protection-doc-details.component';

describe('ProtectionDocDetailsComponent', () => {
  let component: ProtectionDocDetailsComponent;
  let fixture: ComponentFixture<ProtectionDocDetailsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProtectionDocDetailsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProtectionDocDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
