import * as moment from 'moment'
import { Component, LOCALE_ID, Inject, OnInit } from '@angular/core'
import {
  ListApplicantCompletedProjectsViewModel,
  ListApplicantProjectsViewModel,
  ListSurveysByProgramIdViewModel,
  ListProgramByCohortContactViewModel,
  ProgramsClient,
  ProjectsClient,
  SurveysClient,
  EnrollmentsClient,
} from 'src/app/shared/api.generated.clients'
import { SecureStorage } from 'src/app/core/api-authorization'
import { MenuItem } from 'primeng/api'
import { ActivatedRoute, Router } from '@angular/router'
import { ProfileFacade } from '../../profile/common/profile-facade.service'

interface ISelectedTabsApplicant {
  programs: boolean
  inProgress: boolean
  completed: boolean
  grouped: boolean
}

@Component({
  selector: 'app-programs-leadership',
  templateUrl: './programs-leadership.component.html',
  styleUrls: ['./programs-leadership.component.scss'],
})
export class ProgramsLeadershipComponent implements OnInit {
  public ready = false
  public role: number
  public inprogressProgram: ListProgramByCohortContactViewModel[]
  public profileInfo: any
  public userRole: number

  constructor(
    private ls: SecureStorage,
    private programs: ProgramsClient,
    private projects: ProjectsClient,
    private surveys: SurveysClient,
    private profile: ProfileFacade,
    private enrollmentsClient: EnrollmentsClient,
    private router: Router,
    private route: ActivatedRoute,
    @Inject(LOCALE_ID) public locale: string
  ) {}

  getInProgressPrograms() {
    this.ready = false
    this.programs.inprogressPrograms().subscribe((data) => {
      this.inprogressProgram = data
      this.ready = true
    })
  }

  async ngOnInit() {
    const userProfile = JSON.parse(this.ls.getItem('profile_info'))
    if (userProfile) {
      this.role = userProfile.role
      this.userRole = userProfile.role
    }
    this.getInProgressPrograms()
  }
}
