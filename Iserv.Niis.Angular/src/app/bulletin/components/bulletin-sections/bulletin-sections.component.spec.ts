import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BulletinSectionsComponent } from './bulletin-sections.component';

describe('BulletinSectionsComponent', () => {
  let component: BulletinSectionsComponent;
  let fixture: ComponentFixture<BulletinSectionsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [BulletinSectionsComponent]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BulletinSectionsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
