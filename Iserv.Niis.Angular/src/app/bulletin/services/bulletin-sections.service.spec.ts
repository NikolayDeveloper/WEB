import { TestBed, inject } from '@angular/core/testing';

import { BulletinSectionsService } from './bulletin-sections.service';

describe('BulletinSectionsService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [BulletinSectionsService]
    });
  });

  it('should be created', inject([BulletinSectionsService], (service: BulletinSectionsService) => {
    expect(service).toBeTruthy();
  }));
});
