import { tap } from 'rxjs/operators'
import { Location } from '@angular/common'
import { ActivatedRoute } from '@angular/router'
import { FileUploadValidators } from '@iplab/ngx-file-upload'
import { FormControl, FormGroup, Validators } from '@angular/forms'
import { ChangeDetectorRef, Component, OnInit, LOCALE_ID, Inject } from '@angular/core'
import { SectionDataService } from 'src/app/shared/services/section-data.service'
import { NewsFeedsClient } from 'src/app/shared/api.generated.clients'

@Component({
  selector: 'app-add-newsfeed-document',
  template: `
    <div>
      <div class="p-8 flex justify-between items-center border border-gray-400 rounded-t-lg">
        <button pButton type="button" label="Go Back" (click)="goBack()" i18n-label></button>
        <p class="text-2xl">{{ attachmentHeader }}</p>
        <button
          pButton
          [disabled]="documentForm.valid && !invalidFormat ? false : true"
          type="button"
          label="Save"
          i18n-label
          (click)="sendAttachmentData()"
        ></button>
      </div>
      <div class="m-0 p-8 border border-t-0 border-gray-400 rounded-b-lg">
        <form [formGroup]="documentForm" class="p-fluid p-formgrid p-grid profile-form gap-6">
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
              <div *ngIf="documentForm.get('order').invalid && documentForm.get('order').touched">
                <small
                  *ngIf="documentForm.get('order').errors.required"
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
              <label for="title" i18n>Attachment Title <span class="text-red-500">*</span></label>
              <input
                [(ngModel)]="title"
                pInputText
                formControlName="title"
                id="title"
                type="text"
                class="text-lg"
                placeholder="Enter Attachment Title"
                i18n-placeholder
              />
              <div *ngIf="documentForm.get('title').invalid && documentForm.get('title').touched">
                <small
                  *ngIf="documentForm.get('title').errors.required"
                  controlName="title"
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
                  documentForm.get('publishDate').invalid && documentForm.get('publishDate').touched
                "
              >
                <small
                  *ngIf="documentForm.get('publishDate').errors.required"
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
              <div *ngIf="documentForm.get('status').invalid && documentForm.get('status').touched">
                <small
                  *ngIf="documentForm.get('status').errors.required"
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
            <file-upload
              [(ngModel)]="files"
              (change)="checkUploadedFile()"
              [multiple]="false"
              formControlName="files"
              name="files"
              fileslimit="2"
            ></file-upload>
            <small *ngIf="invalidFormat" class="text-red-600 p-2">Wrong file format</small>
          </div>
        </form>
      </div>
    </div>
  `,
})
export class AddNewsfeedDocumentComponent implements OnInit {
  title: string
  date: Date
  dateTime: Date = new Date()
  files: File[]
  status: any
  inform = false
  isScheudledSelected = false
  order: number
  documentId: string = null
  invalidFormat = false
  attachmentHeader = 'Add Document'
  allUploadedFormats: Array<string> = ['docx', 'xlsx', 'pptx', 'pdf', 'bmp', 'jpg', 'jpeg', 'png']
  moduleId: string
  publishDate: Date

  public uploadedFiles: Array<File> = []

  private filesControl = new FormControl(null, FileUploadValidators.filesLimit(1))

  public documentForm = new FormGroup({
    files: this.filesControl,
  })

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
    const publishDateInput = this.documentForm.get('publishDate')
    if (event == 936510000) {
      this.isScheudledSelected = true
      publishDateInput.setValidators(Validators.required)
    } else {
      this.isScheudledSelected = false
      publishDateInput.setValidators(Validators.required)
    }
  }

  checkUploadedFile() {
    this.invalidFormat = !this.allUploadedFormats.some((item) =>
      this.documentForm.value.files[0].name.split('.').pop().includes(item)
    )
  }

  async sendAttachmentData() {
    let uploadedDocument = null

    if (this.documentForm.value.files) {
      uploadedDocument = {
        data: this.documentForm.value.files[0],
        fileName: this.documentForm.value.files[0].name,
      }
    } else if (this.documentId) {
      uploadedDocument = null
    }

    if (this.documentForm.valid && !this.invalidFormat) {
      this.newsfeeds
        .newsfeedsPost(
          uploadedDocument,
          this.moduleId,
          this.documentId,
          this.title,
          0, // duration
          this.order,
          3, // document
          null,
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

  async ngOnInit() {
    this.moduleId = this.route.snapshot.paramMap.get('modulesId')
    this.documentId = this.route.snapshot.paramMap.get('documentId')

    this.documentForm = new FormGroup({
      title: new FormControl(null, [Validators.required]),
      order: new FormControl(null, [Validators.required]),
      files: new FormControl(null, [
        this.documentId ? Validators.nullValidator : Validators.required,
      ]),
      date: new FormControl(null),
      status: new FormControl(null, [Validators.required]),
      inform: new FormControl(false),
      publishDate: new FormControl(null),
    })

    if (this.documentId) {
      this.attachmentHeader = 'Edit Document'

      this.newsfeeds.newsfeedsGet(this.documentId).subscribe((data) => {
        this.title = data.name
        this.order = data.order
        this.status = data.status
        this.inform = data.notifyUsers
        this.publishDate = new Date(data.publishDate)
        this.date = new Date(data.expiryDate)
      })
    }
  }
}
