﻿import { Component, OnInit } from '@angular/core';
import { TransactionService } from './transaction.service'
import { ITransaction } from './transaction'
﻿
﻿

@Component({
    selector: 'transaction',
    templateUrl: './transaction.component.html',
    providers: [TransactionService]
})

export class TransactionComponent implements OnInit {

    transactions: ITransaction[] = [];

    constructor(private transactionService: TransactionService) {
    }

    async ngOnInit() {
        this.transactions = await this.transactionService.getTransaction();
    }
    
    searchResult: ITransaction;

    searchTransaction(transactionId: string) {
        this.searchResult = ((this.transactions.find(x => x.transactionId === transactionId)) as ITransaction);
    };

}







