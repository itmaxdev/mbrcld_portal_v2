import { Component, LOCALE_ID, Inject, OnInit } from '@angular/core'
import { ActivatedRoute, Router } from '@angular/router'
import {
  EnrollmentsClient,
  ListAlumniAvailableProgramViewModel,
  ListAlumniGraduatedProgramViewModel,
  ListProgramByCohortContactViewModel,
  ProgramsClient,
} from 'src/app/shared/api.generated.clients'
import { ProfileFacade } from '../../profile/common/profile-facade.service'

@Component({
  selector: 'app-programs-alumni',
  templateUrl: './programs-alumni.component.html',
  styleUrls: ['./programs-alumni.component.scss'],
})
export class ProgramsAlumniComponent implements OnInit {
  public activeTab: 'inprogress' | 'completed' | 'new' = 'inprogress'
  public ready = false
  public alreadyEnrolled = false
  public incompleteProfile = false
  public inprogressProgram: ListProgramByCohortContactViewModel[]
  public suggestedProgram: ListAlumniAvailableProgramViewModel[]
  public completedProgram: any[]
  public role: number
  constructor(
    private programs: ProgramsClient,
    private profile: ProfileFacade,
    private enrollmentsClient: EnrollmentsClient,
    private router: Router,
    private route: ActivatedRoute,
    @Inject(LOCALE_ID) public locale: string
  ) {}

  ngOnInit(): void {
    const userProfile = JSON.parse(localStorage.getItem('profile_info'))
    this.role = userProfile.role
    this.getInProgressPrograms()
  }

  handleChange(event) {
    switch (event.index) {
      case 0:
        this.getCompletedPrograms()
        break
      case 1:
        this.getInProgressPrograms()
        break
      case 2:
        this.getNewPrograms()
        break
      default:
        this.getCompletedPrograms()
    }
  }

  selectTab(tab: 'inprogress' | 'completed' | 'new') {
    this.activeTab = tab

    switch (tab) {
      case 'inprogress':
        this.getInProgressPrograms()
        break
      case 'completed':
        this.getCompletedPrograms()
        break
      case 'new':
        this.getNewPrograms()
        break
    }
  }

  getInProgressPrograms() {
    this.ready = false
    this.programs.inprogressPrograms().subscribe((data) => {
      this.inprogressProgram = data
      this.ready = true
    })
  }

  getCompletedPrograms1() {
    this.ready = false
    this.programs.graduatedPrograms().subscribe((data) => {
      this.completedProgram = data
      this.ready = true
    })
  }

  getCompletedPrograms() {
    this.ready = false
    this.programs.graduatedPrograms().subscribe((data) => {
      this.completedProgram = data.map((item: any) => ({
        id: item.cohortId,
        name: item.programName,
        name_AR: item.programName_AR,
        description: item.programDescription,
        description_AR: item.programDescription_AR,
        completed: 100, // graduated = completed
        endDate: new Date(item.cohortYear, 11, 31), // fake date from year
        pictureUrl: item.pictureUrl,
      }))

      this.ready = true
    })
  }

  getNewPrograms() {
    this.ready = false
    this.programs.suggestedPrograms().subscribe(async (data) => {
      this.suggestedProgram = data
      const profileCompletion = await this.profile.loadFormProgress()
      if (profileCompletion.completionPercentage < 100) {
        this.incompleteProfile = true
        this.ready = true
      } else {
        const [enrollment] = await this.enrollmentsClient.enrollmentsGet().toPromise()
        if (enrollment?.status === 'applied') {
          this.alreadyEnrolled = true
        }
        this.ready = true
      }
    })
  }

  handleNewProgramClick(item: any) {
    if (this.incompleteProfile) {
      // Profile incomplete — navigate to profile page
      const urlSegments = this.router.url.split('/')
      const programsIndex = urlSegments.findIndex((s) => s === 'programs')
      const basePath = programsIndex >= 0 ? urlSegments.slice(0, programsIndex).join('/') : ''
      this.router.navigateByUrl(basePath + '/profile')
    } else {
      this.router.navigate(['apply', item.id], { relativeTo: this.route })
    }
  }
}
