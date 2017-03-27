import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot } from '@angular/router';
import { Observable } from 'rxjs/Rx';

import { AuthService } from './auth.service';

@Injectable()
export class NoAuthGuard implements CanActivate {
  public constructor(
    private router: Router,
    private authService: AuthService) {}

  public canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> {
    return this.authService.isAuthenticated.take(1).map(bool => !bool);
  }
}
