import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { HomeComponent } from './features/home/home.component';
import { DiscoverComponent } from './features/discover/discover.component';
import { MessagesComponent } from './features/messages/messages.component';
import { PortalComponent } from './features/portal/portal.component';
import { NotFoundComponent } from './core/components/not-found/not-found.component';
import { ProfileComponent } from './features/profile/profile.component';
import { PostsComponent } from './features/profile/pages/posts/posts.component';
import { GalleryComponent } from './features/profile/pages/gallery/gallery.component';
import { EditComponent } from './features/profile/pages/edit/edit.component';
import { MessageComponent } from './features/profile/pages/message/message.component';

import {
  canActivateCurrentUserGuard,
  canActivateGuard,
  canActivateNotCurrentUserGuard,
} from './core/guards/auth.guard';
import { canActivateLoginGuard } from './core/guards/auth.guard';
import { preventUnsavedChangesGuard } from './core/guards/prevent-unsaved-changes.guard';

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
    path: 'messages',
    component: MessagesComponent,
    data: { animation: 'Messages' },
    canActivate: [canActivateGuard],
  },
  {
    path: 'games',
    loadChildren: () =>
      import('./features/games/games.module').then((m) => m.GamesModule),
  },
  {
    path: 'admin',
    canActivate: [canActivateGuard],
    loadChildren: () =>
      import('./features/admin/admin.module').then((m) => m.AdminModule),
  },
  { path: 'not-found', component: NotFoundComponent },
  { path: '**', redirectTo: '/not-found' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
