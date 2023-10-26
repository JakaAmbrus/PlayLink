import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HeaderComponent } from './header/header.component';
import { HeaderLogoComponent } from './header/header-logo/header-logo.component';
import { HeaderNavLinksComponent } from './header/header-nav-links/header-nav-links.component';

@NgModule({
  declarations: [AppComponent, HeaderComponent, HeaderLogoComponent, HeaderNavLinksComponent],
  imports: [BrowserModule, AppRoutingModule, HttpClientModule],
  providers: [],
  bootstrap: [AppComponent],
})
export class AppModule {}
