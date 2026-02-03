import { Component, OnInit } from '@angular/core'
import { SecureStorage } from 'src/app/core/api-authorization'
import {
  ProgramsClient,
  ListProgramByUserModuleViewModel,
} from 'src/app/shared/api.generated.clients'

@Component({
  selector: 'app-programs-list3',
  templateUrl: './programs-list3.component.html',
  styleUrls: ['./programs-list3.component.scss'],
})
export class ProgramsList3Component implements OnInit {
  ready = false
  userRole: number
  profileInfo: any
  programsList: ListProgramByUserModuleViewModel[]

  constructor(private programs: ProgramsClient, private ls: SecureStorage) {}

  async ngOnInit() {
    this.ready = false

    await Promise.all([
      this.programs.userPrograms().subscribe((data) => {
        this.profileInfo = JSON.parse(this.ls.getItem('profile_info'))
        this.userRole = this.profileInfo.role
        this.programsList = data
        this.ready = true
      }),
    ])
  }
}
