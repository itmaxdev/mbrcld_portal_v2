import { EducationFacadeService } from './education-facade.service'
import { IEducationQualification } from './models'
import { Component, OnInit } from '@angular/core'
import { FormControl, FormGroup, Validators } from '@angular/forms'
import { ActivatedRoute, Router } from '@angular/router'
import { ConfirmationService } from 'primeng/api'
import { requiredIf2, inputLanguage } from 'src/app/shared/validators'

const DegreeOptions: DropdownOption[] = [
  {
    label: $localize`Associate`,
    value: 1,
  },
  {
    label: $localize`Bachelor`,
    value: 2,
  },
  {
    label: $localize`Master`,
    value: 3,
  },
  {
    label: $localize`Doctoral`,
    value: 4,
  },
]

const cityOptions = [
  { value: 'Abu Dhabi', label: $localize`Abu Dhabi` },
  { value: 'Ajman', label: $localize`Ajman` },
  { value: 'Dubai', label: $localize`Dubai` },
  { value: 'Fujairah', label: $localize`Fujairah` },
  { value: 'Ras Al Khaimah', label: $localize`Ras Al Khaimah` },
  { value: 'Sharjah', label: $localize`Sharjah` },
  { value: 'Umm al-Quwain', label: $localize`Umm al-Quwain` },
]

@Component({
  selector: 'app-education',
  templateUrl: './education.component.html',
  styleUrls: ['./education.component.scss'],
  providers: [ConfirmationService],
})
export class EducationComponent implements OnInit {
  educationQualificationForm: FormGroup
  educationQualifications: IEducationQualification[] = []
  editingEducationQualification: IEducationQualification = {}
  showDropDown: boolean
  isFormSubmitting = false
  ready = false
  role: number

  private _showAddEducationQualificationDialog = false

  get showAddLanguageDialog() {
    return this._showAddEducationQualificationDialog
  }

  set showAddLanguageDialog(show: boolean) {
    this._showAddEducationQualificationDialog = show
    if (!show) {
      this.editingEducationQualification = {}
      this.educationQualificationForm.reset()
    }
  }

  get degreeOptions(): DropdownOption[] {
    return DegreeOptions
  }

  get cityOptions(): any[] {
    return cityOptions
  }

  constructor(
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private facade: EducationFacadeService,
    private confirmationService: ConfirmationService
  ) {}

  async ngOnInit() {
    const profileInfo = JSON.parse(localStorage.getItem('profile_info'))
    this.role = profileInfo.role

    this.buildForm()
    await this.facade
      .loadEducationQualifications()
      .then((data) => (this.educationQualifications = data))
    this.ready = true

    this.educationQualificationForm.get('country').valueChanges.subscribe((val) => {
      this.educationQualificationForm.controls.city.setValue('')
      this.showDropDown = val == 'AE' ? true : false
    })
  }

  async addEducation() {
    if (!this.educationQualificationForm.valid || this.isFormSubmitting) {
      return
    }

    const formValue = this.educationQualificationForm.value as FormModel

    this.isFormSubmitting = true
    try {
      const id = await this.facade.addEducationQualification(formValue)
      this.educationQualifications.push({ id, ...formValue })
      this.showAddLanguageDialog = false
    } catch (error) {
      // TODO
    } finally {
      this.isFormSubmitting = false
    }
  }

  removeEducation(educationQualification: IEducationQualification) {
    this.confirmationService.confirm({
      message: $localize`Are you sure?`,
      accept: async () => {
        await this.facade.removeEducationQualification(educationQualification.id)
        this.educationQualifications.splice(
          this.educationQualifications.indexOf(educationQualification),
          1
        )
      },
    })
  }

  getDegreeLabel(value: number) {
    return this.degreeOptions.find((o) => o.value === value)?.label
  }

  openAddEducationQualificationDialog() {
    this._showAddEducationQualificationDialog = true
  }

  openEducationDialogForEditing(education: IEducationQualification) {
    this.editingEducationQualification = education
    this.educationQualificationForm.patchValue(education)
    this.showAddLanguageDialog = true
  }

  async updateEducation() {
    this.isFormSubmitting = true
    const formValues = this.educationQualificationForm.value

    await this.facade.updateEducation(this.editingEducationQualification.id, formValues)
    Object.assign(this.editingEducationQualification, formValues)

    this.showAddLanguageDialog = false
    this.isFormSubmitting = false
  }

  goToNextPage(): void {
    this.router.navigate([this.role == 4 ? '../languages' : '../training'], {
      relativeTo: this.activatedRoute,
    })
  }

  private buildForm(): void {
    this.educationQualificationForm = new FormGroup({
      degree: new FormControl('', [Validators.required]),
      specialization: new FormControl('', [
        Validators.required,
        inputLanguage('en'),
        Validators.maxLength(70),
      ]),
      specialization_AR: new FormControl('', [
        Validators.required,
        inputLanguage('ar'),
        Validators.maxLength(70),
      ]),
      university: new FormControl('', [Validators.required, inputLanguage('en')]),
      university_AR: new FormControl('', [Validators.required, inputLanguage('ar')]),
      country: new FormControl('', [Validators.required]),
      city: new FormControl('', [requiredIf2(() => this.showDropDown)]),
      graduationDate: new FormControl('', [Validators.required]),
    } as FormModel)
  }
}

type FormModel = { [key in keyof IEducationQualification]: any }

interface DropdownOption {
  label: string
  value: number
}
