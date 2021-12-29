import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { AppComponent } from './app.component';
import { ChatsComponent } from './chats/chats.component';
import { UserComponent } from './user/user.component';
import { LoginComponent } from './user/login/login.component';
import { SignupComponent } from './user/signup/signup.component';
import { HttpModule } from '@angular/http';
import { AlertComponent } from './alert/alert.component';
import { UserchatsComponent } from './chats/userchats/userchats.component';
import { LoaderComponent } from './loader/loader.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgxTypeaheadModule } from 'ngx-typeahead';

@NgModule({
  declarations: [
    AppComponent,
    ChatsComponent,
    UserComponent,
    LoginComponent,
    SignupComponent,
    AlertComponent,
    UserchatsComponent,
    LoaderComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    BrowserAnimationsModule,
    HttpModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    NgxTypeaheadModule,
    RouterModule.forRoot([
      { path: '', redirectTo: '/login', pathMatch: 'full' },
      {
        path: 'login', component: UserComponent,
        children: [{ path: '', component: LoginComponent }]
      },
      {
        path: 'signup', component: UserComponent,
        children: [{ path: '', component: SignupComponent }]
      },
      {
        path: 'chat', component: ChatsComponent,
        children: [{ path: '', component: UserchatsComponent }]
      },
      {
        path: 'load', component: LoaderComponent
      }
    ])
  ],
  providers: [],
  bootstrap: [AppComponent]

})
export class AppModule { }
