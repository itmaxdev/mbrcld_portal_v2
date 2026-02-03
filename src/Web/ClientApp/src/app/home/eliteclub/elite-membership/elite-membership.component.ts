import { Component, OnInit } from '@angular/core'
import {
  EliteMembershipAttendanceClient,
  ElteMembershipAttendance,
} from 'src/app/shared/api.generated.clients'

@Component({
  selector: 'app-elite-membership',
  templateUrl: './elite-membership.component.html',
})
export class EliteMembershipComponent implements OnInit {
  eliteClubId: string
  membershipType: 'panelDiscussions' | 'consultingForums' | 'ideaLab' = 'panelDiscussions'
  isTabReady = false
  panelDiscussions: Array<ElteMembershipAttendance>
  consultingForums: Array<ElteMembershipAttendance>
  ideaLab: Array<ElteMembershipAttendance>

  constructor(private eliteMembershipAttendanceClient: EliteMembershipAttendanceClient) {}

  ngOnInit(): void {
    const eliteclubId = localStorage.getItem('eliteclubId')
    if (eliteclubId) this.eliteClubId = eliteclubId
    this.getPanelDiscussions()
  }

  getPanelDiscussions() {
    const membershipType = 1
    this.isTabReady = false
    this.eliteMembershipAttendanceClient
      .eliteMembershipAttendanceGet(this.eliteClubId, membershipType)
      .subscribe((data) => {
        this.panelDiscussions = data
        this.isTabReady = true
      })
  }

  getConsultingForums() {
    const membershipType = 2
    this.isTabReady = false
    this.eliteMembershipAttendanceClient
      .eliteMembershipAttendanceGet(this.eliteClubId, membershipType)
      .subscribe((data) => {
        this.consultingForums = data
        this.isTabReady = true
      })
  }

  getIdeaLab() {
    const membershipType = 3
    this.isTabReady = false
    this.eliteMembershipAttendanceClient
      .eliteMembershipAttendanceGet(this.eliteClubId, membershipType)
      .subscribe((data) => {
        this.ideaLab = data
        this.isTabReady = true
      })
  }

  handleChange(event) {
    switch (event.index) {
      case 0:
        this.getPanelDiscussions()
        break
      case 1:
        this.getConsultingForums()
        break
      case 2:
        this.getIdeaLab()
        break
      default:
        this.getPanelDiscussions()
    }
  }
}
