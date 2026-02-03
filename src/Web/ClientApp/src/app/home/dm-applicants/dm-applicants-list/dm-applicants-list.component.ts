import { Component, OnInit } from '@angular/core'
import {
  IListDirectManagerApplicantsViewModel,
  ProfileClient,
} from 'src/app/shared/api.generated.clients'

@Component({
  selector: 'app-dm-applicants',
  templateUrl: './dm-applicants-list.component.html',
  styleUrls: ['./dm-applicants-list.component.scss'],
})
export class DmApplicantsListComponent implements OnInit {
  clickedApplicant: string
  isApplicantsListReady = false
  viewApplicant = false
  applicantsList: IListDirectManagerApplicantsViewModel[]

  constructor(private profile: ProfileClient) {}

  ngOnInit(): void {
    this.profile.directmanagerApplicants().subscribe((data) => {
      this.applicantsList = data
      this.isApplicantsListReady = true
    })
  }

  onApplicantSelect(id) {
    this.clickedApplicant = id
    this.viewApplicant = true
  }
}
