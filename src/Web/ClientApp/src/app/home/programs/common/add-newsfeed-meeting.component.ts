import { Location } from '@angular/common'
import { FormControl, FormGroup, Validators } from '@angular/forms'
import { ChangeDetectorRef, Component, OnInit, LOCALE_ID, Inject } from '@angular/core'
import { ActivatedRoute } from '@angular/router'
import { tap } from 'rxjs/operators'
import { NewsFeedsClient } from 'src/app/shared/api.generated.clients'

@Component({
  selector: 'app-add-newsfeed-meeting',
  template: `
    <div>
      <div class="p-8 flex justify-between items-center border border-gray-400 rounded-t-lg">
        <button pButton type="button" label="Go Back" (click)="goBack()" i18n-label></button>
        <p class="text-2xl">{{ meetingHeader }}</p>
        <button
          pButton
          [disabled]="meetingForm.valid ? false : true"
          type="button"
          label="Save"
          i18n-label
          (click)="sendMeetingData()"
        ></button>
      </div>
      <div class="m-0 p-8 border border-t-0 border-gray-400 rounded-b-lg">
        <form [formGroup]="meetingForm" class="p-fluid p-formgrid p-grid profile-form gap-6">
          <div class="w-full">
            <div class="p-field">
              <label for="title" i18n>Title <span class="text-red-500">*</span></label>
              <input
                [(ngModel)]="this.title"
                pInputText
                formControlName="title"
                id="enterTitle"
                type="text"
                class="text-lg"
                placeholder="Enter Title"
                i18n-placeholder
              />
              <div *ngIf="meetingForm.get('title').invalid && meetingForm.get('title').touched">
                <small
                  *ngIf="meetingForm.get('title').errors.required"
                  controlName="enterTitle"
                  class="p-invalid text-red-600"
                  i18n
                  >This field is required</small
                >
              </div>
            </div>
          </div>
          <div class="w-full">
            <div class="p-field">
              <label for="title" i18n>Meeting Link <span class="text-red-500">*</span></label>
              <input
                [(ngModel)]="meetingLink"
                pInputText
                formControlName="meetingLink"
                id="meetingLink"
                type="text"
                class="text-lg"
                placeholder="Meeting Link"
                i18n-placeholder
              />
              <div
                *ngIf="
                  meetingForm.get('meetingLink').invalid && meetingForm.get('meetingLink').touched
                "
              >
                <small
                  *ngIf="meetingForm.get('meetingLink').errors.required"
                  controlName="order"
                  class="p-invalid text-red-600"
                  i18n
                  >This field is required</small
                >
              </div>
            </div>
          </div>
          <div *ngIf="isScheudledSelected" class="w-full">
            <div class="p-field">
              <label for="publishDate" i18n-placeholder>
                Publish Date
                <span class="text-red-500">*</span>
              </label>
              <p-calendar
                [(ngModel)]="publishDate"
                [monthNavigator]="true"
                [yearNavigator]="true"
                appUseDateRange="freeDate"
                appUseUtc
                appPrimeNGi18n
                appendTo="body"
                inputId="publishDate"
                formControlName="publishDate"
                id="publishDate"
                type="text"
                class="text-lg"
                placeholder="Publish Date"
                i18n-placeholder
              ></p-calendar>
              <div
                *ngIf="
                  meetingForm.get('publishDate').invalid && meetingForm.get('publishDate').touched
                "
              >
                <small
                  *ngIf="meetingForm.get('publishDate').errors.required"
                  controlName="publishDate"
                  class="p-invalid text-red-600"
                  i18n
                >
                  This field is required
                </small>
              </div>
            </div>
          </div>
          <div class="w-full">
            <div class="p-field">
              <label for="status" i18n>Status<span class="text-red-500">*</span></label>
              <p-dropdown
                formControlName="status"
                id="status"
                [(ngModel)]="status"
                placeholder="Select a status"
                [options]="sectionStatusOptions"
                (ngModelChange)="onSelectType($event)"
                i18n-placeholder
              ></p-dropdown>
              <div *ngIf="meetingForm.get('status').invalid && meetingForm.get('status').touched">
                <small
                  *ngIf="contentForm.get('status').errors.required"
                  controlName="status"
                  class="p-invalid text-red-600"
                  i18n
                >
                  This field is required
                </small>
              </div>
            </div>
          </div>
          <div class="w-full">
            <div class="p-field">
              <label for="title" i18n>Duration <span class="text-red-500">*</span></label>
              <p-inputNumber
                [(ngModel)]="duration"
                pInputText
                formControlName="duration"
                id="duration"
                type="text"
                class="text-lg"
                placeholder="Enter Duration"
                mode="decimal"
                inputId="duration"
                [useGrouping]="false"
                i18n-placeholder
              >
              </p-inputNumber>
              <div
                *ngIf="meetingForm.get('duration').invalid && meetingForm.get('duration').touched"
              >
                <small
                  *ngIf="meetingForm.get('duration').errors.required"
                  controlName="duration"
                  class="p-invalid text-red-600"
                  i18n
                  >This field is required</small
                >
              </div>
            </div>
          </div>
          <div class="w-full">
            <div class="p-field">
              <label for="title" i18n>Order <span class="text-red-500">*</span></label>
              <p-inputNumber
                [(ngModel)]="order"
                pInputText
                formControlName="order"
                id="order"
                type="text"
                class="text-lg"
                placeholder="Enter Order"
                mode="decimal"
                inputId="order"
                [useGrouping]="false"
                i18n-placeholder
              >
              </p-inputNumber>
              <div *ngIf="meetingForm.get('order').invalid && meetingForm.get('order').touched">
                <small
                  *ngIf="meetingForm.get('order').errors.required"
                  controlName="order"
                  class="p-invalid text-red-600"
                  i18n
                  >This field is required</small
                >
              </div>
            </div>
          </div>
          <div class="w-full">
            <label for="title" i18n>Meeting Date <span class="text-red-500">*</span></label>
            <p-calendar
              [(ngModel)]="meetingDate"
              [minDate]="meetingDateTime"
              [showTime]="true"
              inputId="enterDate"
              placeholder="Enter Date"
              formControlName="date"
              i18n-placeholder
            ></p-calendar>
            <div *ngIf="meetingForm.get('date').invalid && meetingForm.get('date').touched">
              <small
                *ngIf="meetingForm.get('date').errors.required"
                controlName="enterTitle"
                class="p-invalid text-red-600"
                i18n
                >This field is required</small
              >
            </div>
          </div>
          <div class="w-full">
            <label for="title" i18n>Expire at</label>
            <p-calendar
              [(ngModel)]="date"
              [minDate]="dateTime"
              [showTime]="true"
              inputId="enterDate"
              placeholder="Enter expire date"
              formControlName="date"
              i18n-placeholder
            ></p-calendar>
          </div>
          <div class="p-field">
            <label for="present" i18n>Inform student</label>
            <p-checkbox
              #present
              [(ngModel)]="inform"
              id="present"
              class="ml-3"
              formControlName="inform"
              [binary]="true"
            ></p-checkbox>
          </div>
        </form>
      </div>
    </div>
  `,
})
export class AddNewsFeedMeetingComponent implements OnInit {
  date: Date
  meetingDate: Date
  order: number
  duration: number
  meetingId: string
  meetingLink: string
  meetingForm: FormGroup
  dateTime: Date = new Date()
  meetingDateTime: Date = new Date()
  meetingHeader = 'Add Meeting'
  isScheudledSelected = false
  status: any
  inform = false
  moduleId: string
  publishDate: Date
  title = ''
  text = ''

