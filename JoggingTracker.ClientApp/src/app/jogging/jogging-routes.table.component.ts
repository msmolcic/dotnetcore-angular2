import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router, NavigationExtras } from "@angular/router";

import { AlertService, JoggingRoutesService, JoggingRouteViewModel, JoggingRoutesFilter } from "../shared";

@Component({
  selector: 'jogging-routes-table',
  templateUrl: 'jogging-routes.table.component.html'
})
export class JoggingRoutesTableComponent implements OnInit {
  private joggingRoutes: JoggingRouteViewModel[];
  private filter: JoggingRoutesFilter;

  public constructor(
    private route: ActivatedRoute,
    private router: Router,
    private alertService: AlertService,
    private joggingRoutesService: JoggingRoutesService) {
      this.filter = new JoggingRoutesFilter();
  }

  public ngOnInit() {
    this.route.params.subscribe(data => {
      this.filter.userId = data['userId'];
    });

    this.route.queryParams.subscribe(data => {
      if (data['fromDate'])
        this.filter.fromDate = data['fromDate'];

      if (data['untilDate'])
        this.filter.untilDate = data['untilDate'];

      this.joggingRoutesService
          .getAll(this.filter)
          .subscribe(
            data => this.joggingRoutes = data,
            error => this.alertService.httpCallError(error));
    });
  }

  public deleteRoute(joggingRoute: JoggingRouteViewModel) {
    if (!confirm('Are you sure you want to delete this jogging route?'))
      return;

    this.joggingRoutesService
        .delete(joggingRoute.userId, joggingRoute.id)
        .subscribe(
          data => {
            this.alertService.success("Successfully deleted!");
            this.joggingRoutes.splice(this.joggingRoutes.indexOf(joggingRoute), 1);
          },
          error => {
            this.alertService.httpCallError(error);
          });
  }

  public applyFilter() {
    let params = {};

    if (this.filter.fromDate)
      params['fromDate'] = this.filter.fromDate;

    if (this.filter.untilDate)
      params['untilDate'] = this.filter.untilDate;

    let extras: NavigationExtras = {
      queryParams: params
    };

    this.router.navigate([`/Users/${this.filter.userId}/JoggingRoutes`], extras);
  }
}