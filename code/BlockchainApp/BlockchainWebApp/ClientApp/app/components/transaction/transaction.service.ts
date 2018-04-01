import { Injectable } from '@angular/Core';
import { ITransaction } from './transaction'
import { Http, Response } from '@angular/http'
import { Observable } from 'rxjs/Observable'
import 'rxjs/add/operator/toPromise';

@Injectable()
export class TransactionService {

    constructor(private http: Http) { }

    async getTransaction(): Promise<ITransaction[]> {
        const response = await this.http.get("http://129.213.108.205:3000/api/org.acme.biznet.Trade").toPromise();
        return response.json();
    }

    async getPropertyHistory(propId : string): Promise<ITransaction[]> {
        const response = await this.http.get("http://129.213.108.205:3000/api/queries/AssetHistory?propId=resource%3Aorg.acme.biznet.Property%23" + encodeURI(propId)).toPromise();
        return response.json();
    }
}