import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { IncomingMaterialDetailsComponent } from './details.component';

describe('IncomingMaterialDetailsComponent', () => {
  let component: IncomingMaterialDetailsComponent;
  let fixture: ComponentFixture<IncomingMaterialDetailsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [IncomingMaterialDetailsComponent]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(IncomingMaterialDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
