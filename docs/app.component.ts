import { Component, OnInit } from '@angular/core';
import { ILocation } from './app'
import { ILocationNum } from './appNumber'
import { LocationService } from './app.components.service'

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  providers: [LocationService]
})
export class AppComponent implements OnInit {
	

  locations: ILocation[];
  locationsToNumber: ILocationNum[] = [];
	zoom: number = 5;
	lat: number = 51.678418;
	lng: number = 9.809007;

 constructor(private locationService: LocationService){
  }	

  




   
  /*
  ngOnInit() {
    this.locations = this._locationService.getLocations();


    for (var i in this.locations) {
      this.locationsToNumber[i].lat = parseFloat(this.locations[i].lat)
      this.locationsToNumber[i].lng = parseFloat(this.locations[i].lng)
    }}
  
  */

    ngOnInit() {
      this.locationService.getLocations()
        .subscribe((locationsData) => this.locations = locationsData);

      this.locationsToNumber = [
        {
          lat: parseFloat('51.678418'),
          lng: parseFloat('9.809007'),
        },
        {
          lat: 51.678418,
          lng: 6.809007,
        },
        {
          lat: 49.678418,
          lng: 6.809007,
        },
        {
          lat: 49.678418,
          lng: 9.809007,
        }
      ];
      }
    

  searchLocations() {
    this.locationsToNumber = [
      {
        lat: parseFloat('51.678418'),
        lng: parseFloat('9.809007'),
      },
      {
        lat: 51.678418,
        lng: 6.809007,
      },
      {
        lat: 49.678418,
        lng: 6.809007,
      },
      {
        lat: 49.678418,
        lng: 9.809007,
      }
    ];
    }
  }
  

  

    

