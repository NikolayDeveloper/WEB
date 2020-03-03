import { TestBed, inject } from '@angular/core/testing';

import { NcaLayerApiService } from './nca-layer-api.service';

describe('NcaLayerApiService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [NcaLayerApiService]
    });
  });

  it('should be created', inject([NcaLayerApiService], (service: NcaLayerApiService) => {
    expect(service).toBeTruthy();
  }));
});
