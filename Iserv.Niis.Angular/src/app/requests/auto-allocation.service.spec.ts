import { TestBed, inject } from '@angular/core/testing';

import { AutoAllocationService } from './auto-allocation.service';

describe('AutoAllocationService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [AutoAllocationService]
    });
  });

  it('should be created', inject([AutoAllocationService], (service: AutoAllocationService) => {
    expect(service).toBeTruthy();
  }));
});
