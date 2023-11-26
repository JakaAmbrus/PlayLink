import { Component, Input } from '@angular/core';
import { Post } from 'src/app/_models/posts';
import { LikesService } from 'src/app/_services/likes.service';
import { PostsService } from 'src/app/_services/posts.service';

@Component({
  selector: 'app-post',
  templateUrl: './post.component.html',
  styleUrls: ['./post.component.scss'],
})
export class PostComponent {
  @Input() post: Post | undefined;

  constructor(
    private likesService: LikesService,
    private postsService: PostsService
  ) {}

  toggleLike(post: Post) {
    if (post.isLikedByCurrentUser) {
      console.log('unlike');
      this.likesService.unlikePost(post.postId).subscribe(() => {
        post.isLikedByCurrentUser = false;
        post.likesCount -= 1;
      });
    } else {
      console.log('like');
      this.likesService.likePost(post.postId).subscribe(() => {
        post.isLikedByCurrentUser = true;
        post.likesCount += 1;
      });
    }
  }

  deletePost(postId: number) {
    this.postsService.deletePost(postId).subscribe(() => {
      this.post = undefined;
    });
  }
}
