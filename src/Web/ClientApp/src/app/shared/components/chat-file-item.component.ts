import { Component, Input, OnInit } from '@angular/core'
import { saveAs as importedSaveAs } from 'file-saver'
import { ChatClient } from '../api.generated.clients'

@Component({
  selector: 'app-chat-file-item',
  template: `
    <div class="files-block flex items-center justify-between">
      <p class="text-2xl">{{ name }}</p>
      <p-button (click)="downloadFile()" label="Download"></p-button>
    </div>
  `,
  styles: [
    `
      .files-block p {
        max-width: 24rem;
        width: auto;
        white-space: nowrap;
        text-overflow: ellipsis;
        overflow: hidden;
      }
    `,
  ],
})
export class ChatFileItemComponent implements OnInit {
  @Input() name: string
  @Input() path: string
  @Input() roomId: string

  constructor(private chatClient: ChatClient) {}

  downloadFile() {
    this.chatClient.download(this.roomId, this.path).subscribe((data: any) => {
      importedSaveAs(data, this.name)
    })
  }

  ngOnInit(): void {}
}
