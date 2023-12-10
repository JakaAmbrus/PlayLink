import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { HomeComponent } from './features/home/home.component';
import { DiscoverComponent } from './features/discover/discover.component';
import { GamesComponent } from './features/games/games.component';
import { MessagesComponent } from './features/messages/messages.component';
import { PortalComponent } from './features/portal/portal.component';
import { GameSelectionComponent } from './features/games/pages/game-selection/game-selection.component';
import { HollowXHollowComponent } from './features/games/pages/hollow-x-hollow/hollow-x-hollow.component';
import { PlaysketchPortableComponent } from './features/games/pages/playsketch-portable/playsketch-portable.component';
import { RockPaperScissorsComponent } from './features/games/pages/rock-paper-scissors/rock-paper-scissors.component';
import { NotFoundComponent } from './core/components/not-found/not-found.component';
import { ProfileComponent } from './features/profile/profile.component';
import { PostsComponent } from './features/profile/components/posts/posts.component';
import { GalleryComponent } from './features/profile/components/gallery/gallery.component';
import { EditComponent } from './features/profile/components/edit/edit.component';
import { MessageComponent } from './features/profile/components/message/message.component';
import { AdminComponent } from './features/admin/admin.component';

import {
  canActivateCurrentUserGuard,
  canActivateGuard,
  canActivateNotCurrentUserGuard,
} from './core/guards/auth.guard';
import { canActivateLoginGuard } from './core/guards/auth.guard';
import { preventUnsavedChangesGuard } from './core/guards/prevent-unsaved-changes.guard';
import { adminGuard } from './core/guards/admin.guard';

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
