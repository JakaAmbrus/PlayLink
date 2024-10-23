import {
  CanActivateFn,
  ActivatedRouteSnapshot,
  Router,
  UrlTree,
} from '@angular/router';
import { inject } from '@angular/core';
import { LocalStorageService } from '../services/local-storage.service';

const checkIfLoggedIn = (): boolean => {
  const localStorageService = inject(LocalStorageService);
  const loggedIn = localStorageService.getItem('loggedIn');
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

export const canActivateLoginGuard: CanActivateFn = (): boolean | UrlTree => {
  const router = inject(Router);

  const isLoggedIn = checkIfLoggedIn();
  if (isLoggedIn) {
    return router.parseUrl('/home');
  }
  return true;
};
