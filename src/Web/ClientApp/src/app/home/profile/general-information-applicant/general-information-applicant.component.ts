import { ProfileFacade } from '../common/profile-facade.service'
import { IUserGeneralInformation } from '../common/profile.models'
import { Component, OnInit } from '@angular/core'
import { FormControl, FormGroup, Validators } from '@angular/forms'
import { ActivatedRoute, Router } from '@angular/router'
import { inputLanguage } from 'src/app/shared/validators'
import { takeUntil } from 'rxjs/operators'
import { Subject } from 'rxjs'

const salutationOptions = [
  { value: 1, label: $localize`Mr` },
  { value: 2, label: $localize`Miss` },
  { value: 3, label: $localize`Mrs` },
]

const genderOptions = [
  { label: $localize`Male`, value: 1 },
  { label: $localize`Female`, value: 2 },
]

const maritalStatusOptions = [
  { label: $localize`Single`, value: 1 },
  { label: $localize`Married`, value: 2 },
]

@Component({
  selector: 'app-general-information-applicant',
  templateUrl: './general-information-applicant.component.html',
  styleUrls: ['./general-information-applicant.component.scss'],
})
export class GeneralInformationApplicantComponent implements OnInit {
  generalInformationForm: FormGroup
  isFormSubmitting = false
  ready = false
  private destroy$ = new Subject<boolean>()
  profilePicUrl: string
  profilePicRequiredPopup = false

  get salutationOptions(): any[] {
    return salutationOptions
  }

  get genderOptions(): any[] {
    return genderOptions
  }

  get maritalStatusOptions(): any[] {
    return maritalStatusOptions
  }

  constructor(
    private facade: ProfileFacade,
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private profileFacade: ProfileFacade
  ) {}

  ngOnInit(): void {
    this.buildForm()
    this.facade.loadGeneralInformation().then((data) => {
      this.generalInformationForm.patchValue(data)
      this.ready = true
    })
    this.profileFacade.profilePicChanges.pipe(takeUntil(this.destroy$)).subscribe((profilePic) => {
      this.profilePicUrl = profilePic
    })
  }

  ngOnDestroy() {
    this.destroy$.next(true)
  }

  async onSubmit() {
    if (!this.generalInformationForm.valid || this.isFormSubmitting) {
      return
    }

    if (this.profilePicUrl == null) {
      this.profilePicRequiredPopup = true
      return
    }

    if (this.generalInformationForm.dirty) {
      this.isFormSubmitting = true
      try {
        const formValue = this.generalInformationForm.value as FormModel
        await this.facade.storeGeneralInformation(formValue)
        this.changeLocalStorage(formValue)
        this.generalInformationForm.markAsPristine()
      } finally {
        this.isFormSubmitting = false
      }
    }

    this.router.navigate(['../contact-details'], { relativeTo: this.activatedRoute })
  }

  private buildForm(): void {
    this.generalInformationForm = new FormGroup({
      firstName: new FormControl('', [Validators.required, inputLanguage('en')]),
      firstName_AR: new FormControl('', [Validators.required, inputLanguage('ar')]),
      middleName: new FormControl('', [Validators.required, inputLanguage('en')]),
      middleName_AR: new FormControl('', [Validators.required, inputLanguage('ar')]),
      lastName: new FormControl('', [Validators.required, inputLanguage('en')]),
      lastName_AR: new FormControl('', [Validators.required, inputLanguage('ar')]),
      gender: new FormControl('', [Validators.required]),
      maritalStatus: new FormControl(''),
      birthdate: new FormControl('', [Validators.required]),
      nationality: new FormControl('', [Validators.required]),
    } as FormModel)
  }

  private changeLocalStorage(formValue): void {
    const profileInfoLS = localStorage.getItem('profile_info')
    const profileInfo = JSON.parse(profileInfoLS)

    profileInfo.firstName = formValue.firstName
    profileInfo.firstName_AR = formValue.firstName_AR
    profileInfo.lastName = formValue.lastName
    profileInfo.lastName_AR = formValue.lastName_AR
    profileInfo.middleName = formValue.middleName
    profileInfo.middleName_AR = formValue.middleName_AR

    localStorage.setItem('profile_info', JSON.stringify(profileInfo))
  }
}

type FormModel = { [key in keyof IUserGeneralInformation]: any }
