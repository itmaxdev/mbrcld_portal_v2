import { RegisterService } from './register.service'
import { ApiProblemDetails, IRegisterNewUserCommand } from './register.service'
import { Component, OnDestroy, OnInit } from '@angular/core'
import { FormControl, FormGroup, Validators } from '@angular/forms'
import { ReplaySubject, Subject } from 'rxjs'
import { takeUntil } from 'rxjs/operators'
import { AuthorizationService } from 'src/app/core/api-authorization'
import { MessageService } from 'primeng/api'

import {
  email,
  equalTo,
  password as passwordValidator,
  requiredIf2,
} from 'src/app/shared/validators'

@Component({
  selector: 'app-register',
  templateUrl: './register-new.component.html',
  styleUrls: ['register.component.scss'],
  providers: [MessageService],
})
export class RegisterComponent implements OnInit, OnDestroy {
  registrationForm: FormGroup
  isFormSubmitting = false
  isFormSubmittingSubject$ = new ReplaySubject<boolean>()
  hasRegisteredSuccessfully = false

  private destroy$ = new Subject<boolean>()
  isCaptcha = false
  captchaValidation = false

  get disableForm(): boolean {
    return this.isFormSubmitting
  }

  constructor(
    private registerService: RegisterService,
    private authService: AuthorizationService,
    private messageService: MessageService
  ) {}

  ngOnInit(): void {
    this.buildForm()
    this.isFormSubmittingSubject$.pipe(takeUntil(this.destroy$)).subscribe((isSubmitting) => {
      this.isFormSubmitting = isSubmitting
    })
  }

  ngOnDestroy(): void {
    this.destroy$.next(true)
  }

  async register(): Promise<void> {
    if (this.registrationForm.invalid) {
      return
    }

    if (!this.isCaptcha) {
      this.captchaValidation = true
      return
    }

    this.isFormSubmittingSubject$.next(true)

    try {
      const formValue = this.registrationForm.value as FormModel
      await this.registerService.register(formValue)
      this.hasRegisteredSuccessfully = true
    } catch (error) {
      if (error instanceof ApiProblemDetails) {
        if (error.errors?.includes('DuplicateEmail')) {
          const email = this.registrationForm.get('email').value as string
          this.messageService.add({
            summary: $localize`Email already taken`,
            detail: $localize`The email '${email}' is already taken`,
            closable: true,
            life: 10000,
            severity: 'error',
          })
          return
        } else if (error.errors[0]?.includes('Emirate')) {
          const emirate = this.registrationForm.get('emiratesId').value as string
          this.messageService.add({
            summary: $localize`${error.errors[0]}`,
            detail: $localize`Emirates Id '${emirate}' already exist.`,
            closable: true,
            life: 10000,
            severity: 'error',
          })
          return
        }
      }

      this.messageService.add({
        summary: $localize`Registration failed`,
        detail: $localize`We were unable to complete the registration at this time. Please try again later.`,
        closable: true,
        life: 10000,
        severity: 'error',
      })
    } finally {
      this.isFormSubmittingSubject$.next(false)
    }
  }

  get isEmiratesIdRequired() {
    return (this.registrationForm.get('nationality').value as string)?.toUpperCase() === 'AE'
  }

  private buildForm(): void {
    const password = new FormControl('', [
      Validators.required,
      passwordValidator({
        minLength: 8,
        maxLength: 32,
        atLeastOneDigit: true,
        atLeastOneLowerCaseLetter: true,
        atLeastOneUpperCaseLetter: true,
      }),
    ])

    const confirmPassword = new FormControl('', [Validators.required, equalTo(password)])

    const nationality = new FormControl('', [Validators.required])
    const emiratesId = new FormControl('', [
      requiredIf2(() => {
        return (nationality.value as string)?.toUpperCase() === 'AE'
      }),
    ])

    this.registrationForm = new FormGroup({
      nationality,
      emiratesId,
      password,
      confirmPassword,
      firstName: new FormControl('', [Validators.required]),
      lastName: new FormControl('', [Validators.required]),
      email: new FormControl('', [Validators.required, email()]),
      mobilePhone: new FormControl('', [Validators.required]),
    })

    nationality.valueChanges
      .pipe(takeUntil(this.destroy$))
      .subscribe(() => emiratesId.updateValueAndValidity())
  }

  showCaptchaResponse(event) {
    this.isCaptcha = true
    this.captchaValidation = false
  }
}

type FormModel = { [key in keyof IRegisterNewUserCommand]: any }
