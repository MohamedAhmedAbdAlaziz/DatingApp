import { NgModule, OnInit } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import {
  HTTP_INTERCEPTORS,
  HttpClient,
  HttpClientModule,
} from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

import { NavComponent } from './nav/nav.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { HomeComponent } from './home/home.component';
import { RegisterComponent } from './register/register.component';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { MemberListComponent } from './members/member-list/member-list.component';
import { ListsComponent } from './lists/lists.component';
import { MessagesComponent } from './messages/messages.component';
import { ToastrModule } from 'ngx-toastr';
import { SharedModule } from './_modules/shared.module';
import { TestErrorsComponent } from './errors/test-errors/test-errors.component';
import { ErrorInterceptor } from './_interceptors/error.interceptor';
import { NotFoundComponent } from './errors/not-found/not-found.component';
import { ServerErrorComponent } from './errors/server-error/server-error.component';
import { MemberCardComponent } from './members/member-card/member-card.component';
import { JwtInterceptor } from './_interceptors/jwt.interceptor';
import { MemberEditComponent } from './members/member-edit/member-edit.component';
import { NgxSpinnerModule } from 'ngx-spinner';

import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { CommonModule } from '@angular/common';
import { LoadingInterceptor } from './_interceptors/loading.interceptor';
import { PhotoEditorComponent } from './members/photo-editor/photo-editor.component';
import { TextInputComponent } from './_forms/text-input/text-input.component';
import { DateInputComponent } from './_forms/date-input/date-input.component';
@NgModule({
  declarations: [
    AppComponent,
    NavComponent,
    HomeComponent,
    RegisterComponent,
    MemberDetailComponent,
    MemberListComponent,
    ListsComponent,
    MessagesComponent,
    TestErrorsComponent,
    NotFoundComponent,
    ServerErrorComponent,
    MemberCardComponent,
    MemberEditComponent,
    PhotoEditorComponent,
    TextInputComponent,
    DateInputComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    BrowserAnimationsModule,
    FormsModule,
    SharedModule,
    NgxSpinnerModule,
    CommonModule,
    ReactiveFormsModule,
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: ErrorInterceptor,
      multi: true,
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: JwtInterceptor,
      multi: true,
    },
    { provide: HTTP_INTERCEPTORS, useClass: LoadingInterceptor, multi: true },
  ],

  bootstrap: [AppComponent],
})
export class AppModule {}
