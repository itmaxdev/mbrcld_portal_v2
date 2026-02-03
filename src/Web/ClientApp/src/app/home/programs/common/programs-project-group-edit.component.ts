import {
  AfterViewChecked,
  ChangeDetectorRef,
  Component,
  OnInit,
  ViewEncapsulation,
} from '@angular/core'
import { FormControl, FormGroup, Validators } from '@angular/forms'
import { ActivatedRoute } from '@angular/router'
import { tap } from 'rxjs/operators'
import { ProjectsClient, TopicsClient } from 'src/app/shared/api.generated.clients'
import { SectionDataService } from 'src/app/shared/services/section-data.service'
import * as moment from 'moment'

@Component({
  selector: 'app-programs-project-group-edit',
  template: `
    <div>
      <div class="p-8 flex justify-between items-center border border-gray-400 rounded-t-lg">
        <div class="inline-flex items-center cursor-pointer contents" (click)="goBack()">
          <img src="assets/images/ico-arrow-left.svg" class="w-6 h-6" alt="" />
          <p class="text-lg ml-2 text-gray-700" i18n>
            Go Back
          </p>
        </div>
        <p class="text-2xl text-header" i18n>Edit Group Project</p>
        <div class="w-20 w-20div"></div>
      </div>
      <div class="m-0 p-8 border border-t-0 border-gray-400 rounded-b-lg content-container">
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
                id="topic"
                optionLabel="topicName"
                [showClear]="true"
                [disabled]="isDisabled"
                i18n-placeholder
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
                [attr.disabled]="isDisabled ? true : null"
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
                    [attr.disabled]="true"
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
                maxFileSize="1000000"
                ngDefaultControl
                [showUploadButton]="false"
                [showCancelButton]="false"
                [disabled]="isDisabledUploadField"
              >
                <ng-template pTemplate="content"> </ng-template>
              </p-fileUpload>
            </div>
          </form>
          <div class="flex flex-row-reverse pt-8">
            <div class="flex gap-2">
              <a [href]="attachmentUrl" download>
                <button pButton type="button" label="Download Attachment" i18n-label></button>
              </a>
              <div [ngClass]="{ 'grid gap-2 saveButton': projectStatus == 1 }">
                <button
                  *ngIf="isSaveBtn"
                  (click)="openSummaryDialog()"
                  [disabled]="projectForm.valid ? false : true"
                  pButton
                  type="button"
                  label="Save"
                  i18n-label
                ></button>
              </div>
            </div>
          </div>
        </div>
        <ng-template #loading>
          <app-progress-spinner></app-progress-spinner>
        </ng-template>
      </div>
    </div>

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

      @media (max-width: 550px) {
        .w-20div {
          display: none;
        }

        .text-header {
          margin-left: 25px;
        }
      }

      @media (max-width: 335px) {
        .content-container {
          padding: 1rem;
        }

        .p-button {
          padding: 9px;
          font-size: 12px;
        }
      }
    `,
  ],
  encapsulation: ViewEncapsulation.None,
})
export class ProgramsProjectGroupEditComponent implements OnInit, AfterViewChecked {
  id: string
  ready = false
  topic: string
  summaryText = ''
  projectData: any
  topic_ar: string
  uploadedFile: any
  isSaveBtn = false
  topicsData: any[]
  allData: any = []
  isGrouped = false
  isParent: boolean
  isDraftBtn = false
  description: string
  projectType: number
  isApprovalBtn = false
  projectStatus: number
  selectedTopic: string
  attachmentUrl: string
  description_ar: string
  projectForm: FormGroup
  isDisabledUploadField = true
  presDate: string = undefined
  summaryDialog = false
  isDisabled: boolean = undefined

  constructor(
    private topics: TopicsClient,
    private route: ActivatedRoute,
    private cdr: ChangeDetectorRef,
    private projects: ProjectsClient,
    public sectionData: SectionDataService
  ) {}

  goBack() {
    this.sectionData.redirectBack(2)
  }

  onSelect(event) {
    this.uploadedFile = event
  }

  openSummaryDialog() {
    this.summaryDialog = true
  }

  saveProject() {
    if (this.projectStatus === 3 && !this.isParent) {
      const body: any = {
        fileName: this.uploadedFile.currentFiles[0].name,
        data: this.uploadedFile.currentFiles[0],
      }

      if (this.projectForm.valid) {
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
  }

  ngOnInit() {
    this.ready = false
    this.id = this.route.snapshot.paramMap.get('projectId')

    this.projectForm = new FormGroup({
      topicDropDown: new FormControl(null, [Validators.nullValidator]),
      topic: new FormControl(null, [Validators.nullValidator]),
      topic_ar: new FormControl(null, [Validators.nullValidator]),
      presentationDate: new FormControl(null, [Validators.nullValidator]),
      description: new FormControl(null, [Validators.nullValidator]),
      description_ar: new FormControl(null, [Validators.nullValidator]),
      uploadedFile: new FormControl(null, [Validators.required]),
    })

    this.projects.groupProjects().subscribe((groupedData) => {
      this.projectData = groupedData.filter((item) => item.id == this.id)[0]
      this.topics.topics(this.projectData.programId).subscribe((data) => {
        this.topicsData = data

        if (this.projectData.topicId) {
          this.selectedTopic = this.topicsData.filter(
            (topic) => topic.id === this.projectData.topicId
          )[0]
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

        this.isParent = this.projectData.isParent
        if (this.projectStatus === 3 && !this.isParent) {
          this.isDisabledUploadField = false
        }
        this.isSaveBtn = true
        this.isDisabled = true
        this.ready = true
      })
    })
  }

  ngAfterViewChecked() {
    this.cdr.detectChanges()
  }
}
