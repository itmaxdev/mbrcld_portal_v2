import { Component, EventEmitter, Input, OnChanges, Output } from '@angular/core'
import { GetApplicantProfileViewModel, ModulesClient } from '../../api.generated.clients'

/** Normalized profile shape for the common dialog (applicant or team member). */
export interface NormalizedProfileData {
  profilePictureUrl?: string
  name?: string
  email?: string
  linkedInUrl?: string
  nationality?: string
  residenceCountry?: string
  education?: string
  jobTitle?: string
  organization?: string
  aboutMember?: string
}

@Component({
  selector: 'app-applicant-profile-dialog',
  templateUrl: './applicant-profile-dialog.component.html',
  styleUrls: ['./applicant-profile-dialog.component.scss'],
})
export class ApplicantProfileDialogComponent implements OnChanges {
  @Input() visible = false
  /** When set, fetch and show applicant profile (Class List). */
  @Input() applicantId: string
  /** When set, show this profile as-is (Team member). Same design as applicant popup. */
  @Input() profileData: NormalizedProfileData | null | undefined
  @Output() visibleChange = new EventEmitter<boolean>()

  data: NormalizedProfileData
  ready = false

  constructor(private modules: ModulesClient) {}

  ngOnChanges(): void {
    if (!this.visible) {
      this.data = undefined
      this.ready = false
      return
    }
    if (this.profileData != null) {
      this.data = this.normalizeProfileData(this.profileData)
      this.ready = true
      return
    }
    if (this.applicantId) {
      this.ready = false
      this.modules.applicantProfile(this.applicantId).subscribe((profile) => {
        this.data = this.normalizeProfileData((profile as unknown) as NormalizedProfileData)
        this.ready = true
      })
    } else {
      this.data = undefined
      this.ready = false
    }
  }

  /** Normalize API shapes (applicant or team member) to common display shape. */
  private normalizeProfileData(raw: any): NormalizedProfileData {
    if (!raw) return undefined
    return {
      profilePictureUrl: raw.profilePictureUrl ?? raw.pictureUrl,
      name: raw.name,
      email: raw.email,
      linkedInUrl: raw.linkedInUrl ?? raw.linkedIn,
      nationality: raw.nationality ?? raw.nationalityName,
      residenceCountry: raw.residenceCountry ?? raw.residenceCountryName,
      education: raw.education,
      jobTitle: raw.jobTitle ?? raw.jobPosition,
      organization: raw.organization,
      aboutMember: raw.aboutMember ?? raw.aboutInstructor,
    }
  }

  close(): void {
    this.visibleChange.emit(false)
  }
}
