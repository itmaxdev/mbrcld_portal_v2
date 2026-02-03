import { ActivatedRoute } from '@angular/router'
import { Component, OnInit, LOCALE_ID, Inject } from '@angular/core'
import {
  ListSectionsByMaterialIdViewModel,
  MaterialsClient,
  ModulesClient,
  ProgramsClient,
  SectionsClient,
} from 'src/app/shared/api.generated.clients'
import { SecureStorage } from 'src/app/core/api-authorization'
import { SectionDataService } from 'src/app/shared/services/section-data.service'
import { FormControl, FormGroup, Validators } from '@angular/forms'
import { tap } from 'rxjs/operators'
import * as moment from 'moment'

@Component({
  selector: 'app-programs-cohort-materials',
  templateUrl: './programs-cohort-materials.component.html',
  styleUrls: ['./programs-cohort-materials.component.scss'],
})
export class ProgramsCohortMaterialsComponent implements OnInit {
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
    private sections: MaterialsClient,
    private programs: ProgramsClient,
    private materials: MaterialsClient,
    private section: SectionDataService,
    private sectionClient: SectionsClient,
    @Inject(LOCALE_ID) public locale: string
  ) {}

  async openDialog(sectionId: string) {
    await Promise.all([
      this.sectionClient.cohortSections(sectionId).subscribe((data) => {
        data.map((d) => {
          ;(this.topic = d.name),
            (this.topic_ar = d.name_AR),
            (this.duration = d.duration),
            (this.order = d.order),
            (this.startDate = moment(d.startDate).toDate())
        })
      }),
      this.showBasicDialog(sectionId),
    ])
  }

  showBasicDialog(sectionId: string = undefined) {
    this.sectionId = sectionId
    this.modalHeader = 'Edit Section'
    if (!this.sectionId) {
      this.sectionForm.reset()
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
      const sectionData: any = this.sectionForm.value
      await Promise.all([
        this.sectionClient
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
              this.sectionClient.materialSections(this.materialsId).subscribe((data) => {
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

  async ngOnInit() {
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

    await Promise.all([
      this.sectionClient.materialSections(this.materialsId).subscribe((data) => {
        this.profileInfo = JSON.parse(this.ls.getItem('profile_info'))
        this.userRole = this.profileInfo.role
        if (data.length > 0) {
          this.sectionsList = data
        } else {
          this.isExistSections = true
        }
        this.isInstructor = this.profileInfo.role === 4
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
      }),
    ])
  }
}
