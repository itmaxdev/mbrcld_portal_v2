import { Component, Input, OnInit, LOCALE_ID, Inject } from '@angular/core'
import { ActivatedRoute } from '@angular/router'
import { IGetApplicantProfileViewModel, ModulesClient } from 'src/app/shared/api.generated.clients'
import { SectionDataService } from 'src/app/shared/services/section-data.service'

@Component({
  selector: 'app-view-applicant-page',
  templateUrl: './view-applicant-page.component.html',
  styleUrls: ['./view-applicant-page.component.scss'],
})
export class ViewApplicantPageComponent implements OnInit {
  id: string
  applicantData: IGetApplicantProfileViewModel
  visibility = false
  isApplicantDataReady = false
  language: string
  @Input() applicantId: string
  @Input() activeBackBtn = true

  constructor(
    private section: SectionDataService,
    private modules: ModulesClient,
    private route: ActivatedRoute,
    @Inject(LOCALE_ID) locale
  ) {
    this.language = locale
  }

  ngOnInit(): void {
    this.visibility = this.language === 'ar'
    this.isApplicantDataReady = false
    if (this.applicantId) {
      this.id = this.applicantId
    } else {
      this.id = this.route.snapshot.paramMap.get('applicantId')
    }

    this.modules.applicantProfile(this.id).subscribe((data) => {
      this.applicantData = data
      this.isApplicantDataReady = true
    })
  }

  goBack() {
    this.section.redirectBack(2)
  }
}
