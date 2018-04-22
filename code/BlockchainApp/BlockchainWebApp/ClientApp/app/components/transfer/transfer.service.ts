import { Injectable, Inject } from '@angular/core';
import { Http, Response } from '@angular/http'
import { Observable } from 'rxjs/Observable'
import 'rxjs/add/operator/toPromise';
import { getBaseUrl } from '../../app.browser.module';
import { Transfer } from './transfer';

@Injectable()
export class TransferService {

    baseUrl: string;
    constructor(private http: Http) {
        this.baseUrl = getBaseUrl();
    }

    async getTransfers(): Promise<Transfer[]> {
        //const response = await this.http.get("http://129.213.108.205:3000/api/org.example.biznet.Trade").toPromise();
        const response = await this.http.get(this.baseUrl + "api/Transfer").toPromise();

        return response.json();
    }


    async getPropertyHistory(propId: string): Promise<Transfer[]> {
        //const response = await this.http.get("http://129.213.108.205:3000/api/queries/PropertyHistory?propId=resource%3Aorg.example.biznet.Property%23" + encodeURI(propId)).toPromise();
        const response = await this.http.get(this.baseUrl + "api/Transfer/History/" + encodeURI(propId)).toPromise();
        return response.json();
    }
}