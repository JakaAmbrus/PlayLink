import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { HomeComponent } from './home/home.component';
import { DiscoverComponent } from './discover/discover.component';
import { GamesComponent } from './games/games.component';
import { FavoritesComponent } from './favorites/favorites.component';
import { PortalComponent } from './portal/portal.component';
import { GameSelectionComponent } from './games/game-selection/game-selection.component';
import { HollowXHollowComponent } from './games/hollow-x-hollow/hollow-x-hollow.component';
import { PlaysketchPortableComponent } from './games/playsketch-portable/playsketch-portable.component';
import { RockPaperScissorsComponent } from './games/rock-paper-scissors/rock-paper-scissors.component';

import { canActivateGuard } from './_guards/auth.guard';
import { canActivateLoginGuard } from './_guards/auth.guard';

const routes: Routes = [
  {
    path: 'portal',
    component: PortalComponent,
    canActivate: [canActivateLoginGuard],
  },
  {
    path: 'home',
    component: HomeComponent,
    data: { animation: 'Home' },
    canActivate: [canActivateGuard],
  },
  {
    path: 'discover',
    component: DiscoverComponent,
    data: { animation: 'Discover' },
    canActivate: [canActivateGuard],
  },
  {
    path: 'games',
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
  {
    path: 'favorites',
    component: FavoritesComponent,
    data: { animation: 'Favorites' },
    canActivate: [canActivateGuard],
  },
  {
    path: '',
    redirectTo: '/portal',
    pathMatch: 'full',
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
