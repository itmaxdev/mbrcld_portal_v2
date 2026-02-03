import { Component, OnInit } from '@angular/core'

@Component({
  selector: 'app-individual-dev-membership',
  templateUrl: './individual-dev-membership.component.html',
})
export class IndividualDevMembershipComponent implements OnInit {
  individaulMembershipTabs: string

  constructor() {}

  handleChange(event) {
    switch (event.index) {
      case 0:
        this.individaulMembershipTabs = '0'
        break
      case 1:
        this.individaulMembershipTabs = '1'
        break
      case 2:
        this.individaulMembershipTabs = '2'
        break
    }
    localStorage.setItem('individaulMembershipTabs', JSON.stringify(this.individaulMembershipTabs))
  }

  checkLocalStorage() {
    const localDevMembershipTabs = localStorage.getItem('individaulMembershipTabs')
    if (localDevMembershipTabs) {
      this.individaulMembershipTabs = JSON.parse(localDevMembershipTabs)
    } else {
      this.individaulMembershipTabs = '0'
      localStorage.setItem(
        'individaulMembershipTabs',
        JSON.stringify(this.individaulMembershipTabs)
      )
    }
  }

  ngOnInit(): void {
    this.checkLocalStorage()
  }
}
