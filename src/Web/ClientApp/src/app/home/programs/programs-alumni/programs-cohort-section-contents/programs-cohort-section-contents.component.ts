import { ActivatedRoute } from '@angular/router'
import { ChangeDetectorRef, Component, OnInit, LOCALE_ID, Inject } from '@angular/core'
import {
  ContentsClient,
  ListSectionsByMaterialIdViewModel,
  SectionsClient,
} from 'src/app/shared/api.generated.clients'
import { SectionDataService } from 'src/app/shared/services/section-data.service'
import { tap } from 'rxjs/operators'
import { Location } from '@angular/common'
import { SecureStorage } from 'src/app/core/api-authorization'

enum Status {
  Unread = 1,
  Review = 936510000,
  Done = 936510001,
}
@Component({
  selector: 'app-programs-cohort-section-contents',
  templateUrl: './programs-cohort-section-contents.component.html',
  styleUrls: ['./programs-cohort-section-contents.component.scss'],
})
export class ProgramsCohortSectionComponent implements OnInit {
  ready = false
  generatedURL = ''
  sectionName = ''
  sectionOrder: number
  materialsId: string
  contentReady = false
  sectionContents: any
  currentSectionId = ''
  sectionId = ''
  profileInfo: any
  userRole: number
  isInstructor: boolean
  isExistSections = true
  activeIndex = 1
  currentStatusText: string
  sectionsDataIds: Array<string> = []
  allSections: ListSectionsByMaterialIdViewModel[]

  statuses = [1, 936510000, 936510001]
  statusBar = {
    Unread: true,
    Review: true,
    Done: true,
  }

  statusText = {
    Unread: $localize`This activity has yet to be attempted.`,
    Review: $localize`You have marked this for review. Remember to come back to it when you have time`,
    Done: $localize`Good work, you have marked this activity as complete.`,
  }

  constructor(
    private ls: SecureStorage,
    private location: Location,
    private route: ActivatedRoute,
    private sections: SectionsClient,
    private contents: ContentsClient,
    private cdRef: ChangeDetectorRef,
    private section: SectionDataService,
    @Inject(LOCALE_ID) public locale: string
  ) {}

  goBack() {
    this.section.redirectBack(2)
  }

  async handleChange(event) {
    const sectionId = this.sectionsDataIds[event.index]
    const currentUrl = this.section.redirectBack(1, true)
    this.generatedURL = `${currentUrl}/${sectionId}`
    this.location.go(this.generatedURL)
    this.sectionName = ''
    this.sectionOrder = null

    this.statusBar = {
      Unread: true,
      Review: true,
      Done: true,
    }

    this.contentReady = false

    await Promise.all([
      this.contents.sectionContents(sectionId).subscribe((data) => {
        this.sectionContents = data
        this.sections.sectionsGet(sectionId).subscribe((data) => {
          this.sectionId = data.id
          this.sectionName = data.name
          this.sectionOrder = data.order
          this.activeIndex = data.order - 1
          this.contentReady = true
          this.statusBar[Status[data.status]] = false
        })
      }),
    ])
  }

  async getFirstSection(id) {
    this.ready = false

    await Promise.all([
      this.contents.sectionContents(id).subscribe((data) => {
        this.sectionContents = data
        this.contentReady = true
      }),
    ])
  }

  updateStatus(updateTo: string) {
    this.currentSectionId = this.route.snapshot.paramMap.get('sectionId')

    this.statusBar = {
      Unread: true,
      Review: true,
      Done: true,
    }

    this.statusBar[updateTo] = false

    this.currentStatusText = this.statusText[updateTo]
    this.sections
      .sectionsPut(this.sectionId, Status[updateTo])
      .pipe(tap(() => {}))
      .toPromise()

    this.allSections.map((item) => {
      if (item.id == this.currentSectionId) {
        const activeElement = document.querySelectorAll('li.p-highlight.ng-star-inserted')[0]
        if (updateTo === 'Done') {
          if (!activeElement.classList.contains('finished')) {
            activeElement.classList.add('finished')
          }
        } else {
          if (activeElement.classList.contains('finished')) {
            activeElement.classList.remove('finished')
          }
        }
        return
      }
    })
  }

  ngAfterViewChecked() {
    this.cdRef.detectChanges()
  }

  async ngOnInit() {
    const currentUrl = this.section.redirectBack(1, true)

    this.ready = false
    this.currentSectionId = this.route.snapshot.paramMap.get('sectionId')
    this.materialsId = this.route.snapshot.paramMap.get('materialsId')
    this.profileInfo = JSON.parse(this.ls.getItem('profile_info'))
    this.userRole = this.profileInfo.role
    this.isInstructor = this.profileInfo.role === 4
    this.generatedURL = `${currentUrl}/${this.currentSectionId}`

    this.statusBar = {
      Unread: true,
      Review: true,
      Done: true,
    }

    await Promise.all([
      this.sections.materialSections(this.materialsId).subscribe((data) => {
        if (data.length > 0) {
          this.allSections = data
          this.allSections.map((item) => this.sectionsDataIds.push(item.id))
          if (!this.currentSectionId) {
            this.currentSectionId = this.sectionsDataIds[0]
          }
          this.getFirstSection(this.currentSectionId)
        } else {
          this.isExistSections = false
        }
        this.sections.sectionsGet(this.currentSectionId).subscribe((data) => {
          this.sectionId = data.id
          this.sectionName = data.name
          this.sectionOrder = data.order
          this.activeIndex = data.order - 1
          this.statusBar[Status[data.status]] = false
          this.currentStatusText = this.statusText[Status[data.status]]

          setTimeout(() => {
            const activeElement = document.querySelectorAll('li.ng-star-inserted')
            this.allSections.map((item, index) => {
              if (item.status == this.statuses[2]) {
                activeElement[index].classList.add('finished')
              }
            })
          }, 300)
          this.ready = true
        })
      }),
    ])
  }
}
