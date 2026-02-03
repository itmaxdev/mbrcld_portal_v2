import { Component, OnInit } from '@angular/core'
import { SecureStorage } from 'src/app/core/api-authorization'

@Component({
  selector: 'app-programs-main-page',
  templateUrl: './programs-main-page.component.html',
  styleUrls: ['./programs-main-page.component.scss'],
})
export class ProgramsMainPageComponent implements OnInit {
  role: number
  profileInfo: any

  constructor(private ls: SecureStorage) {}

  ngOnInit(): void {
    this.profileInfo = JSON.parse(this.ls.getItem('profile_info'))
    this.role = this.profileInfo.role
  }
}
