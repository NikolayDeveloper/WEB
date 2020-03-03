import { TestBed, inject } from '@angular/core/testing';

import { IncomingMaterialsService } from './incoming-materials.service';

describe('TasksService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [IncomingMaterialsService]
    });
  });

  it('should be created', inject([IncomingMaterialsService], (service: IncomingMaterialsService) => {
    expect(service).toBeTruthy();
  }));
});
