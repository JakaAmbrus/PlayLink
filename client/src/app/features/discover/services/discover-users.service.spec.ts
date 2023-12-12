import { TestBed } from '@angular/core/testing';

import { DiscoverUsersService } from './discover-users.service';

describe('DiscoverUsersService', () => {
  let service: DiscoverUsersService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(DiscoverUsersService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
