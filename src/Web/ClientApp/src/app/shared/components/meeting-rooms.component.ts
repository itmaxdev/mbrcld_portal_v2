import { Component, Input, OnInit, ViewEncapsulation, LOCALE_ID, Inject } from '@angular/core'
import { MessageService } from 'primeng/api'
import { ChatClient } from '../api.generated.clients'

@Component({
  selector: 'app-meeting-rooms',
  template: `
    <div class="meetingRoomCard">
      <img *ngIf="room.image" [src]="room.image" class="cardImage" />
      <div class="p-3 row-gap-3 flex flex-col">
        <div class="pl-2 pr-2">
          <h4 *ngIf="room.name" class="font-semibold text-lg">
            {{ room.name }}
          </h4>
          <p *ngIf="room.description" class="text-content text-gray-700 text-sm">
            {{ room.description }}
          </p>
        </div>
        <div>
          <button
            type="button"
            [label]="buttonLabel"
            pButton
            class="w-full h-10"
            (click)="alreadyFollowing ? removeUserFromRoom() : addUserToRoom()"
          ></button>
        </div>
      </div>
    </div>
    <p-toast></p-toast>
  `,
  styles: [
    `
      .meetingRoomCard {
        border: 1px solid #e2e8f0;
        border-radius: 12px;
        max-width: 245px;
      }

      .cardImage {
        width: 245px;
        height: 153px;
        border-radius: 12px 12px 0px 0px;
      }

      .text-content {
        overflow: hidden;
        text-overflow: ellipsis;
        width: 100%;
        display: -webkit-box;
        -webkit-line-clamp: 2;
        -webkit-box-orient: vertical;
        height: 40px;
      }
    `,
  ],
  providers: [MessageService],
  encapsulation: ViewEncapsulation.None,
})
export class MeetingRoomsComponent implements OnInit {
  @Input() room: any
  @Input() userId: string

  public buttonLabel: string
  public alreadyFollowing = true

  constructor(private chatClient: ChatClient, @Inject(LOCALE_ID) private locale: string,private messageService: MessageService) {}

  isExistInChat() {
    const participant = this.room.participants.find((p: any) => p.userId === this.userId)
    if (this.locale == 'en') {
      if (participant) {
        this.alreadyFollowing = true
        this.buttonLabel = 'Unfollow'
      } else {
        this.alreadyFollowing = false
        this.buttonLabel = 'Follow'
      }
    } else {
      if (participant) {
        this.alreadyFollowing = true
        this.buttonLabel = 'الغاء المتابعة'
      } else {
        this.alreadyFollowing = false
        this.buttonLabel = 'اتبع'
      }
    }
  }

  addUserToRoom() {
    this.chatClient.roomPost(this.userId, this.room.id).subscribe(() => {
      this.alreadyFollowing = true
      if (this.locale == 'en') {
        this.buttonLabel = 'Unfollow';
        this.messageService.add({
          severity: 'success',
          life: 10000,
          closable: true,
          summary: 'Topic followed',
          detail: 'To start discussing the followed topics, please click on the chat button',
        })
      } else {
        this.buttonLabel = 'الغاء المتابعة';
        this.messageService.add({
          severity: 'success',
          life: 10000,
          closable: true,
          summary: 'الموضوع يتبع',
          detail: 'يرجى الضغط على زر المحادثة للبدء في مناقشة الموضوعات المتبعة',
        });
      }
    })
  }

  removeUserFromRoom() {
    this.chatClient.roomDelete(this.userId, this.room.id).subscribe(() => {
      this.alreadyFollowing = false
      if (this.locale == 'en') {
        this.buttonLabel = 'Follow'
      } else {
        this.buttonLabel = 'اتبع'
      }
    })
  }

  ngOnInit() {
    this.isExistInChat()
  }
}
