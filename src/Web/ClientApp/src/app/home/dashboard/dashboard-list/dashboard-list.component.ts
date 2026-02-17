import { Component, OnInit } from '@angular/core'
import { DashboardClient, DashboardViewModel, IKPI } from 'src/app/shared/api.generated.clients'

@Component({
  selector: 'app-dashboard-list',
  templateUrl: './dashboard-list.component.html',
  styleUrls: ['./dashboard-list.component.scss'],
})
export class DashboardListComponent implements OnInit {
  role: number
  ready = false
  dashboardTabs: string
  name: string
  name_AR: string
  value: number
  details: IKPI[]
  data: DashboardViewModel[]

  constructor(private dashboard: DashboardClient) {}

  handleChange(event) {
    switch (event.index) {
      case 0:
        this.dashboardTabs = '0'
        break
      case 1:
        this.dashboardTabs = '1'
        break
      case 2:
        this.dashboardTabs = '2'
        break
      case 3:
        this.dashboardTabs = '3'
        break
      case 4:
        this.dashboardTabs = '4'
        break
      case 5:
        this.dashboardTabs = '5'
        break
      case 6:
        this.dashboardTabs = '6'
        break
    }
    localStorage.setItem('dashboard_tabs', this.dashboardTabs)
  }

  checkLocalStorage() {
    const localDashboardTabs = localStorage.getItem('dashboard_tabs')
    if (localDashboardTabs) {
      this.dashboardTabs = localDashboardTabs
    } else {
      this.dashboardTabs = '0'
      localStorage.setItem('dashboard_tabs', this.dashboardTabs)
    }
  }

  getUserRole() {
    const profileInfo = JSON.parse(localStorage.getItem('profile_info'))
    this.role = profileInfo.role
  }

  getItem(name: string): DashboardViewModel | undefined {
    return this.data?.find((x) => x.name === name)
  }

  getPercentage(value: number, total: number): number {
    if (!total || total === 0) return 0
    return (value / total) * 100
  }

  ngOnInit() {
    this.ready = false
    this.checkLocalStorage()
    this.getUserRole()
    this.dashboard.dashboard('').subscribe((data: DashboardViewModel[]) => {
      this.data = data
      this.ready = true
    })
  }
}
