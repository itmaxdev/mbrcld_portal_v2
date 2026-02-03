import { tap } from 'rxjs/operators'
import { ActivatedRoute } from '@angular/router'
import { Component, OnInit } from '@angular/core'
import { inputLanguage } from 'src/app/shared/validators'
import { FormControl, FormGroup, Validators } from '@angular/forms'
import {
  CalendarClient,
  ChatClient,
  GetInstructorProfileViewModel,
  GetUniversityProfileViewModel,
  IListTeamMembersViewModel,
  ListMaterialsByModuleIdViewModel,
  ListUserNewsFeedsViewModel,
  MaterialsClient,
  ModulesClient,
  NewsFeedsClient,
  ProgramsClient,
  UniversityTeamMembersClient,
} from 'src/app/shared/api.generated.clients'
import { SecureStorage } from 'src/app/core/api-authorization'
import { SectionDataService } from 'src/app/shared/services/section-data.service'
import { DomSanitizer } from '@angular/platform-browser'

interface SelectedTabsApplicantModule {
  materials: boolean
  overview: boolean
  instructor: boolean
  team: boolean
  live: boolean
  class: boolean
  newsfeed: boolean
}

@Component({
  selector: 'app-programs-modules-applicant',
  templateUrl: './programs-modules-applicant.component.html',
  styleUrls: ['./programs-modules-applicant.component.scss'],
})
export class ProgramsModulesApplicantComponent implements OnInit {
  id: string
  date: Date
  topic: string
  order: number
  ready = false
  programId = ''
  moduleName = ''
  programName = ''
  userRole: number
  onUpdate = false
  profileInfo: any
  topic_ar: string
  location: string
  duration: number
  roomId: string
  moduleId: string
  moduleOrder: number
  modalHeader: string
  // role: number
  overviewText: any
  displayBasic: boolean
  materialForm: FormGroup
  isExistMaterials = true
  isModuleNameReady = false
  instructorId: string
  isProgramNameReady = false
  isInstructorProfileReady = false
  viewMemberDialog = false
  viewMemberReady = false
  viewMemberData: any
  isInstructor = false
  materialId: string = undefined
  isClassListReady = false
  isTeamReady = false
  startDate: string
  isOverviewExist = true
  universityData: GetUniversityProfileViewModel
  userId: string
  liveClassesData: any[]
  classListData: any[]
  isCalendarReady = false
  members: IListTeamMembersViewModel[]
  instructorProfileInfo: GetInstructorProfileViewModel
  selectedTabsApplicantModule: SelectedTabsApplicantModule
  materialsData: ListMaterialsByModuleIdViewModel[]
  generatedURL = ''
  type: string
  role: string
  newsFeedList: ListUserNewsFeedsViewModel[]
  isnewsFeedListReady = false
  isExistSections = false
  // duration: number

  constructor(
    private ls: SecureStorage,
    private route: ActivatedRoute,
    private modules: ModulesClient,
    private chatClient: ChatClient,
    private programs: ProgramsClient,
    private calendar: CalendarClient,
    protected sanitizer: DomSanitizer,
    private materials: MaterialsClient,
    private section: SectionDataService,
    public materialsClient: MaterialsClient,
    private universityTeamMembers: UniversityTeamMembersClient,
    private newsFeedsClient: NewsFeedsClient
  ) {}

  goBack() {
    this.section.redirectBack(2)
  }

  handleChange(event) {
    switch (event.index) {
      case 1:
        this.selectedTabsApplicantModule = {
          newsfeed: false,
          overview: true,
          materials: false,
          instructor: false,
          team: false,
          live: false,
          class: false,
        }
        break
      case 2:
        this.selectedTabsApplicantModule = {
          newsfeed: false,
          overview: false,
          materials: true,
          instructor: false,
          team: false,
          live: false,
          class: false,
        }
        break
      case 3:
        this.selectedTabsApplicantModule = {
          newsfeed: false,
          overview: false,
          materials: false,
          instructor: true,
          team: false,
          live: false,
          class: false,
        }
        break
      case 4:
        this.selectedTabsApplicantModule = {
          newsfeed: false,
          overview: false,
          materials: false,
          instructor: false,
          team: true,
          live: false,
          class: false,
        }
        break
      case 5:
        this.selectedTabsApplicantModule = {
          newsfeed: false,
          overview: false,
          materials: false,
          instructor: false,
          team: false,
          live: true,
          class: false,
        }
        break
      case 6:
        this.selectedTabsApplicantModule = {
          newsfeed: false,
          overview: false,
          materials: false,
          instructor: false,
          team: false,
          live: false,
          class: true,
        }
        break
      default:
        this.selectedTabsApplicantModule = {
          newsfeed: true,
          overview: false,
          materials: false,
          instructor: false,
          team: false,
          live: false,
          class: false,
        }
    }
    localStorage.setItem(
      'selected_tab_applicant_module',
      JSON.stringify(this.selectedTabsApplicantModule)
    )
  }

  async openDialog(idMaterial) {
    await Promise.all([
      this.materials.materialsGet(idMaterial).subscribe((data) => {
        this.topic = data.name
        this.topic_ar = data.name_AR
        this.location = data.location
        this.duration = data.duration
        this.order = data.order
        this.showBasicDialog(idMaterial)
      }),
    ])
  }

