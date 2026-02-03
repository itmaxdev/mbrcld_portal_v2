import { Component, OnInit } from '@angular/core'
import { CalendarClient, ProfileClient } from 'src/app/shared/api.generated.clients'

@Component({
  selector: 'app-dm-attendance',
  templateUrl: './dm-attendance-list.component.html',
  styleUrls: ['./dm-attendance-list.component.scss'],
})
export class DmAttendanceListComponent implements OnInit {
  products: any[]
  selectedProduct: any
  liveClassesData: any[]
  isReady = false
  isReadyLiveClassesData = false

  constructor(private profile: ProfileClient, private calendar: CalendarClient) {}

  ngOnInit(): void {
    this.profile.directmanagerApplicants().subscribe((data) => {
      this.products = data
      for (let count = 0; count < this.products.length; count++) {
        this.products[count].isActive = false
      }
      this.isReady = true
    })
  }

  onApplicantSelect(id) {
    this.selectedProduct = true
    this.isReadyLiveClassesData = false
    if (id) {
      for (let count = 0; count < this.products.length; count++) {
        if (this.products[count].id === id) {
          this.products[count].isActive = true
        } else {
          this.products[count].isActive = false
        }
      }
      this.calendar.meetings(id).subscribe((data) => {
        this.liveClassesData = data
        this.isReadyLiveClassesData = true
      })
    } else {
      this.selectedProduct = undefined
    }
  }
}
