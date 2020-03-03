import { TestBed, inject } from '@angular/core/testing';

import { InternalMaterialsService } from './internal-materials.service';

describe('InternalMaterialsService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [InternalMaterialsService]
    });
  });

  it('should be created', inject([InternalMaterialsService], (service: InternalMaterialsService) => {
    expect(service).toBeTruthy();
  }));
});
