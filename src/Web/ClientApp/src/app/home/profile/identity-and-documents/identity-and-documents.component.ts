import { ProfileFacade } from '../common/profile-facade.service'
import { IUserIdentityDetails } from '../common/profile.models'
import { Component, OnDestroy, OnInit } from '@angular/core'
import { FormControl, FormGroup, Validators } from '@angular/forms'
import { ActivatedRoute, Router } from '@angular/router'
import { requiredIf2 } from 'src/app/shared/validators'
import { ApiProblemDetails } from 'src/app/shared/api.generated.clients'
import { MessageService } from 'primeng/api'
import { Subject } from 'rxjs'

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
  selector: 'app-identity-and-documents',
  templateUrl: './identity-and-documents.component.html',
  styleUrls: ['./identity-and-documents.component.scss'],
  providers: [MessageService],
})
export class IdentityAndDocumentsComponent implements OnInit, OnDestroy {
  identityForm: FormGroup
  isFormSubmitting = false
  ready = false

  private nationality: string

  get passportIssuingAuthorityOptions(): any[] {
    return passportIssuingAuthorityOptions
  }

  private destroy$ = new Subject<boolean>()
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
        this.identityForm.patchValue(data)
      }),
      this.facade.loadGeneralInformation().then((data) => {
        this.nationality = data.nationality
      }),
    ])

    this.ready = true
  }

  ngOnDestroy(): void {
    this.destroy$.next(true)
  }

  private buildForm(): void {
    this.identityForm = new FormGroup({
      emiratesId: new FormControl('', [requiredIf2(() => this.isEmiratyCitizen)]),
      emiratesIdExpiryDate: new FormControl('', [requiredIf2(() => this.isEmiratyCitizen)]),
      passportNumber: new FormControl('', [Validators.required]),
      passportExpiryDate: new FormControl('', [Validators.required]),
      passportIssuingAuthority: new FormControl('', [requiredIf2(() => this.isEmiratyCitizen)]),
    } as FormModel)
  }

  async onSubmit() {
    if (!this.identityForm.valid || this.isFormSubmitting) {
      return
    }

    if (this.identityForm.dirty) {
      this.isFormSubmitting = true
      const formValue = this.identityForm.value as FormModel
      try {
        await this.facade.storeIdentity(formValue)
        this.router.navigate(['../professional-experience'], { relativeTo: this.activatedRoute })
      } catch (error) {
        this.isFormSubmitting = false
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
      } finally {
        this.isFormSubmitting = false
      }
    } else {
      this.router.navigate(['../professional-experience'], { relativeTo: this.activatedRoute })
    }
  }

  get isEmiratyCitizen(): boolean {
    return this.nationality?.toUpperCase() === 'AE'
  }
}

type FormModel = { [key in keyof IUserIdentityDetails]: any }
