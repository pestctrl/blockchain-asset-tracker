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
  locationsToNumber: ILocationNum[];
	zoom: number = 10;
    lat: number = 29.72552
    lng: number = -95.348388


 constructor(private locationService: LocationService){
  }	
 
 ngOnInit() {

 }
    

 searchLocations(locationID: string) {
    /*
    this.locationsToNumber = [
      {
        latitude: parseFloat('	29.72552'),
        longitude: parseFloat('-95'),
      },
      {
        latitude: 30,
        longitude: -96,
      },
      {
        latitude: 28,
        longitude: -97,
      }
    ];
    */
    this.locationService.getLocations(locationID)
     .subscribe((locationsData) => this.locations = locationsData);



   /*
   this.locationsToNumber[0].latitude = parseFloat(this.locations[0].latitude)
   this.locationsToNumber[0].longitude = parseFloat(this.locations[0].longiture)
   this.locationsToNumber[1].latitude = parseFloat(this.locations[0].latitude)
   this.locationsToNumber[1].longitude = parseFloat(this.locations[0].longiture)
   this.locationsToNumber[2].latitude = parseFloat(this.locations[0].latitude)
   this.locationsToNumber[2].longitude = parseFloat(this.locations[0].longiture)

   */
   /*
    this.locationsToNumber[0].latitude = 29
    this.locationsToNumber[0].longitude = -95
    this.locationsToNumber[1].latitude = 28
    this.locationsToNumber[1].longitude =-96
    this.locationsToNumber[2].latitude = 27
    this.locationsToNumber[2].longitude = -94
   

   
   */

   
   

    


    }
  }
  

  

    

