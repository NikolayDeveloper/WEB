import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { IncomingMaterialsComponent } from './incoming-materials.component';

describe('IncomingMaterialsComponent', () => {
  let component: IncomingMaterialsComponent;
  let fixture: ComponentFixture<IncomingMaterialsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ IncomingMaterialsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(IncomingMaterialsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
