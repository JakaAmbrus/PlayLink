import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Message } from 'src/app/_models/messages';
import { MessagesService } from 'src/app/_services/messages.service';

@Component({
  selector: 'app-message',
  templateUrl: './message.component.html',
  styleUrls: ['./message.component.scss'],
})
export class MessageComponent implements OnInit {
  username: any;
  messages: Message[] = [];

  constructor(
    private messagesService: MessagesService,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.loadMessages();
  }

  loadMessages(): void {
    this.username = this.route.parent?.snapshot.paramMap.get('username');
    if (!this.username) {
      return;
    }

    this.messagesService.getMessageThread(this.username).subscribe({
      next: (messages) => {
        this.messages = messages;
        console.log(messages);
      },
    });
  }
}
