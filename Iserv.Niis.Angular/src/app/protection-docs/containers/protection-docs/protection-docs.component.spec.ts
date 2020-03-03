import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProtectionDocsComponent } from './protection-docs.component';

describe('ProtectionDocsComponent', () => {
  let component: ProtectionDocsComponent;
  let fixture: ComponentFixture<ProtectionDocsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProtectionDocsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProtectionDocsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
