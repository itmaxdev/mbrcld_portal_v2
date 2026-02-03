import { Component, Input, LOCALE_ID, Inject, OnInit, ViewEncapsulation } from '@angular/core'
import { MenuItem } from 'primeng/api'
import { ListAlumniAvailableProgramViewModel } from 'src/app/shared/api.generated.clients'
import { MessageService } from 'primeng/api'
import { Router } from '@angular/router'
import { SectionDataService } from 'src/app/shared/services/section-data.service'

@Component({
  selector: 'app-program-suggested-view',
  template: ` <p-toast key="tr"></p-toast>
    <a [routerLink]="isEnroll ? ['../apply/' + data.id] : null" class="w-full">
      <div
        class="border rounded-lg border-gray-400 flex gap-2 p-2"
        (click)="!isEnroll ? showCompletionMessage() : null"
      >
        <div class="header-img mr-4 mb-4 sm:mb-0">
          <img [src]="data.pictureUrl" alt="" class="header-img rounded-lg" />
        </div>
        <div class="text-content grid content-around">
          <h1 class="text-caption text-2xl pb-2 sm:text-xl sm:pb-0 text-black">
            {{ locale == 'ar' ? data.name_AR : data.name }}
          </h1>
          <p class="info-text text-lg text-gray">
            {{ locale == 'ar' ? data.desription_AR : data.desription }}
          </p>
        </div>
      </div>
    </a>`,
  styles: [
    `
      .header-img {
        width: 150px;
        height: 114px;
        object-fit: cover;
        min-width: 150px !important;
      }
      .info-text {
        height: 70px;
        overflow: hidden;
        width: 100%;
        display: -webkit-box;
        -webkit-line-clamp: 3;
        -webkit-box-orient: vertical;
      }
    `,
  ],
  encapsulation: ViewEncapsulation.None,
  providers: [MessageService],
})
export class ProgramSuggestedViewComponent implements OnInit {
  public items: MenuItem[]
  public isEnroll: boolean
  @Input() data: ListAlumniAvailableProgramViewModel
  @Input() incompleteProfile: boolean
  @Input() alreadyEnrolled: boolean
  @Input() isActiveProgramID: boolean

  constructor(
    private messageService: MessageService,
    private router: Router,
    private sectionData: SectionDataService,
    @Inject(LOCALE_ID) public locale: string
  ) {
    // if (!this.incompleteProfile && !this.alreadyEnrolled && this.isActiveProgramID) {
    //   this.isEnroll = true
    // } else if (this.incompleteProfile) {
    //   this.isEnroll = false
    // }
  }

  ngOnInit() {
    if (this.incompleteProfile) {
      this.isEnroll = false
    } else {
      this.isEnroll = true
    }
  }

  showCompletionMessage() {
    this.messageService.add({
      key: 'tr',
      life: 5000,
      severity: 'warn',
      summary: 'Warning',
      detail: 'Please complete your profile. Redirect to the profile page after 5 seconds.',
    })
    setTimeout(() => {
      this.router.navigateByUrl(this.sectionData.redirectBack(1, true) + '/profile')
    }, 5000)
  }
}
