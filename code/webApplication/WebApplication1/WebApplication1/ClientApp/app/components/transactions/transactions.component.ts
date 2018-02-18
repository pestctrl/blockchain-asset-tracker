import { Component, Inject } from '@angular/core';
import { Http } from '@angular/http';

@Component({
    selector: 'transactions',
    templateUrl: './transactions.component.html'
})
export class TransactionsComponent {
    public transactions: TransactionHistory[];

    constructor(http: Http, @Inject('BASE_URL') baseUrl: string) {
        http.get(baseUrl + 'api/SampleData/TransactionHistories').subscribe(result => {
            this.transactions = result.json() as TransactionHistory[];
        }, error => console.error(error));
    }
}

interface TransactionHistory {
    getDate: string;
    getTime: string;
    entry: string;
    participant: string;
}