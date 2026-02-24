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
import { LOCALE_ID, Inject } from '@angular/core'

@Component({
  selector: 'app-programs-materials',
  templateUrl: './programs-materials.component.html',
  styleUrls: ['./programs-materials.component.scss'],
})
export class ProgramsMaterialsComponent implements OnInit {
  public surveysData: ListSurveysByProgramIdViewModel[] = []
  activeTab = 0
  activeProjectTab = 0
  id: string
  ready = false
  programName = ''
  completed: number
  isInstructorOrAdmin = false
  modulesData: ListModulesByProgramIdViewModel[]
  moduleProgram: ListApplicantProjectsViewModel[]
  groupedProgram: any[]
  completedProgram: ListApplicantCompletedProjectsViewModel[]
  selectedProject: ListApplicantProjectsViewModel
  activeToolsId: string | null = null
  sending = false
  displayEditModal = false

  constructor(
    private ls: SecureStorage,
    private route: ActivatedRoute,
    private modules: ModulesClient,
    private programs: ProgramsClient,
    private section: SectionDataService,
    private projects: ProjectsClient,
    private surveys: SurveysClient,
    @Inject(LOCALE_ID) public locale: string
  ) {}

  goBack() {
    this.section.redirectBack(1)
  }

  ngOnInit() {
    this.id = this.route.snapshot.paramMap.get('programId')
    const { role } = JSON.parse(this.ls.getItem('profile_info'))
    this.isInstructorOrAdmin = role === 4 || role === 6
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
    this.setTab(event.index)
  }

  setTab(index: number) {
    this.activeTab = index
    switch (index) {
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
    this.setProjectTab(event.index)
  }

  setProjectTab(index: number) {
    this.activeProjectTab = index
    switch (index) {
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

  getConvertedDuration(minutes: any) {
    return this.section.convertMinuteToHours(minutes?.toString())
  }

  getProjectStatusData(status: number) {
    const statuses = [
      { name: 'Draft', ar: 'مسودة', class: 'colorDanger', progress: 0 },
      { name: 'Submitted for Approval', ar: 'تم تقديم الطلب', class: 'colorWarning', progress: 10 },
      { name: 'In Progress', ar: 'قيد التنفيذ', class: 'colorWarning', progress: 50 },
      { name: 'Under Review', ar: 'قيد المراجعة', class: 'colorWarning', progress: 75 },
      { name: 'Completed', ar: 'مكتمل', class: 'colorSuccess', progress: 100 },
      { name: 'Grouped', ar: 'مجمعة', class: 'colorInfo', progress: 100 },
      { name: 'Ready for Presentation', ar: 'جاهز للعرض', class: 'colorSuccess', progress: 100 },
      { name: 'Accepted', ar: 'مقبول', class: 'colorSuccess', progress: 100 },
      { name: 'Rejected', ar: 'مرفوض', class: 'colorDanger', progress: 0 },
      { name: 'UnderReview', ar: 'قيد المراجعة', class: 'colorWarning', progress: 75 },
    ]
    const data = statuses[status - 1] || { name: 'N/A', ar: 'غير متوفر', class: '', progress: 0 }
    return {
      statusName: this.locale === 'ar' ? data.ar : data.name,
      colorClass: data.class,
      progress: data.progress,
    }
  }

  toggleTools(id: string, event: MouseEvent) {
    event.stopPropagation()
    this.activeToolsId = this.activeToolsId === id ? null : id
  }

  openEditModal(item: ListApplicantProjectsViewModel) {
    this.selectedProject = JSON.parse(JSON.stringify(item)) // Clone to avoid direct mutation
    this.activeToolsId = null
    this.displayEditModal = true
  }

  deleteProject(item: ListApplicantProjectsViewModel) {
    if (
      confirm(
        this.locale === 'ar'
          ? 'هل أنت متأكد أنك تريد حذف هذا المشروع؟'
          : 'Are you sure you want to delete this project?'
      )
    ) {
      // Since no delete API is found in ProjectsClient, we simulate local removal for now.
      this.moduleProgram = this.moduleProgram.filter((p) => p.id !== item.id)
      console.log('Delete project', item.id)
    }
  }

  saveProject(status: number) {
    this.sending = true
    this.projects
      .applicantProject(
        this.selectedProject.id,
        this.selectedProject.topic,
        this.selectedProject.topic, // topic_ar
        this.selectedProject.description,
        this.selectedProject.description, // description_ar
        this.selectedProject.topicId,
        status,
        null
      )
      .subscribe(() => {
        this.sending = false
        this.getApplicantInProgressProjects()
        this.displayEditModal = false
        this.selectedProject = null
      })
  }
}
