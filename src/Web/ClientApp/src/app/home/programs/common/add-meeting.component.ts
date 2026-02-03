import { Location } from '@angular/common'
import { FormControl, FormGroup, Validators } from '@angular/forms'
import { ChangeDetectorRef, Component, OnInit } from '@angular/core'
import { ActivatedRoute } from '@angular/router'
import { ContentsClient } from 'src/app/shared/api.generated.clients'
import { SectionDataService } from 'src/app/shared/services/section-data.service'
import { tap } from 'rxjs/operators'

@Component({
  selector: 'app-add-meeting',
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
                [(ngModel)]="meetingTitle"
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
              [(ngModel)]="date"
              [minDate]="dateTime"
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
        </form>
      </div>
    </div>
  `,
})
export class AddMeetingComponent implements OnInit {
  date: Date
  order: number
  meetingData: any
  duration: number
  sectionId: string
  meetingId: string
  meetingLink: string
  meetingTitle: string
  meetingForm: FormGroup
  dateTime: Date = new Date()
  meetingHeader = 'Add Meeting'

  constructor(
    private _location: Location,
    private route: ActivatedRoute,
    private cdr: ChangeDetectorRef,
    private contents: ContentsClient,
    private sectionData: SectionDataService
  ) {
    this.dateTime.setDate(this.dateTime.getDate())
  }

  async sendMeetingData() {
    this.meetingId = this.route.snapshot.paramMap.get('meetingId')
      ? this.route.snapshot.paramMap.get('meetingId')
      : undefined

    if (this.meetingForm.valid) {
      await Promise.all([
        this.contents
          .contentsPost(
            undefined,
            this.sectionId,
            this.meetingId,
            this.meetingForm.value.title,
            this.meetingForm.value.duration,
            this.meetingForm.value.order,
            4,
            undefined,
            this.meetingForm.value.meetingLink,
            this.meetingForm.value.date
          )
          .pipe(
            tap(() => {
              this.sectionData.redirectBack(2)
            })
          )
          .toPromise(),
      ])
    }
  }

  goBack() {
    this._location.back()
  }

  ngAfterViewChecked() {
    this.cdr.detectChanges()
  }

  async ngOnInit() {
    this.sectionId = this.route.snapshot.paramMap.get('sectionId')
    this.meetingId = this.route.snapshot.paramMap.get('meetingId')

    this.meetingForm = new FormGroup({
      date: new FormControl(null, [Validators.required]),
      title: new FormControl(null, [Validators.required]),
      order: new FormControl(null, [Validators.required]),
      duration: new FormControl(null, [Validators.required]),
      meetingLink: new FormControl(null, [Validators.required]),
    })

    if (this.meetingId) {
      this.meetingHeader = 'Edit Meeting'
      await Promise.all([
        this.contents.sectionContents(this.sectionId).subscribe((data) => {
          this.meetingData = data.filter((item) => item.id == this.meetingId)
          this.order = this.meetingData[0].order
          this.date = this.meetingData[0].startDate
          this.meetingLink = this.meetingData[0].url
          this.duration = this.meetingData[0].duration
          this.meetingTitle = this.meetingData[0].name
        }),
      ])
    }
  }
}
