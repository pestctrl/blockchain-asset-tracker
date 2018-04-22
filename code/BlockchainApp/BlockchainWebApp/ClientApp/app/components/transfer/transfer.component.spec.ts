﻿/// <reference path="../../../../node_modules/@types/jasmine/index.d.ts" />

import { TransferComponent } from './transfer.component';
import { TestBed, async, ComponentFixture } from '@angular/core/testing';
﻿import { HttpModule } from '@angular/http';


describe('Transfercomponent tests',
    () => {
        let fixture: ComponentFixture<TransferComponent>;
        let component: TransferComponent;

        beforeEach(async () => {
            TestBed.configureTestingModule({
                imports: [ HttpModule ],
                declarations: [TransferComponent]
            });

        });
        
        beforeEach(() => {
            fixture = TestBed.createComponent(TransferComponent);
            component = fixture.componentInstance;
        });

        it('should pass this test', function () {
            expect(true).toBeTruthy();
        });

        it('should have selector set',
            function () {
                const annotations = Reflect.getMetadata('annotations', TransferComponent)[0];

                expect(annotations.selector).toEqual('transfer');
            });

        it('should have an empty transition list on create', function () {
            expect(component.transfers).toEqual([]);
        });

        it('Displays the title', async(() => {

            const titleText = fixture.nativeElement.querySelector('h2').textContent;
            expect(titleText).toEqual('Search Result');
        }));   
    });
