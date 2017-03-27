import { Injectable } from '@angular/core';
import { URLSearchParams } from '@angular/http';
import { Observable } from 'rxjs/Observable';

import { JoggingRoute, JoggingRoutesFilter, JoggingRouteViewModel, WeeklyRecord } from '../models';
import { ApiService } from './api.service';

@Injectable()
export class JoggingRoutesService {
    public constructor(private apiService: ApiService) { }

    public getAll(filter: JoggingRoutesFilter): Observable<JoggingRouteViewModel[]> {
        let searchParams = new URLSearchParams();

        if (filter.fromDate)
            searchParams.set('fromDate', filter.fromDate.toString());

        if (filter.untilDate)
            searchParams.set('untilDate', filter.untilDate.toString());

        return this.apiService.get(this.getBaseUrl(filter.userId), searchParams);
    }

    public get(userId: string, routeId: string): Observable<JoggingRoute> {
        return this.apiService.get(`${this.getBaseUrl(userId)}/` + routeId);
    }

    public post(userId: string, joggingRoute: JoggingRoute): Observable<JoggingRoute> {
        return this.apiService.post(this.getBaseUrl(userId), joggingRoute);
    }

    public update(userId: string, routeId: string, joggingRoute: JoggingRoute): Observable<void> {
        return this.apiService.put(`${this.getBaseUrl(userId)}/` + routeId, joggingRoute);
    }

    public delete(userId: string, routeId: string): Observable<void> {
        return this.apiService.delete(`${this.getBaseUrl(userId)}/` + routeId);
    }

    public getWeeklyRecords(userId: string): Observable<WeeklyRecord[]> {
        return this.apiService.get(`/Users/${userId}/WeeklyRecords`);
    }

    private getBaseUrl(userId: string): string {
        return `/Users/${userId}/JoggingRoutes`;
    }
}