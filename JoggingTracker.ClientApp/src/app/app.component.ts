import { Component, OnInit } from '@angular/core';

import { AuthService } from './shared';

@Component({
  selector: 'jogging-tracker-app',
  templateUrl: './app.component.html'
})
export class AppComponent implements OnInit {
  public constructor (private authService: AuthService) {}

  public ngOnInit() {
    this.authService.tryRefreshUserIdentity();
  }
}
