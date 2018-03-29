import { Component, OnInit } from '@angular/core';
import { Http } from '@angular/http';
import { TransactionService } from '../transaction/transaction.service';
import { google, GoogleMap } from '@agm/core/services/google-maps-types';
import { LatLngBounds } from '@agm/core';

@Component({
	selector: 'maps',
	templateUrl: './maps.component.html',
	styleUrls: ['./maps.component.css'], 
	providers: [TransactionService]
})
export class MapsComponent implements OnInit {
	zoom: number = 5;
	lat: number = 51.678418;
	lng: number = 9.809007;
	markers: marker[];
	map: any;

	ngOnInit() {
		this.mapsService.getTransaction()
			.subscribe(result => {
				console.log("Transactions received!");
				this.markers = result;
				this.lat = this.markers[0].latitude;
				this.lng = this.markers[0].longitude;
			});
		console.log("ngOnInit finish");
	}

	protected mapReady(map: any) {
		console.log("Map is ready!");
		this.map = map;
	}

	constructor(private mapsService : TransactionService) {
	}
}

interface marker {

	latitude: number;
	longitude: number;
}
