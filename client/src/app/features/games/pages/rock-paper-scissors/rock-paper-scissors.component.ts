import { Component, OnInit, OnDestroy } from '@angular/core';
import { NgIf } from '@angular/common';

@Component({
    selector: 'app-rock-paper-scissors',
    templateUrl: './rock-paper-scissors.component.html',
    styleUrls: ['./rock-paper-scissors.component.scss'],
    standalone: true,
    imports: [NgIf],
})
export class RockPaperScissorsComponent implements OnInit, OnDestroy {
  showIframe = false;

  ngOnInit(): void {
    this.showIframe = true;
  }

  ngOnDestroy(): void {
    this.showIframe = false;
  }
}
