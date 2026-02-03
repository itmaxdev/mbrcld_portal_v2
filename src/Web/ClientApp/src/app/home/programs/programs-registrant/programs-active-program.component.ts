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
    <div class="program-detail-container" *ngIf="programDetail">
      <img
        class="program-image"
        [ngClass]="{ 'img-rtl': language == 'ar' }"
        [src]="programDetail.pictureUrl"
      />
      <div class="program-detail-content" [ngClass]="{ rtl: language == 'ar' }">
        <h1 class="program-section-title p-text-secondary" i18n>Active Programs</h1>

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
          <button pButton label="Go Back" i18n-label class=" mr-8" (click)="goBack()"></button>
          <div *ngIf="!incompleteProfile && alreadyEnrolled && programId">
            <button pButton label="Enroll" i18n-label class="w-64 " (click)="onClick()"></button>
          </div>
          <div *ngIf="incompleteProfile">
            <button
              pButton
              label="Complete your profile"
              i18n-label
              class="w-64 p-button-secondary"
              routerLink="../../../profile"
            ></button>
          </div>
        </div>
      </div>
    </div>
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
