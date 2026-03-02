import { Component, EventEmitter, Input, OnChanges, Output } from '@angular/core'
import { GetApplicantProfileViewModel, ModulesClient } from '../../api.generated.clients'

@Component({
  selector: 'app-applicant-profile-dialog',
  templateUrl: './applicant-profile-dialog.component.html',
  styleUrls: ['./applicant-profile-dialog.component.scss'],
})
export class ApplicantProfileDialogComponent implements OnChanges {
  @Input() visible = false
  @Input() applicantId: string
  @Output() visibleChange = new EventEmitter<boolean>()

  data: GetApplicantProfileViewModel
  ready = false

  constructor(private modules: ModulesClient) {}

  ngOnChanges(): void {
    if (this.visible && this.applicantId) {
      this.ready = false
      this.modules.applicantProfile(this.applicantId).subscribe((profile) => {
        this.data = profile
        this.ready = true
      })
    } else if (!this.visible) {
      this.data = undefined
      this.ready = false
    }
  }

  close(): void {
    this.visibleChange.emit(false)
  }
}