  showBasicDialog(materialId: string = undefined) {
    this.materialId = materialId
    this.modalHeader = 'Edit Material'
    if (!this.materialId) {
      this.topic = ''
      this.topic_ar = ''
      this.location = ''
      this.duration = null
      this.order = null
      this.modalHeader = 'Add New Material'
    }
    this.displayBasic = true
  }

  viewMember(event: string) {
    this.viewMemberReady = false
    this.viewMemberDialog = true
    this.universityTeamMembers.universityTeamMemberGet(event).subscribe((data) => {
      this.viewMemberData = data
      this.viewMemberReady = true
    })
  }

  addMaterial() {
    this.moduleId = this.route.snapshot.paramMap.get('modulesId')

    if (this.materialForm.valid) {
      const allData = this.materialForm.value
      this.onUpdate = true
      return this.materialsClient
        .materialsPost(
          this.materialId,
          this.moduleId,
          allData.name,
          allData.nameAr,
          allData.location,
          allData.duration,
          allData.order,
          allData.startDate
        )
        .pipe(
          tap(() => {
            this.onUpdate = false
            window.location.reload()
          })
        )
        .toPromise()
    }
  }

  async ngOnInit() {
    const currentUrl = this.section.redirectBack(1, true)
    this.generatedURL = `${currentUrl}/${this.id}`
    this.ready = false
    this.isCalendarReady = false
    this.isProgramNameReady = false
    this.isModuleNameReady = false
    this.isClassListReady = false
    this.isInstructorProfileReady = false
    this.id = this.route.snapshot.paramMap.get('modulesId')
    this.programId = this.route.snapshot.paramMap.get('programId')

    const profileInfo = JSON.parse(localStorage.getItem('profile_info'))
    this.role = profileInfo.role
    this.userId = profileInfo.id

    this.chatClient.chat(this.id).subscribe((data) => {
      const chatData = JSON.parse(data)
      if (chatData.length > 0) {
        this.roomId = chatData[0].id
      }
    })

    const localSelectedTab = localStorage.getItem('selected_tab_applicant_module')
    if (localSelectedTab) {
      this.selectedTabsApplicantModule = JSON.parse(localSelectedTab)
    } else {
      this.selectedTabsApplicantModule = {
        newsfeed: false,
        overview: false,
        materials: true,
        instructor: false,
        team: false,
        live: false,
        class: false,
      }
      localStorage.setItem(
        'selected_tab_applicant_module',
        JSON.stringify(this.selectedTabsApplicantModule)
      )
    }

    if (this.role == '2') {
      this.newsFeedsClient.moduleNewsfeeds(this.id).subscribe((data) => {
        this.newsFeedList = data as ListUserNewsFeedsViewModel[]
        this.isnewsFeedListReady = true
        if (this.newsFeedList.length == 0) {
          this.isExistSections = true
        }
      })
    }

    this.materialForm = new FormGroup({
      name: new FormControl(null, [Validators.required]),
      order: new FormControl(null, [Validators.required]),
      nameAr: new FormControl(null, [Validators.required, inputLanguage('ar')]),
      location: new FormControl(null, [Validators.required]),
      startDate: new FormControl(null, [Validators.required]),
      duration: new FormControl(null, [Validators.required]),
      publishDate: new FormControl(''),
      status: new FormControl(null, [Validators.required]),
    })

    this.profileInfo = JSON.parse(this.ls.getItem('profile_info'))
    this.userRole = this.profileInfo.role
    // this.isInstructor = this.profileInfo.role === 4
    this.programId = this.route.snapshot.paramMap.get('programId')
    this.role = JSON.parse(localStorage.getItem('profile_info')).role

    this.materials.moduleMaterials(this.id).subscribe((data) => {
      if (data.length > 0) {
        this.materialsData = data
      } else {
        this.isExistMaterials = false
      }
      this.ready = true
    })
    if (this.programId) {
      this.programs.programs(this.programId).subscribe((data) => {
        this.isProgramNameReady = true
        this.programName = data.name
      })
    }
    this.modules.modules(this.id).subscribe((data) => {
      this.instructorId = data.instructorId
      this.isModuleNameReady = true
      this.moduleName = data.name
      this.moduleOrder = data.order
      if (!data.overview || data.overview == '') {
        this.isOverviewExist = false
      } else {
        this.overviewText = data.overview
        this.overviewText = this.sanitizer.bypassSecurityTrustHtml(this.overviewText)
      }
    })

    this.modules.universityProfile(this.id).subscribe((data) => {
      this.universityData = data
      this.isInstructorProfileReady = true
    })

    this.universityTeamMembers.applicant(this.id).subscribe((data) => {
      this.members = data
      this.isTeamReady = true
    })

    this.calendar.meetings(undefined).subscribe((data) => {
      this.liveClassesData = data
      this.isCalendarReady = true
    })

    this.modules.moduleApplicants(this.id).subscribe((data) => {
      this.classListData = data
      this.isClassListReady = true
    })
  }
}
