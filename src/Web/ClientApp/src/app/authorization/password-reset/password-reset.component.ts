import { Component, Inject, LOCALE_ID } from '@angular/core'
import { FormControl, FormGroup, Validators } from '@angular/forms'
import { ActivatedRoute, Router } from '@angular/router'
import { MessageService } from 'primeng/api'
import { AuthorizationService } from 'src/app/core/api-authorization/authorization.service'
import { email } from 'src/app/shared/validators'
import { PasswordResetFacadeService } from './password-reset-facade.service'
import Swiper from 'swiper'
import { Autoplay, EffectCreative } from 'swiper/modules'

@Component({
  selector: 'app-password-reset',
  templateUrl: './password-reset.component.html',
  styleUrls: ['./password-reset.component.scss'],
  providers: [MessageService],
})
export class PasswordResetComponent {
  resetPasswordForm: FormGroup
  isSubmitting = false
  isFormSubmitted = false

  get disableForm(): boolean {
    return this.isSubmitting
  }

  constructor(
    private authService: AuthorizationService,
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private messageService: MessageService,
    private facade: PasswordResetFacadeService,
    @Inject(LOCALE_ID) public locale: string
  ) {
    this.buildForm()
  }

  ngAfterViewInit() {
    this.runSwiper()
  }

  async resetPassword() {
    if (this.resetPasswordForm.invalid) {
      return
    }

    this.isSubmitting = true
    try {
      const formValue = this.resetPasswordForm.value

      await this.facade.resetPassword(formValue)
      this.isFormSubmitted = true
    } catch (error) {
      console.log(error)
    } finally {
      this.isSubmitting = false
    }
  }

  private buildForm() {
    this.resetPasswordForm = new FormGroup({
      email: new FormControl('', [Validators.required, email()]),
    })
  }

  runSwiper() {
    new Swiper('[data-homeslider] .swiper', {
      modules: [Autoplay, EffectCreative],

      speed: 1000,
      slidesPerView: 1,
      // loop: true,
      autoplay: {
        delay: 4000,
        disableOnInteraction: false,
      },
      effect: 'creative',
      creativeEffect: {
        limitProgress: 1,

        prev: {
          shadow: false,
          translate: ['0%', '-100%', -400],
          origin: 'left bottom',
        },
        next: {
          shadow: false,
          translate: ['115%', '15%', -400],
          origin: 'left bottom',
        },
      },
      on: {},
    })
  }
}
