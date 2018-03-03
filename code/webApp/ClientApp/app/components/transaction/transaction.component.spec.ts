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

        it('should have an empty transition list on create', function () {
            expect(component.transactions).toEqual([]);
        });

        it('should have an displace the information when ngOnInit is call', function () {
            component.ngOnInit();


            let transaction = [
                {
                    date: "12-12-1212",
                    entryType: "setupNetwork",
                    participant: "John",
                    value: "1234",
                    transactionId: "123"
                },
                {
                    date: "12-13-1212",
                    entryType: "addAsset",
                    participant: "John",
                    value: "4563",
                    transactionId: "342"
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
    });
