import { tap } from 'rxjs/operators'
import { ActivatedRoute } from '@angular/router'
import { Component, ElementRef, OnInit, ViewChild, LOCALE_ID, Inject } from '@angular/core'
import { FormControl, FormGroup, Validators } from '@angular/forms'
import {
  ChatClient,
  GetInstructorProjectViewModel,
  ListInstructorProjectsViewModel,
  ListMaterialsByModuleIdViewModel,
  MaterialsClient,
  MaterialStatuses,
  ModulesClient,
  ProgramsClient,
  ProjectsClient,
  NewsFeedsClient,
  ListUserNewsFeedsViewModel,
} from 'src/app/shared/api.generated.clients'
import { SecureStorage } from 'src/app/core/api-authorization'
import { SectionDataService } from 'src/app/shared/services/section-data.service'
import { MessageService } from 'primeng/api'
import * as moment from 'moment'

interface ISelectedTabs {
  materials: boolean
  accelerated: boolean
  submitted: boolean
  overview: boolean
  class: boolean
  newsFeed: boolean
}

@Component({
  selector: 'app-programs-modules-instructor',
  templateUrl: './programs-modules-instructor.component.html',
  styleUrls: ['./programs-modules-instructor.component.scss'],
  providers: [MessageService],
})
export class ProgramsModulesInstructorComponent implements OnInit {
  id: string
  date: Date
  topic: string
  order: number
  overviewText: string
  ready = false
  userId: string
  programId = ''
  generatedURL = ''
  moduleName = ''
  programName = ''
  userRole: number
  onUpdate = false
  profileInfo: any
  topic_ar: string
  location: string
  duration: number
  moduleId: string
  moduleOrder: number
  modalHeader: string
  roomName = ''
  roomId: string
  startDate: Date
  publishDate: Date
  status: MaterialStatuses
  displayBasic: boolean
  isApplicantsReady = false
  applicantsDialog = false
  topicAccelerated: string
  descriptionAccelerated: string
  materialForm: FormGroup
  acceleratedProjectForm: FormGroup
  overviewForm: FormGroup
  isExistMaterials = true
  isExistTemplate = true
  isExistAccelerated = true
  acceleratedProjectId: string
  isModuleNameReady = false
  isProgramNameReady = false
  isInstructor = false
  materialId: string = undefined
  topicAcceleratedAr: string
  classListData: any[]
  descriptionAcceleratedAr: string
  isClassListReady = false
  isOpenedAcceleratedModal = false
  isScheudledSelected = false
  projectHeader: string
  applicantsList: any[] = []
  templateData: GetInstructorProjectViewModel[]
  materialsData: ListMaterialsByModuleIdViewModel[]
  underreviewData: ListInstructorProjectsViewModel[]
  selectedTabs: ISelectedTabs
  newsFeedList: ListUserNewsFeedsViewModel[]

  isnewsFeedListReady = false
  role: number
  isExistSections = false
  @ViewChild('openChat') openChat: ElementRef<HTMLElement>

  statusOptions = [
    {
      value: MaterialStatuses.Draft,
      label: this.locale === 'en' ? 'Draft' : 'مسودة',
    },
    {
      value: MaterialStatuses.Scheduled,
      label: this.locale === 'en' ? 'Scheduled' : 'مقرر للنشر',
    },
    {
      value: MaterialStatuses.Published,
      label: this.locale === 'en' ? 'Published' : 'نشر',
    },
  ]

  constructor(
    private ls: SecureStorage,
    private route: ActivatedRoute,
    private modules: ModulesClient,
    private chatClient: ChatClient,
    private projects: ProjectsClient,
    private programs: ProgramsClient,
    private materials: MaterialsClient,
    private section: SectionDataService,
    private messageService: MessageService,
    public materialsClient: MaterialsClient,
    private newsFeedsClient: NewsFeedsClient,
    @Inject(LOCALE_ID) public locale: string
  ) {}

  onSelectType(event) {
    const publishDateInput = this.materialForm.get('publishDate')
    if (event == MaterialStatuses.Scheduled) {
      this.isScheudledSelected = true
      publishDateInput.setValidators(Validators.required)
    } else {
      this.isScheudledSelected = false
      publishDateInput.clearValidators()
    }
    publishDateInput.updateValueAndValidity()
  }

  goBack() {
    this.section.redirectBack(2)
  }

  openDialog(idMaterial) {
    this.materials.materialsGet(idMaterial).subscribe((data) => {
      this.topic = data.name
      this.topic_ar = data.name_AR
      this.location = data.location
      this.duration = data.duration
      this.order = data.order
      this.startDate = moment(data.startDate).toDate()
      this.publishDate = data.publishDate ? moment(data.publishDate).toDate() : ('' as any)
      this.status = data.status
      this.showBasicDialog(idMaterial)
    })
  }

