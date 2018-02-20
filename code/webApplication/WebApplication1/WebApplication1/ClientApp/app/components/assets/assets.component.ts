import { Component, Inject } from '@angular/core';
import { Http } from '@angular/http';

@Component({
    selector: 'assets',
    templateUrl: './assets.component.html'
})
export class AssetsComponent {
    public assets: Assets[];

    constructor(http: Http, @Inject('BASE_URL') baseUrl: string) {
        http.get(baseUrl + 'api/SampleData/Assetsinfo').subscribe(result => {
            this.assets = result.json() as Assets[];
        }, error => console.error(error));
    }
}

interface Assets {
    classId: string;
    assetId: string;
    owner: string;
    value: string;
}