  constructor(
    private _location: Location,
    private route: ActivatedRoute,
    private cdr: ChangeDetectorRef,
    private newsfeeds: NewsFeedsClient,
    @Inject(LOCALE_ID) public locale: string
  ) {
    this.dateTime.setDate(this.dateTime.getDate())
  }

  sectionStatusOptions = [
    {
      value: 936510000,
      label: this.locale === 'en' ? 'Scheduled' : 'مقرر للنشر',
    },
    {
      value: 1,
      label: this.locale === 'en' ? 'Published immediately' : 'للنشر على الفور',
    },
  ]

  onSelectType(event) {
    const publishDateInput = this.meetingForm.get('publishDate')
    if (event == 936510000) {
      this.isScheudledSelected = true
      publishDateInput.setValidators(Validators.required)
    } else {
      this.isScheudledSelected = false
      publishDateInput.setValidators(Validators.required)
    }
  }

  sendMeetingData() {
    if (this.meetingForm.valid) {
      this.newsfeeds
        .newsfeedsPost(
          null,
          this.moduleId,
          this.meetingId,
          this.title,
          this.duration,
          this.order,
          4, // meeting
          this.text,
          this.meetingLink,
          this.status,
          this.inform,
          null,
          this.publishDate,
          this.date
        )
        .pipe(
          tap(() => {
            this.goBack()
          })
        )
        .toPromise()
    }
  }

  goBack() {
    this._location.back()
  }

  ngAfterViewChecked() {
    this.cdr.detectChanges()
  }

  async ngOnInit() {
    this.meetingId = this.route.snapshot.paramMap.get('meetingId')
    this.moduleId = this.route.snapshot.paramMap.get('modulesId')

    this.meetingForm = new FormGroup({
      date: new FormControl(null),
      meetingDate: new FormControl(null),
      title: new FormControl(null, [Validators.required]),
      order: new FormControl(null, [Validators.required]),
      duration: new FormControl(null, [Validators.required]),
      meetingLink: new FormControl(null, [Validators.required]),
      status: new FormControl(null, [Validators.required]),
      inform: new FormControl(false),
      publishDate: new FormControl(null),
      text: new FormControl(null),
    })

    if (this.meetingId) {
      this.meetingHeader = 'Edit Meeting'
      this.newsfeeds.newsfeedsGet(this.meetingId).subscribe((data) => {
        this.title = data.name
        this.meetingLink = data.url
        this.order = data.order
        this.text = data.text
        this.status = data.status
        this.duration = data.duration
        this.inform = data.notifyUsers
        this.publishDate = new Date(data.publishDate)
        this.date = new Date(data.expiryDate)
      })
    }
  }
}
