import {
  CanActivateFn,
  ActivatedRouteSnapshot,
  Router,
  UrlTree,
} from '@angular/router';
import { inject } from '@angular/core';

export const currentUserRestrictionGuard: CanActivateFn = (
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
