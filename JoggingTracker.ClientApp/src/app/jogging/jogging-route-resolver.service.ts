import { Injectable, } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve, Router, RouterStateSnapshot } from '@angular/router';
import { Observable } from 'rxjs/Rx';

import { JoggingRoute, JoggingRoutesService } from '../shared';

@Injectable()
export class JoggingRouteResolver implements Resolve<JoggingRoute> {
  public constructor(
    private joggingRoutesService: JoggingRoutesService,
    private router: Router) {}

  public resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<any> {
    return this.joggingRoutesService
      .get(route.params['userId'], route.params['id'])
      .catch((err) => this.router.navigateByUrl('/'));
  }
}