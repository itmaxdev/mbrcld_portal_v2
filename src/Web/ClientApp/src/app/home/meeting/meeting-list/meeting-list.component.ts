import { Component, OnInit } from '@angular/core'
import {
  ListAllProgramsViewModel,
  ProfileClient,
  ProgramsClient,
  SearchAlumniViewModel,
  ChatClient,
} from 'src/app/shared/api.generated.clients'
import { ISectorOption } from '../../profile/professional-experience/models'
import { ProfessionalExperienceFacadeService } from '../../profile/professional-experience/professional-experience-facade.service'
import { SignalrService } from '../../programs/message-chat/signalr.service'

@Component({
  selector: 'app-meeting-list',
  templateUrl: './meeting-list.component.html',
  styleUrls: ['./meeting-list.component.scss'],
})
export class MeetingListComponent implements OnInit {
  public isSearched = false
  public noUserFinded = true
  public ready = false
  public allPrograms: ListAllProgramsViewModel[]
  public cohortYears: any[]
  public sectorOptions: ISectorOption[]
  public selectedProgram: ListAllProgramsViewModel
  public selectedYear: any
  public selectedSector: ISectorOption
  public findedUsers: SearchAlumniViewModel[]
  public isFindUserDone = false
  public meetingRooms: any[]
  public role: number
  public userId: string
  public userProfile: any
  public meetingTabs: string

  constructor(
    private profile: ProfileClient,
    private programs: ProgramsClient,
    private facade: ProfessionalExperienceFacadeService,
    public signalRService: SignalrService,
    private chatClient: ChatClient
  ) {
    this.userProfile = JSON.parse(localStorage.getItem('profile_info'))
  }

  setUserId() {
    this.userId = this.signalRService.userId = this.userProfile.id
  }

  setUserRole() {
    this.role = this.userProfile.role
  }

  async ngOnInit() {
    this.setUserId()
    this.setUserRole()
    this.ready = false
    await Promise.all([
      this.programs.allPrograms().subscribe((data) => {
        this.allPrograms = data
      }),
      this.facade.loadSectorOptions().then((options) => {
        this.sectorOptions = options
      }),
      this.chatClient.adminRooms().subscribe((data) => {
        this.meetingRooms = data
      }),
      
    ]).then(() => {
      this.ready = true
    })
  }
  getcohortYears(programId){
    this.programs.cohortYears(programId.value.id).subscribe((data) => {
      this.cohortYears = []
      data.map((item) => {
        const cohortObj = {
          label: item.toString(),
          value: item.toString(),
        }
        this.cohortYears.push(cohortObj)
      })
    });
  }
  handleChange(event) {
    switch (event.index) {
      case 0:
        this.meetingTabs = '0'
        break
      case 1:
        this.meetingTabs = '1'
        break
    }
  }

  searchMeeting() {
    this.isSearched = true
    this.isFindUserDone = false
    this.profile
      .searchAlumniCriteria(
        this.selectedProgram ? this.selectedProgram.id : undefined,
        this.selectedSector ? this.selectedSector.value : undefined,
        this.selectedYear ? this.selectedYear.value : undefined
      )
      .subscribe((data) => {
        this.findedUsers = data
        this.isFindUserDone = true
        this.noUserFinded = this.findedUsers.length === 0 ? true : false
      })
  }
}
