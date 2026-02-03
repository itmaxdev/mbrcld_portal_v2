import {
  OnInit,
  Component,
  AfterViewChecked,
  ChangeDetectorRef,
  ViewEncapsulation,
} from '@angular/core'
import { FormControl, FormGroup, Validators } from '@angular/forms'
import { ActivatedRoute } from '@angular/router'
import { catchError, tap } from 'rxjs/operators'
import {
  IListTopicsByProgramIdViewModel,
  ListApplicantProjectsViewModel,
  ProjectsClient,
  TopicsClient,
} from 'src/app/shared/api.generated.clients'
import { SectionDataService } from 'src/app/shared/services/section-data.service'
import * as moment from 'moment'
import { MessageService } from 'primeng/api'
import { throwError } from 'rxjs'

@Component({
  selector: 'app-programs-project-edit',
  template: `
    <p-toast position="top-right" key="tr"></p-toast>
    <div>
      <div class="p-8 flex justify-between items-center border border-gray-400 rounded-t-lg">
        <div class="inline-flex items-center cursor-pointer contents" (click)="goBack()">
          <img src="assets/images/ico-arrow-left.svg" class="w-6 h-6" alt="" />
          <p class="go-back text-lg ml-2" i18n>
            Go Back
          </p>
        </div>
        <p class="text-2xl" i18n>Edit Project</p>
        <div class="w-16"></div>
      </div>
      <div class="m-0 p-8 border border-t-0 border-gray-400 rounded-b-lg form-header">
        <div *ngIf="ready; else loading">
          <form [formGroup]="projectForm" class="p-fluid p-formgrid p-grid profile-form gap-6">
            <div class="w-full grid grid-cols-2 gap-8">
              <div class="w-full">
                <div *ngIf="isDisabled != undefined" class="p-field">
                  <label for="topic" class="mb-2"
                    >Subject <span class="text-red-500">*</span></label
                  >
                  <input
                    [(ngModel)]="topic"
                    pInputText
                    formControlName="topic"
                    id="enterTopic"
                    type="text"
                    class="text-lg"
                    placeholder="Enter Topic"
                    [attr.disabled]="isDisabled ? true : null"
                    i18n-placeholder
                  />
                  <div *ngIf="projectForm.get('topic').invalid && projectForm.get('topic').touched">
                    <small controlName="topic" class="p-invalid" i18n>This field is required</small>
                  </div>
                </div>
              </div>
              <div class="w-full">
                <div *ngIf="isDisabled != undefined" class="p-field" dir="rtl">
                  <label for="topic" class="mb-2">الموضوع</label>
                  <input
                    [(ngModel)]="topic_ar"
                    pInputText
                    formControlName="topic_ar"
                    id="enterTopicAr"
                    type="text"
                    class="text-lg"
                    placeholder="الموضوع"
                    [attr.disabled]="isDisabled ? true : null"
                  />
                  <div
                    *ngIf="
                      projectForm.get('topic_ar').invalid && projectForm.get('topic_ar').touched
                    "
                  >
                    <small
                      *ngIf="projectForm.get('topic_ar').errors.required"
                      controlName="enterTopicAr"
                      class="p-invalid text-red-600"
                      i18n
                      >This field is required</small
                    >
                    <small
                      *ngIf="
                        !projectForm.get('topic_ar').errors.required &&
                        projectForm.get('topic_ar').errors.language
                      "
                      controlName="enterTopic"
                      class="p-invalid text-red-600"
                      >Please provide input in the correct language</small
                    >
                  </div>
                </div>
              </div>
            </div>
            <div *ngIf="projectType !== 2" class="w-full">
              <label for="topic" class="mb-2" i18n>Topic <span class="text-red-500">*</span></label>
              <p-dropdown
                [options]="topicsData"
                [(ngModel)]="selectedTopic"
                formControlName="topicDropDown"
                placeholder="Select a Topic"
                optionLabel="topicName"
                [disabled]="isDisabled ? true : null"
                [showClear]="true"
              ></p-dropdown>
            </div>
            <div *ngIf="presDate" class="w-full">
              <label for="presentationDate" class="mb-2">Presentation Date</label>
              <input
                [(ngModel)]="presDate"
                pInputText
                formControlName="presentationDate"
                id="presentationDate"
                type="text"
                class="text-lg"
                [attr.disabled]="true"
              />
            </div>
            <div class="w-full grid grid-cols-2 gap-8">
              <div class="w-full">
                <div *ngIf="isDisabled != undefined" class="p-field">
                  <label for="description" class="mb-2"
                    >Description <span class="text-red-500">*</span></label
                  >
                  <textarea
                    [(ngModel)]="description"
                    pInputTextarea
                    formControlName="description"
                    id="enterDescription"
                    type="text"
                    class="text-lg"
                    placeholder="Description"
                    [attr.disabled]="isDisabled ? true : null"
                  ></textarea>
                  <div
                    *ngIf="
                      projectForm.get('description').invalid &&
                      projectForm.get('description').touched
                    "
                  >
                    <small controlName="description" class="p-invalid" i18n
                      >This field is required</small
                    >
                  </div>
                </div>
              </div>
              <div class="w-full">
                <div *ngIf="isDisabled != undefined" class="p-field" dir="rtl">
                  <label for="topic" class="mb-2">الوصف</label>
                  <textarea
                    [(ngModel)]="description_ar"
                    pInputTextarea
                    formControlName="description_ar"
                    id="enterDescriptionAr"
                    type="text"
                    class="text-lg"
                    placeholder="الموضوع"
                    [attr.disabled]="isDisabled ? true : null"
                  ></textarea>
                  <div
                    *ngIf="
                      projectForm.get('description_ar').invalid &&
                      projectForm.get('description_ar').touched
                    "
                  >
                    <small
                      *ngIf="projectForm.get('description_ar').errors.required"
                      controlName="enterDescriptionAr"
                      class="p-invalid text-red-600"
                      i18n
                      >This field is required</small
                    >
                    <small
                      *ngIf="
                        !projectForm.get('description_ar').errors.required &&
                        projectForm.get('description_ar').errors.language
                      "
                      controlName="enterName"
                      class="p-invalid text-red-600"
                      >Please provide input in the correct language</small
                    >
                  </div>
                </div>
              </div>
            </div>
            <div *ngIf="!(projectStatus == 1)" class="w-full">
              <p-fileUpload
                name="uploadedFile"
                formControlName="uploadedFile"
                url="./upload.php"
                (onSelect)="onSelect($event)"
                maxFileSize="5242880"
                ngDefaultControl
                [showUploadButton]="false"
                [showCancelButton]="false"
              >
                <ng-template pTemplate="content"> </ng-template>
              </p-fileUpload>
            </div>
          </form>
          <div class="flex flex-row-reverse pt-8">
            <div *ngIf="projectStatus == 3 && attachmentUrl" class="flex ml-2">
              <a [href]="attachmentUrl" download>
                <button pButton type="button" label="Download Attachment" i18n-label></button>
              </a>
            </div>
            <div class="flex">
              <button
                *ngIf="isDraftBtn"
                (click)="saveAsDraftOrApprove(1)"
                [disabled]="projectForm.valid ? false : true"
                pButton
                type="button"
                label="Save As Draft"
                i18n-label
              ></button>
              <button
                *ngIf="isApprovalBtn"
                class="send-button"
                (click)="openApproveDialog()"
                [disabled]="projectForm.valid ? false : true"
                pButton
                type="button"
                label="Send for Approval"
                i18n-label
              ></button>
              <button
                *ngIf="isSaveBtn"
                class="ml-2"
                (click)="openSummaryDialog()"
                [disabled]="uploadedFile ? false : true"
                pButton
                type="button"
                label="Save"
                i18n-label
              ></button>
            </div>
          </div>
        </div>
        <ng-template #loading>
          <app-progress-spinner></app-progress-spinner>
        </ng-template>
      </div>
    </div>
    <p-dialog
      header="Add Attachment"
      [(visible)]="isOpenSummaryDialog"
      [style]="{ width: '50vw' }"
      [baseZIndex]="10000"
      i18n-header
    >
      <div>
        <form [formGroup]="approveFileForm">
          <p-fileUpload
            name="approveFile"
            formControlName="approveFile"
            url="./upload.php"
            (onSelect)="approveFileSelect($event)"
            maxFileSize="5242880"
            ngDefaultControl
            [showUploadButton]="false"
            [showCancelButton]="false"
          >
            <ng-template pTemplate="content"> </ng-template>
          </p-fileUpload>
        </form>
      </div>
      <ng-template pTemplate="footer">
        <p-button
          icon="pi pi-check"
          (click)="saveAsDraftOrApprove(2)"
          label="Yes"
          styleClass="p-button-text"
          i18n-label
        ></p-button>
        <p-button
          icon="pi pi-times"
          (click)="isOpenSummaryDialog = false"
          label="No"
          i18n-label
        ></p-button>
      </ng-template>
    </p-dialog>

    <p-dialog
      header="Summary"
      [(visible)]="summaryDialog"
      [style]="{ width: '50vw' }"
      [baseZIndex]="10000"
      i18n-header
    >
      <div class="w-full">
        <textarea
          [(ngModel)]="summaryText"
          rows="5"
          cols="30"
          pInputTextarea
          class="w-full"
        ></textarea>
      </div>
      <ng-template pTemplate="footer">
        <p-button
          icon="pi pi-check"
          (click)="saveProject()"
          label="Yes"
          styleClass="p-button-text"
          i18n-label
        ></p-button>
        <p-button
          icon="pi pi-times"
          (click)="summaryDialog = false"
          label="No"
          i18n-label
        ></p-button>
      </ng-template>
    </p-dialog>
  `,
  styles: [
    `
      .p-button-icon-only {
        display: none;
      }

      .go-back {
        color: #4a5568;
      }

      .send-button {
        margin-left: 10px;
      }

      .form-header {
        direction: initial;
      }
    `,
  ],
  encapsulation: ViewEncapsulation.None,
  providers: [MessageService],
})
export class ProgramsProjectEditComponent implements OnInit, AfterViewChecked {
  id: string
  topic: string
  ready = false
  topic_ar: string
  summaryText = ''
  uploadedFile: any
  isSaveBtn = false
  isDraftBtn = false
  selectedTopic: any
  description: string
  projectType: number
  summaryDialog = false
  isApprovalBtn = false
  projectStatus: number
  attachmentUrl: string
  description_ar: string
  projectForm: FormGroup
  approveUploadedFile: any
  approveFileForm: FormGroup
  isOpenSummaryDialog = false
  presDate: string = undefined
  isDisabled: boolean = undefined
  projectData: ListApplicantProjectsViewModel
  topicsData: IListTopicsByProgramIdViewModel[]

