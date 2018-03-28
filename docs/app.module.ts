import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';


import { AppComponent } from './app.component';
import { AgmCoreModule } from '@agm/core';


@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
	AgmCoreModule.forRoot({
      apiKey: 'AIzaSyBrBJaey9uhFkm2XarLV6ecFnOnQ8HQ7Ys'
    })
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
