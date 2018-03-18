import { Component, Inject } from '@angular/core';
import { Http } from '@angular/http';

@Component({
    selector: 'participants',
    templateUrl: './participants.component.html'
})

export class ParticipantsComponent {
    public participants: Participants[];
    constructor(http: Http, @Inject('BASE_URL') baseUrl: string) {
        http.get(baseUrl + 'api/SampleData/Participantsinfo').subscribe(result => {
            this.participants = result.json() as Participants[];
        }, error => console.error(error));
    }
}

interface Participants {
    class: string;
    participantId: string;
    firstName: string;
}