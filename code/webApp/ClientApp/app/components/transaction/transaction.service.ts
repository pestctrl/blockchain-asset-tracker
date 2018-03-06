import { Injectable } from '@angular/Core';
import { ITransaction } from './transaction'
import { Http, Response } from '@angular/http'
import { Observable } from 'rxjs/Observable'
import 'rxjs/add/operator/map'

@Injectable()
export class TransactionService {

    constructor(private http: Http) { }
    
    getTransaction(): Observable<ITransaction[]> {
        return this.http.get("http://129.213.108.205:3000/api/org.acme.biznet.Trade")
            .map((response: Response) => <ITransaction[]>response.json());
    }
}