import { TestBed, inject } from '@angular/core/testing';

import { ExpertSearchSimilarService } from './expert-search-similar.service';

describe('ExperSearchSimilarService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [ExpertSearchSimilarService]
    });
  });

  it('should be created', inject([ExpertSearchSimilarService], (service: ExpertSearchSimilarService) => {
    expect(service).toBeTruthy();
  }));
});
