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
  ],
})
export class MessagesComponent implements OnInit, OnDestroy {
  messages: Message[] = [];
  pagination: Pagination | undefined;
  container: string = 'Unread';
  pageNumber: number = 1;
  pageSize: number = 5;
  buttons = messageButtons;
  isLoading: boolean = true;
  loadMessagesError: boolean = false;
  messageParams: MessageParams | undefined;
  messageCountController: number = 0;
  private destroy$ = new Subject<void>();

  constructor(
    private messageDisplayService: MessageDisplayService,
    private localStorageService: LocalStorageService
  ) {}

  ngOnInit(): void {
    this.container = this.localStorageService.getItem('Container') || 'Unread';

    this.loadMessages(this.container, true);
  }

  loadMessages(selectedOption: string, initialLoad = false): void {
    if (this.container === selectedOption && !initialLoad) {
      // If the selected option is the same as the current one, do nothing
      return;
    }
    this.container = selectedOption;

    this.localStorageService.setItem('Container', this.container);
    this.messageParams = {
      pageNumber: this.pageNumber,
      pageSize: this.pageSize,
      container: this.container,
    };

    this.isLoading = true;
    this.loadMessagesError = false;

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
          }
        },
        error: () => {
          this.isLoading = false;
          this.loadMessagesError = true;
        },
      });
  }

  pageChanged(event: any): void {
    this.pageNumber = event;
    this.loadMessages(this.container);
  }

  onMessageDelete(id: number): void {
    this.messageDisplayService
      .deleteMessage(id)
      .pipe(first())
      .subscribe({
        next: () => {
          this.messages?.splice(
            this.messages.findIndex((m) => m.privateMessageId === id),
            1
          );
          this.messageCountController++;
        },
      });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
