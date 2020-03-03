import { TestBed, inject } from '@angular/core/testing';

import { BibliographicDataService } from './bibliographic-data.service';

describe('BibliographicDataService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [BibliographicDataService]
    });
  });

  it('should be created', inject([BibliographicDataService], (service: BibliographicDataService) => {
    expect(service).toBeTruthy();
  }));
});
