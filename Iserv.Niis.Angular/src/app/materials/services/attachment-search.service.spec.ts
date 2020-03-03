import { TestBed, inject } from '@angular/core/testing';

import { AttachmentSearchService } from './attachment-search.service';

describe('AttachmentSearchService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [AttachmentSearchService]
    });
  });

  it('should be created', inject([AttachmentSearchService], (service: AttachmentSearchService) => {
    expect(service).toBeTruthy();
  }));
});
