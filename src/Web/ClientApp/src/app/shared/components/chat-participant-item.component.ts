import { Component, Input, OnInit, Output, LOCALE_ID, Inject } from '@angular/core'
import { EventEmitter } from '@angular/core'
import * as moment from 'moment'

@Component({
  selector: 'app-chat-participant-item',
  template: `
    <div
      class="chat-participants-item sm:grid sm:grid-cols-9 border-b mt-2 p-2 cursor-pointer hover:bg-gray-100"
      [ngClass]="{ active: active }"
      (click)="openMessage()"
    >
      <div class="participant-image flex col-span-2">
        <img
          class="w-12 h-12 rounded-full"
          [src]="image ? image : url ? '/profile-pictures/' + url : defaultImage"
          alt=""
        />
        <div
          *ngIf="unreadMessagesCount > 0"
          class="notification-circle w-5 h-5 flex justify-center items-center rounded-full bg-green-400"
        >
          <p class="text-white">{{ unreadMessagesCount }}</p>
        </div>
      </div>
      <div class="col-span-5">
        <p class="participant-name font-semibold">{{ name ? name : defaultRoomName }}</p>
        <p class="last-message text-gray-700">
          {{ message ? message : defaultLastMessage }}
        </p>
      </div>
      <div class="message-date flex justify-end col-span-2">
        <div *ngIf="displayDate">
          <p class="text-gray-700">{{ displayDate }}</p>
        </div>
      </div>
    </div>
  `,
  styles: [
    `
      .last-message {
        height: 20px;
        white-space: nowrap;
        width: 100%;
        overflow: hidden;
        text-overflow: ellipsis;
      }

      .active {
        border-left: 2px solid #0071be;
        background: #f8f8f8;
      }
      .notification-circle {
        position: relative;
        right: 10px;
        bottom: 2px;
      }
    `,
  ],
})
export class ChatParticipantItemComponent implements OnInit {
  @Input() id: string
  @Input() date: Date
  @Input() url: string
  @Input() name: string
  @Input() roomId: string
  @Input() message: string
  @Input() image: string
  @Input() active: boolean
  @Input() currentRoomId: string
  @Input() unreadMessagesCount: number
  @Output() changeParticipant: EventEmitter<any> = new EventEmitter()
  displayDate: string
  defaultLastMessage = 'No messages yet'
  defaultRoomName = 'No Name'
  defaultImage = 'assets/images/chat-default-image.png'

  constructor(@Inject(LOCALE_ID) private locale: string) {}

  openMessage() {
    this.changeParticipant.emit(this.id)
  }

  ngOnInit(): void {
    if (this.locale == 'en') {
      this.defaultLastMessage = 'No messages yet'
    } else {
      this.defaultLastMessage = 'لا يوجد محادثات'
    }

    if (this.date) {
      this.displayDate = moment(this.date).lang('en').format('h:mm a')
    } else {
      this.displayDate = null
    }

    if (this.currentRoomId == this.roomId) {
      this.unreadMessagesCount = 0
    }
  }
}
