import { Component, OnInit, LOCALE_ID, Inject } from '@angular/core'
import { FormControl, FormGroup, Validators } from '@angular/forms'
import { ActivatedRoute, Router } from '@angular/router'
import { MessageService } from 'primeng/api'
import { AuthorizationService } from 'src/app/core/api-authorization'
import { ProfileClient, RegisterClient } from 'src/app/shared/api.generated.clients'
import { environment } from '../../../environments/environment'
import { RegisterService } from '../register/register.service'
import { ChangeDetectorRef } from '@angular/core'

@Component({
  selector: 'app-login',
  templateUrl: './login-new.component.html',
  styleUrls: ['./login.component.scss'],
  providers: [MessageService],
})
export class LoginComponent implements OnInit {
  private baseUrl: string
  loginForm: FormGroup
  isSubmitting = false
  uaeUrl: string
  clientId: string
  redirectToUAE: string
  redirectUrl: string
  firstLoginURL: string
  firstLoginParams: string

  getProfileDataUrl = 'https://qa-id.uaepass.ae/trustedx-resources/openid/v1/users/me'

  get disableForm(): boolean {
    return this.isSubmitting
  }

  constructor(
    private register: RegisterClient,
    private registerService: RegisterService,
    private authService: AuthorizationService,
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private messageService: MessageService,
    private profile: ProfileClient,
    private cdref: ChangeDetectorRef,
    @Inject(LOCALE_ID) public locale: string
  ) {
    this.buildForm()
    this.baseUrl = environment.baseUrl
    this.uaeUrl = environment.uaeUrl
    this.clientId = environment.clientID
    this.redirectUrl = this.baseUrl + '/' + locale + '/authorize/login'
    this.firstLoginURL = this.baseUrl + '/' + locale + '/registrant/profile/general-information'

    this.redirectToUAE =
      this.uaeUrl +
      '?redirect_uri=' +
      this.baseUrl +
      '/' +
      locale +
      '/authorize/login&client_id=' +
      this.clientId +
      '&response_type=code&state=ShNP22hyl1jUU2RGjTRkpg==&scope=urn:uae:digitalid:profile:general&acr_values=urn:safelayer:tws:policies:authentication:level:low&ui_locales=en'
  }

  ngOnInit(): void {
     this.activatedRoute.queryParams.subscribe((params) => {
       this.firstLoginParams = params.fLogin
     })

     const uaeToken = localStorage.getItem('uaeCode')
     if (uaeToken) {
       this.isSubmitting = true
       try {
         this.register.token(uaeToken, this.redirectUrl).subscribe((data) => {
          //  console.log('Token Data: ' + data)
           this.register.profile(data.access_token).subscribe((profileData) => {
            //  console.log('Profile Data: ' + profileData)
            //  console.log('Email: ' + profileData.email)
            //  console.log('Access Token: ' + profileData.access_token)
             this.loginUAE(profileData, profileData.email, data.access_token)
           })
         })
       } catch (e) {
         console.log('Error: ', e)
       }
     }
     this.isSubmitting = false
  }

  ngAfterViewInit() {
     if (localStorage.getItem('isUAE') && localStorage.getItem('isUAE') === 'false') {
       this.userCancelMessage()
       localStorage.removeItem('isUAE')
     }
     this.cdref.detectChanges()
  }

  async removeLocal() {
    if (localStorage.getItem('uaeCode')) {
      localStorage.removeItem('uaeCode')
    }
    localStorage.setItem('isUAE', 'false')
    window.location.href = this.redirectToUAE
  }

  async loginUAE(profileData, email: string, password: string) {
    if (email && password) {
      this.isSubmitting = true

      try {
        try {
          await this.authService.acquireAccessTokenWithPasswordFlow(email, password)
        } catch (error) {
          if (error.status === 400) {
            const newUserData = {
              firstName: profileData.firstnameEN,
              lastName: profileData.lastnameEN,
              nationality: 'AE',
              email: profileData.email,
              mobilePhone: '+' + profileData.mobile,
              password: 'P@ssw0rd',
              emiratesId: profileData.idn,
              isUAELogin: true,
            }
            this.registerService.register(newUserData).then((value) => {
              this.loginUAE(profileData, email, password)
            })
          } else {
            this.messageService.add({
              severity: 'error',
              life: 6000,
              closable: true,
              summary: $localize`Login Failed`,
              detail: $localize`We were unable to log you in at this time. Please try again later.`,
            })
          }
        }

        this.profile.profile().subscribe(async (data) => {
          this.authService.profileInfo = data
          localStorage.setItem('profile_info', JSON.stringify(data))
          const returnUrl = this.activatedRoute.snapshot.queryParams?.returnUrl
          await this.router.navigate(['/'])
        })
      } finally {
        this.isSubmitting = false
      }
    }
  }

  async login() {
    if (this.loginForm.invalid) {
      return
    }

    this.isSubmitting = true

    try {
      try {
        const { email, password } = this.loginForm.value
        await this.authService.acquireAccessTokenWithPasswordFlow(email, password)
      } catch (error) {
        if (error.status === 400) {
          this.loginForm.get('password').reset()
          this.messageService.add({
            severity: 'error',
            life: 3000,
            summary: $localize`Invalid email or password`,
          })
        } else {
          this.messageService.add({
            severity: 'error',
            life: 6000,
            closable: true,
            summary: $localize`Login Failed`,
            detail: $localize`We were unable to log you in at this time. Please try again later.`,
          })
        }
        return
      }

      this.profile.profile().subscribe(async (data) => {
        this.authService.profileInfo = data
        localStorage.setItem('profile_info', JSON.stringify(data))
        const returnUrl = this.activatedRoute.snapshot.queryParams?.returnUrl
        if (this.firstLoginParams == 'true') {
          window.location.href = this.firstLoginURL
        }
        if (returnUrl) {
          await this.router.navigateByUrl(returnUrl)
        } else {
          // TODO remove magical string
          await this.router.navigate(['/'])
        }
      })
    } finally {
      this.isSubmitting = false
    }
  }

  private buildForm() {
    this.loginForm = new FormGroup({
      email: new FormControl('', [Validators.required]),
      password: new FormControl('', [Validators.required]),
    })
  }

  userCancelMessage() {
    this.messageService.add({
      severity: 'error',
      life: 6000,
      closable: true,
      summary: $localize`Login Failed`,
      detail: $localize`User cancelled the Login`,
    })
  }
}
