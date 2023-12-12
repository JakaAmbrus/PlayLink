import { TestBed } from '@angular/core/testing';
import { CanActivateFn } from '@angular/router';

import { currentUserRestrictionGuard } from './current-user-restriction.guard';

describe('currentUserRestrictionGuard', () => {
  const executeGuard: CanActivateFn = (...guardParameters) => 
      TestBed.runInInjectionContext(() => currentUserRestrictionGuard(...guardParameters));

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeGuard).toBeTruthy();
  });
});
