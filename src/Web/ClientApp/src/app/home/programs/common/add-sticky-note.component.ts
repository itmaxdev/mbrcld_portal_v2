import { Location } from '@angular/common'
import { ActivatedRoute } from '@angular/router'
import { FormControl, FormGroup, Validators } from '@angular/forms'
import { Component, OnInit, ChangeDetectorRef, LOCALE_ID, Inject } from '@angular/core'
import { ContentsClient } from 'src/app/shared/api.generated.clients'
import { SectionDataService } from 'src/app/shared/services/section-data.service'
import { NewsFeedsClient } from 'src/app/shared/api.generated.clients'

@Component({
  selector: 'app-add-stick-notes',
  template: `
    <div>
      <div class="p-8 flex justify-between items-center border border-gray-400 rounded-t-lg">
        <button pButton type="button" label="Go Back" (click)="goBack()" i18n-label></button>
        <p class="text-2xl">{{ contentHeader }}</p>
        <button
          pButton
          [disabled]="noteForm.valid ? false : true"
          type="button"
          label="Save"
          i18n-label
          (click)="sendContentData()"
        ></button>
      </div>
      <div class="m-0 p-8 border border-t-0 border-gray-400 rounded-b-lg">
        <form [formGroup]="noteForm" class="p-fluid p-formgrid p-grid profile-form gap-6">
          <div class="mb-4 w-full">
            <label for="text" i18n>Text <span class="text-red-500">*</span></label>
            <input
              [(ngModel)]="text"
              id="text"
              pInputText
              type="text"
              placeholder="Stick note text"
              name="newStick"
              i18n-placeholder
              formControlName="text"
            />
            <div *ngIf="noteForm.get('text').invalid && noteForm.get('text').touched">
              <small
                *ngIf="noteForm.get('text').errors.required"
                controlName="enterTitle"
                class="p-invalid text-red-600"
                i18n
                >This field is required</small
              >
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
              <div *ngIf="noteForm.get('order').invalid && noteForm.get('order').touched">
                <small
                  *ngIf="noteForm.get('order').errors.required"
                  controlName="order"
                  class="p-invalid text-red-600"
                  i18n
                  >This field is required</small
                >
              </div>
            </div>
          </div>
          <div *ngIf="!sectionId" class="w-full">
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
                  *ngIf="noteForm.get('publishDate').invalid && noteForm.get('publishDate').touched"
                >
                  <small
                    *ngIf="noteForm.get('publishDate').errors.required"
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
                <div *ngIf="noteForm.get('status').invalid && noteForm.get('status').touched">
                  <small
                    *ngIf="noteForm.get('status').errors.required"
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
            <div class="p-field mt-4">
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
          </div>
        </form>
      </div>
    </div>
  `,
})
export class AddStickyNotesComponent implements OnInit {
  newStick = ''
  contentHeader = 'Add Stick Note'
  noteForm: FormGroup
  isScheudledSelected = false
  moduleId: string
  sectionId: string
  title = ''
  order: number
  text = ''
  publishDate: Date
  dateTime: Date = new Date()
  status: any
  inform = false
  date: Date
  stickNoteId: string
  name: string

  constructor(
    private newsfeeds: NewsFeedsClient,
    private route: ActivatedRoute,
    private _location: Location,
    private contents: ContentsClient,
    public sectionData: SectionDataService,
    private cdRef: ChangeDetectorRef,
    @Inject(LOCALE_ID) public locale: string
  ) {}

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
    const publishDateInput = this.noteForm.get('publishDate')
    if (event == 936510000) {
      this.isScheudledSelected = true
      publishDateInput.setValidators(Validators.required)
    } else {
      this.isScheudledSelected = false
      publishDateInput.setValidators(Validators.required)
    }
  }

  sendContentData() {
    if (this.noteForm.valid) {
      if (this.sectionId) {
        this.contents
          .contentsPost(
            null,
            this.sectionId,
            this.stickNoteId,
            this.name,
            0,
            this.order,
            5, // stick note
            this.text,
            undefined
          )
          .subscribe(() => {
            this.goBack()
          })
      } else {
        this.newsfeeds
          .newsfeedsPost(
            null,
            this.moduleId,
            this.stickNoteId,
            this.name,
            0,
            this.order,
            5, // stick note
            this.text,
            undefined,
            this.status,
            this.inform,
            null,
            this.publishDate,
            this.date
          )
          .subscribe(() => {
            this.goBack()
          })
      }
    }
  }

  goBack() {
    this._location.back()
  }

  ngAfterViewChecked() {
    this.cdRef.detectChanges()
  }

  ngOnInit() {
    this.name = 'Stick note'
    this.moduleId = this.route.snapshot.paramMap.get('modulesId')
    this.sectionId = this.route.snapshot.paramMap.get('sectionId')
    this.stickNoteId = this.route.snapshot.paramMap.get('sticknoteId')

    this.noteForm = new FormGroup({
      order: new FormControl(null, [Validators.required]),
      text: new FormControl(null, [Validators.required]),
    })
    if (!this.sectionId) {
      this.noteForm.addControl('date', new FormControl(null))
      this.noteForm.addControl('status', new FormControl(null, [Validators.required]))
      this.noteForm.addControl('inform', new FormControl(null))
      this.noteForm.addControl('publishDate', new FormControl(null))
    }

    if (this.stickNoteId) {
      this.contentHeader = 'Edit Stick note'

      if (this.sectionId) {
        this.contents.sectionContents(this.sectionId).subscribe((data) => {
          const contentData = data.filter((item) => item.id == this.stickNoteId)
          this.text = contentData[0].text
          this.name = contentData[0].name
          this.order = contentData[0].order
        })
      } else {
        this.newsfeeds.newsfeedsGet(this.stickNoteId).subscribe((data) => {
          this.name = data.name
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
}
