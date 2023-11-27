import { Component, OnInit } from '@angular/core';
import { PostsService } from '../_services/posts.service';
import { Post } from '../_models/posts';
import { PostsStateService } from '../_services/state/posts-state.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
})
export class HomeComponent implements OnInit {
  isLoading: boolean = true;
  posts: Post[] = [];
  pageNumber: number = 1;
  pageSize: number = 8;
  totalPosts: number | undefined;
  allPostsLoaded: boolean = false;

  constructor(
    private postsService: PostsService,
    private postsStateService: PostsStateService
  ) {}

  ngOnInit(): void {
    this.loadPosts();
  }

  loadPosts() {
    //if there are posts in the state, prevents another API call
    // const cachedPostsArr = this.postsStateService.getPosts();
    this.postsService.getPosts(this.pageNumber, this.pageSize).subscribe({
      next: (response) => {
        const loadedPosts = response.result;
        if (loadedPosts) {
          this.posts.push(...loadedPosts);
          this.totalPosts = response.pagination?.totalItems;
          this.isLoading = false;
          if (!response.pagination || loadedPosts.length < this.pageSize) {
            this.allPostsLoaded = true;
          }
        }
      },
      error: (err) => console.error(err),
    });
  }

  onScroll() {
    if (!this.allPostsLoaded) {
      this.pageNumber++;
      this.loadPosts();
    }
  }

  onPostUpload(post: Post) {
    this.posts = [post, ...this.posts];
  }

  onPostDelete(postId: number) {
    this.posts = this.posts.filter((post) => post.postId !== postId);
  }
}