  constructor(
    private topics: TopicsClient,
    private route: ActivatedRoute,
    private cdr: ChangeDetectorRef,
    private projects: ProjectsClient,
    private messageService: MessageService,
    public sectionData: SectionDataService
  ) {}

  goBack() {
    this.sectionData.redirectBack(2)
  }

  onSelect(event) {
    this.uploadedFile = event
  }

  approveFileSelect(event) {
    this.approveUploadedFile = event
  }

  openApproveDialog() {
    this.isOpenSummaryDialog = true
  }

  openSummaryDialog() {
    this.summaryDialog = true
  }

  showError() {
    this.messageService.add({
      key: 'tr',
      severity: 'error',
      summary: 'Error',
      detail: 'Topic reached the Maximum capacity, please select a different one',
    })
  }

  saveAsDraftOrApprove(status) {
    let body: any = undefined
    if (status === 2 && this.approveUploadedFile) {
      body = {
        fileName: this.approveUploadedFile.currentFiles[0].name,
        data: this.approveUploadedFile.currentFiles[0],
      }
    }

    if (this.projectForm.valid) {
      this.projects
        .applicantProject(
          this.id,
          this.topic,
          this.topic_ar,
          this.description,
          this.description_ar,
          this.selectedTopic.id,
          status,
          body
        )
        .pipe(
          catchError((err) => {
            this.showError()
            this.isOpenSummaryDialog = false
            return throwError(err)
          }),
          tap(() => {
            this.sectionData.redirectBack(2)
          })
        )
        .toPromise()
    }
  }

