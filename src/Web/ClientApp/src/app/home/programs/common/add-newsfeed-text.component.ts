import { tap } from 'rxjs/operators'
import { Location } from '@angular/common'
import { ActivatedRoute } from '@angular/router'
import { FormControl, FormGroup, Validators } from '@angular/forms'
import { Component, OnInit, ChangeDetectorRef, LOCALE_ID, Inject } from '@angular/core'
import { SectionDataService } from 'src/app/shared/services/section-data.service'
import { NewsFeedsClient } from 'src/app/shared/api.generated.clients'

@Component({
  selector: 'app-add-newsfeed-text',
  template: `
    <div>
      <div class="p-8 flex justify-between items-center border border-gray-400 rounded-t-lg">
        <button pButton type="button" label="Go Back" (click)="goBack()" i18n-label></button>
        <p class="text-2xl">{{ contentHeader }}</p>
        <button
          pButton
          [disabled]="contentForm.valid ? false : true"
          type="button"
          label="Save"
          i18n-label
          (click)="sendContentData()"
        ></button>
      </div>
      <div class="m-0 p-8 border border-t-0 border-gray-400 rounded-b-lg">
        <form [formGroup]="contentForm" class="p-fluid p-formgrid p-grid profile-form gap-6">
          <div class="w-full">
            <div class="p-field">
              <label for="title" i18n>Title <span class="text-red-500">*</span></label>
              <input
                [(ngModel)]="title"
                pInputText
                formControlName="title"
                id="enterTitle"
                type="text"
                class="text-lg"
                placeholder="Enter Title"
                i18n-placeholder
              />
              <div *ngIf="contentForm.get('title').invalid && contentForm.get('title').touched">
                <small
                  *ngIf="contentForm.get('title').errors.required"
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
                inputId="withoutgrouping"
                [useGrouping]="false"
                i18n-placeholder
              >
              </p-inputNumber>
              <div *ngIf="contentForm.get('order').invalid && contentForm.get('order').touched">
                <small
                  *ngIf="contentForm.get('order').errors.required"
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
                  contentForm.get('publishDate').invalid && contentForm.get('publishDate').touched
                "
              >
                <small
                  *ngIf="contentForm.get('publishDate').errors.required"
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
              <div *ngIf="contentForm.get('status').invalid && contentForm.get('status').touched">
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
          <div class="w-full">
            <p-editor
              [(ngModel)]="text"
              name="text"
              formControlName="text"
              [style]="{ height: '230px' }"
            ></p-editor>
            <div *ngIf="contentForm.get('text').invalid && contentForm.get('text').touched">
              <small
                *ngIf="contentForm.get('text').errors.required"
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
export class AddTextNewsfeedComponent implements OnInit {
  contentForm: FormGroup
  text = ''
  contentHeader = 'Add Text'
  textId: string = null
  date: Date
  dateTime: Date = new Date()
  isScheudledSelected = false
  status: any
  inform = false
  moduleId: string
  title = ''
  order: number
  publishDate: Date

  constructor(
    private _location: Location,
    private route: ActivatedRoute,
    private cdRef: ChangeDetectorRef,
    public sectionData: SectionDataService,
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
    const publishDateInput = this.contentForm.get('publishDate')
    if (event == 936510000) {
      this.isScheudledSelected = true
      publishDateInput.setValidators(Validators.required)
    } else {
      this.isScheudledSelected = false
      publishDateInput.setValidators(Validators.required)
    }
  }

  sendContentData() {
    if (this.contentForm.valid) {
      this.newsfeeds
        .newsfeedsPost(
          null,
          this.moduleId,
          this.textId,
          this.title,
          0,
          this.order,
          1, // text
          this.text,
          undefined,
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
    this.cdRef.detectChanges()
  }

  ngOnInit() {
    this.textId = this.route.snapshot.paramMap.get('textId')
    this.moduleId = this.route.snapshot.paramMap.get('modulesId')

    this.contentForm = new FormGroup({
      date: new FormControl(null),
      title: new FormControl(null, [Validators.required]),
      order: new FormControl(null, [Validators.required]),
      text: new FormControl(null, [Validators.required]),
      status: new FormControl(null, [Validators.required]),
      inform: new FormControl(false),
      publishDate: new FormControl(null),
    })

    if (this.textId) {
      this.contentHeader = 'Edit Content'

      this.newsfeeds.newsfeedsGet(this.textId).subscribe((data) => {
        this.title = data.name
        this.order = data.order
        this.text = data.text
        this.status = data.status
        this.inform = data.notifyUsers
        this.publishDate = new Date(data.publishDate)
        this.date = new Date(data.expiryDate)
      })
    }
  }
}
