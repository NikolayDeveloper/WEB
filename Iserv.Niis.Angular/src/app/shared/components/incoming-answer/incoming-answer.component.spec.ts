import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { IncomingAnswerComponent } from './incoming-answer.component';

describe('IncomingAnswerComponent', () => {
  let component: IncomingAnswerComponent;
  let fixture: ComponentFixture<IncomingAnswerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ IncomingAnswerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(IncomingAnswerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
