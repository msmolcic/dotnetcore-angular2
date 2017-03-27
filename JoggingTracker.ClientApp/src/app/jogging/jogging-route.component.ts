import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

import * as moment from 'moment';

import { AlertService, JoggingRoute, JoggingRoutesService } from '../shared';

@Component({
  selector: 'jogging-route-page',
  templateUrl: './jogging-route.component.html'
})
export class JoggingRouteComponent implements OnInit {
  private isSubmitting: boolean = false;
  private addRouteMode: boolean = true;
  private joggingRouteForm: FormGroup;
  private joggingRoute: JoggingRoute;
  private userId: string;

  private dateTimeFormat: string = 'YYYY-MM-DDThh:mm:ss';

  public constructor(
    private route: ActivatedRoute,
    private router: Router,
    private joggingRoutesService: JoggingRoutesService,
    private alertService: AlertService,
    private formBuilder: FormBuilder) { }

  public ngOnInit() {
    // Initialize jogging route form.
    this.joggingRouteForm = this.formBuilder.group({
      'distanceKilometers': [''],
      'startTime': [moment().format(this.dateTimeFormat)],
      'endTime': [moment().format(this.dateTimeFormat)]
    });

    this.route.url.subscribe(data => {
      // If last piece of the URL equals to 'Add', user is adding a new route.
      this.addRouteMode = data[data.length - 1].path === 'Add';
    });

    this.route.params.subscribe(data => {
      this.userId = data['userId'];
    });

    if (!this.addRouteMode) {
      this.route.data.subscribe(
        (data: {joggingRoute: JoggingRoute}) => {
          this.joggingRoute = data.joggingRoute;
          this.joggingRouteForm.patchValue({distanceKilometers: this.joggingRoute.distanceKilometers});
          this.joggingRouteForm.patchValue({startTime: moment(this.joggingRoute.startTime).format(this.dateTimeFormat)});
          this.joggingRouteForm.patchValue({endTime: moment(this.joggingRoute.endTime).format(this.dateTimeFormat)});
        });
    }
  }

  public submitForm() {
    this.isSubmitting = true;

    if (this.addRouteMode)
      this.addRoute();
    else
      this.saveChanges();
      
    this.isSubmitting = false;
  }

  private addRoute() {
    this.joggingRoutesService
        .post(this.userId, this.joggingRouteForm.value)
        .subscribe(
          data => {
            this.alertService.success('Successfully created!', true);
            this.router.navigateByUrl(`/Users/${this.userId}/JoggingRoutes`);
          },
          error => {
            this.alertService.httpCallError(error, this.joggingRouteForm);
          });
  }

  private saveChanges() {
    this.joggingRoutesService
        .update(this.userId, this.joggingRoute.id, this.joggingRouteForm.value)
        .subscribe(
          data => {
            this.alertService.success('Successfully saved changes.');
          },
          error => {
            this.alertService.httpCallError(error, this.joggingRouteForm);
          });
  }
}
