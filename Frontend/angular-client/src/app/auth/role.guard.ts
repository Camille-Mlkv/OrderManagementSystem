import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { inject } from '@angular/core';

export const RoleGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  const expectedRoles = route.data['roles'] as string[];
  const role = authService.getClaim<string>('Role');

  if (!role) {
    router.navigate(['/sign-in']);
    return false;
  }

  if (expectedRoles && !expectedRoles.includes(role)) {
    router.navigate(['/sign-in']);
    return false;
  }

  return true;
};
