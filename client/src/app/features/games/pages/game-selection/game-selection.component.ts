import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';
import { QuizWidgetComponent } from './quiz-widget/quiz-widget.component';

@Component({
    selector: 'app-game-selection',
    templateUrl: './game-selection.component.html',
    styleUrls: ['./game-selection.component.scss'],
    standalone: true,
    imports: [QuizWidgetComponent, RouterLink],
})
export class GameSelectionComponent {}
