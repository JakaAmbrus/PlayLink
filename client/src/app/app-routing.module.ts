import { NgModule } from '@angular/core';
import { PreloadAllModules, RouterModule, Routes } from '@angular/router';

import { PortalComponent } from './features/portal/portal.component';
import { NotFoundComponent } from './core/components/not-found/not-found.component';

import { canActivateGuard } from './core/guards/auth.guard';
import { currentUserRestrictionGuard } from './features/profile/guards/current-user-restriction.guard';
import { notCurrentUserRestrictionGuard } from './features/profile/guards/not-current-user-restriction.guard';
import { canActivateLoginGuard } from './core/guards/auth.guard';
import { preventUnsavedChangesGuard } from './features/profile/guards/prevent-unsaved-changes.guard';
import { adminGuard } from './features/admin/guards/admin.guard';

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
    data: { animation: 'Home' },
    canActivate: [canActivateGuard],
    loadComponent: () =>
      import('./features/home/home.component').then((m) => m.HomeComponent),
  },
  {
    path: 'discover',
    data: { animation: 'Discover' },
    canActivate: [canActivateGuard],
    loadComponent: () =>
      import('./features/discover/discover.component').then(
        (m) => m.DiscoverComponent
      ),
  },
  {
    path: 'user/:username',
    canActivate: [canActivateGuard],
    loadComponent: () =>
      import('./features/profile/profile.component').then(
        (m) => m.ProfileComponent
      ),
    children: [
      { path: '', redirectTo: 'posts', pathMatch: 'full' },
      {
        path: 'posts',
        data: { animation: 'Posts' },
        loadComponent: () =>
          import('./features/profile/pages/posts/posts.component').then(
            (m) => m.PostsComponent
          ),
      },
      {
        path: 'gallery',
        data: { animation: 'Gallery' },
        loadComponent: () =>
          import('./features/profile/pages/gallery/gallery.component').then(
            (m) => m.GalleryComponent
          ),
      },
      {
        path: 'edit',
        data: { animation: 'Edit' },
        canDeactivate: [preventUnsavedChangesGuard],
        canActivate: [notCurrentUserRestrictionGuard],
        loadComponent: () =>
          import('./features/profile/pages/edit/edit.component').then(
            (m) => m.EditComponent
          ),
      },
      {
        path: 'message',
        data: { animation: 'Message' },
        canActivate: [currentUserRestrictionGuard],
        loadComponent: () =>
          import('./features/profile/pages/message/message.component').then(
            (m) => m.MessageComponent
          ),
      },
    ],
  },
  {
    path: 'messages',
    data: { animation: 'Messages' },
    canActivate: [canActivateGuard],
    loadComponent: () =>
      import('./features/messages/messages.component').then(
        (m) => m.MessagesComponent
      ),
  },
  {
    path: 'games',
    loadComponent: () =>
      import('./features/games/games.component').then((m) => m.GamesComponent),
    data: { animation: 'Games' },
    canActivate: [canActivateGuard],
    children: [
      {
        path: '',
        loadComponent: () =>
          import(
            './features/games/pages/game-selection/game-selection.component'
          ).then((m) => m.GameSelectionComponent),
      },
      {
        path: 'hollow-x-hollow',
        loadComponent: () =>
          import(
            './features/games/pages/hollow-x-hollow/hollow-x-hollow.component'
          ).then((m) => m.HollowXHollowComponent),
      },
      {
        path: 'playsketch-portable',
        loadComponent: () =>
          import(
            './features/games/pages/playsketch-portable/playsketch-portable.component'
          ).then((m) => m.PlaysketchPortableComponent),
      },
      {
        path: 'rock-paper-scissors',
        loadComponent: () =>
          import(
            './features/games/pages/rock-paper-scissors/rock-paper-scissors.component'
          ).then((m) => m.RockPaperScissorsComponent),
      },
    ],
  },
  {
    path: 'admin',
    canActivate: [canActivateGuard, adminGuard],
    loadComponent: () =>
      import('./features/admin/admin.component').then((m) => m.AdminComponent),
  },
  {
    path: 'about-site',
    canActivate: [canActivateGuard],
    loadComponent: () =>
      import('./core/components/about-site/about-site.component').then(
        (m) => m.AboutSiteComponent
      ),
  },
  {
    path: 'not-found',
    loadComponent: () =>
      import('./core/components/not-found/not-found.component').then(
        (m) => m.NotFoundComponent
      ),
  },
  { path: '**', redirectTo: '/not-found' },
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes, { preloadingStrategy: PreloadAllModules }),
  ],
  exports: [RouterModule],
})
export class AppRoutingModule {}
