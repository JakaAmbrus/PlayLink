import { CoreModule } from './core/core.module';

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
import { NgOptimizedImage } from '@angular/common';
import { ProfileNavigationComponent } from './features/profile/components/profile-navigation/profile-navigation.component';
import { TimeagoModule } from 'ngx-timeago';

import { AppComponent } from './app.component';
import { HeaderComponent } from './core/components/header/header.component';
import { HeaderLogoComponent } from './core/components/header/components/header-logo/header-logo.component';
import { HeaderNavLinksComponent } from './core/components/header/components/header-nav-links/header-nav-links.component';
import { HomeComponent } from './features/home/home.component';
import { PortalComponent } from './features/portal/portal.component';
import { DiscoverComponent } from './features/discover/discover.component';
import { GamesComponent } from './features/games/games.component';
import { MessagesComponent } from './features/messages/messages.component';
import { HollowXHollowComponent } from './features/games/pages/hollow-x-hollow/hollow-x-hollow.component';
import { PlaysketchPortableComponent } from './features/games/pages/playsketch-portable/playsketch-portable.component';
import { RockPaperScissorsComponent } from './features/games/pages/rock-paper-scissors/rock-paper-scissors.component';
import { GameSelectionComponent } from './features/games/pages/game-selection/game-selection.component';
import { AuthFormComponent } from './features/portal/components/auth-form/auth-form.component';
import { LoginComponent } from './features/portal/components/auth-form/login/login.component';
import { RegisterComponent } from './features/portal/components/auth-form/register/register.component';
import { LoginLogoComponent } from './features/portal/components/login-logo/login-logo.component';
import { HeaderDropdownComponent } from './core/components/header/components/header-dropdown/header-dropdown.component';
import { NotFoundComponent } from './core/components/not-found/not-found.component';
import { UserCardComponent } from './features/discover/components/user-card/user-card.component';
import { ProfileComponent } from './features/profile/profile.component';
import { PostsComponent } from './features/profile/components/posts/posts.component';
import { MessageComponent } from './features/profile/components/message/message.component';
import { EditComponent } from './features/profile/components/edit/edit.component';
import { PostComponent } from './shared/components/post/post.component';
import { CommentComponent } from './shared/components/comment/comment.component';
import { RelativeTimePipe } from './shared/pipes/relative-time.pipe';
import { PostSkeletonComponent } from './shared/components/post-skeleton/post-skeleton.component';
import { UploadPostComponent } from './shared/components/upload-post/upload-post.component';
import { QuizWidgetComponent } from './features/games/game-selection/quiz-widget/quiz-widget.component';
import { UploadCommentComponent } from './shared/components/upload-comment/upload-comment.component';
import { TimeAgoPipe } from './shared/pipes/time-ago.pipe';
import { MessageDisplayComponent } from './features/messages/components/message-display/message-display.component';
import { LimitTextPipe } from './shared/pipes/limit-text.pipe';
import { FirstWordPipe } from './shared/pipes/first-word.pipe';
import { MessageContentComponent } from './features/profile/components/message/message-content/message-content.component';
import { RelativeUrlPipe } from './shared/pipes/relative-url.pipe';
import { UserAvatarComponent } from './shared/components/user-avatar/user-avatar.component';
import { ObjectToArrayPipe } from './shared/pipes/object-to-array.pipe';
import { HeaderNotificationsComponent } from './core/components/header/components/header-notifications/header-notifications.component';
import { FriendDisplayComponent } from './features/home/friend-list/friend-display/friend-display.component';
import { AdminComponent } from './features/admin/admin.component';
import { AdminUserDisplayComponent } from './features/admin/components/admin-user-display/admin-user-display.component';
import { ProfileUserCardComponent } from './features/profile/components/profile-user-card/profile-user-card.component';
import { FriendListComponent } from './features/home/friend-list/friend-list.component';
import { HomeUserCardComponent } from './features/home/home-user-card/home-user-card.component';
import { SearchbarComponent } from './features/discover/components/searchbar/searchbar.component';
import { OnlineUsersListComponent } from './features/messages/components/online-users-list/online-users-list.component';
import { OnlineUserDisplayComponent } from './features/messages/components/online-users-list/online-user-display/online-user-display.component';
import { NearestBdUsersListComponent } from './features/messages/components/nearest-bd-users-list/nearest-bd-users-list.component';
import { NearestBdUserDisplayComponent } from './features/messages/components/nearest-bd-users-list/nearest-bd-user-display/nearest-bd-user-display.component';

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
  bootstrap: [AppComponent],
  imports: [
    BrowserModule,
    CoreModule,
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
