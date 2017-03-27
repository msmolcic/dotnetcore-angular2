import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve, Router, RouterStateSnapshot } from '@angular/router';
import { Observable } from 'rxjs/Rx';

import { AuthService } from '../shared';

@Injectable()
export class HomeAuthResolver implements Resolve<boolean> {
  public constructor(
    private router: Router,
    private authService: AuthService) { }

  public resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> {
    return this.authService.isAuthenticated.take(1);
  }
}
