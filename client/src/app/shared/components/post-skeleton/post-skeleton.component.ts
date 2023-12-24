import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-post-skeleton',
  templateUrl: './post-skeleton.component.html',
  styleUrls: ['./post-skeleton.component.scss'],
  standalone: true,
})
export class PostSkeletonComponent {
  @Input() loadingError: boolean = false;
}
