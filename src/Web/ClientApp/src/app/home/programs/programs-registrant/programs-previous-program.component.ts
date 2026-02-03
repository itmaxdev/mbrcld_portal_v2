import { Component, OnInit, ViewEncapsulation, Inject, LOCALE_ID } from '@angular/core'
import { ActivatedRoute } from '@angular/router'
import { Location } from '@angular/common'
import {
  GetProgramDetailsByIdViewModel,
  ProgramsClient,
} from 'src/app/shared/api.generated.clients'

@Component({
  selector: 'app-program-active-programs',
  template: `
    <div class="program-detail-container" *ngIf="programDetail">
      <img
        class="program-image"
        [ngClass]="{ 'img-rtl': language == 'ar' }"
        [src]="programDetail.pictureUrl"
      />
      <div class="program-detail-content" [ngClass]="{ rtl: language == 'ar' }">
        <h1 class="program-section-title p-text-secondary">Previous Programs</h1>

        <h1 class="text-3xl font-bold mb-8" style="color: var(--primary-color)">
          {{ language == 'en' ? programDetail.name : programDetail.name_AR }}
        </h1>
        <div class="p-flex">
          <div
            class="p-col-6 col-sm-12 ql-editor px-0"
            [innerHTML]="
              language == 'en' ? programDetail.longDescription : programDetail.longDescription_AR
            "
          ></div>
        </div>

        <div class="flex gap-4 py-10">
          <button pButton label="Go Back" i18n-label class="mr-8" (click)="goBack()"></button>
        </div>
      </div>
    </div>

    <!-- <div class="border border-t-0 border-gray-400 rounded-lg bg-white p-4">
    <div class="inline-flex items-center cursor-pointer contents" (click)="goBack()">
      <img src="assets/images/ico-arrow-left.svg" class="w-6 h-6" alt="" />
      <p class="text-lg ml-2 text-gray-700" i18n>
        Go Back
      </p>
    </div>
    <img [src]="programDetail.pictureUrl" class="block object-cover object-top flex-grow" />
    <div class="flex flex-col">
      <h3 class="text-xl font-semibold mb-8">
        {{ language == 'en' ? programDetail.name : programDetail.name_AR }}
      </h3>
      <div
        class="ql-editor"
        [innerHTML]="
          language == 'en' ? programDetail.longDescription : programDetail.longDescription_AR
        "
      ></div>
    </div>
  </div> -->
  `,
  styles: [
    `
      :host {
        position: absolute;
        top: 0;
        left: 0;
        right: 0;
        width: 100%;
        height: 100%;
      }
      .program-detail-container {
        background-size: cover !important;
        min-height: 100vh;
      }
      .program-image {
        width: 100%;
        height: 100vh;
        object-fit: cover;
        position: fixed;
        z-index: 0;
        top: 0;
        left: 0;
      }
      .program-image.img-rtl {
        transform: scaleX(-1);
      }
      .program-detail-content {
        background: linear-gradient(to right, #dadbdc 40%, transparent);
        padding: 3em;
        padding-top: 100px;
        min-height: 100vh;
        z-index: 1;
        position: absolute;
        top: 0;
        left: 0;
        width: 100%;
      }
      .program-detail-content.rtl {
        background: linear-gradient(to left, #dadbdc 40%, transparent);
      }
    `,
  ],
  // encapsulation: ViewEncapsulation.None,
})
export class PreviousProgramComponent implements OnInit {
  programDetail: GetProgramDetailsByIdViewModel = {} as any
  ready = false
  language: string

  constructor(
    private programs: ProgramsClient,
    private _location: Location,
    private route: ActivatedRoute,
    @Inject(LOCALE_ID) locale
  ) {
    this.language = locale
  }

  async ngOnInit() {
    const programId = this.route.snapshot.paramMap.get('programId')

    const data = await this.programs.programDetails(programId).toPromise()
    if (data) {
      this.programDetail = data
    }

    this.ready = true
  }

  goBack() {
    this._location.back()
  }
}
