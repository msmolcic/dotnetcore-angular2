import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Rx';
import { ApiService } from './api.service';

import { Gender, User, UserRegistration } from '../models';

@Injectable()
export class UserService {
  private baseUrl: string;

  public constructor (private apiService: ApiService) {
    this.baseUrl = '/Users';
  }

  public getAll(): Observable<User[]> {
    return this.apiService.get(this.baseUrl);
  }

  public get(userId: string): Observable<User> {
    return this.apiService.get(`${this.baseUrl}/` + userId);
  }

  public register(userRegistration: UserRegistration): Observable<void> {
    return this.apiService.post(`${this.baseUrl}/Register`, userRegistration);
  }

  public update(userId: string, user: User): Observable<void> {
    return this.apiService.put(`${this.baseUrl}/` + userId, user);
  }

  public delete(userId: string): Observable<void> {
    return this.apiService.delete(`${this.baseUrl}/` + userId);
  }

  // Returns an array of existing genders.
  public getGenders(): Gender[] {
    return [
      { name: 'Female', value: 0 },
      { name: 'Male', value: 1 }
    ];
  }
}