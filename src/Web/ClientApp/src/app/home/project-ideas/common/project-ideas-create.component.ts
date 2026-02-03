import { Component, OnInit, LOCALE_ID, Inject } from '@angular/core'
import { FormControl, FormGroup, Validators } from '@angular/forms'
import { ConfirmationService } from 'primeng/api'
import { ActivatedRoute } from '@angular/router'
import { Location } from '@angular/common'
import { tap } from 'rxjs/operators'
import { FileParameter, ProjectIdeasClient } from 'src/app/shared/api.generated.clients'
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
    <div
      class="m-0 p-8 border border-gray-400 rounded-t-lg bg-white flex justify-center items-center"
    >
      <h1 class="text-2xl" i18n *ngIf="locale === 'en'">Add Idea</h1>
      <h1 class="text-2xl" i18n *ngIf="locale === 'ar'">جميع الأفكار</h1>
    </div>
    <div class="m-0 p-8 border border-t-0 border-gray-400 rounded-b-lg bg-white">
      <form class="p-fluid p-formgrid p-grid profile-form gap-6" [formGroup]="ideaForm">
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
              i18n-chooseLabel
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
})
export class ProjectIdeasCreateComponent implements OnInit {
  public ideaForm: FormGroup
  public ideaId: string = undefined
  public sectorOptions: ISectorOption[]
  public onSave = false
  public isEditingMode = false
  public uploadedFile: FileParameter = undefined
  public isOtherSectorSelected = false

  constructor(
    private location: Location,
    private projectIdeas: ProjectIdeasClient,
    private route: ActivatedRoute,
    private confirmationService: ConfirmationService,
    private facade: ProfessionalExperienceFacadeService,
    @Inject(LOCALE_ID) public locale: string
  ) {}

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
    this.buildForm()
    await Promise.all([
      this.facade.loadSectorOptions().then((options) => {
        this.sectorOptions = options
      }),
    ])
    this.ideaId = this.route.snapshot.paramMap.get('ideaId')
    if (this.ideaId) {
      this.projectIdeas.projectIdeasGetById(this.ideaId).subscribe((data) => {
        if (data.projectIdeaStatus === 3) {
          this.isEditingMode = true
        }
        this.uploadedFile = data.uploadedFile
        this.ideaForm.patchValue({
          name: data.name,
          description: data.description,
          budget: data.budget,
          body: data.body,
          benchmark: data.benchmark,
          selectedSector: this.sectorOptions.find((opt) => opt.value == data.sectorId),
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
      this.projectIdeas
        .projectIdeasPost(
          this.uploadedFile,
          this.ideaId,
          this.ideaForm.value.description,
          this.ideaForm.value.name,
          this.ideaForm.value.body,
          this.ideaForm.value.benchmark,
          status,
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
