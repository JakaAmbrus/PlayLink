import { TestBed } from '@angular/core/testing';

import { CacheManagerService } from './cache-manager.service';

describe('CacheManagerService', () => {
  let service: CacheManagerService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CacheManagerService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
