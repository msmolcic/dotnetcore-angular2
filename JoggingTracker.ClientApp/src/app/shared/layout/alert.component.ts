import { Component, OnInit } from '@angular/core';

import { AlertService } from '../services';

@Component({
  selector: 'alert',
  templateUrl: './alert.component.html',
  styleUrls: ['alert.component.css']
})
export class AlertComponent {
    private message: any;

    public constructor (private alertService: AlertService) { }

    public ngOnInit() {
        this.alertService.getMessage().subscribe(message => {
            this.message = message;
        });
    }

    public closeWindow() {
        this.message = null;
    }
}