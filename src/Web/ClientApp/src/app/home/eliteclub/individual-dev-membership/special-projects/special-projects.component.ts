import { Inject, LOCALE_ID, Component, OnInit } from '@angular/core'
import { ProfileClient, SpecialProjectsClient } from 'src/app/shared/api.generated.clients'
import { DisclaimerTypes } from 'src/app/shared/components/disclaimer.component'

@Component({
  selector: 'app-special-projects',
  templateUrl: './special-projects.component.html',
})
export class SpecialProjectsComponent implements OnInit {
  public searchingIdeas = false
  public myIdeas = false
  public projectData: any[]
  public showDisclaimer = false

  constructor(
    @Inject(LOCALE_ID) private locale: string,
    private specialProjects: SpecialProjectsClient,
    private profileClient: ProfileClient
  ) {}

  ngOnInit(): void {
    this.profileClient
      .userDisclaimerGet(DisclaimerTypes.SpecialProjectDisclaimer)
      .subscribe((data) => {
        if (data.disclaimer) this.getUserSpecialProjects()
        else this.showDisclaimer = true
      })
  }

  handleDiclaimerApprove() {
    this.showDisclaimer = false
    this.getUserSpecialProjects()
  }

  getUserSpecialProjects() {
    this.specialProjects.userSpecialProjects().subscribe((data) => {
      this.projectData = data
      this.searchingIdeas = true
    })
  }

  getLocale(): string {
    if (this.locale === 'ar') {
      return 'في حال تم اعتماد المشروع، تكون ملكية المشروع تابعة للمركز'
    } else {
      return 'Upon project approval, the center will have full ownership of the project'
    }
  }
}
