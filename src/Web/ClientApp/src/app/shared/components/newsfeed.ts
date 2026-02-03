import { Component, OnInit, Input } from '@angular/core'
import { ListUserNewsFeedsViewModel } from 'src/app/shared/api.generated.clients'
import { ActivatedRoute } from '@angular/router'

@Component({
  selector: 'app-newsfeed',
  template: `
    <div class="mt-4">
      <div *ngIf="isExistSections; else sections" class="mt-12">
        <p class="text-sm text-center text-gray-700" i18n>No SECTIONS Added</p>
      </div>
      <ng-template #sections>
        <div>
          <div class="pb-4" [ngSwitch]="item.type">
            <div *ngSwitchCase="1">
              <div class="container">
                <div class="h-24 w-full bg-gray-200 flex header">
                  <img class="img" [src]="item.profilePictureUrl" />
                  <div class="newsItemHeader">
                    <p class="name">{{ item.instructorName }} | {{ item.moduleName }}</p>
                    <p class="date" *ngIf="role == '4'">{{ item.publishDate }}</p>
                  </div>
                </div>
                <div>
                  <h1 class="text-xl font-semibold text-blue-800 text">{{ item.name }}</h1>
                  <div
                    [innerHTML]="item.text"
                    id="textarea"
                    class="content-text content-block text-lg text ql-editor "
                    dir="ltr"
                    [ngStyle]="{ '-webkit-line-clamp': lessText ? '100000000' : '5' }"
                  ></div>
                </div>
              </div>
              <div>
                <button class="btn" *ngIf="showMoreButtonVisible" (click)="showMore()">
                  {{ this.buttonText }}
                </button>
                <div class="pl-4 pr-4 pb-2">
                  <app-newsfeed-social-panel
                    [id]="item.id"
                    [liked]="item.liked"
                    [likesCount]="item.likes"
                    [commentsCount]="commentsCount"
                    [sharesCount]="shares"
                    [type]="2"
                  ></app-newsfeed-social-panel>
                </div>
              </div>
            </div>
            <div *ngSwitchCase="2">
              <div class="container">
                <div class="h-24 w-full bg-gray-200 flex header">
                  <img class="img" [src]="item.profilePictureUrl" />
                  <p class="name">{{ item.instructorName }} |</p>
                  <p class="name">{{ item.moduleName }}</p>
                  <p class="date">{{ item.publishDate }}</p>
                </div>
                <div class="video">
                  <app-video-content [name]="item.name" [videoUrl]="item.url"></app-video-content>
                </div>
              </div>
              <div>
                <div class="pl-4 pr-4 pb-2 panel">
                  <app-newsfeed-social-panel
                    [id]="item.id"
                    [liked]="item.liked"
                    [likesCount]="item.likes"
                    [commentsCount]="commentsCount"
                    [sharesCount]="shares"
                    [type]="2"
                  ></app-newsfeed-social-panel>
                </div>
              </div>
            </div>
            <div *ngSwitchCase="3">
              <div class="container">
                <div class="h-24 w-full bg-gray-200 flex header">
                  <img class="img" [src]="item.profilePictureUrl" />
                  <p class="name">{{ item.instructorName }} |</p>
                  <p class="name">{{ item.moduleName }}</p>
                  <p class="date">{{ item.publishDate }}</p>
                </div>
                <app-document-content
                  [name]="item.name"
                  [documentUrl]="item.documentUrl"
                ></app-document-content>
              </div>
              <div>
                <div class="pl-4 pr-4 pb-2">
                  <app-newsfeed-social-panel
                    [id]="item.id"
                    [liked]="item.liked"
                    [likesCount]="item.likes"
                    [commentsCount]="commentsCount"
                    [sharesCount]="shares"
                    [type]="2"
                  ></app-newsfeed-social-panel>
                </div>
              </div>
            </div>
            <div *ngSwitchCase="4">
              <div class="container">
                <div class="h-24 w-full bg-gray-200 flex header">
                  <img class="img" [src]="item.profilePictureUrl" />
                  <p class="name">{{ item.instructorName }} |</p>
                  <p class="name">{{ item.moduleName }}</p>
                  <p class="date">{{ item.publishDate }}</p>
                </div>
                <app-meeting-content
                  [name]="item.name"
                  [link]="item.url"
                  [date]="item.startDate"
                ></app-meeting-content>
              </div>
              <div>
                <div class="pl-4 pr-4 pb-2">
                  <app-newsfeed-social-panel
                    [id]="item.id"
                    [liked]="item.liked"
                    [likesCount]="item.likes"
                    [commentsCount]="commentsCount"
                    [sharesCount]="shares"
                    [type]="2"
                  ></app-newsfeed-social-panel>
                </div>
              </div>
            </div>
            <div *ngSwitchCase="5">
              <div class="container">
                <div class="h-24 w-full bg-gray-200 flex header">
                  <img class="img" [src]="item.profilePictureUrl" />
                  <div class="newsItemHeader">
                    <p class="name">{{ item.instructorName }} | {{ item.moduleName }}</p>
                    <p class="date">{{ item.publishDate }}</p>
                  </div>
                </div>

                <div class="relative flex justify-center ">
                  <img src="assets/images/stick-note.png" class="relative w-full z-0 stickImg " />
                  <div
                    [innerHTML]="item.text"
                    class="content-text content-block text-lg text z-1  absolute top-0 left-0  stickText ql-editor"
                    dir="ltr"
                    [ngStyle]="{ '-webkit-line-clamp': '15' }"
                  ></div>
                </div>

                <!-- <app-text-content [name]="item.name" [text]="item.text"></app-text-content -->
              </div>
              <div>
                <div class="pl-4 pr-4 pb-2">
                  <app-newsfeed-social-panel
                    [id]="item.id"
                    [liked]="item.liked"
                    [likesCount]="item.likes"
                    [commentsCount]="commentsCount"
                    [sharesCount]="shares"
                    [type]="2"
                  ></app-newsfeed-social-panel>
                </div>
              </div>
            </div>
          </div>
        </div>
      </ng-template>
    </div>

    <ng-template #loading>
      <div class="flex flex-grow justify-center items-center py-8">
        <p-progressSpinner></p-progressSpinner>
      </div>
    </ng-template>
  `,
  styles: [
    `
      .content-block ol,
      ul {
        margin-block-start: 1em;
        margin-block-end: 1em;
        padding-inline-start: 40px;
      }
      .content-block ol {
        list-style: decimal !important;
      }
      .content-block ul {
        list-style: disc !important;
      }

      .header {
        margin-left: 0%;
        margin-top: 30px;
      }

      .panel {
        margin-left: -1%;
      }

      .content-text {
        color: #718096;
        width: 100%;
      }

      .stickImg {
        height: 31rem;
        width: 31rem;
      }
      .container {
        border: none;
        width: 770px;
      }

      .date {
        margin-left: -325px;
        margin-top: 50px;
      }

      .text {
        margin-left: 25px;
        margin-top: 10px;
      }

      .img {
        border-radius: 50%;
        height: 50px;
        width: 50px;
        margin: 20px;
      }

      .name {
        margin-left: 5px;
        margin-top: 30px;
      }

      .footer {
        border: none;
        width: 770px;
        height: 80px;
      }

      .video {
        margin-top: 10px;
      }

      .btn {
        outline: none;
        display: flex;
        margin-bottom: 20px;
        margin-top: 1rem;
        margin-left: 2.5rem;
        display: flex;
        justify-content: center;
        background-color: #dde1e3;
      }
      .stickText {
        margin-left: 28%;
        width: 44%;
        margin-top: 30px;
      }
      .newsItemHeader {
        margin-top: 24px;
        display: flex;
        flex-direction: column;
      }
      .newsItemHeader > p {
        margin: 0;
      }
    `,
  ],
})
export class NewsfeedComponent implements OnInit {
  @Input() comments: number
  @Input() item: any
  id: string
  newsFeedList: ListUserNewsFeedsViewModel[]
  isnewsFeedListReady = false
  isExistSections = false
  ready = false
  buttonText: string
  role: string
  lessText: boolean
  likeCount: number
  profileInfo: any
  commentsCount: number
  commentsData: any[]
  type: number
  news: ListUserNewsFeedsViewModel[]
  publishDate: string
  numberOfRows: number
  showMoreButtonVisible = true

  constructor(private route: ActivatedRoute) {}

  ngOnInit() {
    if (this.item.text != null) {
      const textLength = this.item.text.split(' ').length
      if (textLength <= 80) {
        this.showMoreButtonVisible = false
      }
    }
    this.role = JSON.parse(localStorage.getItem('profile_info')).role
    this.id = this.route.snapshot.paramMap.get('modulesId')
    this.lessText = false
    this.buttonText = 'Show more'
  }

  showMore() {
    if (this.lessText) {
      this.buttonText = 'Show more'
      this.lessText = false
    } else {
      this.buttonText = 'Show less'
      this.lessText = true
    }
  }
}
