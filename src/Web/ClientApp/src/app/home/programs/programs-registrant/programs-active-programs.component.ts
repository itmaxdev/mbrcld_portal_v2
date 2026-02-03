import { Component, Input, OnInit, ViewEncapsulation } from '@angular/core'
import { Inject, LOCALE_ID } from '@angular/core'
import { ListActiveProgramsViewModel } from 'src/app/shared/api.generated.clients'

@Component({
  selector: 'app-programs-active-programs',
  template: ` <a [routerLink]="programUrl" class="w-full">
    <p-card
      [styleClass]="data.openForRegistration ? 'program-card program-card-active' : 'program-card'"
    >
      <ng-template pTemplate="header">
        <img alt="Card" [src]="data.pictureUrl" class="header-img" />
      </ng-template>
      <p class="text-xl">{{ language === 'en' ? data.name : data.name_AR }}</p>
      <p class="info-text">{{ language === 'en' ? data.description : data.description_AR }}</p>
    </p-card>

    <!-- <div class="border rounded-lg border-gray-400 flex gap-2 p-2 programs-grid-element bg-white">
      <div class="header-img mb-4 sm:mb-0">
        <img
          [style]="!data.openForRegistration ? 'filter: grayscale(100%)' : ''"
          [src]="data.pictureUrl"
          class="header-img rounded-lg"
        />
      </div>
      <div class="text-content grid content-around">
        <h1 class="text-caption text-2xl pb-2 sm:text-xl sm:pb-0 text-black">
          {{ language === 'en' ? data.name : data.name_AR }}
        </h1>
        <p class="info-text text-lg text-gray ">
          {{ language === 'en' ? data.description : data.description_AR }}
        </p>
      </div>
    </div> -->
  </a>`,
  styles: [
    `
      .header-img {
        width: 100%;
        height: 160px;
        object-fit: cover;
        min-width: 150px !important;
      }

      .programs-grid-element {
        min-width: 240px;
        min-height: 350px;
        flex-direction: column;
        max-width: 250px;
      }
      /* 
      .info-text {
        height: 70px;
        width: 100%;
        padding-top: 25px;
        overflow: hidden;
        display: -webkit-box;
        -webkit-line-clamp: 2;
        -webkit-box-orient: vertical;
      } */
    `,
  ],
  encapsulation: ViewEncapsulation.None,
})
export class ProgramActiveProgramsComponent implements OnInit {
  @Input() data: ListActiveProgramsViewModel
  public programId: string
  public language: string

  constructor(@Inject(LOCALE_ID) locale) {
    this.language = locale
  }

  get programUrl() {
    if (this.data.openForRegistration) return 'program/' + this.programId
    else return 'program/previous/' + this.programId
  }

  ngOnInit() {
    this.programId = this.data.id
  }
}
