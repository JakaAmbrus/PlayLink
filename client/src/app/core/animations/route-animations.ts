import {
  style,
  query,
  trigger,
  transition,
  animateChild,
  group,
  animate,
} from '@angular/animations';

export const slideInAnimation = trigger('routeAnimations', [
  transition(
    'Discover => Home, Games => Discover, Games => Home, Messages => Home, Messages => Discover, Games => Messages, Edit => Gallery, Edit => Posts, Message => Gallery, Message => Posts, Gallery => Posts',
    [
      style({ position: 'relative' }),
      query(':enter, :leave', [
        style({
          top: 0,
          width: '100%',
          position: 'absolute',
        }),
      ]),
      query(':enter', [style({ transform: 'translateX(-100%)' })]),
      query(':leave', animateChild()),
      group([
        query(':leave', [
          animate('280ms ease-out', style({ transform: 'translateX(100%)' })),
        ]),
        query(':enter', [
          animate('280ms ease-out', style({ transform: 'translateX(0%)' })),
        ]),
      ]),
      query(':enter', animateChild()),
    ]
  ),
  transition(
    'Home => Discover, Home => Games, Home => Messages, Discover => Games, Discover => Messages, Messages => Games, Posts => Gallery, Posts => Edit, Posts => Message, Gallery => Edit, Gallery => Message',
    [
      style({ position: 'relative' }),
      query(':enter, :leave', [
        style({
          top: 0,
          width: '100%',
          position: 'absolute',
        }),
      ]),
      query(':enter', [style({ transform: 'translateX(100%)' })]),
      query(':leave', animateChild()),
      group([
        query(':leave', [
          animate('280ms ease-out', style({ transform: 'translateX(-100%)' })),
        ]),
        query(':enter', [
          animate('280ms ease-out', style({ transform: 'translateX(0%)' })),
        ]),
      ]),
      query(':enter', animateChild()),
    ]
  ),
]);
