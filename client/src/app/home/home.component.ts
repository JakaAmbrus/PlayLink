import { Component, OnInit } from '@angular/core';
import { PostsService } from '../_services/posts.service';
import { Post } from '../_models/posts';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
})
export class HomeComponent implements OnInit {
  isLoading: boolean = true;
  posts: Post[] = [];
  pageNumber = 1;
  pageSize = 8;
  totalPosts: number | undefined;

  constructor(private postsService: PostsService) {}

  ngOnInit(): void {
    this.loadPosts();
  }

  loadPosts() {
    this.postsService.getPosts(this.pageNumber, this.pageSize).subscribe({
      next: (response) => {
        if (response.result) {
          this.posts.push(...response.result);
          this.totalPosts = response.pagination?.totalItems;
          this.isLoading = false;
        }
      },
      error: (err) => console.error(err),
    });
  }

  onScroll() {
    if (this.totalPosts) {
      if (this.posts.length < this.totalPosts) {
        this.pageNumber++;
        this.loadPosts();
      }
    }
  }

  onPostUpload(post: Post) {
    this.posts = [post, ...this.posts];
  }

  onPostDelete(postId: number) {
    this.posts = this.posts.filter((post) => post.postId !== postId);
  }
}
