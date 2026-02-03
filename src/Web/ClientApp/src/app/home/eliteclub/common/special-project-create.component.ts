import { Component, OnInit, LOCALE_ID, Inject } from '@angular/core'
import { FormControl, FormGroup, Validators } from '@angular/forms'
import { ConfirmationService } from 'primeng/api'
import { ActivatedRoute } from '@angular/router'
import { Location } from '@angular/common'
import { tap } from 'rxjs/operators'
import {
  FileParameter,
  IListSpecialProjectTopicsViewModel,
  SpecialProjectsClient,
  SpecialProjectTopicsClient,
} from 'src/app/shared/api.generated.clients'
import { ProfessionalExperienceFacadeService } from '../../profile/professional-experience/professional-experience-facade.service'
import { ISectorOption } from '../../profile/professional-experience/models'

@Component({
  selector: 'app-project-ideas-create',
  template: `
    <div class="inline-flex items-center mb-8 cursor-pointer contents" (click)="goBack()">
      <img src="assets/images/ico-arrow-left.svg" class="w-6 h-6" alt="" />
      <p class="text-lg ml-2 text-gray-700" i18n>
        Go Back
      </p>
    </div>
    <div class="m-0 p-8 border border-gray-400 rounded-t-lg flex justify-center items-center">
      <h1 class="text-2xl" i18n>Add Special Project Idea</h1>
    </div>
    <div class="m-0 p-8 border border-t-0 border-gray-400 rounded-b-lg">
      <form class="p-fluid p-formgrid p-grid profile-form gap-6" [formGroup]="ideaForm">
        <div class="w-full mb-4">
          <label for="projectTopic" i18n>Topic</label>
          <p-dropdown
            formControlName="projectTopic"
            [options]="projectTopics"
            placeholder="Select a topic"
            i18n-placeholder
            optionLabel="specialProjectTopicName"
            [showClear]="true"
            inputId="specialProjectTopicId"
            [ngClass]="{ showClear: visibility }"
          ></p-dropdown>
        </div>
        <div class="w-full">
          <div class="p-field">
            <label for="name" i18n>Name</label>
            <input
              pInputText
              id="name"
              type="text"
              class="text-lg"
              placeholder="Enter the name here"
              i18n-placeholder
              formControlName="name"
            />
            <small controlName="name" class="p-invalid"></small>
          </div>
        </div>
        <div class="w-full">
          <div class="p-field">
            <label for="description" i18n>Short description</label>
            <input
              pInputText
              id="description"
              type="text"
              class="text-lg"
              placeholder="Enter the description of the idea"
              i18n-placeholder
              formControlName="description"
            />
            <small controlName="description" class="p-invalid"></small>
          </div>
        </div>
        <div class="w-full">
          <div class="p-field">
            <label for="body" i18n>Description</label>
            <textarea
              pInputTextarea
              class="w-full"
              [rows]="2"
              [autoResize]="false"
              formControlName="body"
              placeholder="Enter the body of the idea"
              i18n-placeholder
            ></textarea>
            <small controlName="body" class="p-invalid"></small>
          </div>
        </div>
        <div class="w-full">
          <div class="p-field">
            <label for="budget" i18n>Estimated Budget</label>
            <p-inputNumber inputId="integeronly" class="w-full" formControlName="budget">
            </p-inputNumber>
            <small controlName="budget" class="p-invalid"></small>
          </div>
        </div>
        <div class="w-full mb-4">
          <label for="selectedSector" i18n>Sector</label>
          <p-dropdown
            formControlName="selectedSector"
            [options]="sectorOptions"
            placeholder="Select a Sector"
            i18n-placeholder
            optionLabel="label"
            [showClear]="true"
            inputId="sector"
            (ngModelChange)="onSelectType($event)"
            [ngClass]="{ showClear: visibility }"
          ></p-dropdown>
        </div>
        <div *ngIf="isOtherSectorSelected" class="w-full">
          <label for="otherSector" i18n>Other Sector</label>
          <input
            pInputText
            formControlName="otherSector"
            type="text"
            id="otherSector"
            placeholder="Enter Other"
            i18n-placeholder
          />
        </div>
        <div class="w-full">
          <div class="p-field">
            <label for="benchmark" i18n>Benchmark</label>
            <textarea
              pInputTextarea
              class="w-full"
              [rows]="2"
              [autoResize]="false"
              formControlName="benchmark"
              placeholder="Enter the benchmark"
              i18n-placeholder
            ></textarea>
            <small controlName="benchmark" class="p-invalid"></small>
          </div>
        </div>
        <div class="w-full">
          <div class="p-field">
            <p-fileUpload
              name="myfile[]"
              [showUploadButton]="false"
              [showCancelButton]="false"
              (onRemove)="onFileClear()"
              (onSelect)="onFileUpload($event)"
              formControlName="file"
              ngDefaultControl
              chooseLabel="Choose"
              appPrimeNGi18n
            ></p-fileUpload>
          </div>
        </div>
      </form>
      <p-confirmDialog [style]="{ width: '50vw' }" [baseZIndex]="10000"></p-confirmDialog>
      <div class="flex flex-row-reverse">
        <p-button
          label="Save as Draft"
          (click)="isEditingMode ? confirm(1) : saveIdea(1)"
          [disabled]="!ideaForm.valid || onSave"
          i18n-label
        ></p-button>
        <p-button
          label="Send for Approval"
          (click)="isEditingMode ? confirm(2) : saveIdea(2)"
          class="mx-2"
          [disabled]="!ideaForm.valid || onSave"
          i18n-label
        ></p-button>
      </div>
    </div>
  `,
  providers: [ConfirmationService],
  styleUrls: ['./special-project-create.component.scss'],
})
export class ProjectIdeasCreateComponent implements OnInit {
  public ideaForm: FormGroup
  public ideaId: string = undefined
  public sectorOptions: ISectorOption[]
  public projectTopics: IListSpecialProjectTopicsViewModel[]
  public onSave = false
  public isEditingMode = false
  public uploadedFile: FileParameter = undefined
  public isOtherSectorSelected = false
  public visibility = false
  public language: string

