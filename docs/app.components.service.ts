import { Injectable } from '@angular/Core';
import { ILocation } from './app'
import { Http, Response } from '@angular/http'
import { Observable } from 'rxjs/Observable'
import 'rxjs/add/operator/map'

@Injectable()
export class LocationService {

  constructor(private http: Http) {}
  

  getLocations(locationID: string): Observable<ILocation[]> {
    return this.http.get("http://129.213.108.205:3000/api/queries/AssetHistory?propId=resource%3Aorg.acme.biznet.Property%23Property%2520A")
      .map((response: Response) => <ILocation[]>response.json());
  }
  
  /*
   getLocations(): ILocation[] { 
     return [
  {
      lat: '51.678418',
      lng: '9.809007',
  },
  {
  lat: '51.678418',
    lng: '6.809007',
  },
{
  lat: '49.678418',
    lng:' 6.809007',
  },
{
  lat: '49.678418',
    lng: '9.809007',
  }];
   }*/
}
