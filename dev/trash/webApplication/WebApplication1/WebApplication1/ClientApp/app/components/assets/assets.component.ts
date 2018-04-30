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

    class = ''; id = ''; owner = ''; value = '';

    createAsset() {
        this.class = "3211";
        this.id = "2223";
        this.owner = "Paul";
        this.value = "232";
    }


    asset = ['Son', 'Giang', 'Xing', 'Paul', 'Benson'];

    addAsset(newAsset: string) {
        if (newAsset) {
            this.asset.push(newAsset);
        }
    }
}

class Assets {
    class: string;
    assetId: string;
    owner: string;
    value: string;
}

