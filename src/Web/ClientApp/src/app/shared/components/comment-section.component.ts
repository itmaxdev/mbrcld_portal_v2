import * as moment from 'moment'
import { Component, Input, OnInit, ViewEncapsulation } from '@angular/core'

@Component({
  selector: 'app-comment-section',
  template: `
    <div class="pl-6 pr-3 pt-2 bg-gray-100 rounded-lg grid gap-2 comment-container">
      <div class="p-grid">
        <div class="p-col-3 flex gap-2">
          <div class="flex align-center">
            <img class="w-10 h-10 rounded-full" src="assets/images/avatar.png" />
            <div>
              <div class="pl-4">
                <p class="text-gray-700 text-lg font-semibold" i18n>{{ userName }}</p>
                <p dir="ltr" class="text-gray-700 text-sm" i18n>{{ date }}</p>
              </div>
            </div>
          </div>
        </div>
        <div class="p-col-9 flex p-flex-column justify-between">
          <p class="text-lg font-medium text-gray-800" i18n>{{ comment }}</p>
        </div>
      </div>
    </div>
  `,
  styles: [
    `
      .comment-container {
        border: 1px solid #dedede;
        background-color: #f4f4f2;
        border-radius: 0.75rem;
      }
    `,
  ],
  encapsulation: ViewEncapsulation.None,
})
export class CommentSectionComponent implements OnInit {
  @Input() date: any
  @Input() id: string
  @Input() comment: string
  @Input() userName: string

  constructor() {}

  ngOnInit(): void {
    this.date = moment(this.date).lang('en').fromNow()
  }
}
