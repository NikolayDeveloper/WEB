import { TestBed, inject } from '@angular/core/testing';

import { OutgoingMaterialsService } from './outgoing-materials.service';

describe('OutgoingMaterialsService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [OutgoingMaterialsService]
    });
  });

  it('should be created', inject([OutgoingMaterialsService], (service: OutgoingMaterialsService) => {
    expect(service).toBeTruthy();
  }));
});
