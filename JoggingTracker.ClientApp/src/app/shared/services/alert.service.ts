import { Injectable } from '@angular/core';
import { Router, NavigationStart } from '@angular/router';
import { FormGroup } from '@angular/forms';
import { Observable } from 'rxjs';
import { Subject } from 'rxjs/Subject';
import { normalizeCamelCase } from '../utils';

@Injectable()
export class AlertService {
    private subject = new Subject<any>();
    private keepAfterNavigationChange = false;

    public constructor(private router: Router) {
      // Clear alert message on route change.
      this.router.events.subscribe(event => {
        if (!(event instanceof NavigationStart))
          return;

        // Preserve message for a single location change.
        if (this.keepAfterNavigationChange) {
          this.keepAfterNavigationChange = false;
          return;
        }

        // Clear alert.
        this.subject.next();
      });
    }

    public httpCallError(error: any, form: FormGroup = null) {
      var errorMessage: string;
      
      for (var key in error) {
        if (key === '') {
          if (errorMessage) {
            errorMessage += '\n' + error[key];
          }
          else {
            errorMessage = error[key];
          }

          continue;
        }

        var control = form.get(normalizeCamelCase(key));
        control.setErrors({
          'remote': error[key][0]
        });
      }

      if (errorMessage) {
        this.error(errorMessage);
      }
    }

    public error(message: string, keepAfterNavigationChange: boolean = false) {
        this.keepAfterNavigationChange = keepAfterNavigationChange;
        this.subject.next({ type: 'error', text: message });
    }

    public success(message: string, keepAfterNavigationChange: boolean = false) {
      this.keepAfterNavigationChange = keepAfterNavigationChange;
      this.subject.next({ type: 'success', text: message });
    }

    public getMessage(): Observable<any> {
      return this.subject.asObservable();
    }
}