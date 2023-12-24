import { Component, OnDestroy, OnInit } from '@angular/core';
import { PostsService } from '../../shared/services/posts.service';
import { Post } from '../../shared/models/posts';
import { FriendListComponent } from './components/friend-list/friend-list.component';
import { PostComponent } from '../../shared/components/post/post.component';
import { InfiniteScrollModule } from 'ngx-infinite-scroll';
import { PostSkeletonComponent } from '../../shared/components/post-skeleton/post-skeleton.component';
import { NgIf, NgFor } from '@angular/common';
import { UploadPostComponent } from '../../shared/components/upload-post/upload-post.component';
import { HomeUserCardComponent } from './components/home-user-card/home-user-card.component';
import { Subject, takeUntil } from 'rxjs';
import { CacheManagerService } from 'src/app/core/services/cache-manager.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
  standalone: true,
  imports: [
    HomeUserCardComponent,
    UploadPostComponent,
    NgIf,
    PostSkeletonComponent,
    InfiniteScrollModule,
    NgFor,
    PostComponent,
    FriendListComponent,
  ],
})
export class HomeComponent implements OnInit, OnDestroy {
  isLoading: boolean = true;
  posts: Post[] = [];
  pageNumber: number = 1;
  pageSize: number = 6;
  totalPosts: number | undefined;
  allPostsLoaded: boolean = false;
  private destroy$ = new Subject<void>();

  constructor(
    private postsService: PostsService,
    private cacheManager: CacheManagerService
  ) {}

  ngOnInit(): void {
    const cachedPosts = this.cacheManager.getCache<Post[]>('posts');
    if (cachedPosts && cachedPosts.length > 0) {
      this.isLoading = false;
      this.posts = cachedPosts;
    } else {
      this.loadPosts();
    }
  }

  loadPosts(): void {
    this.postsService
      .getPosts(this.pageNumber, this.pageSize)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (response) => {
          const loadedPosts = response.result;
          if (loadedPosts) {
            this.posts.push(...loadedPosts);
            this.totalPosts = response.pagination?.totalItems;
            this.cacheManager.setCache('posts', this.posts);
            this.isLoading = false;
            if (!response.pagination || loadedPosts.length < this.pageSize) {
              this.allPostsLoaded = true;
            }
          }
        },
      });
  }

  onScroll(): void {
    if (!this.allPostsLoaded) {
      this.pageNumber++;
      this.loadPosts();
    }
  }

  onPostUpload(post: Post): void {
    this.posts = [post, ...this.posts];
    this.cacheManager.setCache('posts', this.posts);
  }

  onPostDelete(postId: number): void {
    this.posts = this.posts.filter((post) => post.postId !== postId);
    this.cacheManager.setCache('posts', this.posts);
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
