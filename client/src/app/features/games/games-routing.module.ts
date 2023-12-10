import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { GamesComponent } from './games.component';
import { GameSelectionComponent } from './pages/game-selection/game-selection.component';
import { HollowXHollowComponent } from './pages/hollow-x-hollow/hollow-x-hollow.component';
import { PlaysketchPortableComponent } from './pages/playsketch-portable/playsketch-portable.component';
import { RockPaperScissorsComponent } from './pages/rock-paper-scissors/rock-paper-scissors.component';
import { canActivateGuard } from 'src/app/core/guards/auth.guard';

const routes: Routes = [
  {
    path: '',
    component: GamesComponent,
    data: { animation: 'Games' },
    canActivate: [canActivateGuard],
    children: [
      {
        path: '',
        component: GameSelectionComponent,
      },
      {
        path: 'hollow-x-hollow',
        component: HollowXHollowComponent,
      },
      {
        path: 'playsketch-portable',
        component: PlaysketchPortableComponent,
      },
      {
        path: 'rock-paper-scissors',
        component: RockPaperScissorsComponent,
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class GamesRoutingModule {}
