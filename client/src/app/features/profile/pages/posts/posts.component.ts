import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Post } from 'src/app/shared/models/posts';
import { PostsService } from 'src/app/shared/services/posts.service';
import { PostComponent } from '../../../../shared/components/post/post.component';
import { InfiniteScrollModule } from 'ngx-infinite-scroll';
import { PostSkeletonComponent } from '../../../../shared/components/post-skeleton/post-skeleton.component';
import { NgIf, NgFor } from '@angular/common';

@Component({
    selector: 'app-posts',
    templateUrl: './posts.component.html',
    styleUrls: ['./posts.component.scss'],
    standalone: true,
    imports: [
        NgIf,
        PostSkeletonComponent,
        InfiniteScrollModule,
        NgFor,
        PostComponent,
    ],
})
export class PostsComponent implements OnInit {
  isLoading: boolean = true;
  posts: Post[] = [];
  pageNumber: number = 1;
  pageSize: number = 4;
  totalPosts: number | undefined;
  allPostsLoaded: boolean = false;
  username: any;
  noPosts: boolean = false;

  constructor(
    private postsService: PostsService,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.loadUserPosts();
  }

  loadUserPosts() {
    this.username = this.route.parent?.snapshot.paramMap.get('username');
    if (!this.username) {
      return;
    }

    this.postsService
      .getPostsByUsername(this.username, this.pageNumber, this.pageSize)
      .subscribe({
        next: (response) => {
          const loadedPosts = response.result;
          if (loadedPosts) {
            this.posts.push(...loadedPosts);
            this.totalPosts = response.pagination?.totalItems;
            this.isLoading = false;
            if (!response.pagination || loadedPosts.length < this.pageSize) {
              this.allPostsLoaded = true;
            }
            if (this.posts.length === 0) {
              this.noPosts = true;
            }
          }
        },
        error: (err) => console.error(err),
      });
  }

  onScroll() {
    if (!this.allPostsLoaded) {
      this.pageNumber++;
      this.loadUserPosts();
    }
  }

  onPostDelete(postId: number) {
    this.posts = this.posts.filter((post) => post.postId !== postId);
  }
}