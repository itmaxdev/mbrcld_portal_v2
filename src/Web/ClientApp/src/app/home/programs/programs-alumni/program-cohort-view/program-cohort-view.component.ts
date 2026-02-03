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

@Component({
  selector: 'app-program-cohort-view',
  templateUrl: './program-cohort-view.component.html',
  styleUrls: ['./program-cohort-view.component.scss'],
})
export class ProgramCohortViewComponent implements OnInit {
  modules: Array<IListModulesByEliteClubIdViewModel> = []
  public cohortId: string
  public cohortProjects: ListProjectsByCohortIdViewModel[]
  public role: number
  constructor(
    private projects: ProjectsClient,
    private modulesService: ModulesClient,
    private route: ActivatedRoute,
    private _location: Location,
    @Inject(LOCALE_ID) private locale: string
  ) {}

  ngOnInit() {
    const { role } = JSON.parse(localStorage.getItem('profile_info'))
    this.role = role
    this.cohortId = this.route.snapshot.paramMap.get('cohortId')

    this.modulesService.cohortModules(this.cohortId).subscribe((data) => {
      if (data) {
        if (this.locale == 'en') {
          data.forEach(
            (item) =>
              (item.startDate = moment(item.startDate).lang('en').format('[Start] DD/MM/YY') as any)
          )
        } else {
          data.forEach(
            (item) =>
              (item.startDate = moment(item.startDate)
                .lang('en')
                .format('[تاريخ البدء] DD/MM/YY') as any)
          )
        }
        this.modules = data
      }
    })

    this.projects.cohortProjects(this.cohortId).subscribe((data) => {
      this.cohortProjects = data
    })
  }

  goBack() {
    this._location.back()
  }
}
