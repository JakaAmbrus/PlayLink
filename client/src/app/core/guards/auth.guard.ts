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

export const canActivateCurrentUserGuard: CanActivateFn = (
  route: ActivatedRouteSnapshot
): boolean | UrlTree => {
  const router = inject(Router);
  const loggedInUsername = localStorage.getItem('user');
  const routeUsername = route.parent?.paramMap.get('username');
  if (loggedInUsername !== routeUsername) {
    return router.parseUrl('/home');
  }
  return true;
};

export const canActivateNotCurrentUserGuard: CanActivateFn = (
  route: ActivatedRouteSnapshot
): boolean | UrlTree => {
  const router = inject(Router);
  const loggedInUsername = localStorage.getItem('user');
  const routeUsername = route.parent?.paramMap.get('username');
  if (loggedInUsername === routeUsername) {
    return router.parseUrl('/home');
  }
  return true;
};
