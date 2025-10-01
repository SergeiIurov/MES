import {inject} from '@angular/core';
import {ActivatedRouteSnapshot, CanActivateFn, Router, RouterStateSnapshot} from '@angular/router';
import {AuthService} from '../auth-service';
import {Roles} from '../../enums/roles';

export const authorizeGuard: CanActivateFn = (route: ActivatedRouteSnapshot, state: RouterStateSnapshot) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  if (authService.role === Roles[Roles.Admin]) {
    return true;
  } else if (authService.role === Roles[Roles.Operator] && route.routeConfig.path === 'input-form') {
    return true;
  } else {
    return router.parseUrl('/');
  }
};
