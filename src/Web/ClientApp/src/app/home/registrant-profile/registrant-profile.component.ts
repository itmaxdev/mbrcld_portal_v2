import { Component, OnInit } from '@angular/core'

@Component({
  selector: 'app-registrant-profile',
  templateUrl: './registrant-profile.component.html',
  // styleUrls: ['./registrant-profile.component.scss']
})
export class RegistrantProfileComponent implements OnInit {
  role: number

  constructor() {}

  ngOnInit(): void {
    const profileInfo = JSON.parse(localStorage.getItem('profile_info'))
    if (profileInfo) {
      this.role = profileInfo.role
    }
  }
}
