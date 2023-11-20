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
    'Discover => Home, Games => Discover, Games => Home, Favorites => Home, Favorites => Discover, Games => Favorites, Edit => Gallery, Edit => Posts, Message => Gallery, Message => Posts, Gallery => Posts',
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
    'Home => Discover, Home => Games, Home => Favorites, Discover => Games, Discover => Favorites, Favorites => Games, Posts => Gallery, Posts => Edit, Posts => Message, Gallery => Edit, Gallery => Message',
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
  // transition('* => Profile', [
  //   style({ opacity: 0, transform: 'scale(0.5)' }),
  //   animate('500ms ease', style({ opacity: 1, transform: 'scale(1)' })),
  // ]),
  // Edit => Gallery, Edit => Posts, Message = > Gallery, Message => Posts, Gallery => Posts

  // transition(
  //   'Edit => Gallery, Edit => Posts, Message = > Gallery, Message => Posts, Gallery => Posts',
  //   [
  //     style({ position: 'relative' }),
  //     query(':enter, :leave', [
  //       style({
  //         top: 0,
  //         width: '100%',
  //         position: 'absolute',
  //       }),
  //     ]),
  //     query(':enter', [style({ transform: 'translateX(100%)' })]),
  //     query(':leave', animateChild()),
  //     group([
  //       query(':leave', [
  //         animate('280ms ease-out', style({ transform: 'translateX(-100%)' })),
  //       ]),
  //       query(':enter', [
  //         animate('280ms ease-out', style({ transform: 'translateX(0%)' })),
  //       ]),
  //     ]),
  //     query(':enter', animateChild()),
  //   ]
  // ),
]);
