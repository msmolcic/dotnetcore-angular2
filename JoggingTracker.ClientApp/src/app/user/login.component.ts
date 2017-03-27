import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';

import { AlertService, AuthService } from '../shared';

@Component({
  selector: 'login-page',
  templateUrl: './login.component.html'
})
export class LoginComponent implements OnInit {
  private isSubmitting: boolean = false;
  private loginForm: FormGroup;

  public constructor(
    private router: Router,
    private authService: AuthService,
    private alertService: AlertService,
    private formBuilder: FormBuilder) { }

  public ngOnInit() {
    this.loginForm = this.formBuilder.group({
      'username': [''],
      'password': ['']
    });
  }

  public submitForm() {
    this.isSubmitting = true;

    this.authService
        .login(this.loginForm.value)
        .subscribe(
          data => this.router.navigateByUrl('/'),
          error => this.alertService.httpCallError(error, this.loginForm));

    this.isSubmitting = false;
  }
}
