import { TestBed, inject } from '@angular/core/testing';

import { DocumentRequestService } from './document-request.service';

describe('DocumentRequestService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [DocumentRequestService]
    });
  });

  it('should be created', inject([DocumentRequestService], (service: DocumentRequestService) => {
    expect(service).toBeTruthy();
  }));
});
