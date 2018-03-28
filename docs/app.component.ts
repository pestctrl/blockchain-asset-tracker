import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
	
	zoom: number =5;
	
	
	lat: number = 51.678418;
	lng: number = 9.809007;

  markers: marker[] = [
  {
	  lat: 51.678418,
	  lng: 9.809007,
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
  
  constructor(){
  }



		

    }

interface marker{

	lat: number;
	lng: number;
}
