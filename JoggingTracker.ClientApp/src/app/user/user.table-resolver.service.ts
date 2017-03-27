import { Injectable, } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve, Router, RouterStateSnapshot } from '@angular/router';
import { Observable } from 'rxjs/Rx';

import { User, UserService, AuthService } from '../shared';

@Injectable()
export class UserTableResolver implements Resolve<User[]> {
  public constructor(
    private authService: AuthService,
    private userService: UserService,
    private router: Router) {}

  public resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<any> {
    if (!this.authService.hasAdminRole() && !this.authService.hasUserManagerRole()) {
      this.router.navigateByUrl('/');
      return;
    }

    return this.userService
      .getAll()
      .catch((err) => this.router.navigateByUrl('/'));
  }
}