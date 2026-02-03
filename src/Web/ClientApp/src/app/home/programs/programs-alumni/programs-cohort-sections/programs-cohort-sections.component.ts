import { tap } from 'rxjs/operators'
import { ActivatedRoute } from '@angular/router'
import { Component, OnInit, LOCALE_ID, Inject } from '@angular/core'
import {
  ListSectionsByMaterialIdViewModel,
  MaterialsClient,
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
}

@Component({
  selector: 'app-programs-cohort-sections',
  templateUrl: './programs-cohort-sections.component.html',
  styleUrls: ['./programs-cohort-sections.component.scss'],
})
export class ProgramsCohortSectionListComponent implements OnInit {
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
  modalHeader: string
  moduleOrder: number
  displayBasic: boolean
  materialOrder: number
  sectionForm: FormGroup
  isExistSections = false
  isModuleNameReady = false
  isProgramNameReady = false
  isMaterialNameReady = false
  isInstructor = false
  sectionId: string = undefined
  sectionsList: ListSectionsByMaterialIdViewModel[]

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

  async openDialog(sectionId: string) {
    await Promise.all([
      this.sections.sectionsGet(sectionId).subscribe((data) => {
        this.topic = data.name
        this.topic_ar = data.name_AR
        this.duration = data.duration
        this.order = data.order
        this.startDate = moment(data.startDate).toDate()
        this.showBasicDialog(sectionId)
      }),
    ])
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
            sectionData.startDate
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
    })

    this.sections.cohortSections(this.materialsId).subscribe((data) => {
      this.profileInfo = JSON.parse(this.ls.getItem('profile_info'))
      this.userRole = this.profileInfo.role
      if (data) {
        this.sectionsList = data
        this.isInstructor = this.profileInfo.role === 4
        return (this.ready = true)
      }
      return (this.isExistSections = true)
    }),
      (this.isMaterialNameReady = true)
    this.materials.materialsGet(this.materialsId).subscribe((data) => {
      this.materialName = data.name
      this.materialOrder = data.order
    })
  }
}
