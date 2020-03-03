import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ImageFieldsComponent } from './image-fields.component';

describe('ImageFieldsComponent', () => {
  let component: ImageFieldsComponent;
  let fixture: ComponentFixture<ImageFieldsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ImageFieldsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ImageFieldsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
