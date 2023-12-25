import { Component, OnDestroy, OnInit } from '@angular/core';
import { Pagination } from '../../shared/models/pagination';
import { Message } from '../../shared/models/message';
import { MessagesService } from '../profile/services/message.service';
import { messageButtons } from './constants/message-buttons';
import { MessageParams } from './models/messageParams';
import { OnlineUsersListComponent } from './components/online-users-list/online-users-list.component';
import { NgxPaginationModule } from 'ngx-pagination';
import { MessageDisplayComponent } from './components/message-display/message-display.component';
import { CommonModule, NgFor } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { NearestBdUsersListComponent } from './components/nearest-bd-users-list/nearest-bd-users-list.component';
import { Subject, first, takeUntil } from 'rxjs';
import { MessageDisplayService } from './services/message-display.service';
import { LocalStorageService } from 'src/app/core/services/local-storage.service';
import { SpinnerComponent } from '../../shared/components/spinner/spinner.component';

@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.scss'],
  standalone: true,
  imports: [
    CommonModule,
    NearestBdUsersListComponent,
    MatButtonToggleModule,
    FormsModule,
    NgFor,
    MessageDisplayComponent,
    NgxPaginationModule,
    OnlineUsersListComponent,
    SpinnerComponent,
  ],
})
export class MessagesComponent implements OnInit, OnDestroy {
  messages: Message[] = [];
  pagination: Pagination | undefined;
  container: string = 'Unread';
  lastLoadedContainer: string | null = null;
  pageNumber: number = 1;
  pageSize: number = 5;
  buttons = messageButtons;
  isLoading: boolean = false;
  loadMessagesError: boolean = false;
  changingPage: boolean = false;
  messageParams: MessageParams | undefined;
  messageCountController: number = 0;
  private destroy$ = new Subject<void>();

  constructor(
    private messageDisplayService: MessageDisplayService,
    private localStorageService: LocalStorageService
  ) {}

  ngOnInit(): void {
    this.container = this.localStorageService.getItem('Container') || 'Unread';

    this.loadMessages();
  }

  loadMessages(): void {
    if (
      this.isLoading ||
      (this.lastLoadedContainer === this.container && !this.changingPage)
    ) {
      return;
    }
    this.lastLoadedContainer = this.container;
    this.localStorageService.setItem('Container', this.container);
    this.messageParams = {
      pageNumber: this.pageNumber,
      pageSize: this.pageSize,
      container: this.container,
    };

    this.isLoading = true;
    this.loadMessagesError = false;
    this.messages = [];

    this.messageDisplayService
      .getUserMessages(this.messageParams)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (response) => {
          if (response.result && response.pagination) {
            this.messageCountController = 0;
            this.messages = response.result;
            this.pagination = response.pagination;
            this.isLoading = false;
            this.changingPage = false;
          }
        },
        error: () => {
          this.isLoading = false;
          this.loadMessagesError = true;
          this.messages = [];
          this.pagination = undefined;
          this.changingPage = false;
        },
      });
  }

  pageChanged(event: any): void {
    this.pageNumber = event;
    this.changingPage = true;
    this.loadMessages();
  }

  onMessageDelete(id: number): void {
    this.messages?.splice(
      this.messages.findIndex((m) => m.privateMessageId === id),
      1
    );

    this.messageCountController++;
    if (this.messages.length == 0) {
      this.changingPage = true;
      this.pageNumber++;
      this.loadMessages();
    }
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
