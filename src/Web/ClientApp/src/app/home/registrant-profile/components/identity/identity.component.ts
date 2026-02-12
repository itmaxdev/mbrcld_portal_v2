import { Component, OnInit, OnDestroy } from '@angular/core'
import { FormControl, FormGroup, Validators } from '@angular/forms'
import { ActivatedRoute, Router } from '@angular/router'
import { Subject } from 'rxjs'
import { takeUntil } from 'rxjs/operators'
import { ProfileFacade } from '../../../profile/common/profile-facade.service'
import { IUserIdentityDetails } from '../../../profile/common/profile.models'
import { requiredIf2 } from 'src/app/shared/validators'
import { MessageService } from 'primeng/api'
import { ApiProblemDetails } from 'src/app/shared/api.generated.clients'

const passportIssuingAuthorityOptions = [
  { value: 1, label: $localize`Abu Dhabi` },
  { value: 2, label: $localize`Ajman` },
  { value: 3, label: $localize`Dubai` },
  { value: 4, label: $localize`Fujairah` },
  { value: 5, label: $localize`Ras Al Khaimah` },
  { value: 6, label: $localize`Sharjah` },
  { value: 7, label: $localize`Umm Al-Quwian` },
]

@Component({
  selector: 'app-registrant-identity',
  templateUrl: './identity.component.html',
  styleUrls: ['./identity.component.scss'],
  providers: [MessageService],
})
export class RegistrantIdentityComponent implements OnInit, OnDestroy {
  identityForm: FormGroup
  isFormSubmitting = false
  ready = false
  private nationality: string
  private destroy$ = new Subject<boolean>()

  get passportIssuingAuthorityOptions(): any[] {
    return passportIssuingAuthorityOptions
  }

  constructor(
    private facade: ProfileFacade,
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private messageService: MessageService
  ) {}

  async ngOnInit() {
    this.buildForm()

    await Promise.all([
      this.facade.loadIdentityDetails().then((data) => {
        // Format dates to YYYY-MM-DD for standard <input type="date">
        const patchData: any = { ...data }
        if (data.emiratesIdExpiryDate) {
          patchData.emiratesIdExpiryDate = this.formatDate(data.emiratesIdExpiryDate)
        }
        if (data.passportExpiryDate) {
          patchData.passportExpiryDate = this.formatDate(data.passportExpiryDate)
        }
        this.identityForm.patchValue(patchData)
      }),
      this.facade.loadGeneralInformation().then((data) => {
        this.nationality = data.nationality
      }),
    ])

    this.ready = true
  }

  ngOnDestroy(): void {
    this.destroy$.next(true)
    this.destroy$.complete()
  }

  private buildForm(): void {
    this.identityForm = new FormGroup({
      emiratesId: new FormControl('', [requiredIf2(() => this.isEmiratyCitizen)]),
      emiratesIdExpiryDate: new FormControl('', [requiredIf2(() => this.isEmiratyCitizen)]),
      passportNumber: new FormControl('', [Validators.required]),
      passportExpiryDate: new FormControl('', [Validators.required]),
      passportIssuingAuthority: new FormControl('', [requiredIf2(() => this.isEmiratyCitizen)]),
    })
  }

  private formatDate(date: any): string {
    const d = new Date(date)
    const year = d.getFullYear()
    const month = String(d.getMonth() + 1).padStart(2, '0')
    const day = String(d.getDate()).padStart(2, '0')
    return `${year}-${month}-${day}`
  }

  async onSubmit() {
    if (!this.identityForm.valid || this.isFormSubmitting) {
      this.identityForm.markAllAsTouched()
      return
    }

    this.isFormSubmitting = true
    const formValue = this.identityForm.getRawValue()

    // Convert strings back to Date objects
    const identityDetails: IUserIdentityDetails = {
      emiratesId: formValue.emiratesId,
      emiratesIdExpiryDate: formValue.emiratesIdExpiryDate
        ? new Date(formValue.emiratesIdExpiryDate)
        : null,
      passportNumber: formValue.passportNumber,
      passportExpiryDate: formValue.passportExpiryDate
        ? new Date(formValue.passportExpiryDate)
        : null,
      passportIssuingAuthority: formValue.passportIssuingAuthority,
    }

    try {
      await this.facade.storeIdentity(identityDetails)
      // Navigate to next tab - Experience as per registrant profile routes
      this.router.navigate(['../experience'], { relativeTo: this.activatedRoute })
    } catch (error) {
      if (error instanceof ApiProblemDetails) {
        if (error.errors[0]?.includes('Emirate')) {
          this.messageService.add({
            summary: $localize`${error.errors[0]}`,
            detail: $localize`Emirates Id '${formValue.emiratesId}' already exist.`,
            closable: true,
            life: 10000,
            severity: 'error',
          })
          return
        }
      }
      console.error('Error saving identity:', error)
    } finally {
      this.isFormSubmitting = false
    }
  }

  get isEmiratyCitizen(): boolean {
    return this.nationality?.toUpperCase() === 'AE'
  }
}
