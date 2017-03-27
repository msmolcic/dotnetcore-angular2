import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

import * as moment from 'moment';

import { AlertService, UserService, User } from '../shared';
import { Gender } from '../shared/models';

@Component({
  selector: 'registration-page',
  templateUrl: './registration.component.html'
})
export class RegistrationComponent implements OnInit {
  private isSubmitting: boolean = false;
  private userRegistrationMode: boolean = true;
  private userForm: FormGroup;
  private user: User;
  private genders: Gender[];

  public constructor(
    private route: ActivatedRoute,
    private router: Router,
    private userService: UserService,
    private alertService: AlertService,
    private formBuilder: FormBuilder) { }

  public ngOnInit() {
    // Initialize shared part of the user form.
    this.userForm = this.formBuilder.group({
      'name': [''],
      'surname': [''],
      'birthDate': [''],
      'gender': [0]
    });

    this.genders = this.userService.getGenders();

    this.route.url.subscribe(data => {
      // If last piece of the URL equals to 'Register', user registration
      // form should be displayed. User edit form is displayed otherwise.
      this.userRegistrationMode = data[data.length - 1].path === 'Register';

      if (this.userRegistrationMode) {
        this.userForm.addControl('username', new FormControl());
        this.userForm.addControl('email', new FormControl());
        this.userForm.addControl('password', new FormControl());
        this.userForm.addControl('confirmPassword', new FormControl());
      }
    });

    if (!this.userRegistrationMode) {
      this.route.data.subscribe(
        (data: {user: User}) => {
          this.user = data.user;
          this.userForm.patchValue(this.user);
          this.userForm.patchValue({birthDate: moment(this.user.birthDate).format('YYYY-MM-DD')});
        });
    }
  }

  public submitForm() {
    this.isSubmitting = true;

    if (this.userRegistrationMode)
      this.registerUser();
    else
      this.saveChanges();
      
    this.isSubmitting = false;
  }

  private registerUser() {
    this.userService
        .register(this.userForm.value)
        .subscribe(
          data => {
            this.alertService.success('Successfully registered! Enter your credentials to proceed...', true);
            this.router.navigateByUrl('/Login');
          },
          error => {
            this.alertService.httpCallError(error, this.userForm);
          });
  }

  private saveChanges() {
    this.userService
        .update(this.user.id, this.userForm.value)
        .subscribe(
          data => {
            this.alertService.success('Successfully saved changes.');
          },
          error => {
            this.alertService.httpCallError(error, this.userForm);
          });
  }
}
