import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { HomeComponent } from './home/home.component';
import { DiscoverComponent } from './discover/discover.component';
import { GamesComponent } from './games/games.component';
import { FavoritesComponent } from './favorites/favorites.component';

const routes: Routes = [
  { path: 'home', component: HomeComponent },
  { path: 'discover', component: DiscoverComponent },
  { path: 'games', component: GamesComponent },
  { path: 'favorites', component: FavoritesComponent },
  { path: '', redirectTo: '/home', pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
