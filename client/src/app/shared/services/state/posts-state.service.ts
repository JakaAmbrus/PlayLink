import { Injectable } from '@angular/core';
import { Post } from 'src/app/shared/models/posts';

@Injectable({
  providedIn: 'root',
})
//manages state of my posts array, so that I can reduce the number of API calls
export class PostsStateService {
  private posts: Post[] = [];

  getPosts(): Post[] {
    return this.posts;
  }

  setPosts(posts: Post[]): void {
    this.posts = posts;
  }

  addPost(post: Post): void {
    this.posts.unshift(post);
  }

  removePost(postId: number): void {
    this.posts = this.posts.filter((post) => post.postId !== postId);
  }

  updatePost(post: Post): void {
    const postIndex = this.posts.findIndex(
      (post) => post.postId === post.postId
    );
    if (postIndex !== -1) {
      this.posts[postIndex] = post;
    }
  }
}
