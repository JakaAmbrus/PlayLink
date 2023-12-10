import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { HomeComponent } from './home/home.component';
import { DiscoverComponent } from './discover/discover.component';
import { GamesComponent } from './games/games.component';
import { MessagesComponent } from './messages/messages.component';
import { PortalComponent } from './portal/portal.component';
import { GameSelectionComponent } from './games/game-selection/game-selection.component';
import { HollowXHollowComponent } from './games/hollow-x-hollow/hollow-x-hollow.component';
import { PlaysketchPortableComponent } from './games/playsketch-portable/playsketch-portable.component';
import { RockPaperScissorsComponent } from './games/rock-paper-scissors/rock-paper-scissors.component';
import { NotFoundComponent } from './errors/not-found/not-found.component';
import { ProfileComponent } from './profile/profile.component';
import { PostsComponent } from './profile/posts/posts.component';
import { GalleryComponent } from './profile/gallery/gallery.component';
import { EditComponent } from './profile/edit/edit.component';
import { MessageComponent } from './profile/message/message.component';
import { AdminComponent } from './admin/admin.component';

import {
  canActivateCurrentUserGuard,
  canActivateGuard,
  canActivateNotCurrentUserGuard,
} from './core/guards/auth.guard';
import { canActivateLoginGuard } from './core/guards/auth.guard';
import { preventUnsavedChangesGuard } from './_guards/prevent-unsaved-changes.guard';
import { adminGuard } from './_guards/admin.guard';

const routes: Routes = [
  {
    path: '',
    redirectTo: '/portal',
    pathMatch: 'full',
  },
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
    path: 'user/:username',
    component: ProfileComponent,
    canActivate: [canActivateGuard],
    children: [
      { path: '', redirectTo: 'posts', pathMatch: 'full' },
      {
        path: 'posts',
        component: PostsComponent,
        data: { animation: 'Posts' },
      },
      {
        path: 'gallery',
        component: GalleryComponent,
        data: { animation: 'Gallery' },
      },
      {
        path: 'edit',
        component: EditComponent,
        data: { animation: 'Edit' },
        canDeactivate: [preventUnsavedChangesGuard],
        canActivate: [canActivateCurrentUserGuard],
      },
      {
        path: 'message',
        component: MessageComponent,
        data: { animation: 'Message' },
        canActivate: [canActivateNotCurrentUserGuard],
      },
    ],
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
    path: 'messages',
    component: MessagesComponent,
    data: { animation: 'Messages' },
    canActivate: [canActivateGuard],
  },
  {
    path: 'admin',
    component: AdminComponent,
    canActivate: [canActivateGuard, adminGuard],
  },
  { path: 'not-found', component: NotFoundComponent },
  { path: '**', redirectTo: '/not-found' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
