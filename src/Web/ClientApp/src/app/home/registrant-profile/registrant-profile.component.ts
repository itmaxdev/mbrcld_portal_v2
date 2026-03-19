import { Component, OnInit } from '@angular/core'
import { ProfileFacade } from '../profile/common/profile-facade.service'

@Component({
  selector: 'app-registrant-profile',
  templateUrl: './registrant-profile.component.html',
  // styleUrls: ['./registrant-profile.component.scss']
})
export class RegistrantProfileComponent implements OnInit {
  role: number
  completionPercentage = 0
  progressReady = false

  constructor(private profileFacade: ProfileFacade) {}

  async ngOnInit(): Promise<void> {
    const profileInfo = JSON.parse(localStorage.getItem('profile_info'))
    if (profileInfo) {
      this.role = profileInfo.role
    }

    try {
      const progress = await this.profileFacade.loadFormProgress()
      this.completionPercentage = progress?.completionPercentage ?? 0
    } finally {
      this.progressReady = true
    }
  }
}
