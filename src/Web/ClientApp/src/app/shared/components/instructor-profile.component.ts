import { Component, Input } from '@angular/core'

@Component({
  selector: 'app-instructor-profile',
  template: `<div>
    <div class="grid grid-cols-1 gap-2 sm:grid-cols-3">
      <div class="left-sidebar sm:col-span-1 flex flex-col">
        <div class="profile-image w-5/6">
          <img
            class="w-full max-h-80"
            [src]="info.pictureUrl"
            onerror="this.src='assets/images/no-photo.png'"
            alt=""
          />
        </div>
        <div class="social-info mt-8">
          <div class="email flex items-center">
            <img class="w-6 h-6" src="assets/images/ico-email.svg" alt="" />
            <p class="text-lg text-gray-600 ml-2">{{ info.email }}</p>
          </div>
        </div>
      </div>
      <div class="right-sidebar col-span-2 grid gap-12">
        <div class="main-info grid gap-2">
          <h1 class="text-2xl">{{ info.name }}</h1>
          <span class="text-lg">{{ info.jobPosition }}</span>
          <div class="linkedIn-block">
            <a [attr.href]="info.linkedIn ? info.linkedIn : null"
              ><img class="w-6 h-6" src="assets/images/ico-linkedin.svg" alt=""
            /></a>
          </div>
        </div>
        <div class="human-info grid gap-2">
          <p class="text-lg" dir="ltr">
            {{ info.aboutMember }}
          </p>
        </div>
        <div class="short-info grid gap-2">
          <p class="text-lg font-bold">
            <span i18n>Nationality</span>
            <span>:</span>
            <span class="font-normal"> {{ info.nationalityName }}</span>
          </p>
          <p class="text-lg font-bold">
            <span i18n>Residence country</span>
            <span>:</span>
            <span class="font-normal"> {{ info.residenceCountryName }}</span>
          </p>
          <p class="text-lg font-bold">
            <span i18n>Education</span>
            <span>:</span>
            <span class="font-normal"> {{ info.education }}</span>
          </p>
          <p class="text-lg font-bold">
            <span i18n>Current job position</span>
            <span>:</span>
            <span class="font-normal"> {{ info.jobPosition }}</span>
          </p>
        </div>
      </div>
    </div>
  </div>`,
})
export class InstructorProfileComponent {
  @Input() info: any

  constructor() {}
}
