import { Component, OnInit } from '@angular/core'
import { IListUserSurveysUrlViewModel, SurveysClient } from 'src/app/shared/api.generated.clients'
import { MessageService } from 'primeng/api'

@Component({
  selector: 'app-survey-list',
  templateUrl: './survey-list.component.html',
  styleUrls: ['./survey-list.component.scss'],
  providers: [MessageService]
})
export class SurveyListComponent implements OnInit {
  ready = false
  surveysData: IListUserSurveysUrlViewModel[]

  constructor(private surveys: SurveysClient, private messageService: MessageService) { }

  ngOnInit(): void {
    this.ready = false
    this.surveys.suveys().subscribe((data) => {
      this.surveysData = data
      this.ready = true
    })
  }
  disabledItemClick() {
    this.messageService.add({
      summary: $localize`Invalid operation`,
      detail: $localize`Please complete your profile first!`,
      severity: 'warn',
      closable: true,
      life: 5000,
    })
  }
}
