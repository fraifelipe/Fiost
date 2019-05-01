import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app.routing';
import { AppComponent } from './app.component';
import { SharedModule } from './shared/shared.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FeedComponent } from './applications/feed/feed.component';
import { ChatComponent } from './applications/chat/chat.component';
import { LoginComponent } from './applications/login/login.component';
import { RegisterComponent } from './applications/register/register.component';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { GamesComponent } from './applications/games/games.component';
import { FeedCardComponent } from './applications/feed/feed-card/feed-card.component';

@NgModule({
  declarations: [
    AppComponent,
    FeedComponent,
    ChatComponent,
    LoginComponent,
    RegisterComponent,
    GamesComponent,
    FeedCardComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    SharedModule,
    BrowserAnimationsModule,
    ReactiveFormsModule,
    HttpClientModule,
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }