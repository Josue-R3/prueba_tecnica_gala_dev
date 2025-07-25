import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

export function authGuard() {
  const authService = inject(AuthService);
  const router = inject(Router);

  if (authService.isAuthenticatedSync()) {
    return true;
  } else {
    router.navigate(['/login']);
    return false;
  }
}

export function loginGuard() {
  const authService = inject(AuthService);
  const router = inject(Router);

  if (authService.isAuthenticatedSync()) {
    router.navigate(['/dashboard']);
    return false;
  } else {
    return true;
  }
}
