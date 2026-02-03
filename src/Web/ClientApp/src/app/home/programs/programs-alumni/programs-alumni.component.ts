import { Component, LOCALE_ID, Inject, OnInit } from '@angular/core'
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
  public ready = false
  public alreadyEnrolled = false
  public incompleteProfile = false
  public inprogressProgram: ListProgramByCohortContactViewModel[]
  public suggestedProgram: ListAlumniAvailableProgramViewModel[]
  public completedProgram: ListAlumniGraduatedProgramViewModel[]
  public role: number
  constructor(
    private programs: ProgramsClient,
    private profile: ProfileFacade,
    private enrollmentsClient: EnrollmentsClient,
    @Inject(LOCALE_ID) public locale: string
  ) {}

  ngOnInit(): void {
    const userProfile = JSON.parse(localStorage.getItem('profile_info'))
    this.role = userProfile.role
    this.getCompletedPrograms()
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

  getInProgressPrograms() {
    this.ready = false
    this.programs.inprogressPrograms().subscribe((data) => {
      this.inprogressProgram = data
      this.ready = true
    })
  }

  getCompletedPrograms() {
    this.ready = false
    this.programs.graduatedPrograms().subscribe((data) => {
      this.completedProgram = data
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
}
