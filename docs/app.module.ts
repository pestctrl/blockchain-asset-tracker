import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpModule } from '@angular/http';

import { AppComponent } from './app.component';
import { AgmCoreModule } from '@agm/core';


@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    HttpModule,
	AgmCoreModule.forRoot({
      apiKey: 'AIzaSyBrBJaey9uhFkm2XarLV6ecFnOnQ8HQ7Ys'
    })
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
