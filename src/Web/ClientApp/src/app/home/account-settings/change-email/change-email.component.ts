import { Component, OnInit } from '@angular/core'
import { FormGroup, FormControl, Validators } from '@angular/forms'
import { email } from 'src/app/shared/validators'
import { Router } from '@angular/router'
import { ChangeEmailFacadeService } from './change-email-facade.service'
import { MessageService } from 'primeng/api'
import { ApiProblemDetails } from 'src/app/shared/api.generated.clients'

@Component({
  selector: 'app-change-email',
  templateUrl: './change-email.component.html',
  styleUrls: ['./change-email.component.css'],
  providers: [MessageService],
})
export class ChangeEmailComponent implements OnInit {
  changeEmailForm: FormGroup
  isFormSubmitting = false
  isFormSubmitted = false

  constructor(
    private router: Router,
    private facade: ChangeEmailFacadeService,
    private messageService: MessageService
  ) {}

  ngOnInit() {
    this.buildForm()
  }

  buildForm() {
    this.changeEmailForm = new FormGroup({
      newEmail: new FormControl('', [Validators.required, email()]),
    })
  }

  goHome() {
    this.router.navigate(['/'])
  }

  showCaptchaResponse(event: any) {
    this.messageService.add({
      severity: 'info',
      summary: 'Succees',
      detail: 'User Responded',
      sticky: true,
    })
  }

  // async submitChangeEmail() {
  //   this.isFormSubmitting = true
  //   try {
  //     const formValues = this.changeEmailForm.value
  //     await this.facade.changeEmail(formValues)
  //     this.isFormSubmitted = true
  //   } catch (error) {
  //     if (error.status === 422) {
  //       this.messageService.add({
  //         severity: 'error',
  //         summary: $localize`Invalid email`,
  //         detail: $localize`This email is not available`,
  //         life: 10000,
  //       })
  //     }
  //   } finally {
  //     this.isFormSubmitting = false
  //   }
  // }

  async submitChangeEmail() {
    if (this.changeEmailForm.invalid) {
      this.changeEmailForm.markAllAsTouched() // 👈 trigger validation
      return // 👈 stop submit
    }

    this.isFormSubmitting = true

    try {
      const formValues = this.changeEmailForm.value
      await this.facade.changeEmail(formValues)
      this.isFormSubmitted = true
    } catch (error) {
      if (error.status === 422) {
        this.messageService.add({
          severity: 'error',
          summary: $localize`Invalid email`,
          detail: $localize`This email is not available`,
          life: 10000,
        })
      }
    } finally {
      this.isFormSubmitting = false
    }
  }
}
