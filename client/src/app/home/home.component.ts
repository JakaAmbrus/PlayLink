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

  constructor(private postsService: PostsService) {}

  ngOnInit(): void {
    this.loadPosts();
  }

  loadPosts() {
    this.postsService.getPosts().subscribe({
      next: (response) => {
        this.posts = response.posts;
        this.isLoading = false;
        console.log(response.posts);
      },
      error: (err) => console.error(err),
    });
  }

  onPostUpload(post: Post) {
    this.posts = [post, ...this.posts];
  }
}
