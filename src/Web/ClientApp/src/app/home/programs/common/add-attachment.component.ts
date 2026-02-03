import { tap } from 'rxjs/operators'
import { Location } from '@angular/common'
import { ActivatedRoute } from '@angular/router'
import { FileUploadValidators } from '@iplab/ngx-file-upload'
import { FormControl, FormGroup, Validators } from '@angular/forms'
import { ChangeDetectorRef, Component, OnInit } from '@angular/core'
import { ContentsClient } from 'src/app/shared/api.generated.clients'
import { SectionDataService } from 'src/app/shared/services/section-data.service'

@Component({
  selector: 'app-add-attachment',
  template: `
    <div>
      <div class="p-8 flex justify-between items-center border border-gray-400 rounded-t-lg">
        <button pButton type="button" label="Go Back" (click)="goBack()" i18n-label></button>
        <p class="text-2xl">{{ attachmentHeader }}</p>
        <button
          pButton
          [disabled]="attachmentForm.valid && !invalidFormat ? false : true"
          type="button"
          label="Save"
          i18n-label
          (click)="sendAttachmentData()"
        ></button>
      </div>
      <div class="m-0 p-8 border border-t-0 border-gray-400 rounded-b-lg">
        <form [formGroup]="attachmentForm" class="p-fluid p-formgrid p-grid profile-form gap-6">
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
              <div
                *ngIf="attachmentForm.get('title').invalid && attachmentForm.get('title').touched"
              >
                <small
                  *ngIf="attachmentForm.get('title').errors.required"
                  controlName="title"
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
              <div
                *ngIf="attachmentForm.get('order').invalid && attachmentForm.get('order').touched"
              >
                <small
                  *ngIf="attachmentForm.get('order').errors.required"
                  controlName="order"
                  class="p-invalid text-red-600"
                  i18n
                  >This field is required</small
                >
              </div>
            </div>
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
export class AddAttachmentComponent implements OnInit {
  title: string
  files: File[]
  sectionId: string
  documentData: any
  order = ''
  documentId: string = null
  invalidFormat = false
  attachmentHeader = 'Add Attachment'
  allUploadedFormats: Array<string> = ['docx', 'xlsx', 'pptx', 'pdf', 'bmp', 'jpg', 'jpeg', 'png']

  public uploadedFiles: Array<File> = []

  private filesControl = new FormControl(null, FileUploadValidators.filesLimit(1))

  public attachmentForm = new FormGroup({
    files: this.filesControl,
  })

  constructor(
    private _location: Location,
    private route: ActivatedRoute,
    private cdRef: ChangeDetectorRef,
    private contents: ContentsClient,
    public sectionData: SectionDataService
  ) {}

  checkUploadedFile() {
    this.invalidFormat = !this.allUploadedFormats.some((item) =>
      this.attachmentForm.value.files[0].name.split('.').pop().includes(item)
    )
  }

  async sendAttachmentData() {
    this.documentId = this.route.snapshot.paramMap.get('documentId')
      ? this.route.snapshot.paramMap.get('documentId')
      : undefined

    let uploadedDocument = null

    if (this.attachmentForm.value.files) {
      uploadedDocument = {
        data: this.attachmentForm.value.files[0],
        fileName: this.attachmentForm.value.files[0].name,
      }
    } else if (this.documentId) {
      uploadedDocument = null
    }

    this.documentId = this.route.snapshot.paramMap.get('documentId')
      ? this.route.snapshot.paramMap.get('documentId')
      : undefined

    if (this.attachmentForm.valid && !this.invalidFormat) {
      await Promise.all([
        this.contents
          .contentsPost(
            uploadedDocument,
            this.sectionId,
            this.documentId,
            this.attachmentForm.value.title,
            0,
            this.attachmentForm.value.order,
            3,
            undefined,
            undefined,
            new Date()
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
    this.cdRef.detectChanges()
  }

  async ngOnInit() {
    this.sectionId = this.route.snapshot.paramMap.get('sectionId')

    this.documentId = this.route.snapshot.paramMap.get('documentId')

    this.attachmentForm = new FormGroup({
      title: new FormControl(null, [Validators.required]),
      order: new FormControl(null, [Validators.required]),
      files: new FormControl(null, [
        this.documentId ? Validators.nullValidator : Validators.required,
      ]),
    })

    if (this.documentId) {
      this.attachmentHeader = 'Edit Attachment'
      this.documentId = undefined

      if (history.state.data) {
        this.documentId = history.state.data.contentId
      }
      if (this.documentId) {
        this.contents.sectionContents(history.state.data.sectionId).subscribe((data) => {
          this.documentData = data.filter((item) => item.id == this.documentId)
          this.title = this.documentData[0].name
          this.order = this.documentData[0].order
        })
      } else {
        this._location.back()
      }
    }
  }
}
