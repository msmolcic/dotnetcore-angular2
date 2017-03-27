import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Rx';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import { ReplaySubject } from 'rxjs/ReplaySubject';
import 'rxjs/add/operator/map';

import { ApiService } from './api.service';
import { JwtService } from './jwt.service';
import { UserIdentity } from '../models';


@Injectable()
export class AuthService {
  private currentUserSubject = new BehaviorSubject<UserIdentity>(new UserIdentity());
  public currentUser = this.currentUserSubject.asObservable().distinctUntilChanged();

  private isAuthenticatedSubject = new ReplaySubject<boolean>(1);
  public isAuthenticated = this.isAuthenticatedSubject.asObservable();

  public constructor (
    private apiService: ApiService,
    private jwtService: JwtService) { }

  // Attempts to log in user with provided credentials.
  // Stores JsonWebToken and UserIdentity on successful login.
  public login(credentials): Observable<UserIdentity> {
    return this.apiService
      .post('/users/login', credentials)
      .map(loginResponse => {
        this.setJsonWebToken(loginResponse.jsonWebToken);
        this.setUserIdentity(loginResponse.userIdentity);
        return loginResponse;
      });
  }
  
  // Clears every trace of current authentication,
  // reseting it to the initial state when no one was logged in.
  public logout() {
    this.jwtService.destroyToken();
    this.currentUserSubject.next(new UserIdentity());
    this.isAuthenticatedSubject.next(false);
  }

  // Attempts to refresh UserIdentity from the server if user is logged in,
  // clears every trace of current authentication otherwise.
  public tryRefreshUserIdentity() {
    if (!this.jwtService.getToken()) {
      this.logout();
      return;
    }

    this.getUserIdentity()
        .subscribe(
          userIdentity => this.setUserIdentity(userIdentity),
          error => console.log(error));
  }

  // Returns UserIdentity of the currently logged user.
  public getCurrentUser(): UserIdentity {
    return this.currentUserSubject.value;
  }

  // Determines whether user has 'Admin' role.
  public hasAdminRole(): boolean {
    return this.hasRole("Admin");
  }
  
  // Determines whether user has 'UserManager' role.
  public hasUserManagerRole(): boolean {
    return this.hasRole("UserManager");
  }
  
  // Determines whether user has 'User' role.
  public hasUserRole(): boolean {
    return this.hasRole("User");
  }

  // Saves the JsonWebToken to the LocalStorage.
  private setJsonWebToken(token: string) {
    this.jwtService.saveToken(token);
  }

  // Sets the identity of the current user and notifies that user is now authenticated.
  private setUserIdentity(userIdentity: UserIdentity) {
    this.currentUserSubject.next(userIdentity);
    this.isAuthenticatedSubject.next(true);
  }

  // Returns UserIdentity of the authenticated user from the server.
  private getUserIdentity(): Observable<UserIdentity> {
    return this.apiService.get('/users/identity');
  }

  // Checks whether user has a provided role.
  private hasRole(roleName): boolean {
    return this.currentUserSubject.value.roles.indexOf(roleName) !== -1;
  }
}