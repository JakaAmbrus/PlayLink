import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';

export const adminGuard: CanActivateFn = () => {
  const router = inject(Router);
  const isAdmin = checkIfAdmin();
  if (!isAdmin) {
    return router.parseUrl('/home');
  }
  return true;
};

const checkIfAdmin = (): boolean => {
  const storedRoles = localStorage.getItem('roles');
  const roles = storedRoles ? JSON.parse(storedRoles) : [];
  return roles.includes('Admin');
};
