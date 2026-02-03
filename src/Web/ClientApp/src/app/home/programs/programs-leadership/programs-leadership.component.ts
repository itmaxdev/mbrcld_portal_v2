import * as moment from 'moment'
import { Component, OnInit } from '@angular/core'
import {
  ListApplicantCompletedProjectsViewModel,
  ListApplicantProjectsViewModel,
  ListSurveysByProgramIdViewModel,
  ListProgramByCohortContactViewModel,
  ProgramsClient,
  ProjectsClient,
} from 'src/app/shared/api.generated.clients'
import { SecureStorage } from 'src/app/core/api-authorization'
import { MenuItem } from 'primeng/api'
import { SurveysClient } from 'src/app/shared/api.generated.clients'

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
  userRole: number
  profileInfo: any
  items: MenuItem[]
  moment: any = moment
  ready = false
  selectedTabs: ISelectedTabsApplicant
  isReadyCompletedProjects = false
  isReadyInProgressProjects = false
  isReadyGroupedProjects = false
  surveyReady = false
  groupedProgram: any[]
  moduleProgram: ListApplicantProjectsViewModel[]
  inprogressProgram: ListProgramByCohortContactViewModel[]
  completedProgram: ListApplicantCompletedProjectsViewModel[]
  surveysData: ListSurveysByProgramIdViewModel[]

  constructor(
    private ls: SecureStorage,
    private programs: ProgramsClient,
    private projects: ProjectsClient,
    private surveys: SurveysClient
  ) {}

  handleChange(event) {
    switch (event.index) {
      case 0:
        this.selectedTabs = {
          programs: false,
          inProgress: true,
          completed: false,
          grouped: false,
        }
        this.getApplicantInProgressProjects()
        break
      case 1:
        this.selectedTabs = {
          programs: false,
          inProgress: false,
          completed: true,
          grouped: false,
        }
        this.getApplicantCompletedProjects()
        break
      case 2:
        this.selectedTabs = {
          programs: false,
          inProgress: false,
          completed: false,
          grouped: true,
        }
        this.getApplicantGroupedProjects()
        break
      case 3:
        this.surveys.programSurveys().subscribe((data) => {
          this.surveysData = data
          this.surveyReady = true
        })
        break
      default:
        this.selectedTabs = {
          programs: true,
          inProgress: false,
          completed: false,
          grouped: false,
        }
        this.getPrograms()
    }
    localStorage.setItem('selected_tab_applicant', JSON.stringify(this.selectedTabs))
  }

  getPrograms() {
    this.ready = false
    this.programs.inprogressPrograms().subscribe((data) => {
      this.inprogressProgram = data
      this.profileInfo = JSON.parse(this.ls.getItem('profile_info'))
      this.userRole = this.profileInfo.role
      this.ready = true
    })
  }

  getApplicantInProgressProjects() {
    this.isReadyInProgressProjects = false
    this.projects.applicantProjects().subscribe((data) => {
      this.moduleProgram = data
      this.isReadyInProgressProjects = true
    })
  }

  getApplicantGroupedProjects() {
    this.isReadyGroupedProjects = false
    this.projects.groupProjects().subscribe((data) => {
      this.groupedProgram = data
      this.isReadyGroupedProjects = true
    })
  }

  getApplicantCompletedProjects() {
    this.isReadyCompletedProjects = false
    this.projects.applicantCompletedProjects().subscribe((data) => {
      this.completedProgram = data
      this.isReadyCompletedProjects = true
    })
  }

  async ngOnInit() {
    this.ready = false
    this.getPrograms()
    // const localSelectedTab = localStorage.getItem('selected_tab_applicant')
    // if (localSelectedTab) {
    //   this.selectedTabs = JSON.parse(localSelectedTab)
    //   if (this.selectedTabs.programs) {
    //     this.getPrograms()
    //   } else if (this.selectedTabs.inProgress) {
    //     this.getApplicantInProgressProjects()
    //   } else if (this.selectedTabs.completed) {
    //     this.getApplicantCompletedProjects()
    //   } else if (this.selectedTabs.grouped) {
    //     this.getApplicantGroupedProjects()
    //   }
    // } else {
    //   this.selectedTabs = {
    //     programs: true,
    //     inProgress: false,
    //     completed: false,
    //     grouped: false,
    //   }
    //   localStorage.setItem('selected_tab_applicant', JSON.stringify(this.selectedTabs))
    //   this.getPrograms()
    // }

    // Promise.all([
    //   this.programs.inprogressPrograms().subscribe((data) => {
    //     this.inprogressProgram = data
    //     this.profileInfo = JSON.parse(this.ls.getItem('profile_info'))
    //     this.userRole = this.profileInfo.role
    //     this.ready = true
    //   }),
    //   this.projects.applicantProjects().subscribe((data) => {
    //     this.moduleProgram = data
    //   }),
    //   this.projects.applicantCompletedProjects().subscribe((data) => {
    //     this.completedProgram = data
    //   }),
    // ])
  }
}
