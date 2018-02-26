import { Injectable } from '@angular/Core';
import { ITransaction } from './transaction'

@Injectable()
export class TransactionService {
    getTransaction(): ITransaction[] {
        return [
            { date: "12-12-1212", entryType: "setupNetwork", participant: "John", value: "1234", transactionId: "123" },
            { date: "12-13-1212", entryType: "addAsset", participant: "John", value: "4563", transactionId: "342" }
        ];
    }
}