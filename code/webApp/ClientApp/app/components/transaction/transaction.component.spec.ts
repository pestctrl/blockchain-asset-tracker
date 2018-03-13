﻿/// <reference path="../../../../node_modules/@types/jasmine/index.d.ts" />

import { TransactionComponent } from './transaction.component';
import { TestBed, async, ComponentFixture } from '@angular/core/testing';
﻿import { HttpModule } from '@angular/http';


describe('Transactioncomponent tests',
    () => {
        let fixture: ComponentFixture<TransactionComponent>;
        let component: TransactionComponent;

        beforeEach(async () => {
            TestBed.configureTestingModule({
                imports: [ HttpModule ],
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

        it('Displays the title', async(() => {

            const titleText = fixture.nativeElement.querySelector('h2').textContent;
            expect(titleText).toEqual('Search Result');
        }));   
    });
