import { Inject, LOCALE_ID, Component, OnInit } from '@angular/core'
import { ProfileClient, ProjectIdeasClient } from 'src/app/shared/api.generated.clients'
import { DisclaimerTypes } from 'src/app/shared/components/disclaimer.component'

@Component({
  selector: 'app-project-ideas-list',
  templateUrl: './project-ideas-list.component.html',
  styleUrls: ['./project-ideas-list.component.scss'],
})
export class ProjectIdeasListComponent implements OnInit {
  public myIdeas = false
  public projectData: any[]
  public searchingIdeas = false
  public allIdeasLable = 'All Ideas'
  public myIdeasLable = 'My Ideas'
  public showDisclaimer = false

  constructor(
    @Inject(LOCALE_ID) private locale: string,
    private profileClient: ProfileClient,
    private projectIdeas: ProjectIdeasClient
  ) {}

  ngOnInit(): void {
    if (this.locale === 'ar') {
      this.allIdeasLable = 'جميع الأفكار'
      this.myIdeasLable = 'أفكاري'
    }
    this.profileClient.userDisclaimerGet(DisclaimerTypes.IdeaHubDisclaimer).subscribe((data) => {
      if (data.disclaimer) this.getProjectIdeas()
      else this.showDisclaimer = true
    })
  }

  handleDiclaimerApprove() {
    this.showDisclaimer = false
    this.getProjectIdeas()
  }

  filterMyIdeas() {
    this.searchingIdeas = false
    if (this.myIdeas) {
      this.projectIdeas.userProjectIdeas().subscribe((data) => {
        this.projectData = data
        this.searchingIdeas = true
      })
    } else {
      this.getProjectIdeas()
    }
  }

  searchIdeas(searchedValue) {
    this.searchingIdeas = false
    const value = searchedValue.target.value ? searchedValue.target.value : ''

    if (value !== '') {
      this.projectIdeas.search(value).subscribe((data) => {
        this.projectData = data
        this.searchingIdeas = true
      })
    } else {
      this.getProjectIdeas()
    }
  }

  getProjectIdeas() {
    this.projectIdeas.projectIdeasGet().subscribe((data) => {
      this.projectData = data
      this.searchingIdeas = true
    })
  }

  getLocale(): string {
    if (this.locale === 'ar') {
      return 'في حال تم قبول الفكرة ، تكون ملكية الفكرة تابعة للمركز'
    } else {
      return 'Upon idea approval, the center will have full ownership of the idea'
    }
  }
}
