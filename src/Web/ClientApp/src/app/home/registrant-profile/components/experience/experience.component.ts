import { Component, OnInit, OnDestroy } from '@angular/core'
import { FormControl, FormGroup, Validators } from '@angular/forms'
import { ActivatedRoute, Router } from '@angular/router'
import { Subject, combineLatest } from 'rxjs'
import { takeUntil } from 'rxjs/operators'
import { ProfessionalExperienceFacadeService } from '../../../profile/professional-experience/professional-experience-facade.service'
import { IProfessionalExperience } from '../../../profile/professional-experience/models'
import { requiredIf2, inputLanguage } from 'src/app/shared/validators'
import { ConfirmationService } from 'primeng/api'

const organizationSizeOptions = [
  { value: 1, label: $localize`1 - 10` },
  { value: 2, label: $localize`11 - 50` },
  { value: 3, label: $localize`51 - 100` },
  { value: 4, label: $localize`101 - 500` },
  { value: 5, label: $localize`+500` },
]

const positionLevelOptions = [
  { value: 0, label: $localize`Emirate` },
  { value: 1, label: $localize`Country` },
  { value: 2, label: $localize`Regional` },
  { value: 3, label: $localize`Global` },
]

const organizationLevelOptions = [
  { value: 0, label: $localize`Country` },
  { value: 1, label: $localize`Country` },
  { value: 2, label: $localize`Regional` },
  { value: 3, label: $localize`Global` },
]

@Component({
  selector: 'app-registrant-experience',
  templateUrl: './experience.component.html',
  styleUrls: ['./experience.component.scss'],
  providers: [ConfirmationService],
})
export class RegistrantExperienceComponent implements OnInit, OnDestroy {
  experienceForm: FormGroup
  professionalExperiences: IProfessionalExperience[] = []
  editingExperience: IProfessionalExperience = {}

  isFormSubmitting = false
  ready = false

  industryOptions = []
  sectorOptions = []

  otherSectorOptionValue: string
  otherIndustryOptionValue: string

  private destroy$ = new Subject<boolean>()
  showExperienceDialog = false

  get organizationSizeOptions() {
    return organizationSizeOptions
  }

  get positionLevelOptions() {
    return positionLevelOptions
  }

  get organizationLevelOptions() {
    return organizationLevelOptions
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

  async fetchContent() {
    try {
      const [experiences, industries, sectors] = await Promise.all([
        this.facade.loadProfessionalExperiences(),
        this.facade.loadIndustryOptions(),
        this.facade.loadSectorOptions(),
      ])

      this.professionalExperiences = experiences
      this.industryOptions = industries
      this.sectorOptions = sectors

      if (industries.length > 0) {
        this.otherIndustryOptionValue = industries[industries.length - 1].value
      }
      if (sectors.length > 0) {
        this.otherSectorOptionValue = sectors[sectors.length - 1].value
      }
    } catch (error) {
      console.error('Error fetching experience content:', error)
    }
  }

  ngOnDestroy() {
    this.destroy$.next(true)
    this.destroy$.complete()
  }

  private buildForm() {
    const industry = new FormControl('', [Validators.required])
    const sector = new FormControl('', [Validators.required])

    this.experienceForm = new FormGroup(
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
        to: new FormControl('', [requiredIf2(() => !this.experienceForm?.get('present').value)]),
        industry: industry,
        sector: sector,
        otherIndustry: new FormControl('', [requiredIf2(() => this.isOtherIndustryOptionSelected)]),
        otherSector: new FormControl('', [requiredIf2(() => this.isOtherSectorOptionSelected)]),
        present: new FormControl(false),
        positionLevel: new FormControl('', [Validators.required]),
        organizationLevel: new FormControl('', [Validators.required]),
      },
      { validators: this.validateForm.bind(this) }
    )

    combineLatest([industry.valueChanges, sector.valueChanges])
      .pipe(takeUntil(this.destroy$))
      .subscribe(() => {
        this.experienceForm.get('otherIndustry').updateValueAndValidity()
        this.experienceForm.get('otherSector').updateValueAndValidity()
      })
  }

  private validateForm() {
    if (
      this.experienceForm?.get('present').value &&
      this.professionalExperiences.find((e) => !e.to)?.id !== this.editingExperience?.id
    ) {
      return { singleCurrentJob: true }
    }
    return null
  }

  get isOtherSectorOptionSelected(): boolean {
    return this.experienceForm?.get('sector').value === this.otherSectorOptionValue
  }

  get isOtherIndustryOptionSelected(): boolean {
    return this.experienceForm?.get('industry').value === this.otherIndustryOptionValue
  }

  get fromDateValue(): Date {
    return this.experienceForm?.get('from').value
  }

  openAddDialog() {
    this.editingExperience = {}
    this.experienceForm.reset({ present: false })
    this.showExperienceDialog = true
  }

  openEditDialog(experience: IProfessionalExperience) {
    this.editingExperience = experience
    // Format dates for <input type="date">
    const patchValue = { ...experience, present: !experience.to }
    if (experience.from) patchValue.from = this.formatDate(experience.from) as any
    if (experience.to) patchValue.to = this.formatDate(experience.to) as any

    this.experienceForm.patchValue(patchValue)
    this.showExperienceDialog = true
  }

  private formatDate(date: any): string {
    const d = new Date(date)
    return d.toISOString().split('T')[0]
  }

  async saveExperience() {
    if (this.experienceForm.invalid || this.isFormSubmitting) {
      this.experienceForm.markAllAsTouched()
      return
    }

    this.isFormSubmitting = true
    try {
      const formValue = { ...this.experienceForm.value }

      // Convert string dates from <input type="date"> to Date objects
      if (formValue.from) {
        formValue.from = new Date(formValue.from)
      }
      if (formValue.to) {
        formValue.to = new Date(formValue.to)
      } else if (formValue.present) {
        formValue.to = null
      }

      if (this.editingExperience.id) {
        await this.facade.updateProfessionalExperience(this.editingExperience.id, formValue)
      } else {
        await this.facade.addProfessionalExperience(formValue)
      }

      await this.fetchContent()
      this.showExperienceDialog = false
    } catch (error) {
      console.error('Error saving experience:', error)
    } finally {
      this.isFormSubmitting = false
    }
  }

  async removeExperience(experience: IProfessionalExperience) {
    this.confirmationService.confirm({
      message: $localize`Are you sure you want to delete this experience?`,
      accept: async () => {
        await this.facade.removeProfessionalExperience(experience.id)
        await this.fetchContent()
      },
    })
  }

  getSectorLabel(sectorId: string) {
    return this.sectorOptions.find((e) => e.value === sectorId)?.label || sectorId
  }

  getIndustryLabel(industryId: string) {
    return this.industryOptions.find((e) => e.value === industryId)?.label || industryId
  }

  onSubmit() {
    // Navigate to next tab (Education)
    this.router.navigate(['../education'], { relativeTo: this.activatedRoute })
  }
}
