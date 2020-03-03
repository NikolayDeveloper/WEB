import { TestBed, inject } from '@angular/core/testing';

import { IntegrationWith1cService } from './integration-with-1c.service';

describe('IntegrationWith1cService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [IntegrationWith1cService]
    });
  });

  it('should be created', inject([IntegrationWith1cService], (service: IntegrationWith1cService) => {
    expect(service).toBeTruthy();
  }));
});
