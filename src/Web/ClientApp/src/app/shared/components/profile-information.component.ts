import { Component, OnInit, Inject, LOCALE_ID } from '@angular/core'
import { ProfileFacade } from '../../home/profile/common/profile-facade.service'

@Component({
  selector: 'app-profile-information',
  template: `
    <div *ngIf="role === 3; else otherRoles" class="alumni-profile-container rounded-lg">
      <div
        [ngClass]="
          locale !== 'ar' ? 'alumni-profile flex items-end' : 'alumni-profile-ar flex items-end'
        "
      >
        <app-profile-image></app-profile-image>
        <div class="profile-info flex gap-4 col-span-5 items-center">
          <div class="alumni-hat-icon-container">
            <img src="assets/images/graduation-hat.svg" class="w-6 h-6 hat-icon" />
          </div>
          <h1 class="text-xl font-semibold" i18n>{{ fullName }}</h1>
        </div>
      </div>
    </div>
    <ng-template #otherRoles>
      <div class="flex grid-cols-6 items-center mb-8">
        <app-profile-image></app-profile-image>
        <div class="profile-info grid gap-4 col-span-5" style="margin-left: 16px; width: 100%;">
          <div class="flex gap-4 items-center">
            <div *ngIf="role === 2" class="applicant-book-icon-container">
              <img src="assets/images/open-book.svg" class="w-6 h-6 book-icon" />
            </div>
            <h1 class="text-xl font-semibold" i18n>{{ fullName }}</h1>
          </div>
          <div *ngIf="role === 2" class="w-3/5">
            <app-progress-bar [value]="percentage"></app-progress-bar>
          </div>
        </div>
      </div>
    </ng-template>
  `,
  styles: [
    `
      .alumni-profile-container {
        background: url('assets/images/alumni-background.jpeg');
        background-size: cover;
        background-position-x: center;
        background-position-y: center;
        height: 255px;
        position: relative;
        margin-bottom: 100px;
      }

      .alumni-profile {
        position: absolute;
        bottom: -50px;
        left: 20px;
      }

      .alumni-hat-icon-container {
        background: #e89600;
        border-radius: 50%;
        padding: 3px;
      }

      .alumni-hat-icon-container > .hat-icon {
        max-width: unset;
        filter: invert(100%) sepia(0%) saturate(0%) hue-rotate(224deg) brightness(220%)
          contrast(200%);
      }

      .applicant-book-icon-container {
        background: #0071be;
        border-radius: 50%;
        padding: 3px;
      }

      .applicant-book-icon-container > .book-icon {
        filter: invert(100%) sepia(0%) saturate(0%) hue-rotate(224deg) brightness(220%)
          contrast(200%);
      }

      .alumni-profile-ar {
        position: absolute;
        bottom: -50px;
        right: 20px;
      }
    `,
  ],
})
export class ProfileInformationComponent implements OnInit {
  fullName: string
  percentage = 0
  role: number

  constructor(private facade: ProfileFacade, @Inject(LOCALE_ID) private locale: string) {}

  async ngOnInit() {
    const formProgress = await this.facade.loadFormProgress()
    this.percentage = formProgress.completionPercentage
    const profileInfo = JSON.parse(localStorage.getItem('profile_info'))
    this.role = profileInfo.role
    if (this.locale === 'ar')
      if (profileInfo.firstName_AR !== null && profileInfo.lastName_AR != null)
        return (this.fullName = `${profileInfo.firstName_AR} ${profileInfo.lastName_AR}`)
      else return (this.fullName = ' ')
    return (this.fullName = `${profileInfo.firstName} ${profileInfo.lastName}`)
  }
}
