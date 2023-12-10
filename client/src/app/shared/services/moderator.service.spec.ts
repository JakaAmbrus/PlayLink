import { TestBed } from '@angular/core/testing';

import { ModeratorService } from './moderator.service';

describe('ModeratorService', () => {
  let service: ModeratorService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ModeratorService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
