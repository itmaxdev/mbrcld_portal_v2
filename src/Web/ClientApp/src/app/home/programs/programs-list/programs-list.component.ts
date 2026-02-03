import { Component, OnInit } from '@angular/core'
import {
  EnrollmentsClient,
  ListActiveProgramsViewModel,
  ProgramsClient,
} from 'src/app/shared/api.generated.clients'
import { ProfileFacade } from '../../profile/common/profile-facade.service'

@Component({
  selector: 'app-programs-list',
  templateUrl: './programs-list.component.html',
  styleUrls: ['./programs-list.component.scss'],
})
export class ProgramsListComponent implements OnInit {
  alreadyEnrolled = false
  incompleteProfile = false
  isActiveProgramID: string = undefined
  ready = false
  public allActivePrograms: ListActiveProgramsViewModel[] = []

  constructor(
    private enrollmentsClient: EnrollmentsClient,
    private profile: ProfileFacade,
    private programs: ProgramsClient
  ) {}

  async ngOnInit() {
    this.allActivePrograms = await this.programs.allActivePrograms().toPromise()
    this.programs.activeProgram().subscribe((data) => {
      if (data) {
        this.isActiveProgramID = data.id
      }
    })
    const profileCompletion = await this.profile.loadFormProgress()
    if (profileCompletion.completionPercentage < 100) {
      this.incompleteProfile = true
    } else {
      const [enrollment] = await this.enrollmentsClient.enrollmentsGet().toPromise()
      if (enrollment?.status === 'applied') {
        this.alreadyEnrolled = true
      }
    }

    this.ready = true
  }

  get filterActivePrograms() {
    return this.allActivePrograms.filter((x) => x.openForRegistration === true)
  }

  get filterOtherPrograms() {
    return this.allActivePrograms.filter((x) => x.openForRegistration === false)
  }
}
