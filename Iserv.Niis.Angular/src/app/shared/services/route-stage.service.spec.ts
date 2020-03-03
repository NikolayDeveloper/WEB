import { TestBed, inject } from '@angular/core/testing';

import { RouteStageService } from './route-stage.service';

describe('RouteStageService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [RouteStageService]
    });
  });

  it('should be created', inject([RouteStageService], (service: RouteStageService) => {
    expect(service).toBeTruthy();
  }));
});
