import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { IpcFieldsComponent } from './ipc-fields.component';

describe('IpcFieldsComponent', () => {
  let component: IpcFieldsComponent;
  let fixture: ComponentFixture<IpcFieldsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ IpcFieldsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(IpcFieldsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
