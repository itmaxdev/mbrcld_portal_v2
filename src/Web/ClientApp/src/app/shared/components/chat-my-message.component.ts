import { Component, Input, OnInit, ViewEncapsulation } from '@angular/core'
import * as moment from 'moment'
import { ChatClient } from '../api.generated.clients'
import { GlobalVariablesService } from '../services/global-variables.service'
import { saveAs as importedSaveAs } from 'file-saver'

@Component({
  selector: 'app-chat-my-message',
  template: `
    <div class="my-message w-full flex justify-end">
      <div [ngSwitch]="type" class="w-5/6">
        <div *ngSwitchCase="0" class="message-block p-3 rounded-lg text-white">
          <p>{{ text }}</p>
          <p class="message-date text-right">{{ formatedDate }}</p>
        </div>
        <div
          *ngSwitchCase="1"
          class="attachment-download p-3 rounded-lg text-white flex justify-end"
        >
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
      .message-block {
        background: #0071be;
      }
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
export class ChatMyMessageComponent implements OnInit {
  @Input() date: Date
  @Input() text: string
  @Input() type: number
  @Input() file: any
  @Input() roomId: string
  formatedDate: string

  constructor(private globalVariables: GlobalVariablesService, private chatClient: ChatClient) {}

  downloadFile(url) {
    this.chatClient.download(this.roomId, url).subscribe((data: any) => {
      importedSaveAs(data, this.file.fileName)
    })
  }

  ngOnInit(): void {
    this.globalVariables.groupParticipantMessages = true

    this.formatedDate = moment(this.date).lang('en').format('h:mm a')
  }
}
