import { Component, OnInit } from '@angular/core'

@Component({
  selector: 'app-programs-modules',
  templateUrl: './programs-modules.component.html',
  styleUrls: ['./programs-modules.component.scss'],
})
export class ProgramsModulesComponent implements OnInit {
  role: number
  constructor() {}

  ngOnInit(): void {
    const profileInfo = JSON.parse(localStorage.getItem('profile_info'))
    this.role = profileInfo.role
  }
}
