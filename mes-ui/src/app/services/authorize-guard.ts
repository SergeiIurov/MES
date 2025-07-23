import {inject} from '@angular/core';
import {CanActivateFn, Router} from '@angular/router';
import {AuthService} from './auth-service';
import {Roles} from '../enums/roles';

export const authorizeGuard: CanActivateFn = () => {
  const authService = inject(AuthService);
  const router = inject(Router);
  if (authService.role === Roles.Admin) {
    return true;
  } else {
    return router.parseUrl('/');
  }
};
