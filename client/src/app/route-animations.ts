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
    'Discover => Home, Games => Discover, Games => Home, Favorites => Home, Favorites => Discover, Favorites => Games',
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
          animate('250ms ease-out', style({ transform: 'translateX(100%)' })),
        ]),
        query(':enter', [
          animate('250ms ease-out', style({ transform: 'translateX(0%)' })),
        ]),
      ]),
      query(':enter', animateChild()),
    ]
  ),
  transition(
    'Home => Discover, Home => Games, Home => Favorites, Discover => Games, Discover => Favorites, Games => Favorites',
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
          animate('250ms ease-out', style({ transform: 'translateX(-100%)' })),
        ]),
        query(':enter', [
          animate('250ms ease-out', style({ transform: 'translateX(0%)' })),
        ]),
      ]),
      query(':enter', animateChild()),
    ]
  ),
]);
