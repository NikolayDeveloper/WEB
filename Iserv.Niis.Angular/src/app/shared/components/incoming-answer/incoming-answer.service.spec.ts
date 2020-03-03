import { TestBed, inject } from '@angular/core/testing';

import { IncomingAnswerService } from './incoming-answer.service';

describe('IncomingAnswerService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [IncomingAnswerService]
    });
  });

  it('should be created', inject([IncomingAnswerService], (service: IncomingAnswerService) => {
    expect(service).toBeTruthy();
  }));
});
