import {inject} from '@angular/core';
import {CanActivateFn, Router} from '@angular/router';
import {AuthService} from './auth-service';

export const authEntryPointGuard: CanActivateFn = () => {
  const authService = inject(AuthService);
  const router = inject(Router);
  if (authService.isAuthenticated) {
    return authService.isAuthenticated;
  } else {
    return router.navigate(['/login'], {queryParams: {returnUrl: 'scan'}});
  }
};
