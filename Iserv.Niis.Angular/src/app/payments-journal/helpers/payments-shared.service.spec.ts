import { TestBed, inject } from '@angular/core/testing';

import { PaymentsSharedService } from './payments-shared.service';

describe('PaymentsSharedService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [PaymentsSharedService]
    });
  });

  it('should be created', inject([PaymentsSharedService], (service: PaymentsSharedService) => {
    expect(service).toBeTruthy();
  }));
});
