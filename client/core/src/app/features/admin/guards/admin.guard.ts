import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { LocalStorageService } from 'src/app/core/services/local-storage.service';

export const adminGuard: CanActivateFn = () => {
  const router = inject(Router);
  const isAdmin = checkIfAdmin();
  if (!isAdmin) {
    return router.parseUrl('/home');
  }
  return true;
};

const checkIfAdmin = (): boolean => {
  const localStorageService = inject(LocalStorageService);
  const storedRoles = localStorageService.getItem('roles');
  if (typeof storedRoles === 'string') {
    const roles = JSON.parse(storedRoles);
    return roles.includes('Admin');
  }
  return false;
};
