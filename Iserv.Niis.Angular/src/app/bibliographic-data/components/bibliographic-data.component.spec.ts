import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BibliographicDataComponent } from './bibliographic-data.component';

describe('BibliographicDataComponent', () => {
  let component: BibliographicDataComponent;
  let fixture: ComponentFixture<BibliographicDataComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BibliographicDataComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BibliographicDataComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should be created', () => {
    expect(component).toBeTruthy();
  });
});
