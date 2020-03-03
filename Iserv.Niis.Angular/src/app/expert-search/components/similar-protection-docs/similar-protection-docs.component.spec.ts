import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SimilarProtectionDocsComponent } from './similar-protection-docs.component';

describe('SimilarProtectionDocsComponent', () => {
  let component: SimilarProtectionDocsComponent;
  let fixture: ComponentFixture<SimilarProtectionDocsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SimilarProtectionDocsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SimilarProtectionDocsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
