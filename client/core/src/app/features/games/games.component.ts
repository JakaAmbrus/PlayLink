import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';

@Component({
    selector: 'app-games',
    templateUrl: './games.component.html',
    styleUrls: ['./games.component.scss'],
    standalone: true,
    imports: [RouterOutlet],
})
export class GamesComponent {}
