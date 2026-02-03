import { Component, Input, OnInit, ViewEncapsulation } from '@angular/core'
import * as moment from 'moment'

@Component({
  selector: 'app-project-cohort-card',
  template: ` <div
    class="border rounded-lg border-gray-400 flex gap-2 p-2 justify-between bg-white"
  >
    <div class="grid gap-2">
      <h1 class="text-caption text-2xl pb-2 sm:text-xl sm:pb-0 text-black">{{ data.topic }}</h1>
      <p class="info-text text-lg text-gray">{{ data.description }}</p>
      <p class="text-gray-500 m-0">
        <span i18n>Start Date</span>
        <span>:</span>
        <span> {{ startDate }}</span>
      </p>
      <p class="text-gray-500">
        <span>End Date</span>
        <span>:</span>
        <span> {{ endDate }}</span>
      </p>
      <a [href]="data.attachmentUrl" download class="download-button p-2 text-white bg-blue-400"
        >Download Attachment <i class="pi pi-download"></i
      ></a>
    </div>
  </div>`,
  styles: [
    `
      .download-button {
        width: max-content;
      }
      .info-text {
        max-height: 70px;
        overflow: hidden;
        width: 100%;
        display: -webkit-box;
        -webkit-line-clamp: 3;
        -webkit-box-orient: vertical;
      }
    `,
  ],
})
export class ProjectCohortCardComponent implements OnInit {
  public startDate: string
  public endDate: string
  @Input() data: any

  constructor() {}

  ngOnInit() {
    this.startDate = moment(this.data.startDate).lang('en').format('dddd, MMMM Do YYYY, h:mm:ss a')
    this.endDate = moment(this.data.endDate).lang('en').format('dddd, MMMM Do YYYY, h:mm:ss a')
  }
}
