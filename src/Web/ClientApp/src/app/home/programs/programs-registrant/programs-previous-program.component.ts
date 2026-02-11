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
    <!-- 
     old code:
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
    </div> -->
    <div class="mainContainer" *ngIf="programDetail">
      <div class="gridWrap">
        <div class="generalCard">
          <div class="inner">
            <div class="cardTitleWrap">
              <div class="cardTitle">Programs</div>
              <div class="cardTools">
                <a (click)="goBack()" class="backBtn" style="cursor: pointer;">
                  <span>Back</span>
                  <div class="iconBox">
                    <svg
                      width="24"
                      height="24"
                      viewBox="0 0 24 24"
                      fill="none"
                      xmlns="http://www.w3.org/2000/svg"
                    >
                      <path
                        d="M9 14L4 9L9 4"
                        stroke="currentcolor"
                        stroke-width="2"
                        stroke-linecap="round"
                        stroke-linejoin="round"
                      />
                      <path
                        d="M20 20V13C20 11.9391 19.5786 10.9217 18.8284 10.1716C18.0783 9.42143 17.0609 9 16 9H4"
                        stroke="currentcolor"
                        stroke-width="2"
                        stroke-linecap="round"
                        stroke-linejoin="round"
                      />
                    </svg>
                  </div>
                </a>
              </div>
            </div>

            <div class="gridWrap border">
              <div class="programDetailsWrap">
                <div class="imgBox" [ngClass]="{ 'img-rtl': language == 'ar' }">
                  <img
                    [src]="programDetail.pictureUrl"
                    alt=""
                    width="543"
                    height="912"
                    loading="lazy"
                  />
                </div>
                <div class="contentBox" [ngClass]="{ rtl: language == 'ar' }">
                  <div class="title">
                    {{ language == 'en' ? programDetail.name : programDetail.name_AR }}
                  </div>
                  <!-- supporting class list : [colorDanger, colorSuccess, colorWarning, colorInfo] -->
                  <div class="statusPin colorSuccess">
                    <span>Previous Program</span>
                  </div>

                  <div
                    class="textBox sm"
                    [innerHTML]="
                      language == 'en'
                        ? programDetail.longDescription
                        : programDetail.longDescription_AR
                    "
                  ></div>

                  <div class="moreWrap">
                    <button (click)="goBack()" class="more wAuto">
                      <span>Go Back</span>
                    </button>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>

      <div class="sideBox">
        <div class="inlineCalenderSmallCard">
          <div class="inner">
            <div class="inlineCalenderSmall" id="inlineCalenderSmall"></div>

            <div class="articleList">
              <div class="articleSmallCard">
                <a href="#" class="inner">
                  <div class="articleImg">
                    <picture class="">
                      <source srcset="./assets/images/articleImg1.webp" type="image/webp" />
                      <source srcset="./assets/images/articleImg1.jpg" type="image/jpeg" />
                      <img
                        src="./assets/images/articleImg1.jpg"
                        alt=""
                        width="80"
                        height="57"
                        loading="lazy"
                      />
                    </picture>
                  </div>

                  <div class="contentBox">
                    <div class="date">27 June 2025</div>
                    <div class="t">
                      Lorem ipsum dolor sit amet, consec tetur adipiscing elit...
                    </div>
                  </div>
                </a>
              </div>
              <div class="articleSmallCard">
                <a href="#" class="inner">
                  <div class="articleImg">
                    <picture class="">
                      <source srcset="./assets/images/articleImg2.webp" type="image/webp" />
                      <source srcset="./assets/images/articleImg2.jpg" type="image/jpeg" />
                      <img
                        src="./assets/images/articleImg2.jpg"
                        alt=""
                        width="80"
                        height="57"
                        loading="lazy"
                      />
                    </picture>
                  </div>

                  <div class="contentBox">
                    <div class="date">27 June 2025</div>
                    <div class="t">
                      Lorem ipsum dolor sit amet, consec tetur adipiscing elit...
                    </div>
                  </div>
                </a>
              </div>
            </div>
          </div>
        </div>
        <div class="leadersHighlightCard">
          <div class="imgBox">
            <picture class="">
              <source srcset="./assets/images/leadersHighlightImg.jpg" type="image/jpeg" />
              <img
                src="./assets/images/leadersHighlightImg.jpg"
                alt=""
                width="314"
                height="188"
                loading="lazy"
              />
            </picture>
          </div>
          <div class="contentBox">
            <div class="title">Leaders Highlights</div>

            <div class="points">
              <div class="p">
                <div class="num" dir="ltr" data-animcount="700">
                  <i class="not-italic">700</i><sup>+</sup>
                </div>
                <span>MBRCLD Alumni</span>
              </div>
              <div class="p">
                <div class="num" dir="ltr" data-animcount="40">
                  <i class="not-italic">40</i><sup>+</sup>
                </div>
                <span>Sectors</span>
              </div>
              <div class="p">
                <div class="num" dir="ltr" data-animcount="1000">
                  <i class="not-italic">1000</i><sup>+</sup>
                </div>
                <span>Transformational ideas and projects</span>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  `,
  styles: [
    `
      // old css:

      // :host {
      //   position: absolute;
      //   top: 0;
      //   left: 0;
      //   right: 0;
      //   width: 100%;
      //   height: 100%;
      // }
      // .program-detail-container {
      //   background-size: cover !important;
      //   min-height: 100vh;
      // }
      // .program-image {
      //   width: 100%;
      //   height: 100vh;
      //   object-fit: cover;
      //   position: fixed;
      //   z-index: 0;
      //   top: 0;
      //   left: 0;
      // }
      // .program-image.img-rtl {
      //   transform: scaleX(-1);
      // }
      // .program-detail-content {
      //   background: linear-gradient(to right, #dadbdc 40%, transparent);
      //   padding: 3em;
      //   padding-top: 100px;
      //   min-height: 100vh;
      //   z-index: 1;
      //   position: absolute;
      //   top: 0;
      //   left: 0;
      //   width: 100%;
      // }
      // .program-detail-content.rtl {
      //   background: linear-gradient(to left, #dadbdc 40%, transparent);
      // }
    `,
  ],
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
