import { CoreModule } from './core/core.module';

import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
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
import { ProfileNavigationComponent } from './features/profile/components/profile-navigation/profile-navigation.component';
import { TimeagoModule } from 'ngx-timeago';

import { AppComponent } from './app.component';

import { HomeComponent } from './features/home/home.component';
import { DiscoverComponent } from './features/discover/discover.component';
import { MessagesComponent } from './features/messages/messages.component';

import { NotFoundComponent } from './core/components/not-found/not-found.component';
import { UserCardComponent } from './features/discover/components/user-card/user-card.component';
import { ProfileComponent } from './features/profile/profile.component';
import { PostsComponent } from './features/profile/pages/posts/posts.component';
import { MessageComponent } from './features/profile/pages/message/message.component';
import { EditComponent } from './features/profile/pages/edit/edit.component';
import { PostComponent } from './shared/components/post/post.component';
import { CommentComponent } from './shared/components/comment/comment.component';
import { RelativeTimePipe } from './shared/pipes/relative-time.pipe';
import { PostSkeletonComponent } from './shared/components/post-skeleton/post-skeleton.component';
import { UploadPostComponent } from './shared/components/upload-post/upload-post.component';
import { UploadCommentComponent } from './shared/components/upload-comment/upload-comment.component';
import { TimeAgoPipe } from './shared/pipes/time-ago.pipe';
import { MessageDisplayComponent } from './features/messages/components/message-display/message-display.component';
import { LimitTextPipe } from './shared/pipes/limit-text.pipe';
import { FirstWordPipe } from './shared/pipes/first-word.pipe';
import { MessageContentComponent } from './features/profile/pages/message/message-content/message-content.component';
import { RelativeUrlPipe } from './shared/pipes/relative-url.pipe';
import { ObjectToArrayPipe } from './shared/pipes/object-to-array.pipe';

import { FriendDisplayComponent } from './features/home/friend-list/friend-display/friend-display.component';
import { ProfileUserCardComponent } from './features/profile/components/profile-user-card/profile-user-card.component';
import { FriendListComponent } from './features/home/friend-list/friend-list.component';
import { HomeUserCardComponent } from './features/home/home-user-card/home-user-card.component';
import { SearchbarComponent } from './features/discover/components/searchbar/searchbar.component';
import { OnlineUsersListComponent } from './features/messages/components/online-users-list/online-users-list.component';
import { OnlineUserDisplayComponent } from './features/messages/components/online-users-list/online-user-display/online-user-display.component';
import { NearestBdUsersListComponent } from './features/messages/components/nearest-bd-users-list/nearest-bd-users-list.component';
import { NearestBdUserDisplayComponent } from './features/messages/components/nearest-bd-users-list/nearest-bd-user-display/nearest-bd-user-display.component';
import { NgOptimizedImage } from '@angular/common';
import { SharedModule } from './shared/shared.module';
import { PortalModule } from './features/portal/portal.module';
import { UserAvatarComponent } from './shared/components/user-avatar/user-avatar.component';
import { HeaderComponent } from './core/components/header/header.component';
import { HeaderLogoComponent } from './core/components/header/components/header-logo/header-logo.component';
import { HeaderNavLinksComponent } from './core/components/header/components/header-nav-links/header-nav-links.component';
import { HeaderDropdownComponent } from './core/components/header/components/header-dropdown/header-dropdown.component';
import { HeaderNotificationsComponent } from './core/components/header/components/header-notifications/header-notifications.component';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    HeaderComponent,
    HeaderLogoComponent,
    HeaderNavLinksComponent,
    HeaderDropdownComponent,
    HeaderNotificationsComponent,
    DiscoverComponent,
    MessagesComponent,
    NotFoundComponent,
    UserCardComponent,
    ProfileComponent,
    PostsComponent,
    MessageComponent,
    EditComponent,
    PostComponent,
    CommentComponent,
    PostSkeletonComponent,
    UploadPostComponent,
    UploadCommentComponent,
    TimeAgoPipe,
    MessageDisplayComponent,
    LimitTextPipe,
    FirstWordPipe,
    MessageContentComponent,
    ProfileNavigationComponent,
    ProfileUserCardComponent,
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
    RelativeUrlPipe,
    RelativeTimePipe,
    PortalModule,
    BrowserModule,
    CoreModule,
    AppRoutingModule,
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
    SharedModule,
    RelativeUrlPipe,
    TimeagoModule.forRoot(),
    ToastrModule.forRoot({
      timeOut: 4000,
      positionClass: 'toast-bottom-right',
      preventDuplicates: true,
      resetTimeoutOnDuplicate: true,
      progressBar: true,
    }),
    ObjectToArrayPipe,
    UserAvatarComponent,
  ],
})
export class AppModule {}
