﻿/// <reference path="../../../../node_modules/@types/jasmine/index.d.ts" />

import { TransactionComponent } from './transaction.component';
import { TestBed, async, ComponentFixture } from '@angular/core/testing';


describe('Transactioncomponent tests',
    () => {
        let fixture: ComponentFixture<TransactionComponent>;
        let component: TransactionComponent;

        beforeEach(async () => {
            TestBed.configureTestingModule({
                declarations: [TransactionComponent]
            });

        });

        beforeEach(() => {
            fixture = TestBed.createComponent(TransactionComponent);
            component = fixture.componentInstance;
        });

        it('should pass this test', function () {
            expect(true).toBeTruthy();
        });

        it('should have selector set',
            function () {
                const annotations = Reflect.getMetadata('annotations', TransactionComponent)[0];

                expect(annotations.selector).toEqual('transaction');
            });

        it('should have templateUrl set',
            function () {
                const annotations = Reflect.getMetadata('annotations', TransactionComponent)[0].template;
               // fixture.detectChanges();
                expect(annotations.templateUrl).toEqual('sd');
            });

        it('should have an empty transition list on create', function () {
            expect(component.transactions).toEqual([]);
        });


/*
        it('should have an displace the information when ngOnInit is call', function () {
            component.ngOnInit();


            let transaction = [
                {
                    "$class": "org.acme.biznet.Trade",
                    "property": "resource:org.acme.biznet.Property#Asset%20A",
                    "newOwner": "resource:org.acme.biznet.Trader#TRADER3",
                    "transactionId": "3ea7f320f98957afc09e7780ba4b3be7aa8579ab55218b0a91881c2577138661",
                    "timestamp": "2018-03-05T22:49:13.553Z"
                },
                {
                    "$class": "org.acme.biznet.Trade",
                    "property": "resource:org.acme.biznet.Property#Asset%20A",
                    "newOwner": "resource:org.acme.biznet.Trader#TRADER2",
                    "transactionId": "4c00e0194de55c1e56916937edaeeb4910a9c181557936f4ed79668b75c755c6",
                    "timestamp": "2018-03-05T22:41:47.457Z"
                }
            ];

            expect(component.transactions).toEqual(transaction);
        });


        it('Displays the title', async(() => {

            const titleText = fixture.nativeElement.querySelector('h2').textContent;
            expect(titleText).toEqual('Search Result');
        }));


        it('Displays transaction from search input', async(() => {

            const searchButton = fixture.nativeElement.querySelector('button');
            const searchValue = fixture.nativeElement.querySelector('input');
            searchValue.value = "123";
            fixture.detectChanges();
            searchButton.click();
            fixture.detectChanges();

            let searchTransaction =
                {
                    date: "12-12-1212",
                    entryType: "setupNetwork",
                    participant: "John",
                    value: "1234",
                    transactionId: "123"
                };
            expect(component.searchResult).toEqual(searchTransaction);
        }));
        */
    });
