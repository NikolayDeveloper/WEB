import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DocumenLinkComponent } from './documen-link.component';

describe('DocumenLinkComponent', () => {
  let component: DocumenLinkComponent;
  let fixture: ComponentFixture<DocumenLinkComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DocumenLinkComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DocumenLinkComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
