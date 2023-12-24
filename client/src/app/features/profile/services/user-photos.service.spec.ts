import { TestBed } from '@angular/core/testing';

import { UserPhotosService } from './user-photos.service';

describe('UserPhotosService', () => {
  let service: UserPhotosService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(UserPhotosService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
