import { Component, OnInit, ViewEncapsulation, Inject, LOCALE_ID } from '@angular/core'
import { ActivatedRoute, Router } from '@angular/router'
import { Location } from '@angular/common'
import {
  GetProgramDetailsByIdViewModel,
  ProgramsClient,
  EnrollmentsClient,
  IAddEnrollmentCommand,
} from 'src/app/shared/api.generated.clients'
import { ProgramApplyService } from '../program-apply/program-apply.service'
import { ProfileFacade } from '../../profile/common/profile-facade.service'

@Component({
  selector: 'app-program-active-programs',
  template: `
    <div class="mainContainer" *ngIf="programDetail">
      <div class="gridWrap">
        <div class="generalCard">
          <div class="inner">
            <div class="cardTitleWrap">
              <div class="cardTitle" i18n>Programs</div>
              <div class="cardTools">
                <a (click)="goBack()" class="backBtn" style="cursor: pointer;">
                  <span i18n>Back</span>
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
                  <div class="statusPin colorInfo">
                    <span i18n>Active Program</span>
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
                      <span i18n>Go Back</span>
                    </button>
                    <ng-container *ngIf="!incompleteProfile && alreadyEnrolled && programId">
                      <button
                        class="more wAuto"
                        style="margin-inline-start: 1rem;"
                        (click)="onClick()"
                      >
                        <span i18n>Enroll</span>
                      </button>
                    </ng-container>
                    <ng-container *ngIf="incompleteProfile">
                      <button
                        class="more wAuto secondary"
                        style="margin-inline-start: 1rem;"
                        routerLink="../../../profile"
                      >
                        <span i18n>Complete your profile</span>
                      </button>
                    </ng-container>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  `,
  styles: [
    `
      // Layout and visuals are handled by shared styles
      // (same structure as PreviousProgramComponent).
    `,
  ],
})
export class ProgramActiveProgramComponent implements OnInit {
  alreadyEnrolled = false
  incompleteProfile = false
  programDetail: GetProgramDetailsByIdViewModel = {} as any
  programId: string
  ready = false
  language: string
  enrollmentId: string

  constructor(
    private programs: ProgramsClient,
    private _location: Location,
    private route: ActivatedRoute,
    private router: Router,
    private profile: ProfileFacade,
    private enrollmentsClient: EnrollmentsClient,
    public shared: ProgramApplyService,
    @Inject(LOCALE_ID) locale
  ) {
    this.language = locale
  }

  async ngOnInit() {
    this.programId = this.route.snapshot.paramMap.get('programId')

    const data = await this.programs.programDetails(this.programId).toPromise()
    if (data) {
      this.programDetail = data

      const profileCompletion = await this.profile.loadFormProgress()
      if (profileCompletion.completionPercentage < 100) {
        this.incompleteProfile = true
        return
      }
      if (data.enrollmentId == null) {
        const body: IAddEnrollmentCommand = {
          programId: this.programId,
          id: this.programId, // Just to don't be null
        }
        const newEnrollmentId = await this.enrollmentsClient.enrollmentsPost(body).toPromise()

        this.enrollmentId = newEnrollmentId
        if (data.enrollmentStatus === 'applied' || !data.enrollmentStatus) {
          this.alreadyEnrolled = true
        }
      } else {
        this.enrollmentId = data.enrollmentId
        if (data.enrollmentStatus === 'applied' || !data.enrollmentStatus) {
          this.alreadyEnrolled = true
        }
      }
    }

    this.ready = true
  }

  goBack() {
    this._location.back()
  }

  onClick() {
    this.shared.setEnrollmentId(this.enrollmentId)
    this.router.navigate([`../../apply/${this.programId}`], { relativeTo: this.route })
  }
}