  constructor(
    private location: Location,
    private route: ActivatedRoute,
    private confirmationService: ConfirmationService,
    private facade: ProfessionalExperienceFacadeService,
    private specialProjectTopics: SpecialProjectTopicsClient,
    private specialProjects: SpecialProjectsClient,
    @Inject(LOCALE_ID) locale
  ) {
    this.language = locale
  }

  onSelectType(event) {
    this.isOtherSectorSelected = event && event.label == 'Other'
    const otherSectorInput = this.ideaForm.get('otherSector')
    if (this.isOtherSectorSelected) {
      otherSectorInput.setValidators(Validators.required)
    } else {
      otherSectorInput.clearValidators()
    }
    otherSectorInput.updateValueAndValidity()
  }

  async ngOnInit() {
    this.visibility = this.language === 'ar'
    this.buildForm()
    await Promise.all([
      this.facade.loadSectorOptions().then((options) => {
        this.sectorOptions = options
      }),
    ])
    this.specialProjectTopics.specialProjectTopics().subscribe((data) => {
      this.projectTopics = data
    })
    this.ideaId = this.route.snapshot.paramMap.get('ideaId')
    if (this.ideaId) {
      this.specialProjects.specialProjectsGet(this.ideaId).subscribe((data) => {
        if (data.specialProjectStatus === 3) {
          this.isEditingMode = true
        }
        this.uploadedFile = data.uploadFile
        this.ideaForm.patchValue({
          name: data.name,
          description: data.description,
          budget: data.budget,
          body: data.body,
          benchmark: data.benchmark,
          selectedSector: this.sectorOptions.find((opt) => opt.value == data.sectorId),
          projectTopic: this.projectTopics.find(
            (opt) => opt.specialProjectTopicId == data.specialProjectTopicId
          ),
          otherSector: data.otherSector,
        })
      })
    }
  }

  buildForm() {
    this.ideaForm = new FormGroup({
      name: new FormControl('', [Validators.required]),
      description: new FormControl('', [Validators.required]),
      body: new FormControl('', [Validators.required]),
      budget: new FormControl('', [Validators.required]),
      benchmark: new FormControl('', [Validators.required]),
      file: new FormControl('', [Validators.nullValidator]),
      selectedSector: new FormControl('', [Validators.required]),
      projectTopic: new FormControl('', [Validators.required]),
      otherSector: new FormControl(''),
    })
  }

  goBack() {
    this.location.back()
  }

  confirm(saveType: number) {
    this.confirmationService.confirm({
      message:
        'The idea is already published, upon  saving, it will not be visible for other. Are you sure you want to save?',
      header: 'Confirmation',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.saveIdea(saveType)
      },
      reject: () => {},
    })
  }

  onFileUpload(event) {
    this.uploadedFile = {
      data: event.currentFiles[0],
      fileName: event.currentFiles[0].name,
    }
  }

  onFileClear() {
    this.uploadedFile = undefined
  }

  saveIdea(status) {
    this.onSave = true
    if (this.ideaForm.valid) {
      this.specialProjects
        .specialProjectsPost(
          this.uploadedFile,
          this.ideaId,
          this.ideaForm.value.description,
          this.ideaForm.value.benchmark,
          this.ideaForm.value.name,
          this.ideaForm.value.body,
          status,
          this.ideaForm.value.projectTopic.specialProjectTopicId,
          this.ideaForm.value.budget,
          this.ideaForm.value.selectedSector.value,
          this.ideaForm.value.otherSector
        )
        .pipe(
          tap(() => {
            this.onSave = false
            this.goBack()
          })
        )
        .toPromise()
    }
  }
}
