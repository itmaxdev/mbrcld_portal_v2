import { tap } from 'rxjs/operators'
import { ActivatedRoute } from '@angular/router'
import { Component, OnInit, LOCALE_ID, Inject } from '@angular/core'
import {
  ListSectionsByMaterialIdViewModel,
  MaterialsClient,
  MaterialStatuses,
  ModulesClient,
  ProgramsClient,
  SectionsClient,
} from 'src/app/shared/api.generated.clients'
import { FormControl, FormGroup, Validators } from '@angular/forms'
import { SecureStorage } from 'src/app/core/api-authorization'
import { SectionDataService } from 'src/app/shared/services/section-data.service'
import * as moment from 'moment'

interface ISectionData {
  name: string
  order: number
  nameAr: string
  duration: number
  startDate: Date | null | undefined
  publishDate: Date
  status: MaterialStatuses
}

@Component({
  selector: 'app-programs-section-list',
  templateUrl: './programs-section-list.component.html',
  styleUrls: ['./programs-section-list.component.scss'],
})
export class ProgramsSectionListComponent implements OnInit {
  topic: string
  order: number
  ready = false
  modulesId = ''
  programId = ''
  moduleName = ''
  materialsId = ''
  programName = ''
  topic_ar: string
  duration: number
  materialName = ''
  userRole: number
  profileInfo: any
  startDate: Date
  publishDate: Date
  status: any
  modalHeader: string
  moduleOrder: number
  displayBasic: boolean
  materialOrder: number
  sectionForm: FormGroup
  isExistSections = false
  isModuleNameReady = false
  isProgramNameReady = false
  isScheudledSelected = false
  isMaterialNameReady = false
  isInstructor = false
  isAdmin = false
  sectionId: string = undefined
  sectionsList: ListSectionsByMaterialIdViewModel[]

  sectionStatusOptions = [
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
    private sections: SectionsClient,
    private programs: ProgramsClient,
    private materials: MaterialsClient,
    private section: SectionDataService,
    @Inject(LOCALE_ID) public locale: string
  ) {}

  onSelectType(event) {
    const publishDateInput = this.sectionForm.get('publishDate')
    if (event == MaterialStatuses.Scheduled) {
      this.isScheudledSelected = true
      publishDateInput.setValidators(Validators.required)
    } else {
      this.isScheudledSelected = false
      publishDateInput.clearValidators()
    }
    publishDateInput.updateValueAndValidity()
  }

  openDialog(sectionId: string) {
    this.sections.sectionsGet(sectionId).subscribe((data) => {
      this.topic = data.name
      this.topic_ar = data.name_AR
      this.duration = data.duration
      this.order = data.order
      this.startDate = moment(data.startDate).toDate()
      this.publishDate = data.publishDate ? moment(data.publishDate).toDate() : ('' as any)
      this.status = data.sectionStatus
      this.showBasicDialog(sectionId)
    })
  }

  showBasicDialog(sectionId: string = undefined) {
    this.sectionId = sectionId
    this.modalHeader = 'Edit Section'
    if (!this.sectionId) {
      this.sectionForm.reset()
      if (this.locale === 'ar') this.modalHeader = 'أضف قسم'
      else this.modalHeader = 'Add New Section'
    }
    this.displayBasic = true
  }

  goBack() {
    this.section.redirectBack(2)
  }

  async saveSection() {
    if (this.sectionForm.valid) {
      const sectionData: ISectionData = this.sectionForm.value
      await Promise.all([
        this.sections
          .sectionsPost(
            this.sectionId,
            this.materialsId,
            sectionData.name,
            sectionData.nameAr,
            sectionData.duration,
            sectionData.order,
            sectionData.startDate,
            sectionData.publishDate,
            sectionData.status
          )
          .pipe(
            tap(() => {
              this.sections.materialSections(this.materialsId).subscribe((data) => {
                this.sectionsList = data
                this.ready = true
                this.displayBasic = false
              })
            })
          )
          .toPromise(),
      ])
    }
  }

  ngOnInit() {
    this.ready = false
    this.isExistSections = false
    this.programId = this.route.snapshot.paramMap.get('programId')
    this.modulesId = this.route.snapshot.paramMap.get('modulesId')
    this.materialsId = this.route.snapshot.paramMap.get('materialsId')

    this.sectionForm = new FormGroup({
      name: new FormControl(null, [Validators.required]),
      nameAr: new FormControl(null, [Validators.nullValidator]),
      startDate: new FormControl(null, [Validators.required]),
      duration: new FormControl(null, [Validators.required]),
      order: new FormControl(null, [Validators.required]),
      publishDate: new FormControl(null, [Validators.required]),
      status: new FormControl(null, [Validators.required]),
    })

    this.sections.materialSections(this.materialsId).subscribe((data) => {
      this.profileInfo = JSON.parse(this.ls.getItem('profile_info'))
      this.userRole = this.profileInfo.role
      if (data.length > 0) {
        this.sectionsList = data
      } else {
        this.isExistSections = true
      }
      this.isInstructor = this.profileInfo.role === 4
      this.isAdmin = this.profileInfo.role === 6
      this.ready = true
    }),
      this.programs.programs(this.programId).subscribe((data) => {
        this.isProgramNameReady = true
        this.programName = data.name
      }),
      this.modules.modules(this.modulesId).subscribe((data) => {
        this.isModuleNameReady = true
        this.moduleName = data.name
        this.moduleOrder = data.order
      }),
      this.materials.materialsGet(this.materialsId).subscribe((data) => {
        this.isMaterialNameReady = true
        this.materialName = data.name
        this.materialOrder = data.order
      })
  }
}
