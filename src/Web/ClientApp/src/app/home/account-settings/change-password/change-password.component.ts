import { Component } from '@angular/core'
import { FormGroup, FormControl, Validators } from '@angular/forms'
import { ActivatedRoute, Router } from '@angular/router'
import { equalTo, password as passwordValidator } from 'src/app/shared/validators'
import { ChangePasswordFacadeService } from './change-password-facade.service'
import { MessageService } from 'primeng/api'

@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
  providers: [MessageService],
})
export class ChangePasswordComponent {
  changePasswordForm: FormGroup
  isFormSubmitting = false
  isFormSubmitted = false

  constructor(
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private facade: ChangePasswordFacadeService,
    private messageService: MessageService
  ) {}

  ngOnInit() {
    this.buildForm()
  }

  buildForm() {
    const currentPassword = new FormControl('', [Validators.required])

    const newPassword = new FormControl('', [
      Validators.required,
      passwordValidator({
        minLength: 8,
        maxLength: 32,
        atLeastOneDigit: true,
        atLeastOneLowerCaseLetter: true,
        atLeastOneUpperCaseLetter: true,
      }),
    ])

    this.changePasswordForm = new FormGroup({
      currentPassword,
      newPassword,
      confirmNewPassword: new FormControl('', [Validators.required, equalTo(newPassword)]),
    })
  }

  async submitChangePassword() {
    this.isFormSubmitting = true
    try {
      const formValues = this.changePasswordForm.value
      await this.facade.changePassword(formValues)
      this.isFormSubmitted = true
    } catch (error) {
      this.messageService.add({
        severity: 'error',
        life: 6000,
        closable: true,
        summary: $localize`Wrong Password`,
        detail: $localize`The Old Password you entered is wrong.`,
      })
      console.log(error)
      return
    } finally {
      this.isFormSubmitting = false
    }
  }
}
