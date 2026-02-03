import { Component, Input, OnInit } from '@angular/core'
import * as moment from 'moment'

@Component({
  selector: 'app-live-classes-item',
  template: `
    <div class="flex p-4">
      <div class="image flex items-center">
        <img class="w-12 h-12" src="assets/images/ico-classes.svg" alt="" />
      </div>
      <div class="information mx-6">
        <div>
          <span class="text-xl font-thin" i18n>Live class</span>
          <span>:</span>
          <span class="text-xl font-thin"> {{ data.name }}</span>
        </div>
        <div>
          <span class="text-xl font-thin" i18n>Start Date</span>
          <span>:</span>
          <span class="text-xl font-thin"> {{ startDate }}</span>
        </div>
        <div>
          <span class="text-xl font-thin" i18n>End Date</span>
          <span>:</span>
          <span class="text-xl font-thin"> {{ endDate }}</span>
        </div>
      </div>
      <div *ngIf="canViewActivity" class="view">
        <a [href]="data.meetingUrl" target="_blank">
          <span class="text-xl text-blue-700" i18n>View Activity</span>
        </a>
      </div>
    </div>
  `,
})
export class LiveClassesItemController implements OnInit {
  @Input() data: any
  @Input() canViewActivity = true
  startDate: string
  endDate: string

  constructor() {}
  ngOnInit(): void {
    this.startDate = moment(this.data.startDate).lang('en').format('MMM Do YYYY, h:mm:ss a')
    this.endDate = moment(this.data.endDate).lang('en').format('MMM Do YYYY, h:mm:ss a')
  }
}
