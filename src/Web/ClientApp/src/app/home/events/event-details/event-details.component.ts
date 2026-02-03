import { ActivatedRoute } from '@angular/router'
import { Component, OnInit } from '@angular/core'
import { Location } from '@angular/common'
import {
  EventClient,
  AnswerForEventsDto,
  EventRegistrantClient,
} from 'src/app/shared/api.generated.clients'
import { SectionDataService } from 'src/app/shared/services/section-data.service'
import { tap } from 'rxjs/operators'
import { GlobalVariablesService } from 'src/app/shared/services/global-variables.service'
import {  ProfileClient} from 'src/app/shared/api.generated.clients'
@Component({
  selector: 'app-event-details',
  templateUrl: './event-details.component.html',
  styleUrls: ['./event-details.component.scss'],
})
/*
  questionnaireStatus
  0: idle
  1: busy
  2: success
  3: failed
*/
export class EventDetailsComponent implements OnInit {
  ready = false
  showQuestionnaire = false
  questionnaireStatus = 0
  questions = []
  answers = {}
  eventItem: any
  id: string
  duration: string
  fromDate: Date
  toDate: Date
  registrantData: any
  upToDate = false

  get showQuestionnaireDialog(): boolean {
    return this.showQuestionnaire
  }

  set showQuestionnaireDialog(value: boolean) {
    this.showQuestionnaire = value
  }

  constructor(
    private _location: Location,
    private route: ActivatedRoute,
    private event: EventClient,
    private sectionData: SectionDataService,
    private eventRegistrant: EventRegistrantClient,
    private globalVariables: GlobalVariablesService,
    private profileClient: ProfileClient

  ) {}

  registerUserToEvent() {

    this.profileClient.profileCompletion().pipe(
      tap((formProgress) => {
       // let myFlag: boolean | null = null;
        this.upToDate = (formProgress as { requiresUpdate: boolean | null }).requiresUpdate;
       // this.upToDate = formProgress.requiresUpdate;
        if (this.upToDate) {
      //const isConfirmed = confirm('Are your image, contact details, and job description up to date? Click OK to confirm and register, or Cancel to update your profile.');
      //if (!isConfirmed) {
      alert('Can you please confirm that your image, contact details, and job description are up to date before registering.');

        const urlParts = window.location.pathname.split('/');
        // pathname: /en/alumni/events/452647f0-a779-f011-b824-00155d149682
        const basePath = urlParts.slice(0, 3).join('/');
        // /en/alumni

        window.location.href = `${basePath}/profile/general-information`;
        return;
      //}
    }
    this.eventRegistrant
      .eventregistrant(this.id)
      .pipe(
        tap(() => {
          alert('✅ Thank you for registering, You will be provided with the event details once your attendance is confirmed.');
          window.location.reload()
        })
      )
          .toPromise()
      })
    )
      .toPromise()
  }

  goBack() {
    this._location.back()
  }

  openQuestionnaire() {
    this.showQuestionnaire = true
  }

  getQuestions() {
    this.event.questions(this.id).subscribe((data) => {
      if (data) {
        this.questions = data
        for (let x = 0; x < data.length; x++) {
          this.answers[data[x].id] = {
            answer: '',
            EventQuestionId: data[x].id,
          }
        }
      }
    })
  }

  submitQuestionnaire() {
    this.questionnaireStatus = 1
    const answers = Object.values<AnswerForEventsDto>(this.answers)
    this.event.answersPost(this.id, answers).subscribe(
      (resp) => {
        this.questionnaireStatus = 0
        this.fetchContent()
      },
      (resp) => {
        this.questionnaireStatus = 0
      }
    )
  }

  ngOnInit() {
    this.id = this.route.snapshot.paramMap.get('id')
    this.registrantData = {
      id: this.id,
      eventId: this.id,
    }
    this.fetchContent()
  }

  fetchContent() {
    this.event.eventsGetById(this.id).subscribe((data) => {
      this.eventItem = data
      if (this.eventItem.alreadyRegistered) {
        this.eventItem.hasQuestions = false
      } else {
        this.getQuestions()
      }
      this.duration = this.sectionData.convertMinuteToHours(this.eventItem.duration)
      this.fromDate = new Date(this.eventItem.fromDate)
      this.toDate = new Date(this.eventItem.toDate)
      this.ready = true
    })
  }
}
