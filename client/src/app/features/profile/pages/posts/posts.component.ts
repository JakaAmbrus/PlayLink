import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Post } from 'src/app/shared/models/posts';
import { PostsService } from 'src/app/shared/services/posts.service';
import { PostComponent } from '../../../../shared/components/post/post.component';
import { InfiniteScrollModule } from 'ngx-infinite-scroll';
import { PostSkeletonComponent } from '../../../../shared/components/post-skeleton/post-skeleton.component';
import { NgIf } from '@angular/common';
import { Subject, takeUntil } from 'rxjs';
import { CacheManagerService } from 'src/app/core/services/cache-manager.service';
import { LocalStorageService } from 'src/app/core/services/local-storage.service';
import { SpinnerComponent } from '../../../../shared/components/spinner/spinner.component';

@Component({
  selector: 'app-posts',
  templateUrl: './posts.component.html',
  styleUrls: ['./posts.component.scss'],
  standalone: true,
  imports: [
    NgIf,
    PostSkeletonComponent,
    InfiniteScrollModule,
    PostComponent,
    SpinnerComponent,
  ],
})
export class PostsComponent implements OnInit, OnDestroy {
  isLoading: boolean = true;
  loadingError: boolean = false;
  newPostsLoading: boolean = false;
  posts: Post[] = [];
  pageNumber: number = 1;
  pageSize: number = 3;
  totalPosts: number | undefined;
  allPostsLoaded: boolean = false;
  username: string | null | undefined;
  noPosts: boolean = false;
  private destroy$ = new Subject<void>();

  constructor(
    private postsService: PostsService,
    private cacheManager: CacheManagerService,
    private route: ActivatedRoute,
    private localStorageService: LocalStorageService
  ) {}

  ngOnInit(): void {
    this.username = this.route.parent?.snapshot.paramMap.get('username');
    if (!this.username) {
      return;
    }
    const cachedPosts = this.cacheManager.getCache<Post[]>(
      'posts' + this.username
    );
    if (cachedPosts) {
      this.isLoading = false;
      this.posts = cachedPosts;
      if (this.posts.length === 0) {
        this.noPosts = true;
      }
      if (
        this.totalPosts !== undefined &&
        this.posts.length >= this.totalPosts
      ) {
        this.allPostsLoaded = true;
      }
    } else {
      this.loadUserPosts();
    }
  }

  loadUserPosts(): void {
    if (!this.username) {
      return;
    }
    this.loadingError = false;

    this.postsService
      .getPostsByUsername(this.username, this.pageNumber, this.pageSize)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (response) => {
          const loadedPosts = response.result;
          if (loadedPosts) {
            this.posts.push(...loadedPosts);
            this.totalPosts = response.pagination?.totalItems;
            this.localStorageService.setItem(
              'totalPosts' + this.username,
              this.totalPosts
            );
            this.cacheManager.setCache('posts' + this.username, this.posts);

            this.isLoading = false;
            this.newPostsLoading = false;

            if (!response.pagination || loadedPosts.length < this.pageSize) {
              this.allPostsLoaded = true;
            }
            if (this.posts.length === 0) {
              this.noPosts = true;
            }
          }
        },
        error: () => {
          this.loadingError = true;
          this.newPostsLoading = false;
        },
      });
  }

  onScroll(): void {
    const totalPosts: number | null = this.localStorageService.getItem(
      'totalPosts' + this.username
    );
    if (!totalPosts) {
      return;
    }
    if (this.posts.length < totalPosts) {
      this.newPostsLoading = true;
      this.pageNumber++;
      this.loadUserPosts();
    }
  }

  onPostDelete(emittedPost: Post): void {
    this.posts = this.posts.filter(
      (post) => post.postId !== emittedPost.postId
    );
    this.cacheManager.setCache('posts' + this.username, this.posts);
    this.cacheManager.clearCache('posts');
  }
  onPostUpdate(emittedPost: Post): void {
    this.cacheManager.setCache('posts' + this.username, this.posts);
    this.cacheManager.clearCache('posts');
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
