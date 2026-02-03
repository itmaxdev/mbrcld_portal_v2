import { Component, OnInit } from '@angular/core'
import {
  DashboardClient,
  DashboardViewModel,
  IKPI,
  ProfileClient,
} from 'src/app/shared/api.generated.clients'

interface IDashboardTabs {
  programs: boolean
  modules: boolean
}

@Component({
  selector: 'app-dm-dashboard-list',
  templateUrl: './dm-dashboard-list.component.html',
  styleUrls: ['./dm-dashboard-list.component.scss'],
})
export class DmDashboardListComponent implements OnInit {
  ready = false
  products: any[]
  isReadyDashboardData = false
  activeItemId: string = undefined
  isReady = false
  selectedProduct: any
  dashboardTabs: IDashboardTabs
  KPIData: IKPI[]
  dashboardData: DashboardViewModel[]

  constructor(private dashboard: DashboardClient, private profile: ProfileClient) {}

  handleChange(event) {
    switch (event.index) {
      case 0:
        this.dashboardTabs = {
          programs: true,
          modules: false,
        }
        break
      case 1:
        this.dashboardTabs = {
          programs: false,
          modules: true,
        }
        break
    }
    localStorage.setItem('dm_dashboard_tabs', JSON.stringify(this.dashboardTabs))
  }

  onApplicantSelect(id) {
    this.activeItemId = id
    this.selectedProduct = true
    this.isReadyDashboardData = false
    if (id) {
      for (let count = 0; count < this.products.length; count++) {
        if (this.products[count].id === id) {
          this.products[count].isActive = true
        } else {
          this.products[count].isActive = false
        }
      }
      this.dashboard.dashboard(id).subscribe((data) => {
        this.isReadyDashboardData = true
        this.dashboardData = data
      })
    } else {
      this.selectedProduct = undefined
    }
  }

  async ngOnInit() {
    this.profile.directmanagerApplicants().subscribe((data) => {
      this.products = data
      for (let count = 0; count < this.products.length; count++) {
        this.products[count].isActive = false
      }
      this.isReady = true
    })
  }
}
