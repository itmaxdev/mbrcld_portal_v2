import { ActivatedRoute } from '@angular/router'
import { Component, OnInit } from '@angular/core'
import {
  ListModulesByProgramIdViewModel,
  ListSurveysByProgramIdViewModel,
  ModulesClient,
  ListApplicantCompletedProjectsViewModel,
  ProgramsClient,
  SurveysClient,
  ProjectsClient,
  ListApplicantProjectsViewModel,
} from 'src/app/shared/api.generated.clients'
import { SecureStorage } from 'src/app/core/api-authorization'
import { SectionDataService } from 'src/app/shared/services/section-data.service'

@Component({
  selector: 'app-programs-materials',
  templateUrl: './programs-materials.component.html',
  styleUrls: ['./programs-materials.component.scss'],
})
export class ProgramsMaterialsComponent implements OnInit {
  public surveysData: ListSurveysByProgramIdViewModel[] = []
  id: string
  ready = false
  programName = ''
  completed: number
  isInstructorOrAdmin = false
  modulesData: ListModulesByProgramIdViewModel[]
  moduleProgram: ListApplicantProjectsViewModel[]
  groupedProgram: any[]
  completedProgram: ListApplicantCompletedProjectsViewModel[]
  constructor(
    private ls: SecureStorage,
    private route: ActivatedRoute,
    private modules: ModulesClient,
    private programs: ProgramsClient,
    private section: SectionDataService,
    private projects: ProjectsClient,
    private surveys: SurveysClient
  ) {}

  goBack() {
    this.section.redirectBack(1)
  }

  ngOnInit() {
    this.id = this.route.snapshot.paramMap.get('programId')
    const { role } = JSON.parse(this.ls.getItem('profile_info'))
    this.isInstructorOrAdmin = (role === 4 || role === 6);
    this.getProgramModules()
  }

  getProgramModules() {
    this.ready = false
    this.modules.programModules(this.id).subscribe((data) => {
      this.modulesData = data
      this.ready = true
    })

    if (this.isInstructorOrAdmin) {
      this.programs.userPrograms().subscribe((data) => {
        data.filter((item) => item.id === this.id)
        this.programName = data[0].name
      })
    } else {
      this.programs.inprogressPrograms().subscribe((data) => {
        data.filter((item) => item.id === this.id)
        this.programName = data[0].name
        this.completed = data[0].completed
      })
    }
  }

  getSurveys() {
    this.ready = false
    this.surveys.programSurveys().subscribe((data) => {
      this.surveysData = data
      this.ready = true
    })
  }

  getApplicantInProgressProjects() {
    this.ready = false
    this.projects.applicantProjects().subscribe((data) => {
      this.moduleProgram = data
      this.ready = true
    })
  }

  getApplicantCompletedProjects() {
    this.ready = false
    this.projects.applicantCompletedProjects().subscribe((data) => {
      this.completedProgram = data
      this.ready = true
    })
  }

  getApplicantGroupedProjects() {
    this.ready = false
    this.projects.groupProjects().subscribe((data) => {
      this.groupedProgram = data
      this.ready = true
    })
  }

  handleChange(event) {
    switch (event.index) {
      case 0:
        this.getProgramModules()
        break
      case 1:
        this.getApplicantInProgressProjects()
        break
      case 2:
        this.getSurveys()
        break
    }
  }

  handleChangeProjects(event) {
    switch (event.index) {
      case 0:
        this.getApplicantInProgressProjects()
        break
      case 1:
        this.getApplicantCompletedProjects()
        break
      case 2:
        this.getApplicantGroupedProjects()
        break
    }
  }
}
