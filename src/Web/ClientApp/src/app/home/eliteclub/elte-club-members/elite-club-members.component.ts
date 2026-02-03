import { Component, OnInit } from '@angular/core'
import {
  EliteClubMembersClient,
  IListEliteClubMembersByIdViewModel,
} from 'src/app/shared/api.generated.clients'

@Component({
  selector: 'app-elite-club-members',
  templateUrl: './elite-club-members.component.html',
})
export class EliteClubMembersComponent implements OnInit {
  eliteclubMembers: Array<IListEliteClubMembersByIdViewModel>

  constructor(private elitclubMembersService: EliteClubMembersClient) {}

  ngOnInit(): void {
    const eliteclubId = localStorage.getItem('eliteclubId')
    if (eliteclubId) {
      this.elitclubMembersService.eliteclubMember(eliteclubId).subscribe((data) => {
        if (data) this.eliteclubMembers = data
      })
    }
  }
}
