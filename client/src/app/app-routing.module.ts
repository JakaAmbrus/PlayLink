import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { HomeComponent } from './home/home.component';
import { DiscoverComponent } from './discover/discover.component';
import { GamesComponent } from './games/games.component';
import { FavoritesComponent } from './favorites/favorites.component';
import { LoginPageComponent } from './login-page/login-page.component';
import { GameSelectionComponent } from './games/game-selection/game-selection.component';
import { HollowXHollowComponent } from './games/hollow-x-hollow/hollow-x-hollow.component';
import { PlaysketchPortableComponent } from './games/playsketch-portable/playsketch-portable.component';
import { RockPaperScissorsComponent } from './games/rock-paper-scissors/rock-paper-scissors.component';

const routes: Routes = [
  { path: 'login', component: LoginPageComponent },
  { path: 'home', component: HomeComponent, data: { animation: 'Home' } },
  {
    path: 'discover',
    component: DiscoverComponent,
    data: { animation: 'Discover' },
  },
  {
    path: 'games',
    component: GamesComponent,
    data: { animation: 'Games' },
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
  {
    path: 'favorites',
    component: FavoritesComponent,
    data: { animation: 'Favorites' },
  },
  { path: '', redirectTo: '/login', pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
