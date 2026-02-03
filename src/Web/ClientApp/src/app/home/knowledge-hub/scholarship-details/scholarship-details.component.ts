import { ActivatedRoute } from '@angular/router'
import { Component, OnInit } from '@angular/core'
import { Location } from '@angular/common'
import { ScholarshipsClient } from 'src/app/shared/api.generated.clients'

@Component({
  selector: 'app-scholarship-details',
  templateUrl: './scholarship-details.component.html',
  styleUrls: ['./scholarship-details.component.scss'],
})
export class ScholarshipDetailsComponent implements OnInit {
  buttonLabels = []
  ready = false
  busy = false
  item: any
  id: string
  fromDate: Date
  toDate: Date
  status: any
  constructor(
    private _location: Location,
    private route: ActivatedRoute,
    private client: ScholarshipsClient
  ) {}

  goBack() {
    this._location.back()
  }

  ngOnInit() {
    this.id = this.route.snapshot.paramMap.get('id')
    this.buttonLabels = [
      $localize`:@@register:Register`,
      $localize`:@@alreadyRegistered:Already Registered`,
    ]
    this.fetchContent()
  }

  async registerUserToScholarshipt() {
    if (this.busy) return
    this.busy = true
    await this.client.registerScholarship(this.id)
    await this.fetchContent()
    this.busy = false
  }

  async fetchContent() {
    const resp = await this.client.getScholarshipDetails(this.id)
    this.ready = true
    if (resp === false) {
      return
    }
    this.item = resp
    this.fromDate = new Date(this.item.fromDate)
    this.toDate = new Date(this.item.toDate)

    switch (this.item.statusCode) {
      case 'UnderReview':
        this.status = { statusName: 'Under Review', statusColor: 'blue' }
        break;
      case 'Accepted':
        this.status = { statusName: 'Accepted', statusColor: 'green' }
        break;
      case 'Rejected':
        this.status = { statusName: 'Under Review', statusColor: 'red' }
        break;
    }
  }
}
