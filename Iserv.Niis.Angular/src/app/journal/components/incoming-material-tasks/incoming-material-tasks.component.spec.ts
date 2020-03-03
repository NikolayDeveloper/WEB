import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { IncomingMaterialTasksComponent } from './incoming-material-tasks.component';

describe('IncomingMaterialTasksComponent', () => {
  let component: IncomingMaterialTasksComponent;
  let fixture: ComponentFixture<IncomingMaterialTasksComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ IncomingMaterialTasksComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(IncomingMaterialTasksComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
