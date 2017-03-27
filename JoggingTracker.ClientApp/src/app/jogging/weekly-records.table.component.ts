import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router, NavigationExtras } from "@angular/router";

import * as moment from 'moment';

import { AlertService, JoggingRoutesService, JoggingRoutesFilter, WeeklyRecord } from "../shared";

@Component({
  selector: 'weekly-records-table',
  templateUrl: 'weekly-records.table.component.html'
})
export class WeeklyRecordsTableComponent implements OnInit {
  private weeklyRecords: WeeklyRecord[];
  private userId: string;

  public constructor(
    private route: ActivatedRoute,
    private router: Router,
    private alertService: AlertService,
    private joggingRoutesService: JoggingRoutesService) { }

  public ngOnInit() {
    this.route.params.subscribe(data => {
      this.userId = data['userId'];
    });

    this.route.data.subscribe(
      (data: {weeklyRecords: WeeklyRecord[]}) => {
        this.weeklyRecords = data.weeklyRecords;
      });
  }

  public viewRoutes(record: WeeklyRecord) {
    let extras: NavigationExtras = {
      queryParams: {
        'fromDate': moment(record.fromDate).format('YYYY-MM-DD'),
        'untilDate': moment(record.untilDate).format('YYYY-MM-DD')
      }
    };

    this.router.navigate([`/Users/${this.userId}/JoggingRoutes`], extras);
  }
}