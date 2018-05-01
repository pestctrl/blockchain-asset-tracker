﻿import { Component, OnInit } from '@angular/core';
import { TransferService } from './transfer.service'
import { Transfer } from './transfer'
﻿
﻿

@Component({
    selector: 'transfer',
    templateUrl: './transfer.component.html',
    providers: [TransferService]
})

export class TransferComponent implements OnInit {

    transfers: Transfer[] = [];

    constructor(private transferService: TransferService) {
    }

    async ngOnInit() {
        this.transfers = await this.transferService.getTransfers();
        console.log("Hello");
        console.log(this.transfers);
    }
    
    searchResult: Transfer;

    searchTransfers(transferId: string) {
        this.searchResult = ((this.transfers.find(x => x.transactionId === transferId)) as Transfer);
    };

}







