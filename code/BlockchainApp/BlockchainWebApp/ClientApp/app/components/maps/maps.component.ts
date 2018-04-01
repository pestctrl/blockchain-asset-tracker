import { Component, AfterViewInit, OnInit } from '@angular/core';
import { Http } from '@angular/http';
import { TransactionService } from '../transaction/transaction.service';
import { google, GoogleMap } from '@agm/core/services/google-maps-types';
import { LatLngBounds } from '@agm/core';
import { Observable } from 'rxjs/Observable';

@Component({
	selector: 'maps',
	templateUrl: './maps.component.html',
	styleUrls: ['./maps.component.css'], 
	providers: [TransactionService]
})
export class MapsComponent implements OnInit {
	zoom: number = 15;
	lat: number = 51.678418;
    lng: number = 9.809007;
    markers: marker[];

    async ngOnInit() {
        this.markers = await this.mapsService.getTransaction();
        let lastElement = this.markers.length - 1;
        this.lat = this.markers[lastElement].latitude;
        this.lng = this.markers[lastElement].longitude;
    }

    

    constructor(private mapsService: TransactionService) {
    }
}

interface marker {

	latitude: number;
	longitude: number;
}