  showBasicDialog(materialId: string = undefined) {
    this.materialId = materialId
    this.modalHeader = 'Edit Material'
    if (!this.materialId) {
      this.materialForm.reset()
      if (this.locale === 'ar') this.modalHeader = 'أضف مادة'
      else this.modalHeader = 'Add New Material'
    }
    this.displayBasic = true
  }

  openAccelerated(event) {
    if (event) {
      this.projectHeader = 'Edit Accelerated Project'
      this.acceleratedProjectId = event
      this.projects.instructorTemplateProject(this.id).subscribe((data) => {
        const editProject = data.filter((item) => item.id == event)
        this.topicAccelerated = editProject[0].topic
        this.descriptionAccelerated = editProject[0].description
        this.topicAcceleratedAr = editProject[0].topic_Ar
        this.descriptionAcceleratedAr = editProject[0].description_Ar
        this.isOpenedAcceleratedModal = true
      })
    } else {
      this.projectHeader = 'Add Accelerated Project'
      this.emptyTheAcceleratorFields()
      this.isOpenedAcceleratedModal = true
    }
  }

  addAcceleratedProject() {
    if (this.acceleratedProjectForm.valid) {
      this.onUpdate = true
      if (this.acceleratedProjectId) {
        const body: any = {
          id: this.acceleratedProjectId,
          name: this.topicAccelerated,
          name_Ar: this.topicAcceleratedAr,
          description: this.descriptionAccelerated,
          description_AR: this.descriptionAcceleratedAr,
        }
        return this.projects
          .instructorProjectPut(body)
          .pipe(
            tap(() => {
              this.projects.instructorTemplateProject(this.id).subscribe((data) => {
                this.templateData = data
                this.onUpdate = false
                this.isOpenedAcceleratedModal = false
              })
            })
          )
          .toPromise()
      } else {
        const profileInfo = JSON.parse(localStorage.getItem('profile_info'))
        const instructorId = profileInfo.id
        const body: any = {
          name: this.topicAccelerated,
          name_Ar: this.topicAcceleratedAr,
          description: this.descriptionAccelerated,
          description_AR: this.descriptionAcceleratedAr,
          instructorId,
          moduleId: this.id,
        }
        return this.projects
          .instructorProjectPost(body)
          .pipe(
            tap(() => {
              this.projects.instructorTemplateProject(this.id).subscribe((data) => {
                this.templateData = data
                if (this.templateData.length == 0) {
                  this.isExistAccelerated = false
                } else {
                  this.isExistAccelerated = true
                }
                this.onUpdate = false
                this.emptyTheAcceleratorFields()
                this.isOpenedAcceleratedModal = false
              })
            })
          )
          .toPromise()
      }
    }
  }

  saveOverview() {
    if (this.overviewForm.valid) {
      const body: any = {
        id: this.id,
        overview: this.overviewText,
      }

      this.modules
        .moduleOverview(body)
        .pipe(
          tap(() => {
            this.showSuccess()
          })
        )
        .toPromise()
    }
  }

  showSuccess() {
    this.messageService.add({
      key: 'tr',
      severity: 'success',
      summary: 'Success',
      detail: 'The Overview was saved!',
    })
  }

