import { TestBed } from '@angular/core/testing';

import { PostsStateService } from './posts-state.service';

describe('PostsStateService', () => {
  let service: PostsStateService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(PostsStateService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
