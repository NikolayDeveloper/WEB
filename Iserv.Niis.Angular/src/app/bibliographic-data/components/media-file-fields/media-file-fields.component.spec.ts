import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MediaFileFieldsComponent } from './media-file-fields.component';

describe('MediaFileFieldsComponent', () => {
  let component: MediaFileFieldsComponent;
  let fixture: ComponentFixture<MediaFileFieldsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MediaFileFieldsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MediaFileFieldsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
