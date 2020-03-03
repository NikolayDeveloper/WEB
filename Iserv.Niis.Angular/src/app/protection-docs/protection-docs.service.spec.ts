import { TestBed, inject } from '@angular/core/testing';

import { ProtectionDocsService } from './protection-docs.service';

describe('ProtectionDocsService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [ProtectionDocsService]
    });
  });

  it('should be created', inject([ProtectionDocsService], (service: ProtectionDocsService) => {
    expect(service).toBeTruthy();
  }));
});
