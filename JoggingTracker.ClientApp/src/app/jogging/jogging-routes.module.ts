import { ModuleWithProviders, NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

import { SharedModule, JoggingRoutesService } from '../shared';
import { JoggingRoutesTableComponent, JoggingRouteComponent, WeeklyRecordsTableComponent } from './';
import { AuthGuard } from '../shared/services';
import { JoggingRouteResolver } from "./jogging-route-resolver.service";
import { WeeklyRecordsTableResolver } from "./weekly-records.table-resolver.service";


const joggingRoutesRouting: ModuleWithProviders = RouterModule.forChild([
  {
    path: 'Users/:userId/JoggingRoutes',
    component: JoggingRoutesTableComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'Users/:userId/JoggingRoutes/Add',
    component: JoggingRouteComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'Users/:userId/JoggingRoutes/:id',
    component: JoggingRouteComponent,
    canActivate: [AuthGuard],
    resolve: {
      joggingRoute: JoggingRouteResolver
    }
  },
  {
    path: 'Users/:userId/WeeklyRecords',
    component: WeeklyRecordsTableComponent,
    canActivate: [AuthGuard],
    resolve: {
      weeklyRecords: WeeklyRecordsTableResolver
    }
  }
]);

@NgModule({
  imports: [
    joggingRoutesRouting,
    SharedModule
  ],
  providers: [
    AuthGuard,
    JoggingRouteResolver,
    WeeklyRecordsTableResolver,
    JoggingRoutesService
  ],
  declarations: [
    JoggingRouteComponent,
    JoggingRoutesTableComponent,
    WeeklyRecordsTableComponent
  ]
})
export class JoggingRoutesModule { }
