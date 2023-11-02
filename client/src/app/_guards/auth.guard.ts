import {
  CanActivateFn,
  ActivatedRouteSnapshot,
  RouterStateSnapshot,
  Router,
  UrlTree,
} from '@angular/router';
import { inject } from '@angular/core';
import { AccountService } from '../_services/account.service';

const checkIfLoggedIn = (): boolean => {
  const loggedIn = localStorage.getItem('loggedIn');
  return loggedIn === 'true';
};

export const canActivateGuard: CanActivateFn = (
  route: ActivatedRouteSnapshot,
  state: RouterStateSnapshot
): boolean | UrlTree => {
  const accountService = inject(AccountService);
  const router = inject(Router);

  const isLoggedIn = checkIfLoggedIn();
  if (!isLoggedIn) {
    return router.parseUrl('/portal');
  }
  return true;
};

export const canActivateLoginGuard: CanActivateFn = (
  route: ActivatedRouteSnapshot,
  state: RouterStateSnapshot
): boolean | UrlTree => {
  const accountService = inject(AccountService);
  const router = inject(Router);

  const isLoggedIn = checkIfLoggedIn();
  if (isLoggedIn) {
    return router.parseUrl('/home');
  }
  return true;
};
