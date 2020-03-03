import { TestBed, inject } from '@angular/core/testing';

import { BulletinsService } from './bulletins.service';

describe('BulletinsService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [BulletinsService]
    });
  });

  it('should be created', inject([BulletinsService], (service: BulletinsService) => {
    expect(service).toBeTruthy();
  }));
});
