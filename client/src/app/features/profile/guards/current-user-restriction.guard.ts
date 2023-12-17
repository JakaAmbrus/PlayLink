import {
  CanActivateFn,
  ActivatedRouteSnapshot,
  Router,
  UrlTree,
} from '@angular/router';
import { inject } from '@angular/core';
import { LocalStorageService } from 'src/app/core/services/local-storage.service';

export const currentUserRestrictionGuard: CanActivateFn = (
  route: ActivatedRouteSnapshot
): boolean | UrlTree => {
  const router = inject(Router);
  const localStorageService = inject(LocalStorageService);
  const loggedInUsername = localStorageService.getItem('username');
  const routeUsername = route.parent?.paramMap.get('username');
  if (loggedInUsername === routeUsername) {
    return router.parseUrl('/home');
  }
  return true;
};
