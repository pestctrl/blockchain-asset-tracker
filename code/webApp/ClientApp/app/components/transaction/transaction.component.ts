import { Component } from '@angular/core';

@Component({
    selector: 'transaction',
    templateUrl: './transaction.component.html'
})

export class TransactionComponent {
    public transactions: transaction = new transaction("12-12-1212", "setupNetwork", "John", "1234", "123");
    searchResult: transaction = new transaction("Test", "", "", "", "");


    searchTransaction(transactionId: string) {
        if (transactionId == this.transactions.transactionId) {
            this.searchResult = this.transactions;
        }
        
    };
}



class transaction {
    date: string;
    entryType: string;
    participant: string;
    value: string;
    transactionId: string;
    constructor(date: string, entryType: string,
        participant: string, value: string, transactionId: string) {
        this.date = date;
        this.entryType = entryType;
        this.participant = participant;
        this.value = value;
        this.transactionId = transactionId;
    }
    
}



