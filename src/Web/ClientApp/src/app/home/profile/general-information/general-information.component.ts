import { ProfileFacade } from '../common/profile-facade.service'
import { IUserGeneralInformation } from '../common/profile.models'
import { Component, OnInit } from '@angular/core'
import { FormControl, FormGroup, Validators } from '@angular/forms'
import { ActivatedRoute, Router } from '@angular/router'
import { takeUntil, tap } from 'rxjs/operators'
import { Subject } from 'rxjs'
import { UniversitiesClient } from 'src/app/shared/api.generated.clients'

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
  selector: 'app-general-information',
  templateUrl: './general-information.component.html',
  styleUrls: ['./general-information.component.scss'],
})
export class GeneralInformationComponent implements OnInit {
  generalInformationForm: FormGroup
  isFormSubmitting = false
  ready = false
  private destroy$ = new Subject<boolean>()
  profilePicUrl: string
  profilePicRequiredPopup = false
  instructorId: string

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
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private profileFacade: ProfileFacade,
    private universities: UniversitiesClient
  ) {}

  ngOnInit(): void {
    const profileInfo = JSON.parse(localStorage.getItem('profile_info'))
    this.instructorId = profileInfo.id

    this.universities.universityGet(this.instructorId).subscribe((data) => {
      this.buildForm(data)
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
        const formValue: any = this.generalInformationForm.value as FormModel
        formValue['id'] = this.instructorId
        this.universities
          .universityPost(formValue)
          .pipe(
            tap(() => {
              this.router.navigate(['../team'], { relativeTo: this.activatedRoute })
            })
          )
          .toPromise()
      } finally {
        this.isFormSubmitting = false
      }
    }
  }

  private buildForm(data): void {
    this.generalInformationForm = new FormGroup({
      name: new FormControl(data.name, [Validators.required]),
      email: new FormControl(data.email, [Validators.required, Validators.email]),
      country: new FormControl(data.country, [Validators.required]),
      city: new FormControl(data.city, [Validators.required]),
      address: new FormControl(data.address, [Validators.required]),
      phone: new FormControl(data.phone, [Validators.required]),
      pobox: new FormControl(data.poBox, [Validators.required]),
      linkedIn: new FormControl(data.linkedIn, [Validators.required]),
      twitter: new FormControl(data.twitter, [Validators.required]),
      instagram: new FormControl(data.instagram, [Validators.required]),
      website: new FormControl(data.website, [Validators.required]),
      aboutUniversity: new FormControl(data.aboutUniversity, [Validators.required]),
    } as FormModel)
  }
}

type FormModel = { [key in keyof IUserGeneralInformation]: any }
