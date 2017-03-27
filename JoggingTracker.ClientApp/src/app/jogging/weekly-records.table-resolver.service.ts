import { Injectable, } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve, Router, RouterStateSnapshot } from '@angular/router';
import { Observable } from 'rxjs/Rx';

import { JoggingRoutesService, WeeklyRecord } from '../shared';

@Injectable()
export class WeeklyRecordsTableResolver implements Resolve<WeeklyRecord[]> {
  public constructor(
    private joggingRoutesService: JoggingRoutesService,
    private router: Router) {}

  public resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<any> {
    return this.joggingRoutesService
      .getWeeklyRecords(route.params["userId"])
      .catch((err) => this.router.navigateByUrl('/'));
  }
}