  saveProject() {
    const body: any = {
      fileName: this.uploadedFile.currentFiles[0].name,
      data: this.uploadedFile.currentFiles[0],
    }

    if (this.uploadedFile) {
      this.projects
        .addAttachment(this.id, this.summaryText, body)
        .pipe(
          tap(() => {
            this.sectionData.redirectBack(2)
          })
        )
        .toPromise()
    }
  }

  ngOnInit() {
    this.showError()
    this.ready = false
    this.id = this.route.snapshot.paramMap.get('projectId')

    this.approveFileForm = new FormGroup({
      approveFile: new FormControl(null, [Validators.required]),
    })

    this.projectForm = new FormGroup({
      topicDropDown: new FormControl(null, [Validators.nullValidator]),
      topic: new FormControl(null, [Validators.required]),
      topic_ar: new FormControl(null, [Validators.nullValidator]),
      presentationDate: new FormControl(null, [Validators.nullValidator]),
      description: new FormControl(null, [Validators.required]),
      description_ar: new FormControl(null, [Validators.nullValidator]),
      uploadedFile: new FormControl(null, [Validators.nullValidator]),
    })

    this.projects.projects(this.id).subscribe((data) => {
      this.projectData = data

      this.topics.topics(this.projectData.programId).subscribe((data) => {
        this.topicsData = data

        if (this.projectData.topicId) {
          this.selectedTopic = this.topicsData.filter(
            (topic) => topic.id === this.projectData.topicId
          )[0]

          if (!this.selectedTopic) {
            this.selectedTopic = this.topicsData[0]
          }
        }

        this.projectStatus = this.projectData.projectStatus
        this.topic = this.projectData.topic
        this.projectType = this.projectData.type
        this.topic_ar = this.projectData.topic_Ar
        this.description = this.projectData.description
        this.description_ar = this.projectData.description_Ar
        this.attachmentUrl = this.projectData.attachmentUrl

        if (this.projectData.presentationDate) {
          this.presDate = moment(this.projectData.presentationDate)
            .lang('en')
            .format('dddd, MMMM Do YYYY, h:mm:ss a')
        }

        if (this.projectType === 2) {
          this.projectForm.get('topicDropDown').clearValidators()
        }

        switch (this.projectStatus) {
          case 1:
            this.isApprovalBtn = true
            this.isDraftBtn = true
            this.isDisabled = false
            break
          case 3:
            this.isSaveBtn = true
            this.isDisabled = true
            this.projectForm.get('topic').clearValidators()
            this.projectForm.get('topic_ar').clearValidators()
            this.projectForm.get('description').clearValidators()
            this.projectForm.get('description_ar').clearValidators()
            this.projectForm.controls['uploadedFile'].setValidators([Validators.required])
            break
        }
        this.ready = true
      })
    })
  }

  ngAfterViewChecked() {
    this.cdr.detectChanges()
  }
}
