import { TestBed, inject } from '@angular/core/testing';

import { BulletinService } from './bulletin.service';

describe('BulletinService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [BulletinService]
    });
  });

  it('should be created', inject([BulletinService], (service: BulletinService) => {
    expect(service).toBeTruthy();
  }));
});
