import { Component, OnInit, Inject, LOCALE_ID } from '@angular/core'
import {
  ListAllProgramsViewModel,
  ProfileClient,
  ProgramsClient,
  SearchAlumniViewModel,
  ChatClient,
} from 'src/app/shared/api.generated.clients'
import { MessageService } from 'primeng/api'
import { ISectorOption } from '../../profile/professional-experience/models'
import { ProfessionalExperienceFacadeService } from '../../profile/professional-experience/professional-experience-facade.service'
import { SignalrService } from '../../programs/message-chat/signalr.service'

@Component({
  selector: 'app-meeting-list',
  templateUrl: './meeting-list.component.html',
  styleUrls: ['./meeting-list.component.scss'],
  providers: [MessageService],
})
export class MeetingListComponent implements OnInit {
  public isSearched = false
  public noUserFinded = true
  public ready = false
  public allPrograms: ListAllProgramsViewModel[] = []
  /** Dedicated list for Program dropdown only; shaped like sector options (label/value/id). */
  private programOptionsList: any[] = []
  public cohortYears: any[] = []
  public sectorOptions: ISectorOption[] = []
  public selectedProgram: any
  public selectedYear: any
  public selectedSector: ISectorOption
  public findedUsers: SearchAlumniViewModel[]
  public isFindUserDone = false
  public meetingRooms: any[] = []
  public role: number
  public userId: string
  public userProfile: any
  public meetingTabs = '0'
  public viewApplicantDialog = false
  public selectedApplicantId: string

  constructor(
    private profile: ProfileClient,
    private programs: ProgramsClient,
    private facade: ProfessionalExperienceFacadeService,
    public signalRService: SignalrService,
    private chatClient: ChatClient,
    private messageService: MessageService,
    @Inject(LOCALE_ID) public locale: string
  ) {
    this.userProfile = JSON.parse(localStorage.getItem('profile_info'))
  }

  isFollowing(room: any): boolean {
    return !!room?.participants?.find((p: any) => p.userId === this.userId)
  }

  getFollowLabel(room: any): string {
    if (this.locale === 'en') {
      return this.isFollowing(room) ? 'Unfollow' : 'Follow'
    }
    return this.isFollowing(room) ? 'الغاء المتابعة' : 'اتبع'
  }

  followRoom(room: any) {
    this.chatClient.roomPost(this.userId, room.id).subscribe(() => {
      if (!room.participants) room.participants = []
      room.participants.push({ userId: this.userId })
      if (this.locale === 'en') {
        this.messageService.add({
          severity: 'success',
          life: 10000,
          closable: true,
          summary: 'Topic followed',
          detail: 'To start discussing the followed topics, please click on the chat button',
        })
      } else {
        this.messageService.add({
          severity: 'success',
          life: 10000,
          closable: true,
          summary: 'الموضوع يتبع',
          detail: 'يرجى الضغط على زر المحادثة للبدء في مناقشة الموضوعات المتبعة',
        })
      }
    })
  }

  unfollowRoom(room: any) {
    this.chatClient.roomDelete(this.userId, room.id).subscribe(() => {
      if (room.participants) {
        room.participants = room.participants.filter((p: any) => p.userId !== this.userId)
      }
    })
  }

  toggleFollow(room: any) {
    if (this.isFollowing(room)) {
      this.unfollowRoom(room)
    } else {
      this.followRoom(room)
    }
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
        // Keep raw list for any other usage
        this.allPrograms = data || []

        // Mirror sector option shape so dropdown renders label correctly
        this.programOptionsList = (data || []).map((p: any) => {
          const id = p.id ?? p.Id
          let label = p.name ?? p.Name ?? p.programName ?? p.ProgramName ?? p.title ?? p.Title ?? ''

          if (!label || String(label).toLowerCase() === 'empty') {
            label = id ? `Program ${id}` : ''
          }

          return {
            id,
            value: id,
            label,
          }
        })
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

  onProgramChange() {
    this.selectedYear = null
    if (this.selectedProgram?.id) {
      this.getcohortYears(this.selectedProgram.id)
    } else {
      this.cohortYears = []
    }
  }

  getcohortYears(programId: string) {
    this.programs.cohortYears(programId).subscribe((data) => {
      this.cohortYears = []
      data.forEach((item) => {
        this.cohortYears.push({
          label: item.toString(),
          value: item.toString(),
        })
      })
    })
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

  viewApplicant(applicantId: string) {
    this.selectedApplicantId = applicantId
    this.viewApplicantDialog = true
  }

  /** Options for Program dropdown; uses dedicated list so Sector selection cannot overwrite it. */
  get programOptionsForDropdown(): ListAllProgramsViewModel[] {
    return this.programOptionsList
  }

  /** True when at least one of Sector, Program, or Cohort Year is selected; enables Search button. */
  get hasAtLeastOneFilter(): boolean {
    return !!(this.selectedSector || this.selectedProgram || this.selectedYear)
  }
}
