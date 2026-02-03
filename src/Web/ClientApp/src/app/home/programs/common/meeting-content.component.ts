import * as moment from 'moment'
import { Component, Input, OnInit } from '@angular/core'

@Component({
  selector: 'app-meeting-content',
  template: `
    <div>
      <div class="border border-gray-300 p-8 grid gap-4">
        <div class="header flex items-center">
          <img class="mr-4 w-12 h-12" src="assets/images/ico-video-camera.svg" alt="" />
          <p class="text-blue-800 text-3xl">{{ name }}</p>
        </div>
        <div class="link">
          <h1 class="text-blue-800 text-xl" i18n>Meeting Link</h1>
          <a class="pl-4 text-gray-600 text-xl hover:underline" target="_blank" [href]="link">{{
            link
          }}</a>
        </div>
        <div class="date flex items-center">
          <img class="mr-4 w-8 h-8" src="assets/images/ico-clock.svg" alt="" />
          <p class="text-blue-800 text-lg">
            <span i18n>
              Start
            </span>
            <span>:</span>
            <span> {{ date }}</span>
          </p>
        </div>
      </div>
    </div>
  `,
})
export class MeetingContentComponent implements OnInit {
  @Input() name: string
  @Input() link: string
  @Input() date: string

  ngOnInit(): void {
    this.date = moment(this.date).lang('en').format('MM/DD/YY hh:mm')
  }
}
