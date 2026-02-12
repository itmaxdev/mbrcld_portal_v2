import { Component, OnInit, OnDestroy } from '@angular/core'
import { FormControl, FormGroup, Validators } from '@angular/forms'
import { ActivatedRoute, Router } from '@angular/router'
import { Subject } from 'rxjs'
import { takeUntil } from 'rxjs/operators'
import { ProfileFacade } from '../../../profile/common/profile-facade.service'
import {
  IUserGeneralInformation,
  IUserContactDetails,
} from '../../../profile/common/profile.models'

const genderOptions = [
  { label: $localize`Male`, value: 1 },
  { label: $localize`Female`, value: 2 },
]

const maritalStatusOptions = [
  { label: $localize`Single`, value: 1 },
  { label: $localize`Married`, value: 2 },
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
  selector: 'app-registrant-general-information',
  templateUrl: './general-information.component.html',
  styleUrls: ['./general-information.component.scss'],
})
export class RegistrantGeneralInformationComponent implements OnInit, OnDestroy {
  generalInformationForm: FormGroup
  isFormSubmitting = false
  ready = false
  profilePicUrl: string
  profilePicRequiredPopup = false
  showDropDown: boolean
  private destroy$ = new Subject<boolean>()

  get genderOptions(): any[] {
    return genderOptions
  }

  get maritalStatusOptions(): any[] {
    return maritalStatusOptions
  }

  get cityOptions(): any[] {
    return cityOptions
  }

  constructor(
    private facade: ProfileFacade,
    private router: Router,
    private activatedRoute: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.buildForm()
    this.facade.loadProfile().then((data) => {
      // Format birthdate to YYYY-MM-DD if it's a Date object,
      // because standard <input type="date"> expects this format.
      if (data.birthdate) {
        const birthdate = new Date(data.birthdate)
        const year = birthdate.getFullYear()
        const month = String(birthdate.getMonth() + 1).padStart(2, '0')
        const day = String(birthdate.getDate()).padStart(2, '0')
        data.birthdate = `${year}-${month}-${day}` as any
      }
      this.generalInformationForm.patchValue(data)
      this.ready = true
    })

    this.facade.profilePicChanges.pipe(takeUntil(this.destroy$)).subscribe((profilePic) => {
      this.profilePicUrl = profilePic
    })

    this.generalInformationForm
      .get('residenceCountry')
      .valueChanges.pipe(takeUntil(this.destroy$))
      .subscribe((val) => {
        this.generalInformationForm.controls.city.setValue('')
        this.showDropDown = val == 'AE' ? true : false
      })
  }

  ngOnDestroy(): void {
    this.destroy$.next(true)
    this.destroy$.complete()
  }

  private buildForm(): void {
    this.generalInformationForm = new FormGroup({
      // General Information fields
      firstName: new FormControl('', [Validators.required]),
      firstName_AR: new FormControl('', [Validators.required]),
      middleName: new FormControl(''),
      middleName_AR: new FormControl(''),
      lastName: new FormControl('', [Validators.required]),
      lastName_AR: new FormControl('', [Validators.required]),
      gender: new FormControl('', [Validators.required]),
      maritalStatus: new FormControl(''),
      birthdate: new FormControl('', [Validators.required]),
      nationality: new FormControl('', [Validators.required]),

      // Contact Details fields
      residenceCountry: new FormControl('', [Validators.required]),
      city: new FormControl('', [Validators.required]),
      email: new FormControl({ value: '', disabled: true }, [Validators.required]),
      businessEmail: new FormControl('', [Validators.required, Validators.email]),
      mobilePhone: new FormControl('', [Validators.required]),
      mobilePhone2: new FormControl(''),
      telephone: new FormControl(''),
      postOfficeBox: new FormControl(''),
      address: new FormControl('', [Validators.required]),
      linkedInUrl: new FormControl(''),
      twitterUrl: new FormControl(''),
      instagramUrl: new FormControl(''),
    })
  }

  async onSubmit() {
    if (!this.generalInformationForm.valid || this.isFormSubmitting) {
      this.generalInformationForm.markAllAsTouched()
      return
    }

    if (this.profilePicUrl == null) {
      this.profilePicRequiredPopup = true
      return
    }

    this.isFormSubmitting = true
    try {
      const formValue = this.generalInformationForm.getRawValue()

      // Store General Information
      const generalInfo: IUserGeneralInformation = {
        firstName: formValue.firstName,
        middleName: formValue.middleName,
        lastName: formValue.lastName,
        firstName_AR: formValue.firstName_AR,
        middleName_AR: formValue.middleName_AR,
        lastName_AR: formValue.lastName_AR,
        gender: formValue.gender,
        maritalStatus: formValue.maritalStatus,
        // Convert string from <input type="date"> back to Date object
        birthdate: formValue.birthdate ? new Date(formValue.birthdate) : null,
        nationality: formValue.nationality,
      }
      await this.facade.storeGeneralInformation(generalInfo)

      // Store Contact Details
      const contactDetails: IUserContactDetails = {
        email: formValue.email,
        businessEmail: formValue.businessEmail,
        mobilePhone: formValue.mobilePhone,
        mobilePhone2: formValue.mobilePhone2,
        telephone: formValue.telephone,
        residenceCountry: formValue.residenceCountry,
        city: formValue.city,
        postOfficeBox: formValue.postOfficeBox,
        address: formValue.address,
        linkedInUrl: formValue.linkedInUrl,
        twitterUrl: formValue.twitterUrl,
        instagramUrl: formValue.instagramUrl,
      }
      await this.facade.storeContactDetails(contactDetails)

      // Navigate to next tab (usually Identity)
      this.router.navigate(['./identity'], { relativeTo: this.activatedRoute.parent })
    } catch (error) {
      console.error('Error saving profile:', error)
    } finally {
      this.isFormSubmitting = false
    }
  }

  triggerProfileUpload() {
    // Logic to trigger profile photo upload popup if needed
    // The current HTML uses fancybox Modal, but we might want to handle it via TS
  }
}
