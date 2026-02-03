import { IProfessionalExperience } from './models'
import { ProfessionalExperienceFacadeService } from './professional-experience-facade.service'
import { Component, OnInit, OnDestroy } from '@angular/core'
import { FormControl, FormGroup, Validators } from '@angular/forms'
import { ActivatedRoute, Router } from '@angular/router'
import { requiredIf2, inputLanguage } from 'src/app/shared/validators'
import { ConfirmationService } from 'primeng/api'
import { combineLatest, Subject } from 'rxjs'
import { takeUntil } from 'rxjs/operators'

const organizationSizeOptions = [
  { value: 1, label: $localize`1 - 10` },
  { value: 2, label: $localize`11 - 50` },
  { value: 3, label: $localize`51 - 100` },
  { value: 4, label: $localize`101 - 500` },
  { value: 5, label: $localize`+500` },
]

const positionLevel = [
  { value: 0, label: 'Emirate' },
  { value: 1, label: 'Country' },
  { value: 2, label: 'Regional' },
  { value: 3, label: 'Global' },
]

const organizationLevel = [
  { value: 0, label: 'Country' },
  { value: 1, label: 'Regional' },
  { value: 2, label: 'Global' },
]

@Component({
  selector: 'app-professional-experience',
  templateUrl: './professional-experience.component.html',
  styleUrls: ['./professional-experience.component.scss'],
  providers: [ConfirmationService],
})
export class ProfessionalExperienceComponent implements OnInit, OnDestroy {
  professionalExperienceForm: FormGroup
  professionalExperiences: IProfessionalExperience[] = []

  editingProfessionalExperience: IProfessionalExperience = {}

  isFormSubmitting = false
  ready = false

  industryOptions = []
  sectorOptions = []

  otherSectorOptionValue: string
  otherIndustryOptionValue: string

  stillWorkingThere = false

  private destroy$ = new Subject<boolean>()

  private _showAddEducationQualificationDialog = false

  get organizationSizeOptions(): any[] {
    return organizationSizeOptions
  }

  get positionLevel(): any[] {
    return positionLevel
  }

  get organizationLevel(): any[] {
    return organizationLevel
  }

  constructor(
    private facade: ProfessionalExperienceFacadeService,
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private confirmationService: ConfirmationService
  ) {}

  async ngOnInit() {
    this.buildForm()

    await this.fetchContent()
    this.ready = true
  }

  fetchContent() {
    return Promise.all([
      this.facade
        .loadProfessionalExperiences()
        .then((data) => (console.log(data), (this.professionalExperiences = data))),
      this.facade
        .loadIndustryOptions()
        .then((options) => {
          this.industryOptions = options
          this.otherIndustryOptionValue = options[options.length - 1].value
        })
        .catch(() => {}),
      this.facade
        .loadSectorOptions()
        .then((options) => {
          this.sectorOptions = options
          this.otherSectorOptionValue = options[options.length - 1].value
        })
        .catch(() => {}),
    ])
  }

  ngOnDestroy() {
    this.destroy$.next(true)
  }

  async addProfessionalExperience() {
    this.isFormSubmitting = true

    try {
      const formValue = this.professionalExperienceForm.value as FormModel
      const id = await this.facade.addProfessionalExperience(formValue)
      this.professionalExperiences.push({ id, ...this.professionalExperienceForm.value })
      this.showProfessionalExperienceDialog = false
    } catch (error) {
      // empty
    } finally {
      this.isFormSubmitting = false
    }
  }

  async removeProfessionalExperience(professionalExperience: IProfessionalExperience) {
    this.confirmationService.confirm({
      message: $localize`Are you sure?`,
      accept: async () => {
        await this.facade.removeProfessionalExperience(professionalExperience.id)
        this.professionalExperiences.splice(
          this.professionalExperiences.indexOf(professionalExperience),
          1
        )
      },
    })
  }

  openProfessionalExperienceDialogForEditing(professionalExperience: IProfessionalExperience) {
    let presentValue = false
    if (!professionalExperience.to) {
      presentValue = true
    }
    this.editingProfessionalExperience = professionalExperience

    this.professionalExperienceForm.patchValue({ present: presentValue, ...professionalExperience })
    this.showProfessionalExperienceDialog = true
  }

