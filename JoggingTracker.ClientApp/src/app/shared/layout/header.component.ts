import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { UserIdentity } from '../models';
import { AuthService } from '../services';

@Component({
  selector: 'layout-header',
  templateUrl: './header.component.html'
})
export class HeaderComponent implements OnInit {
  public currentUser: UserIdentity;

  public constructor(
    private authService: AuthService,
    private router: Router) {}

  public ngOnInit() {
    this.authService.currentUser.subscribe(
      (userData) => {
        this.currentUser = userData;
      });
  }

  public logout() {
    this.authService.logout();
    this.router.navigateByUrl('/');
  }
}
