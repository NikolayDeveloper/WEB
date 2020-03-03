import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { OutgoingMaterialsComponent } from './outgoing-materials.component';


describe('OutgoingMaterialsComponent', () => {
  let component: OutgoingMaterialsComponent;
  let fixture: ComponentFixture<OutgoingMaterialsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ OutgoingMaterialsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(OutgoingMaterialsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
