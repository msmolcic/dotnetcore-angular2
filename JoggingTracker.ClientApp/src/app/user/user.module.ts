import { ModuleWithProviders, NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

import { SharedModule } from '../shared';
import { LoginComponent, RegistrationComponent, UserTableComponent } from './';
import { NoAuthGuard, AuthGuard } from '../shared/services';
import { UserResolver } from "./user-resolver.service";
import { UserTableResolver } from "./user.table-resolver.service";

const userRouting: ModuleWithProviders = RouterModule.forChild([
  {
    path: 'Login',
    component: LoginComponent,
    canActivate: [NoAuthGuard]
  },
  {
    path: 'Register',
    component: RegistrationComponent,
    canActivate: [NoAuthGuard]
  },
  {
    path: 'Users/:id',
    component: RegistrationComponent,
    canActivate: [AuthGuard],
    resolve: {
      user: UserResolver
    }
  },
  {
    path: 'Users',
    component: UserTableComponent,
    canActivate: [AuthGuard],
    resolve: {
      users: UserTableResolver
    }
  }
]);

@NgModule({
  imports: [
    userRouting,
    SharedModule
  ],
  providers: [
    NoAuthGuard,
    AuthGuard,
    UserResolver,
    UserTableResolver
  ],
  declarations: [
    LoginComponent,
    RegistrationComponent,
    UserTableComponent
  ]
})
export class UserModule {}
