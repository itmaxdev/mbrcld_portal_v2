import { ProgramApplyService } from './program-apply.service'
import { Component, OnInit } from '@angular/core'
import { ActivatedRoute, Router } from '@angular/router'
import { EnrollmentsClient, IAddEnrollmentCommand } from 'src/app/shared/api.generated.clients'
import { IEnrollmentStatus } from './program-apply.interface'
import { MenuItem } from 'primeng/api'
import { ChangeDetectorRef } from '@angular/core'

@Component({
  selector: 'app-program-apply',
  templateUrl: 'program-apply.component.html',
  styleUrls: ['program-apply.component.scss'],
})
export class ProgramApplyComponent implements OnInit {
  ready = false
  alreadyEnrolled = false
  activeIndex //= 0
  enrollmentStatus: IEnrollmentStatus = {}
  steps: MenuItem[]

  async stepperChanged(component) {
    this.enrollmentStatus = await this.shared.getEnrollmentStatus()
    this.activeIndex = component.activeIndex
    this.rebindStepper(
      this.enrollmentStatus.isAchievementStepCompleted,
      this.enrollmentStatus.isReferenceStepCompleted,
      this.enrollmentStatus.isQuestionStepCompleted,
      this.enrollmentStatus.isVideoUploaded,
      this.enrollmentStatus.isSmartAssessmentStepCompleted,
      this.enrollmentStatus.isAcknowledgmentStepCompleted
    )
  }

  constructor(
    private enrollmentsClient: EnrollmentsClient,
    private router: Router,
    private route: ActivatedRoute,
    private activatedRoute: ActivatedRoute,
    private cdRef: ChangeDetectorRef,
    public shared: ProgramApplyService
  ) {
    this.rebindStepper()
  }

  ngOnInit() {
    const programId = this.route.snapshot.paramMap.get('activeProgramId')
    this.activeProgramId(programId)
  }

  async activeProgramId(programId) {
    this.shared.setProgramId(programId)
    let enrollmentId = await this.shared.getEnrollmentId()
    if (enrollmentId == null) {
      const body: IAddEnrollmentCommand = {
        programId: programId,
        id: programId,
      }
      const resp = await this.enrollmentsClient.enrollmentsPost(body).toPromise()
      if (resp) enrollmentId = resp
    }
    this.shared.setEnrollmentId(enrollmentId)
    this.enrollmentStatus = await this.shared.getEnrollmentStatus()
    this.rebindStepper(
      this.enrollmentStatus.isAchievementStepCompleted,
      this.enrollmentStatus.isReferenceStepCompleted,
      this.enrollmentStatus.isQuestionStepCompleted,
      this.enrollmentStatus.isVideoUploaded,
      this.enrollmentStatus.isSmartAssessmentStepCompleted,
      this.enrollmentStatus.isAcknowledgmentStepCompleted
    )
    this.ready = true
  }

  async rebindStepper(
    isAchievementStepCompleted = false,
    isReferenceStepCompleted = false,
    isQuestionStepCompleted = false,
    isVideoUploaded = false,
    isSmartAssessmentCompleted = false,
    isAcknowledgmentStepCompleted = false
  ) {
    this.steps = [
      {
        label: $localize`Achievements`,
        routerLink: 'achievements',
        styleClass: isAchievementStepCompleted ? (this.activeIndex != 0 ? 'completed' : '') : '',
      },
      {
        label: $localize`References`,
        routerLink: 'references',
        styleClass: isReferenceStepCompleted
          ? this.activeIndex != 1
            ? 'completed'
            : ''
          : isAchievementStepCompleted
          ? ''
          : 'pointer-events-none',
      },
      {
        label: $localize`Questions`,
        routerLink: 'questions',
        styleClass: isQuestionStepCompleted
          ? this.activeIndex != 2
            ? 'completed'
            : ''
          : isReferenceStepCompleted
          ? ''
          : 'pointer-events-none',
      },
      // {
      //   label: $localize`Smart Assessment`,
      //   routerLink: 'smart-assessment',
      //   styleClass: isSmartAssessmentCompleted
      //     ? this.activeIndex != 3
      //       ? 'completed'
      //       : ''
      //     : isQuestionStepCompleted ? '' : 'pointer-events-none',
      // },
      {
        label: $localize`:@@programUploadVideoTitle:Upload Video`,
        routerLink: 'upload-video',
        styleClass: isVideoUploaded
          ? this.activeIndex != 4
            ? 'completed'
            : ''
          : isQuestionStepCompleted
          ? ''
          : 'pointer-events-none',
      },

      {
        label: $localize`Acknowledgment`,
        routerLink: 'acknowledgment',
        styleClass: isAcknowledgmentStepCompleted
          ? ''
          : isVideoUploaded
          ? ''
          : 'pointer-events-none',
      },
    ]
  }

  ngAfterViewChecked() {
    this.cdRef.detectChanges()
  }
}
