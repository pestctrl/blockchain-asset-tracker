import { assert } from 'chai';
import { AssetsComponent } from './assets.component';
import { TestBed, async, ComponentFixture } from '@angular/core/testing';

let fixture: ComponentFixture<AssetsComponent>;

describe('Assets Component',
    () => {
        beforeEach(() => {
            TestBed.configureTestingModule({ declarations: [AssetsComponent] });
            fixture = TestBed.createComponent(AssetsComponent);
            fixture.detectChanges();
        });

        it('should displace a title', async(() => {
            const titleText = fixture.nativeElement.querySelector('h1').textContent;
            expect(titleText).toEqual('Assets Registry');
        }));

        it('should have data when input new data',
            async(() => {
                const buttonValue = fixture.nativeElement.querySelector('strong');
                const button = fixture.nativeElement.querySelector('button');
                button.click();
                fixture.detectChanges();
                expect(buttonValue.textContent).toEqual('Son');
            }));


    });