  async updateProfessionalExperience() {
    this.isFormSubmitting = true
    const formValues = this.professionalExperienceForm.value

    await this.facade.updateProfessionalExperience(
      this.editingProfessionalExperience.id,
      formValues
    )

    Object.assign(this.editingProfessionalExperience, formValues)

    this.showProfessionalExperienceDialog = false
    this.isFormSubmitting = false
    this.facade.updateFormProgress()
    await this.fetchContent()
  }

  clearToDateField() {
    this.professionalExperienceForm.patchValue({
      to: '',
    })
  }

  goToNextForm(): void {
    this.facade.updateFormProgress()
    this.router.navigate(['../education'], { relativeTo: this.activatedRoute })
  }

  get isOtherSectorOptionSelected(): boolean {
    return this.professionalExperienceForm?.get('sector').value === this.otherSectorOptionValue
  }

  get isOtherIndustryOptionSelected(): boolean {
    return this.professionalExperienceForm?.get('industry').value === this.otherIndustryOptionValue
  }

  get isPresentOptionSelected(): boolean {
    return this.professionalExperienceForm?.get('present').value
  }

  get fromDateValue(): Date {
    return this.professionalExperienceForm?.get('from').value
  }

  get showProfessionalExperienceDialog() {
    return this._showAddEducationQualificationDialog
  }

  set showProfessionalExperienceDialog(show: boolean) {
    this._showAddEducationQualificationDialog = show
    if (!show) {
      this.editingProfessionalExperience = {}
      this.professionalExperienceForm.reset()
    }
  }

  openProfessionalExperienceDialog() {
    this._showAddEducationQualificationDialog = true
  }

  getSectorLabel(sectorId: string) {
    return this.sectorOptions.find((e) => e.value === sectorId)?.label || '--'
  }

  getIndustryLabel(industryId: string) {
    return this.industryOptions.find((e) => e.value === industryId)?.label || '--'
  }

  private buildForm(): void {
    const sector = new FormControl('', [Validators.required])
    const industry = new FormControl('', [Validators.required])
    const positionLevel = new FormControl('', [Validators.required])
    const organizationLevel = new FormControl('', [Validators.required])
    const otherSector = new FormControl('', [
      requiredIf2(() => {
        return this.isOtherSectorOptionSelected
      }),
    ])

    const otherIndustry = new FormControl('', [
      requiredIf2(() => {
        return this.isOtherIndustryOptionSelected
      }),
    ])

    this.professionalExperienceForm = new FormGroup(
      {
        jobTitle: new FormControl('', [Validators.required, inputLanguage('en')]),
        jobTitle_AR: new FormControl('', [Validators.required, inputLanguage('ar')]),
        organizationName: new FormControl('', [
          Validators.required,
          inputLanguage('en'),
          Validators.maxLength(70),
        ]),
        organizationName_AR: new FormControl('', [
          Validators.required,
          inputLanguage('ar'),
          Validators.maxLength(70),
        ]),
        organizationSize: new FormControl('', [Validators.required]),
        from: new FormControl('', [Validators.required]),
        to: new FormControl('', [
          requiredIf2(() => {
            return !this.isPresentOptionSelected
          }),
        ]),
        positionLevel,
        organizationLevel,
        industry,
        otherIndustry,
        sector,
        otherSector,
        present: new FormControl(false),
      } as FormModel,
      [this.validateForm.bind(this)]
    )

    combineLatest([industry.valueChanges, sector.valueChanges])
      .pipe(takeUntil(this.destroy$))
      .subscribe(() => {
        otherIndustry.updateValueAndValidity()
        otherSector.updateValueAndValidity()
      })
  }

  private validateForm() {
    if (
      this.isPresentOptionSelected &&
      this.professionalExperiences.find((e) => !e.to)?.id !== this.editingProfessionalExperience?.id
    ) {
      return { singleCurrentJob: true }
    }
  }
}

type FormModel = { [key in keyof IProfessionalExperience]: any }
