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
    <!-- <div class="flex flex-row-reverse justify-between items-center py-3 w-full lg:justify-end">
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
    </div> -->
    <div class="topNavWrap padder" data-topnav>
      <div class="sToolWrap">
        <div class="searchBox">
          <form class="form-v3">
            <div class="input-field">
              <input type="text" placeholder="Search" />
              <button class="iconBox">
                <svg
                  width="24"
                  height="24"
                  viewBox="0 0 24 24"
                  fill="none"
                  xmlns="http://www.w3.org/2000/svg"
                >
                  <path
                    d="M11 19C15.4183 19 19 15.4183 19 11C19 6.58172 15.4183 3 11 3C6.58172 3 3 6.58172 3 11C3 15.4183 6.58172 19 11 19Z"
                    stroke="currentcolor"
                    stroke-width="2"
                    stroke-linecap="round"
                    stroke-linejoin="round"
                  />
                  <path
                    d="M21 20.9999L16.65 16.6499"
                    stroke="currentcolor"
                    stroke-width="2"
                    stroke-linecap="round"
                    stroke-linejoin="round"
                  />
                </svg>
              </button>
            </div>
          </form>
        </div>
        <a href="javascript:void(0)" class="roundBtn" (click)="toggleLanguage()">
          <span>{{ locale && locale.toString().startsWith('ar') ? 'EN' : 'AR' }}</span>
        </a>
        <a href="javascript:void(0)" class="roundBtn" data-count="7">
          <svg
            width="22"
            height="22"
            viewBox="0 0 22 22"
            fill="none"
            xmlns="http://www.w3.org/2000/svg"
          >
            <path
              d="M11 22C11.7293 22 12.4288 21.7103 12.9445 21.1945C13.4603 20.6788 13.75 19.9793 13.75 19.25H8.25C8.25 19.9793 8.53973 20.6788 9.05546 21.1945C9.57118 21.7103 10.2707 22 11 22ZM12.3681 1.51113C12.3873 1.31994 12.3662 1.12685 12.3062 0.944315C12.2462 0.761779 12.1486 0.59385 12.0197 0.451356C11.8908 0.308863 11.7334 0.194969 11.5578 0.11702C11.3822 0.0390714 11.1921 -0.00120163 11 -0.00120163C10.8079 -0.00120163 10.6178 0.0390714 10.4422 0.11702C10.2666 0.194969 10.1092 0.308863 9.98033 0.451356C9.85142 0.59385 9.75381 0.761779 9.6938 0.944315C9.63378 1.12685 9.61268 1.31994 9.63188 1.51113C8.0775 1.82676 6.68007 2.67013 5.67643 3.89831C4.67279 5.12648 4.12468 6.6639 4.125 8.25C4.125 9.75975 3.4375 16.5 1.375 17.875H20.625C18.5625 16.5 17.875 9.75975 17.875 8.25C17.875 4.9225 15.51 2.145 12.3681 1.51113Z"
              fill="currentcolor"
            />
          </svg>
        </a>
      </div>

      <div class="loginInfoWrap">
        <div class="name">Welcome, User</div>
        <!-- status options available : [online , offline , away]  -->
        <a
          href="javascript:void(0)"
          class="avImg abs"
          data-status="online"
          (click)="menu.toggle($event)"
        >
          <p-slideMenu #menu [model]="menuItems" [popup]="true" appendTo="body"></p-slideMenu>
          <picture>
            <source [srcset]="profilePicUrl" type="image/webp" />
            <source [srcset]="profilePicUrl" type="image/jpeg" />
            <img [src]="profilePicUrl" alt="" width="100" height="100" loading="lazy" />
          </picture>
        </a>
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

  toggleLanguage() {
    const referer = encodeURIComponent(location.href)
    const isArabic = this.locale && this.locale.toString().startsWith('ar')
    const langValue = isArabic ? 'en-US' : 'ar-SA'
    location.href = `/SetPreferredLanguage?lang=${encodeURIComponent(langValue)}&referer=${referer}`
  }

  // openChangeEmail() {
  //   this.router.navigate(['home/account-settings/change-email'])
  // }

  // openChangePassword() {
  //   this.router.navigate(['home/account-settings/change-password'])
  // }
}
