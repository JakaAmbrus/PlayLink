import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ToastrModule } from 'ngx-toastr';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatNativeDateModule } from '@angular/material/core';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { NgxDropzoneModule } from 'ngx-dropzone';
import { NgxPaginationModule } from 'ngx-pagination';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { InfiniteScrollModule } from 'ngx-infinite-scroll';
import { NgOptimizedImage, provideCloudinaryLoader } from '@angular/common';
import { ProfileNavigationComponent } from './profile/components/profile-navigation/profile-navigation.component';
import { TimeagoModule } from 'ngx-timeago';

import { AppComponent } from './app.component';
import { HeaderComponent } from './header/header.component';
import { HeaderLogoComponent } from './header/header-logo/header-logo.component';
import { HeaderNavLinksComponent } from './header/header-nav-links/header-nav-links.component';
import { HomeComponent } from './home/home.component';
import { PortalComponent } from './portal/portal.component';
import { DiscoverComponent } from './discover/discover.component';
import { GamesComponent } from './games/games.component';
import { MessagesComponent } from './messages/messages.component';
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
import { MessageComponent } from './profile/message/message.component';
import { EditComponent } from './profile/edit/edit.component';
import { PostComponent } from './_components/post/post.component';
import { CommentComponent } from './_components/comment/comment.component';
import { RelativeTimePipe } from './_pipes/relative-time.pipe';
import { PostSkeletonComponent } from './_components/post-skeleton/post-skeleton.component';
import { UploadPostComponent } from './_components/upload-post/upload-post.component';
import { QuizWidgetComponent } from './games/game-selection/quiz-widget/quiz-widget.component';
import { UploadCommentComponent } from './_components/upload-comment/upload-comment.component';
import { TimeAgoPipe } from './_pipes/time-ago.pipe';
import { MessageDisplayComponent } from './messages/message-display/message-display.component';
import { LimitTextPipe } from './_pipes/limit-text.pipe';
import { FirstWordPipe } from './_pipes/first-word.pipe';
import { MessageContentComponent } from './profile/message/message-content/message-content.component';
import { RelativeUrlPipe } from './_pipes/relative-url.pipe';
import { UserAvatarComponent } from './_components/user-avatar/user-avatar.component';
import { ObjectToArrayPipe } from './_pipes/object-to-array.pipe';
import { HeaderNotificationsComponent } from './header/header-notifications/header-notifications.component';
import { FriendDisplayComponent } from './home/friend-list/friend-display/friend-display.component';
import { AdminComponent } from './admin/admin.component';
import { AdminUserDisplayComponent } from './admin/admin-user-display/admin-user-display.component';
import { ProfileUserCardComponent } from './profile/components/profile-user-card/profile-user-card.component';
import { FriendListComponent } from './home/friend-list/friend-list.component';
import { HomeUserCardComponent } from './home/home-user-card/home-user-card.component';
import { SearchbarComponent } from './discover/searchbar/searchbar.component';
import { OnlineUsersListComponent } from './messages/online-users-list/online-users-list.component';
import { OnlineUserDisplayComponent } from './messages/online-users-list/online-user-display/online-user-display.component';
import { NearestBdUsersListComponent } from './messages/nearest-bd-users-list/nearest-bd-users-list.component';
import { NearestBdUserDisplayComponent } from './messages/nearest-bd-users-list/nearest-bd-user-display/nearest-bd-user-display.component';

@NgModule({
  declarations: [
    AppComponent,
    AdminComponent,
    AdminUserDisplayComponent,
    HeaderComponent,
    HeaderLogoComponent,
    HeaderNavLinksComponent,
    HomeComponent,
    DiscoverComponent,
    GamesComponent,
    MessagesComponent,
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
    PostComponent,
    CommentComponent,
    RelativeTimePipe,
    PostSkeletonComponent,
    UploadPostComponent,
    QuizWidgetComponent,
    UploadCommentComponent,
    TimeAgoPipe,
    MessageDisplayComponent,
    LimitTextPipe,
    FirstWordPipe,
    MessageContentComponent,
    UserAvatarComponent,
    ProfileNavigationComponent,
    ProfileUserCardComponent,
    HeaderNotificationsComponent,
    FriendListComponent,
    FriendDisplayComponent,
    HomeUserCardComponent,
    SearchbarComponent,
    OnlineUsersListComponent,
    OnlineUserDisplayComponent,
    NearestBdUsersListComponent,
    NearestBdUserDisplayComponent,
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true },
    provideCloudinaryLoader('https://res.cloudinary.com/dsdleukb7'),
  ],
  bootstrap: [AppComponent],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    BrowserAnimationsModule,
    FormsModule,
    ReactiveFormsModule,
    MatDatepickerModule,
    MatInputModule,
    MatFormFieldModule,
    MatNativeDateModule,
    MatAutocompleteModule,
    NgxDropzoneModule,
    NgxPaginationModule,
    MatButtonToggleModule,
    InfiniteScrollModule,
    NgOptimizedImage,
    TimeagoModule.forRoot(),
    ToastrModule.forRoot({
      timeOut: 4000,
      positionClass: 'toast-bottom-right',
      preventDuplicates: true,
      resetTimeoutOnDuplicate: true,
      progressBar: true,
    }),
    RelativeUrlPipe,
    ObjectToArrayPipe,
  ],
})
export class AppModule {}
