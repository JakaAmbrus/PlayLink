import { Component, OnInit } from '@angular/core';
import { Pagination } from '../../shared/models/pagination';
import { Message } from '../../shared/models/messages';
import { MessagesService } from '../../shared/services/messages.service';
import { messageButtons } from './message-buttons';
import { MessageParams } from '../../shared/models/messageParams';

@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.scss'],
})
export class MessagesComponent implements OnInit {
  messages: Message[] = [];
  pagination: Pagination | undefined;
  container: string = 'Unread';
  pageNumber: number = 1;
  pageSize: number = 5;
  buttons = messageButtons;
  isLoading: boolean = false;
  messageParams: MessageParams | undefined;
  messageCountController: number = 0;

  constructor(private messagesService: MessagesService) {}

  ngOnInit(): void {
    this.container = localStorage.getItem('Container') || 'Unread';

    this.loadMessages();
  }

  loadMessages(): void {
    localStorage.setItem('Container', this.container);
    this.messageParams = {
      pageNumber: this.pageNumber,
      pageSize: this.pageSize,
      container: this.container,
    };
    this.isLoading = true;
    this.messagesService.getUserMessages(this.messageParams).subscribe({
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
      },
    });
  }

  pageChanged(event: any): void {
    this.pageNumber = event;
    this.loadMessages();
  }

  onMessageDelete(id: number): void {
    this.messagesService.deleteMessage(id).subscribe({
      next: () => {
        this.messages?.splice(
          this.messages.findIndex((m) => m.privateMessageId === id),
          1
        );
        this.messageCountController++;
      },
    });
  }
}
