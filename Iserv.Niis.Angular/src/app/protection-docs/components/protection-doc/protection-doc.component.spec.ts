import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProtectionDocComponent } from './protection-doc.component';

describe('ProtectionDocComponent', () => {
  let component: ProtectionDocComponent;
  let fixture: ComponentFixture<ProtectionDocComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProtectionDocComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProtectionDocComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
