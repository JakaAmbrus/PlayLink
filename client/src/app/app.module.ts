import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FormsModule } from '@angular/forms';

import { AppComponent } from './app.component';
import { HeaderComponent } from './header/header.component';
import { HeaderLogoComponent } from './header/header-logo/header-logo.component';
import { HeaderNavLinksComponent } from './header/header-nav-links/header-nav-links.component';
import { HomeComponent } from './home/home.component';
import { LoginPageComponent } from './login-page/login-page.component';
import { DiscoverComponent } from './discover/discover.component';
import { GamesComponent } from './games/games.component';
import { FavoritesComponent } from './favorites/favorites.component';
import { HollowXHollowComponent } from './games/hollow-x-hollow/hollow-x-hollow.component';
import { PlaysketchPortableComponent } from './games/playsketch-portable/playsketch-portable.component';
import { RockPaperScissorsComponent } from './games/rock-paper-scissors/rock-paper-scissors.component';
import { GameSelectionComponent } from './games/game-selection/game-selection.component';
import { AuthFormComponent } from './login-page/auth-form/auth-form.component';
import { LoginComponent } from './login-page/auth-form/login/login.component';
import { SignUpComponent } from './login-page/auth-form/sign-up/sign-up.component';

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
    LoginPageComponent,
    AuthFormComponent,
    LoginComponent,
    SignUpComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    BrowserAnimationsModule,
    FormsModule,
  ],
  providers: [],
  bootstrap: [AppComponent],
})
export class AppModule {}
