import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MakeChangesComponent } from './make-changes.component';

describe('MakeChangesComponent', () => {
  let component: MakeChangesComponent;
  let fixture: ComponentFixture<MakeChangesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MakeChangesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MakeChangesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
