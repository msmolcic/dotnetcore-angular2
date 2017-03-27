import { ModuleWithProviders, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule } from '@angular/router';

import { MomentModule } from 'angular2-moment';

import { AppComponent } from './app.component';
import { HomeModule } from './home/home.module';
import { UserModule } from './user/user.module'; 
import { JoggingRoutesModule } from "./jogging/jogging-routes.module";

import {
  AuthGuard,
  NoAuthGuard,

  JwtService,
  ApiService,
  AuthService,
  AlertService,
  UserService,

  AlertComponent,
  HeaderComponent,
  FooterComponent,

  SharedModule
} from './shared';

const rootRouting: ModuleWithProviders = RouterModule.forRoot([
  // Redirect all unknown routes to home.
  { path: '**', redirectTo: '' }
], { useHash: true });

@NgModule({
  declarations: [
    AppComponent,
    AlertComponent,
    HeaderComponent,
    FooterComponent
  ],
  imports: [
    BrowserModule,
    MomentModule,
    rootRouting,
    SharedModule,
    HomeModule,
    UserModule,
    JoggingRoutesModule
  ],
  providers: [
    AuthGuard,
    NoAuthGuard,

    JwtService,
    ApiService,
    AuthService,
    AlertService,
    UserService
  ],
  bootstrap: [AppComponent]
})
export class AppModule {}
