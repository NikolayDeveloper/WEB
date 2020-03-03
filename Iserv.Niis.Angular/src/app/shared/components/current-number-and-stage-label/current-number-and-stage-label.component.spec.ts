import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CurrentNumberAndStageLabelComponent } from './current-number-and-stage-label.component';

describe('CurrentNumberAndStageLabelComponent', () => {
  let component: CurrentNumberAndStageLabelComponent;
  let fixture: ComponentFixture<CurrentNumberAndStageLabelComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CurrentNumberAndStageLabelComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CurrentNumberAndStageLabelComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
