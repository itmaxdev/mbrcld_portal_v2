import { Component, OnInit, OnDestroy, LOCALE_ID, Inject } from '@angular/core'
import { MenuItem } from 'primeng/api'
import { Subject } from 'rxjs'
import { Router } from '@angular/router'
import { AuthorizationService } from 'src/app/core/api-authorization'
import { UserDocumentsClient } from 'src/app/shared/api.generated.clients'
import { ProfileFacade } from '../../profile/common/profile-facade.service'
import { environment } from '../../../../environments/environment'

@Component({
  selector: 'app-user-toolbar',
  styleUrls: ['./user-info/user-info.component.scss'],
  template: `
    <div class="flex flex-row-reverse justify-between items-center py-3 w-full lg:justify-end">
      <div class="flex profile-picture-menu">
        <img
          class="h-12 w-12 object-contain rounded-full cursor-pointer"
          [src]="profilePicUrl"
          onerror="this.src= 'assets/images/no-photo.png'"
          (click)="menu.toggle($event)"
        />
        <p-slideMenu #menu [model]="menuItems" [popup]="true" appendTo="body"></p-slideMenu>
      </div>
      <div class="ms-3 lg:flex">
        <app-language-switcher></app-language-switcher>
      </div>
    </div>
  `,
})
export class UserToolbarComponent implements OnInit, OnDestroy {
  roleName: string
  menuItems: MenuItem[]
  profilePicUrl: string
  private logOutUrl: string

  private destroy$ = new Subject<boolean>()

  constructor(
    private authService: AuthorizationService,
    private profileFacade: ProfileFacade,
    private router: Router,
    private documentsClients: UserDocumentsClient,
    @Inject(LOCALE_ID) public locale: string
  ) {
    this.roleName = this.router.url.split('/')[1]

    this.logOutUrl =
      environment.logOutUrl +
      '?redirect_uri=' +
      environment.baseUrl +
      '/' +
      locale +
      '/' +
      this.roleName +
      '/account-settings/logout'
  }

  ngOnInit() {
    const profileInfo = JSON.parse(localStorage.getItem('profile_info'))
    if (profileInfo && [1, 2, 3].indexOf(profileInfo.role) != -1) {
      this.menuItems = [
        {
          label: $localize`User Manual`,
          icon: 'custom-icon',
          command: () => this.downloadUserManual(),
        },
        {
          label: $localize`Change Email`,
          icon: 'pi pi-envelope',
          url: this.roleName + '/account-settings/change-email',
        },
        {
          label: $localize`Change Password`,
          icon: 'pi pi-key',
          url: this.roleName + '/account-settings/change-password',
        },
        {
          label: $localize`Delete Account`,
          icon: 'pi pi-trash',
          url: this.roleName + '/account-settings/delete-account',
        },
        {
          label: $localize`Logout`,
          icon: 'pi pi-sign-out',
          command: this.logOut.bind(this),
        },
      ]
    } else {
      this.menuItems = [
        {
          label: $localize`User Manual`,
          icon: 'custom-icon',
          command: () => this.downloadUserManual(),
        },
        {
          label: $localize`Change Email`,
          icon: 'pi pi-envelope',
          url: this.roleName + '/account-settings/change-email',
        },
        {
          label: $localize`Change Password`,
          icon: 'pi pi-key',
          url: this.roleName + '/account-settings/change-password',
        },
        {
          label: $localize`Logout`,
          icon: 'pi pi-sign-out',
          command: this.logOut.bind(this),
        },
      ]
    }
    this.profilePicUrl = profileInfo.profilePictureUrl
    this.profileFacade.loadProfile()
  }

  ngOnDestroy() {
    this.destroy$.next(true)
  }

  deleteAccount() {}

  downloadUserManual() {
    this.documentsClients.roleUserManual().subscribe((data) => {
      if (data) {
        const a = document.createElement('a')
        a.setAttribute('download', 'user-manual.pdf')
        a.href = data
        a.click()
        a.remove()
      }
    })
  }

  async logOut() {
    localStorage.removeItem('programId')
    localStorage.removeItem('enrollmentId')
    if (localStorage.getItem('uaeCode')) {
      localStorage.removeItem('uaeCode')
      window.location.href = this.logOutUrl
    } else {
      await this.authService.logout()
      localStorage.removeItem('profile_info')
      localStorage.removeItem('uaeCode')
      this.authService.login()
    }
  }

  // openChangeEmail() {
  //   this.router.navigate(['home/account-settings/change-email'])
  // }

  // openChangePassword() {
  //   this.router.navigate(['home/account-settings/change-password'])
  // }
}
