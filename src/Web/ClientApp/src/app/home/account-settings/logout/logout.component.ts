import { Component, OnInit } from '@angular/core'
import { AuthorizationService } from 'src/app/core/api-authorization'

@Component({
  selector: 'app-logout',
  templateUrl: './logout.component.html',
  styleUrls: ['./logout.component.scss'],
})
export class LogoutComponent implements OnInit {
  constructor(private authService: AuthorizationService) {}

  ngOnInit(): void {
    this.logOut()
  }

  async logOut() {
    await this.authService.logout()
    localStorage.removeItem('profile_info')
    localStorage.removeItem('uaeCode')
    localStorage.removeItem('eliteclubId')
    this.authService.login()
  }
}
