import { Component, OnInit } from '@angular/core'
import { NavigationEnd, NavigationStart, Router } from '@angular/router'
import { Location } from '@angular/common'
import { ProfileFacade } from './home/profile/common/profile-facade.service'
import { MessageService } from 'primeng/api'
import { AuthorizationService } from './core/api-authorization'
import { GlobalVariablesService } from './shared/services/global-variables.service'

let alreadyChecked = false

declare let dataLayer: any[]

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
  providers: [MessageService],
})
export class AppComponent implements OnInit {
  public routeUrls: string[] = []
  public roomId: string

  constructor(
    private authService: AuthorizationService,
    private location: Location,
    private router: Router,
    private profile: ProfileFacade,
    private messageService: MessageService,
    private globalVariables: GlobalVariablesService
  ) {
    this.router.events.subscribe(async (val) => {
      if (val instanceof NavigationStart && val.url.indexOf('/profile') == -1) {
        let profilePrefix = ''
        const info = JSON.parse(localStorage.getItem('profile_info'))
        switch (info.role) {
          case 1:
            profilePrefix = '/registrant'
            break
          case 2:
            profilePrefix = '/applicant'
            break
          case 3:
            profilePrefix = '/alumni'
            break
        }
        if (profilePrefix != '' && !alreadyChecked) {
          alreadyChecked = true
          if (this.globalVariables.isProfilePercentageCompleted === null) {
            const profileCompletion = await this.profile.loadFormProgress()
            if (profileCompletion) {
              this.globalVariables.isProfilePercentageCompleted =
                profileCompletion.completionPercentage == 100
              this.globalVariables.profileIsOutDated = profileCompletion.requiresUpdate
            }
          }

          if (this.globalVariables.isProfilePercentageCompleted === false) {
            try {
              this.router.navigateByUrl(profilePrefix + '/profile')
              this.messageService.add({
                summary: $localize`Invalid operation`,
                detail: $localize`Please complete your profile!`,
                severity: 'warn',
                closable: true,
                life: 5000,
              })
            } catch (err) {
              //
            }
            return false
          }
          // if 6 months
          if (this.globalVariables.profileIsOutDated) {
            this.messageService.add({
              summary: null,
              detail: $localize`:@@profileIsOutDated:Stay up-to-date and make the most of your portal experience by updating your profile every 6 months. Your profile is a reflection of who you are, and keeping it current helps us provide you with the best personalized services and opportunities.`,
              severity: 'warn',
              closable: true,
              life: 8000,
            })
          }
        }
      } else if (val instanceof NavigationStart && val.url.indexOf('/profile') != -1) {
        alreadyChecked = false
      }

      if (!localStorage.getItem('uaeCode')) {
        if (!this.routeUrls.includes(this.location.path())) {
          this.routeUrls.push(this.location.path())
        }
        if (this.routeUrls.length > 0) {
          if (this.routeUrls[0].includes('code=')) {
            const routeArr = this.routeUrls[0].split('&')
            const code = routeArr[0].split('=')[1]
            localStorage.setItem('uaeCode', code)
            localStorage.setItem('isUAE', 'true')
            this.routeUrls = []
          }
        }
      }
    })

    this.router.events.subscribe(event => {
      if (event instanceof NavigationEnd) {
        this.pushGTMEvent(event.urlAfterRedirects);
      }
    });
  }

  ngOnDestroy() { }

  ngOnInit(): void {
    if (!this.location.path().includes('authorize/login')) {
      this.authService.getValidAccessToken()
    }
  }

  private pushGTMEvent(url: string) {

    if (typeof dataLayer !== 'undefined') {
      dataLayer.push({
        'event': 'pageview',
        'page': url
      });
    }
  }
}
