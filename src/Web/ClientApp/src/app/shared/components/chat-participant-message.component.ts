import { Component, Input, OnInit, ViewEncapsulation } from '@angular/core'
import * as moment from 'moment'
import { ChatClient } from '../api.generated.clients'
import { GlobalVariablesService } from '../services/global-variables.service'
import { saveAs as importedSaveAs } from 'file-saver'

@Component({
  selector: 'app-chat-participant-message',
  template: `
    <div class="participant grid gap-2 w-5/6">
      <div [ngSwitch]="type">
        <div *ngSwitchCase="0">
          <div *ngIf="groupedChat" class="participant-info flex items-center gap-2 mb-2">
            <img
              class="w-12 h-12 rounded-full"
              [src]="userAvatar ? '/profile-pictures/' + userAvatar : defaultAvatar"
              alt=""
            />
            <p class="font-semibold">{{ userName }}</p>
          </div>
          <div class="message p-3 bg-gray-300 rounded-lg">
            <p>{{ text }}</p>
            <p class="message-date text-right">{{ formatedDate }}</p>
          </div>
        </div>
        <div *ngSwitchCase="1" class="attachment-download flex items-center gap-2">
          <p-button
            [pTooltip]="file.fileName"
            [label]="file.fileName"
            tooltipPosition="right"
            icon="pi pi-arrow-circle-down"
            class="download-button"
            (click)="downloadFile(file.path)"
          ></p-button>
        </div>
      </div>
    </div>
  `,
  styles: [
    `
      .attachment-download .download-button .p-button.p-ripple {
        background: white;
      }
      .attachment-download .download-button .p-button.p-ripple {
        border: none;
        padding: 0;
      }
      .attachment-download .download-button .p-button.p-ripple .p-button-icon {
        font-size: 1.5rem;
        color: gray;
      }
      .attachment-download .download-button .p-button.p-ripple .p-button-label {
        font-size: 1.5rem;
        max-width: 16rem;
        width: auto;
        white-space: nowrap;
        text-overflow: ellipsis;
        overflow: hidden;
        color: gray;
      }
    `,
  ],
  encapsulation: ViewEncapsulation.None,
})
export class ChatParticipantMessageComponent implements OnInit {
  @Input() text: string
  @Input() date: Date
  @Input() type: number
  @Input() userId: string
  @Input() file: any
  @Input() roomId: string
  @Input() allUsersData: any
  userName: string
  userAvatar: string
  formatedDate: string
  groupedChat: boolean
  defaultAvatar = 'assets/images/chat-default-image.png'

  constructor(private globalVariables: GlobalVariablesService, private chatClient: ChatClient) {}

  downloadFile(url) {
    this.chatClient.download(this.roomId, url).subscribe((data: any) => {
      importedSaveAs(data, this.file.fileName)
    })
  }

  ngOnInit(): void {
    this.formatedDate = moment(this.date).lang('en').format('h:mm a')

    this.groupedChat = this.globalVariables.groupParticipantMessages
    this.globalVariables.groupParticipantMessages = false

    if (!this.globalVariables.roomId) {
      this.globalVariables.roomId = this.roomId
      this.groupedChat = true
    } else if (this.globalVariables.roomId != this.roomId) {
      this.globalVariables.previousUser = this.userId
      this.globalVariables.roomId = this.roomId
      this.groupedChat = true
    } else {
      if (!this.globalVariables.previousUser) {
        this.globalVariables.previousUser = this.userId
        this.groupedChat = true
      } else if (this.globalVariables.previousUser != this.userId) {
        this.groupedChat = true
        this.globalVariables.previousUser = this.userId
      }
    }

    this.allUsersData.map((item) => {
      if (item.userId == this.userId) {
        this.userName = item.fullName
        this.userAvatar = item.userAvater
      }
    })
  }
}
