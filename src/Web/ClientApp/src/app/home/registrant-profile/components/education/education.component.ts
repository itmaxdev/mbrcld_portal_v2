import { Component, OnInit } from '@angular/core'
import { FormControl, FormGroup, Validators } from '@angular/forms'
import { ActivatedRoute, Router } from '@angular/router'
import { ConfirmationService } from 'primeng/api'
import { requiredIf2, inputLanguage } from 'src/app/shared/validators'
import { EducationFacadeService } from '../../../profile/education/education-facade.service'
import { IEducationQualification } from '../../../profile/education/models'

const DegreeOptions = [
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
  selector: 'app-registrant-education',
  templateUrl: './education.component.html',
  styleUrls: ['./education.component.scss'],
  providers: [ConfirmationService],
})
export class RegistrantEducationComponent implements OnInit {
  educationForm: FormGroup
  educationQualifications: IEducationQualification[] = []
  editingEducation: IEducationQualification = {}
  showEducationDialog = false
  isFormSubmitting = false
  ready = false
  showCityDropDown = false

  get degreeOptions() {
    return DegreeOptions
  }

  get cityOptions() {
    return cityOptions
  }

  constructor(
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private facade: EducationFacadeService,
    private confirmationService: ConfirmationService
  ) {}

  async ngOnInit() {
    this.buildForm()
    await this.fetchContent()
    this.ready = true

    this.educationForm.get('country').valueChanges.subscribe((val) => {
      this.educationForm.controls.city.setValue('')
      this.showCityDropDown = val == 'AE' ? true : false
    })
  }

  async fetchContent() {
    try {
      this.educationQualifications = await this.facade.loadEducationQualifications()
    } catch (error) {
      console.error('Error fetching education content:', error)
    }
  }

  private buildForm(): void {
    this.educationForm = new FormGroup({
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
      city: new FormControl('', [requiredIf2(() => this.showCityDropDown)]),
      graduationDate: new FormControl('', [Validators.required]),
    })
  }

  openAddDialog() {
    this.editingEducation = {}
    this.educationForm.reset()
    this.showEducationDialog = true
  }

  openEditDialog(education: IEducationQualification) {
    this.editingEducation = education
    const patchValue = { ...education }
    if (education.graduationDate) {
      patchValue.graduationDate = this.formatDate(education.graduationDate) as any
    }
    this.educationForm.patchValue(patchValue)
    this.showEducationDialog = true
  }

  private formatDate(date: any): string {
    const d = new Date(date)
    return d.toISOString().split('T')[0]
  }

  async saveEducation() {
    if (this.educationForm.invalid || this.isFormSubmitting) {
      this.educationForm.markAllAsTouched()
      return
    }

    this.isFormSubmitting = true
    try {
      const formValue = { ...this.educationForm.value }
      if (formValue.graduationDate) {
        formValue.graduationDate = new Date(formValue.graduationDate)
      }

      if (this.editingEducation.id) {
        await this.facade.updateEducation(this.editingEducation.id, formValue)
      } else {
        await this.facade.addEducationQualification(formValue)
      }

      await this.fetchContent()
      this.showEducationDialog = false
    } catch (error) {
      console.error('Error saving education:', error)
    } finally {
      this.isFormSubmitting = false
    }
  }

  async removeEducation(education: IEducationQualification) {
    this.confirmationService.confirm({
      message: $localize`Are you sure you want to delete this education record?`,
      accept: async () => {
        await this.facade.removeEducationQualification(education.id)
        await this.fetchContent()
      },
    })
  }

  getDegreeLabel(value: number) {
    return this.degreeOptions.find((o) => o.value === value)?.label
  }

  onSubmit() {
    const profileInfo = JSON.parse(localStorage.getItem('profile_info'))
    const role = profileInfo?.role
    this.router.navigate([role == 4 ? '../languages' : '../training-courses'], {
      relativeTo: this.activatedRoute,
    })
  }

  goBack() {
    this.router.navigate(['../experience'], { relativeTo: this.activatedRoute })
  }
}
