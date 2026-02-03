import { Component, LOCALE_ID, Inject, Input, OnInit, ViewEncapsulation } from '@angular/core'
import { ListAlumniGraduatedProgramViewModel } from 'src/app/shared/api.generated.clients'

@Component({
  selector: 'app-program-cohort-card',
  template: ` <a [routerLink]="['cohort/' + data.cohortId]" class="w-full">
    <div class="border rounded-lg border-gray-400 flex gap-2 p-2">
      <div class="header-img mr-4 mb-4 sm:mb-0">
        <img [src]="data.pictureUrl" alt="" class="header-img rounded-lg" />
      </div>
      <div class="text-content grid content-around">
        <h1 class="text-caption text-2xl pb-2 sm:text-xl sm:pb-0 text-black">
          {{ locale == 'ar' ? data.programName_AR : data.programName }}
        </h1>
        <p class="info-text text-lg text-gray">
          {{ locale == 'ar' ? data.programDescription_AR : data.programDescription }}
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
})
export class ProgramCohortCardComponent implements OnInit {
  @Input() data: ListAlumniGraduatedProgramViewModel

  constructor(@Inject(LOCALE_ID) public locale: string) {}

  ngOnInit() {}
}
