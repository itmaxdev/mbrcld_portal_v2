import { ProfileFacade } from '../common/profile-facade.service'
import { IUserContactDetails } from '../common/profile.models'
import { Component, OnInit } from '@angular/core'
import { FormControl, FormGroup, Validators } from '@angular/forms'
import { ActivatedRoute, Router } from '@angular/router'
import { email, url } from 'src/app/shared/validators'

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
  selector: 'app-contact-details',
  templateUrl: './contact-details.component.html',
  styleUrls: ['./contact-details.component.scss'],
})
export class ContactDetailsComponent implements OnInit {
  contactDetailsForm: FormGroup
  isFormSubmitting = false
  ready = false
  role: number
  emailUrl: string
  showDropDown: boolean

  constructor(
    private facade: ProfileFacade,
    private router: Router,
    private activatedRoute: ActivatedRoute
  ) {}

  get cityOptions(): any[] {
    return cityOptions
  }

  ngOnInit(): void {
    const profileInfo = JSON.parse(localStorage.getItem('profile_info'))
    this.role = profileInfo.role

    switch (this.role) {
      case 1:
        this.emailUrl = '/registrant/account-settings/change-email'
        break
      case 2:
        this.emailUrl = '/applicant/account-settings/change-email'
        break
      case 3:
        this.emailUrl = '/alumni/account-settings/change-email'
        break
      default:
        break
    }

    this.buildForm()
    this.facade.loadContactDetails().then((details) => {
      this.contactDetailsForm.patchValue(details)
      this.ready = true
    })

    this.contactDetailsForm.get('residenceCountry').valueChanges.subscribe((val) => {
      this.contactDetailsForm.controls.city.setValue('')
      this.showDropDown = val == 'AE' ? true : false
    })
  }

  private validateSocialMediaSection() {
    const formValue = (this.contactDetailsForm?.value || {}) as FormModel
    if (!formValue.instagramUrl && !formValue.twitterUrl && !formValue.linkedInUrl) {
      return { social: true }
    }
    return null
  }

  private buildForm(): void {
    this.contactDetailsForm = new FormGroup(
      {
        email: new FormControl({ value: '', disabled: true }, [Validators.required, email()]),
        businessEmail: new FormControl('', [Validators.required, email()]),
        mobilePhone: new FormControl('', [Validators.required]),
        mobilePhone2: new FormControl('', []),
        telephone: new FormControl('', [Validators.maxLength(8), Validators.minLength(8)]),
        residenceCountry: new FormControl('', [Validators.required]),
        city: new FormControl('', [Validators.required]),
        postOfficeBox: new FormControl('', []),
        address: new FormControl('', [Validators.required]),
        linkedInUrl: new FormControl('', []),
        twitterUrl: new FormControl('', []),
        instagramUrl: new FormControl('', []),
      } as FormModel,
      [this.validateSocialMediaSection.bind(this)]
    )
  }

  async onSubmit() {
    if (!this.contactDetailsForm.valid || this.isFormSubmitting) {
      return
    }

    if (this.contactDetailsForm.dirty) {
      this.isFormSubmitting = true
      try {
        const formValue = this.contactDetailsForm.value as IUserContactDetails
        await this.facade.storeContactDetails(formValue)
      } finally {
        this.isFormSubmitting = false
      }
    }

    this.router.navigate([this.role == 4 ? '../professional-experience' : '../identity'], {
      relativeTo: this.activatedRoute,
    })
  }
}

type FormModel = { [key in keyof IUserContactDetails]: any }
