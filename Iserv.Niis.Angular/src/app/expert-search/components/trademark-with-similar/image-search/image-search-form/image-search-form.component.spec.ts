import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ImageSearchFormComponent } from './image-search-form.component';

describe('ImageSearchFormComponent', () => {
  let component: ImageSearchFormComponent;
  let fixture: ComponentFixture<ImageSearchFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ImageSearchFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ImageSearchFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
