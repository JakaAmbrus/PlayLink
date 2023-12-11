import { Component, OnInit, OnDestroy } from '@angular/core';
import { NgIf } from '@angular/common';

@Component({
    selector: 'app-playsketch-portable',
    templateUrl: './playsketch-portable.component.html',
    styleUrls: ['./playsketch-portable.component.scss'],
    standalone: true,
    imports: [NgIf],
})
export class PlaysketchPortableComponent implements OnInit, OnDestroy {
  showIframe = false;

  ngOnInit(): void {
    this.showIframe = true;
  }

  ngOnDestroy(): void {
    this.showIframe = false;
  }
}
