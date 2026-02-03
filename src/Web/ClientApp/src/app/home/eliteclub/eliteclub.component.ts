import { Component, OnInit } from '@angular/core'
import { SectionDataService } from 'src/app/shared/services/section-data.service'

@Component({
  selector: 'app-eliteclub',
  templateUrl: './eliteclub.component.html',
})
export class EliteclubComponent implements OnInit {
  eliteClubTabs: string

  constructor(private section: SectionDataService) {}

  handleChange(event) {
    switch (event.index) {
      case 0:
        this.eliteClubTabs = '0'
        break
      case 1:
        this.eliteClubTabs = '1'
        break
      case 2:
        this.eliteClubTabs = '2'
        break
      case 3:
        this.eliteClubTabs = '3'
        break
    }
    localStorage.setItem('eliteClubTabs', JSON.stringify(this.eliteClubTabs))
  }

  checkLocalStorage() {
    const localEliteclubTabs = localStorage.getItem('eliteClubTabs')
    if (localEliteclubTabs) {
      this.eliteClubTabs = JSON.parse(localEliteclubTabs)
    } else {
      this.eliteClubTabs = '0'
      localStorage.setItem('eliteClubTabs', JSON.stringify(this.eliteClubTabs))
    }
  }

  ngOnInit(): void {
    this.checkLocalStorage()
  }

  ngOnDestroy() {
    if (!this.section.checkInEliteClub()) {
      localStorage.removeItem('eliteClubTabs')
      localStorage.removeItem('individaulMembershipTabs')
    }
  }
}
