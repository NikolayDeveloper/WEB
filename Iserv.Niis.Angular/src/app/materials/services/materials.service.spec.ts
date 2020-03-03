import { MaterialsComponent } from '../components/materials/materials.component';
import { TestBed, inject } from '@angular/core/testing';

import { MaterialsService } from './materials.service';

describe('MaterialsService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [MaterialsService]
    });
  });

  it('should be created', inject([MaterialsService], (service: MaterialsComponent) => {
    expect(service).toBeTruthy();
  }));
});
