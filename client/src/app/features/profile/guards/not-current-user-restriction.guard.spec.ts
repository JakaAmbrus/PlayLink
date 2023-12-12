import { TestBed } from '@angular/core/testing';
import { CanActivateFn } from '@angular/router';

import { notCurrentUserRestrictionGuard } from './not-current-user-restriction.guard';

describe('notCurrentUserRestrictionGuard', () => {
  const executeGuard: CanActivateFn = (...guardParameters) => 
      TestBed.runInInjectionContext(() => notCurrentUserRestrictionGuard(...guardParameters));

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeGuard).toBeTruthy();
  });
});
