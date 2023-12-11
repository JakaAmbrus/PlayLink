import { MessagesButtons } from '../../shared/models/navigation';

export const messageButtons: MessagesButtons[] = [
  { value: 'Unread', label: 'Unread', icon: 'mail' },
  { value: 'Inbox', label: 'Inbox', icon: 'drafts' },
  { value: 'Outbox', label: 'Outbox', icon: 'send' },
];
