import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FormsModule } from '@angular/forms';
import { ToastrModule } from 'ngx-toastr';

import { AppComponent } from './app.component';
import { HeaderComponent } from './header/header.component';
import { HeaderLogoComponent } from './header/header-logo/header-logo.component';
import { HeaderNavLinksComponent } from './header/header-nav-links/header-nav-links.component';
import { HomeComponent } from './home/home.component';
import { PortalComponent } from './portal/portal.component';
import { DiscoverComponent } from './discover/discover.component';
import { GamesComponent } from './games/games.component';
import { FavoritesComponent } from './favorites/favorites.component';
import { HollowXHollowComponent } from './games/hollow-x-hollow/hollow-x-hollow.component';
import { PlaysketchPortableComponent } from './games/playsketch-portable/playsketch-portable.component';
import { RockPaperScissorsComponent } from './games/rock-paper-scissors/rock-paper-scissors.component';
import { GameSelectionComponent } from './games/game-selection/game-selection.component';
import { AuthFormComponent } from './portal/auth-form/auth-form.component';
import { LoginComponent } from './portal/auth-form/login/login.component';
import { RegisterComponent } from './portal/auth-form/register/register.component';
import { LoginLogoComponent } from './portal/login-logo/login-logo.component';
import { HeaderDropdownComponent } from './header/header-dropdown/header-dropdown.component';
import { ErrorInterceptor } from './_interceptors/error.interceptor';
import { AuthInterceptor } from './_interceptors/auth.interceptor';
import { NotFoundComponent } from './errors/not-found/not-found.component';
import { UserCardComponent } from './discover/user-card/user-card.component';
import { ProfileComponent } from './profile/profile.component';
import { PostsComponent } from './profile/posts/posts.component';
import { GalleryComponent } from './profile/gallery/gallery.component';
import { MessageComponent } from './profile/message/message.component';
import { EditComponent } from './profile/edit/edit.component';

@NgModule({
  declarations: [
    AppComponent,
    HeaderComponent,
    HeaderLogoComponent,
    HeaderNavLinksComponent,
    HomeComponent,
    DiscoverComponent,
    GamesComponent,
    FavoritesComponent,
    HollowXHollowComponent,
    PlaysketchPortableComponent,
    RockPaperScissorsComponent,
    GameSelectionComponent,
    PortalComponent,
    AuthFormComponent,
    LoginComponent,
    RegisterComponent,
    LoginLogoComponent,
    HeaderDropdownComponent,
    NotFoundComponent,
    UserCardComponent,
    ProfileComponent,
    PostsComponent,
    MessageComponent,
    EditComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    BrowserAnimationsModule,
    FormsModule,
    ToastrModule.forRoot({
      timeOut: 3000,
      positionClass: 'toast-bottom-right',
      preventDuplicates: true,
      resetTimeoutOnDuplicate: true,
      progressBar: true,
    }),
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true },
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
