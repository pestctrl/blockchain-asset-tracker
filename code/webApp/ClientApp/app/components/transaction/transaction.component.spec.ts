/// <reference path="../../../../node_modules/@types/jasmine/index.d.ts" />
import { assert } from 'chai';
import { TransactionComponent } from './transaction.component';
import { TestBed, async, ComponentFixture } from '@angular/core/testing';


describe('Transactioncomponent tests',
    () => {
        let fixture: ComponentFixture<TransactionComponent>;
        let component: TransactionComponent;

        beforeEach(async() => {
            TestBed.configureTestingModule({
                    declarations: [TransactionComponent]
                })
                .compileComponents();
        });

        beforeEach(() => {
            fixture = TestBed.createComponent(TransactionComponent);
            fixture.detectChanges();
            component = fixture.componentInstance;
        });

        it('should pass this test', function() {
            expect(true).toBeTruthy();
        });

        it('should have selector set',
            function() {
                const annotations = Reflect.getMetadata('annotations', TransactionComponent)[0];

                expect(annotations.selector).toEqual('transaction');
            });

        it('should have an empty transations list on create', function () {
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


        //console.log(date);
        //initialize value and use button click to run search function
        //expect(date).toEqual("Test");
        const searchButton = fixture.nativeElement.querySelector('button');
        const searchValue = fixture.nativeElement.querySelector('input');
        searchValue.value = "123";
        searchButton.click();
        fixture.detectChanges();
        const date = fixture.nativeElement.querySelector('#date');
        const entry = fixture.nativeElement.querySelector('#entry');
        const participant = fixture.nativeElement.querySelector('#participant');
        const value = fixture.nativeElement.querySelector('#value');
        const transactionId = fixture.nativeElement.querySelector('#transactionId');
        //console.log(date);
        //matches searchtransaction with default value
        expect(date.innerHTML).toEqual("12-12-1212");
     
    }));

});