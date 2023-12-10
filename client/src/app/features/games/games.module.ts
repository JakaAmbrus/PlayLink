import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { GamesComponent } from './games.component';
import { GameSelectionComponent } from './pages/game-selection/game-selection.component';
import { HollowXHollowComponent } from './pages/hollow-x-hollow/hollow-x-hollow.component';
import { PlaysketchPortableComponent } from './pages/playsketch-portable/playsketch-portable.component';
import { RockPaperScissorsComponent } from './pages/rock-paper-scissors/rock-paper-scissors.component';
import { QuizWidgetComponent } from './pages/game-selection/quiz-widget/quiz-widget.component';
import { GamesRoutingModule } from './games-routing.module';

@NgModule({
  declarations: [
    GamesComponent,
    GameSelectionComponent,
    PlaysketchPortableComponent,
    RockPaperScissorsComponent,
    HollowXHollowComponent,
    QuizWidgetComponent,
  ],
  imports: [CommonModule, GamesRoutingModule],
})
export class GamesModule {}
