import { Component, OnDestroy, OnInit } from '@angular/core';
import { PostsService } from '../../shared/services/posts.service';
import { Post } from '../../shared/models/posts';
import { FriendListComponent } from './components/friend-list/friend-list.component';
import { PostComponent } from '../../shared/components/post/post.component';
import { InfiniteScrollModule } from 'ngx-infinite-scroll';
import { PostSkeletonComponent } from '../../shared/components/post-skeleton/post-skeleton.component';
import { UploadPostComponent } from '../../shared/components/upload-post/upload-post.component';
import { HomeUserCardComponent } from './components/home-user-card/home-user-card.component';
import { Subject, takeUntil } from 'rxjs';
import { CacheManagerService } from 'src/app/core/services/cache-manager.service';
import { LocalStorageService } from 'src/app/core/services/local-storage.service';
import { SpinnerComponent } from '../../shared/components/spinner/spinner.component';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
  standalone: true,
  imports: [
    HomeUserCardComponent,
    UploadPostComponent,
    PostSkeletonComponent,
    InfiniteScrollModule,
    PostComponent,
    FriendListComponent,
    SpinnerComponent,
  ],
})
export class HomeComponent implements OnInit, OnDestroy {
  isLoading: boolean = true;
  loadingError: boolean = false;
  newPostsLoading: boolean = false;
  posts: Post[] = [];
  pageNumber: number = 1;
  pageSize: number = 4;
  totalPosts: number | undefined;
  allPostsLoaded: boolean = false;
  private destroy$ = new Subject<void>();

  constructor(
    private postsService: PostsService,
    private cacheManager: CacheManagerService,
    private localStorageService: LocalStorageService
  ) {}

  ngOnInit(): void {
    const cachedPosts = this.cacheManager.getCache<Post[]>('posts');
    if (cachedPosts) {
      this.isLoading = false;
      this.posts = cachedPosts;
      if (
        this.totalPosts !== undefined &&
        this.posts.length >= this.totalPosts
      ) {
        this.allPostsLoaded = true;
      }
    } else {
      this.loadPosts();
    }
  }

  loadPosts(): void {
    this.loadingError = false;

    this.postsService
      .getPosts(this.pageNumber, this.pageSize)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (response) => {
          const loadedPosts = response.result;
          if (loadedPosts) {
            this.posts.push(...loadedPosts);
            this.totalPosts = response.pagination?.totalItems;
            this.localStorageService.setItem('totalPosts', this.totalPosts);
            this.cacheManager.setCache('posts', this.posts);

            this.isLoading = false;
            this.newPostsLoading = false;
            if (!response.pagination || loadedPosts.length < this.pageSize) {
              this.allPostsLoaded = true;
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
    const totalPosts: number | null =
      this.localStorageService.getItem('totalPosts');
    if (!totalPosts) {
      return;
    }
    if (this.posts.length < totalPosts) {
      this.newPostsLoading = true;
      this.pageNumber++;
      this.loadPosts();
    }
  }

  onPostUpload(post: Post): void {
    this.posts = [post, ...this.posts];
    this.cacheManager.setCache('posts', this.posts);
    this.cacheManager.clearCache('posts' + post.username);
  }

  onPostUpdate(emittedPost: Post): void {
    this.cacheManager.clearCache('posts' + emittedPost.username);
  }

  onPostDelete(emittedPost: Post): void {
    this.posts = this.posts.filter(
      (post) => post.postId !== emittedPost.postId
    );
    this.cacheManager.setCache('posts', this.posts);
    this.cacheManager.clearCache('posts' + emittedPost.username);
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
