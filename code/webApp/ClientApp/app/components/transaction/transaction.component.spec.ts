/// <reference path="../../../../node_modules/@types/jasmine/index.d.ts" />
import { assert } from 'chai';
import { TransactionComponent } from './transaction.component';
import { TestBed, async, ComponentFixture } from '@angular/core/testing';

let fixture: ComponentFixture<TransactionComponent>;
let component: TransactionComponent;

describe('Sample Test', () => {
    it('true is true', () => expect(true).toBe(true));
});

describe('Transaction component', () => {
    beforeEach(() => {
        TestBed.configureTestingModule({ declarations: [TransactionComponent] });
        fixture = TestBed.createComponent(TransactionComponent);
        fixture.detectChanges();
    });


    it('Displays the title', async(() => {
        const titleText = fixture.nativeElement.querySelector('h2').textContent;
        expect(titleText).toEqual('Search Result');
    }));

    it('Displays transaction from search input', async(() => {
        const date = fixture.nativeElement.querySelector('#date');
        const entry = fixture.nativeElement.querySelector('#entry');
        const participant = fixture.nativeElement.querySelector('#participant');
        const value = fixture.nativeElement.querySelector('#value');
        const transactionId = fixture.nativeElement.querySelector('#transactionId');

        //console.log(date);
        //initialize value and use button click to run search function
        expect(date.innerHTML).toEqual("Test");
        const searchButton = fixture.nativeElement.querySelector('button');
        const searchValue = fixture.nativeElement.querySelector('input');
        searchValue.value = "123";
        searchButton.click();
        fixture.detectChanges();

        //console.log(date);
        //matches searchtransaction with default value
        expect(date.innerHTML).toEqual("12-12-1212");
        expect(entry.innerHTML).toEqual("setupNetwork");
        expect(participant.innerHTML).toEqual("John");
        expect(value.innerHTML).toEqual("1234");
        expect(transactionId.innerHTML).toEqual("123");
    }));

});