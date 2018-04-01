import { Injectable, Inject } from '@angular/Core';
import { ITransaction } from './transaction'
import { Http, Response } from '@angular/http'
import { Observable } from 'rxjs/Observable'
import 'rxjs/add/operator/toPromise';
import { getBaseUrl } from '../../app.browser.module';

@Injectable()
export class TransactionService {

    baseUrl: string;
    constructor(private http: Http) {
        this.baseUrl = getBaseUrl();
    }

    async getTransaction(): Promise<ITransaction[]> {
        //const response = await this.http.get("http://129.213.108.205:3000/api/org.acme.biznet.Trade").toPromise();
        const response = await this.http.get(this.baseUrl + "api/Transaction").toPromise();

        return response.json();
    }


    async getPropertyHistory(propId : string): Promise<ITransaction[]> {
        //const response = await this.http.get("http://129.213.108.205:3000/api/queries/PropertyHistory?propId=resource%3Aorg.acme.biznet.Property%23" + encodeURI(propId)).toPromise();
        const response = await this.http.get(this.baseUrl + "api/Transaction/History/" + encodeURI(propId)).toPromise();
        return response.json();
    }
}