  emptyTheAcceleratorFields() {
    this.acceleratedProjectId = undefined
    this.topicAccelerated = ''
    this.descriptionAccelerated = ''
    this.topicAcceleratedAr = ''
    this.descriptionAcceleratedAr = ''
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
          allData.startDate,
          allData.publishDate,
          allData.status
        )
        .pipe(
          tap(() => {
            this.materials.moduleMaterials(this.id).subscribe((data) => {
              this.materialsData = data
              this.isExistMaterials = true
              this.onUpdate = false
              this.displayBasic = false
            })
          })
        )
        .toPromise()
    }
  }

  cols = [
    { field: 'profilePictureUrl', header: 'Avatar' },
    { field: 'name', header: 'Name' },
  ]

  handleChange(event) {
    switch (event.index) {
      case 0:
        this.selectedTabs = {
          newsFeed: true,
          overview: false,
          materials: false,
          accelerated: false,
          submitted: false,
          class: false,
        }
        break
      case 1:
        this.selectedTabs = {
          newsFeed: false,
          overview: true,
          materials: false,
          accelerated: false,
          submitted: false,
          class: false,
        }
        break
      case 2:
        this.selectedTabs = {
          newsFeed: false,
          overview: false,
          materials: true,
          accelerated: false,
          submitted: false,
          class: false,
        }
        break
      case 3:
        this.selectedTabs = {
          newsFeed: false,
          overview: false,
          materials: false,
          accelerated: true,
          submitted: false,
          class: false,
        }
        break
      case 4:
        this.selectedTabs = {
          newsFeed: false,
          overview: false,
          materials: false,
          accelerated: false,
          submitted: true,
          class: false,
        }
        break
      case 5:
        this.selectedTabs = {
          newsFeed: false,
          overview: false,
          materials: false,
          accelerated: false,
          submitted: false,
          class: true,
        }
        break
      default:
        this.selectedTabs = {
          newsFeed: true,
          overview: false,
          materials: false,
          accelerated: false,
          submitted: false,
          class: false,
        }
    }
    localStorage.setItem('selected_tab', JSON.stringify(this.selectedTabs))
  }

  async ngOnInit() {
    this.moduleId = this.route.snapshot.paramMap.get('modulesId')
    this.id = this.route.snapshot.paramMap.get('modulesId')
    const currentUrl = this.section.redirectBack(1, true)
    this.generatedURL = `${currentUrl}/${this.id}`
    this.ready = false
    this.isApplicantsReady = false
    this.isClassListReady = false
    this.isProgramNameReady = false
    this.isModuleNameReady = false
    this.id = this.route.snapshot.paramMap.get('modulesId')
    this.programId = this.route.snapshot.paramMap.get('programId')
    this.role = JSON.parse(localStorage.getItem('profile_info')).role

    this.chatClient.chat(this.id).subscribe((data) => {
      const chatData = JSON.parse(data)
      if (chatData.length > 0) {
        this.roomId = chatData[0].id
      }
    })

    this.overviewForm = new FormGroup({
      overviewText: new FormControl(null, [Validators.required]),
    })

    this.acceleratedProjectForm = new FormGroup({
      topicAccelerated: new FormControl(null, [Validators.required]),
      topicAcceleratedAr: new FormControl('', [Validators.nullValidator]),
      descriptionAccelerated: new FormControl(null, [Validators.required]),
      descriptionAcceleratedAr: new FormControl('', [Validators.nullValidator]),
    })

    this.materialForm = new FormGroup({
      name: new FormControl(null, [Validators.required]),
      order: new FormControl(null, [Validators.required]),
      nameAr: new FormControl('', [Validators.nullValidator]),
      location: new FormControl(null, [Validators.required]),
      startDate: new FormControl(null, [Validators.required]),
      duration: new FormControl(null, [Validators.required]),
      publishDate: new FormControl(''),
      status: new FormControl(null, [Validators.required]),
    })

    const localSelectedTab = localStorage.getItem('selected_tab')
    if (localSelectedTab) {
      this.selectedTabs = JSON.parse(localSelectedTab)
    } else {
      this.selectedTabs = {
        newsFeed: false,
        overview: false,
        materials: true,
        accelerated: false,
        submitted: false,
        class: false,
      }
      localStorage.setItem('selected_tab', JSON.stringify(this.selectedTabs))
    }

    this.profileInfo = JSON.parse(this.ls.getItem('profile_info'))
    this.userRole = this.profileInfo.role
    this.userId = this.profileInfo.id
    this.isInstructor = this.profileInfo.role === 4
    if (this.role == 4) {
      this.newsFeedsClient.instructorOrAdminNewsfeeds(this.moduleId).subscribe((data) => {
        if (data) {
          data.forEach((item) => {
            const temp = moment(item.publishDate).lang('en').format('DD-MM-YYYY')
            item.publishDate = temp as any
          })
        }
        this.newsFeedList = data
        this.isnewsFeedListReady = true
        if (this.newsFeedList.length == 0) {
          this.isExistSections = true
        }
      })
    }
    this.materials.moduleMaterials(this.id).subscribe((data) => {
      if (data.length > 0) {
        this.materialsData = data
      } else {
        this.isExistMaterials = false
      }
      this.ready = true
    })
    this.projects.underreviewProjects(this.id).subscribe((data) => {
      this.underreviewData = data
      if (this.underreviewData.length == 0) {
        this.isExistTemplate = false
      }
    })
    this.projects.instructorTemplateProject(this.id).subscribe((data) => {
      this.templateData = data
      if (this.templateData.length == 0) {
        this.isExistAccelerated = false
      }
    })
    if (this.programId) {
      this.programs.programs(this.programId).subscribe((data) => {
        this.isProgramNameReady = true
        this.programName = data.name
      })
    }
    this.modules.modules(this.id).subscribe((data) => {
      this.isModuleNameReady = true
      this.moduleName = data.name
      this.moduleOrder = data.order
      this.overviewText = data.overview
    })
    this.modules.moduleApplicants(this.id).subscribe((data) => {
      this.classListData = data
      this.isClassListReady = true
    })
  }
}
