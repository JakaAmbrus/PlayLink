import { Component, OnInit } from '@angular/core';
import { Message } from 'src/app/_models/messages';
import { Pagination } from 'src/app/_models/pagination';
import { MessagesService } from 'src/app/_services/messages.service';

@Component({
  selector: 'app-message',
  templateUrl: './message.component.html',
  styleUrls: ['./message.component.scss'],
})
export class MessageComponent implements OnInit {
  messages: Message[] | undefined;
  pagination: Pagination | undefined;
  container: string = 'Unread';
  pageNumber: number = 1;
  pageSize: number = 6;

  constructor(private messagesService: MessagesService) {}

  ngOnInit(): void {
    this.loadMessages();
  }

  loadMessages(): void {
    this.messagesService
      .getUserMessages(this.pageNumber, this.pageSize, this.container)
      .subscribe({
        next: (response) => {
          this.messages = response.body?.messages;
          console.log(this.messages);
        },
        error: (error) => {
          console.error('Error fetching messages', error);
        },
      });
  }
}
