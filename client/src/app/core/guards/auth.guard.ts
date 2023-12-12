import {
  CanActivateFn,
  ActivatedRouteSnapshot,
  Router,
  UrlTree,
} from '@angular/router';
import { inject } from '@angular/core';

const checkIfLoggedIn = (): boolean => {
  const loggedIn = localStorage.getItem('loggedIn');
  return loggedIn === 'true';
};

export const canActivateGuard: CanActivateFn = (): boolean | UrlTree => {
  const router = inject(Router);

  const isLoggedIn = checkIfLoggedIn();
  if (!isLoggedIn) {
    return router.parseUrl('/portal');
  }
  return true;
};

export const canActivateLoginGuard: CanActivateFn = (
  route: ActivatedRouteSnapshot
): boolean | UrlTree => {
  const router = inject(Router);

  const isLoggedIn = checkIfLoggedIn();
  if (isLoggedIn) {
    return router.parseUrl('/home');
  }
  return true;
};
