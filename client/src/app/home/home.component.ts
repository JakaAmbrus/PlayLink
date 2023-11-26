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
  pageSize = 6;

  constructor(private postsService: PostsService) {}

  ngOnInit(): void {
    this.loadPosts();
  }

  loadPosts() {
    this.postsService.getPosts(this.pageNumber, this.pageSize).subscribe({
      next: (response) => {
        this.posts = response.posts;
        this.isLoading = false;
        console.log(response.posts);
      },
      error: (err) => console.error(err),
    });
  }

  onScroll() {
    this.pageNumber++;
    this.loadPosts();
  }

  onPostUpload(post: Post) {
    this.posts = [post, ...this.posts];
  }

  onPostDelete(postId: number) {
    this.posts = this.posts.filter((post) => post.postId !== postId);
  }
}
