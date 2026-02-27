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
        <div class="newsfeedCard">
          <div class="inner">
            <div class="topBox">
              <div class="imgBox">
                <picture>
                  <img
                    [src]="item.profilePictureUrl"
                    alt=""
                    width="100"
                    height="100"
                    loading="lazy"
                  />
                </picture>
              </div>
              <div class="c">
                <div class="title">{{ item.instructorName }} | {{ item.moduleName }}</div>
                <div class="dateBox" *ngIf="item.type != 1 || role == '4'">
                  {{ item.publishDate }}
                </div>
              </div>
            </div>

            <div class="contentBox" [ngSwitch]="item.type">
              <!-- Case 1: Text -->
              <div *ngSwitchCase="1">
                <div class="qTitle" *ngIf="item.name">{{ item.name }}</div>
                <div class="textBox" [ngClass]="{ sm: !lessText }">
                  <div
                    [innerHTML]="item.text"
                    id="textarea"
                    class="content-text content-block text-lg text ql-editor"
                    dir="ltr"
                    [ngStyle]="{ '-webkit-line-clamp': lessText ? '100000000' : '5' }"
                  ></div>
                </div>
                <button class="btn" *ngIf="showMoreButtonVisible" (click)="showMore()">
                  {{ this.buttonText }}
                </button>
              </div>

              <!-- Case 2: Video -->
              <div *ngSwitchCase="2">
                <div class="qTitle" *ngIf="item.name">{{ item.name }}</div>
                <div class="video">
                  <app-video-content [name]="item.name" [videoUrl]="item.url"></app-video-content>
                </div>
              </div>

              <!-- Case 3: Document -->
              <div *ngSwitchCase="3">
                <div class="qTitle" *ngIf="item.name">{{ item.name }}</div>
                <app-document-content
                  [name]="item.name"
                  [documentUrl]="item.documentUrl"
                ></app-document-content>
              </div>

              <!-- Case 4: Meeting -->
              <div *ngSwitchCase="4">
                <div class="qTitle" *ngIf="item.name">{{ item.name }}</div>
                <app-meeting-content
                  [name]="item.name"
                  [link]="item.url"
                  [date]="item.startDate"
                ></app-meeting-content>
              </div>

              <!-- Case 5: Sticky Note -->
              <div *ngSwitchCase="5">
                <div class="relative flex justify-center ">
                  <img src="assets/images/stick-note.png" class="relative w-full z-0 stickImg " />
                  <div
                    [innerHTML]="item.text"
                    class="content-text content-block text-lg text z-1  absolute top-0 left-0  stickText ql-editor"
                    dir="ltr"
                    [ngStyle]="{ '-webkit-line-clamp': '15' }"
                  ></div>
                </div>
              </div>
            </div>

            <div class="pb-2">
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

      .content-text {
        color: #718096;
        width: 100%;
      }

      .stickImg {
        height: 31rem;
        width: 31rem;
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
        padding: 5px 15px;
        border-radius: 5px;
      }
      .stickText {
        margin-left: 28%;
        width: 44%;
        margin-top: 30px;
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
