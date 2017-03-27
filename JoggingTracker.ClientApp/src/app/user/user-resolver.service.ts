import { Injectable, } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve, Router, RouterStateSnapshot } from '@angular/router';
import { Observable } from 'rxjs/Rx';

import { User, UserService } from '../shared';

@Injectable()
export class UserResolver implements Resolve<User> {
  public constructor(
    private userService: UserService,
    private router: Router) {}

  public resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<any> {
    return this.userService
      .get(route.params['id'])
      .catch((err) => this.router.navigateByUrl('/'));
  }
}