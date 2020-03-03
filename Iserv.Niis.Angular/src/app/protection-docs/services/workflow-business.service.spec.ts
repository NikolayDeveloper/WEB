import { TestBed, inject } from '@angular/core/testing';

import { WorkflowBusinessService } from './workflow-business.service';

describe('WorkflowBusinessService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [WorkflowBusinessService]
    });
  });

  it('should be created', inject([WorkflowBusinessService], (service: WorkflowBusinessService) => {
    expect(service).toBeTruthy();
  }));
});
