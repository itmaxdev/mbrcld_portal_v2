import { Component, OnInit, LOCALE_ID, Inject } from '@angular/core'
import { ActivatedRoute } from '@angular/router'
import { Location } from '@angular/common'
import * as moment from 'moment'
import {
  IListModulesByEliteClubIdViewModel,
  ListProjectsByCohortIdViewModel,
  ModulesClient,
  ProjectsClient,
} from 'src/app/shared/api.generated.clients'
import { SectionDataService } from 'src/app/shared/services/section-data.service'

@Component({
  selector: 'app-program-cohort-view',
  templateUrl: './program-cohort-view.component.html',
  styleUrls: ['./program-cohort-view.component.scss'],
})
export class ProgramCohortViewComponent implements OnInit {
  modules: Array<IListModulesByEliteClubIdViewModel> = []
  public cohortId: string
  public cohortProjects: ListProjectsByCohortIdViewModel[] | null = null
  public role: number
  activeTab = 0
  ready = false

  constructor(
    private projects: ProjectsClient,
    private modulesService: ModulesClient,
    private route: ActivatedRoute,
    private _location: Location,
    private sectionData: SectionDataService,
    @Inject(LOCALE_ID) public locale: string
  ) {}

  ngOnInit() {
    const { role } = JSON.parse(localStorage.getItem('profile_info'))
    this.role = role
    this.cohortId = this.route.snapshot.paramMap.get('cohortId')

    this.modulesService.cohortModules(this.cohortId).subscribe((data) => {
      if (data) {
        this.modules = data
      }
      this.ready = true
    })

    this.projects.cohortProjects(this.cohortId).subscribe((data) => {
      this.cohortProjects = data
    })
  }

  setTab(index: number) {
    this.activeTab = index
  }

  getPaddedOrder(order: number | undefined): string {
    const n = order ?? 0
    return n < 10 ? '0' + n : String(n)
  }

  getDurationLabel(minutes: number | undefined): string {
    if (minutes == null) return '0 Hours'
    const hours = Math.floor(minutes / 60)
    const mins = minutes % 60
    const h = hours < 10 ? '0' + hours : String(hours)
    if (mins === 0) return h + ' ' + (hours === 1 ? 'Hour' : 'Hours')
    return this.sectionData.convertMinuteToHours(String(minutes))
  }

  goBack() {
    this._location.back()
  }
}
