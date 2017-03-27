import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from "@angular/router";
import { User, Gender, UserService, AlertService, AuthService } from "../shared";

@Component({
  selector: 'user-table',
  templateUrl: 'user.table.component.html'
})
export class UserTableComponent implements OnInit {
  private users: User[];
  private genders: Gender[];

  public constructor(
    private route: ActivatedRoute,
    private alertService: AlertService,
    private authService: AuthService,
    private userService: UserService) { }

  public ngOnInit() {
    this.route.data.subscribe(
      (data: {users: User[]}) => {
        this.users = data.users;
      });

    this.genders = this.userService.getGenders();
  }

  public deleteUser(user: User) {
    if (!confirm('Are you sure you want to delete this user?'))
      return;

    this.userService
        .delete(user.id)
        .subscribe(
          data => {
            this.alertService.success("Successfully deleted!");
            this.users.splice(this.users.indexOf(user), 1);
          },
          error => {
            this.alertService.httpCallError(error);
          });
  }

  public displayGender(value: number) {
    for (let gender of this.genders) {
      if (gender.value === value)
        return gender.name;
    }
    return 'n/a';
  }
}