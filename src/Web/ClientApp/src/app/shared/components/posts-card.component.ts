import { Component, Input, OnInit, LOCALE_ID, Inject } from '@angular/core'
import { MenuItem } from 'primeng/api'

@Component({
  selector: 'app-posts-card',
  template: `
    <div class="border rounded-lg border-gray-400 grid gap-2 bg-white">
      <div class="flex justify-between items-center pl-4 pr-4 pt-4">
        <div class="grid grid-cols-6 gap-8">
          <app-profile-image [small]="true" [profileImage]="profileImage"></app-profile-image>
          <div class="profile-info col-span-5">
            <h1 class="text-2xl text-blue-500">{{ name }}</h1>
            <p class="text-gray-500">{{ writtenByName }}</p>
            <p dir="ltr" class="text-gray-500 text-right">{{ date }}</p>
          </div>
        </div>
        <div *ngIf="isMyArticles">
          <button
            type="button"
            pButton
            pRipple
            class="bg-white h-8"
            icon="none"
            (click)="menu.toggle($event)"
          >
            <i class="pi pi-ellipsis-h pi-2 text-gray-600" style="font-size: 2rem;"></i>
          </button>
          <p-menu #menu [popup]="true" [model]="items"></p-menu>
        </div>
      </div>
      <div class="pl-4 pr-4">
        <a [routerLink]="['item/' + id, { type: type }]">
          <p class="text-gray-500 text-lg ql-editor" [innerHtml]="description"></p>
        </a>
      </div>
      <div [class]="content ? 'grid grid-cols-2 gap-4' : null">
        <ng-container [ngSwitch]="type">
          <ng-container *ngSwitchCase="2">
            <ng-container [ngSwitch]="postType">
              <ng-container *ngSwitchCase="0">
                <a [routerLink]="['item/' + id, { type: type }]">
                  <img
                    class="article-image w-full object-cover"
                    [src]="imgUrl"
                    onerror="javascript:this.remove()"
                    style="object-fit: contain;"
                    alt=""
                  />
                  <div *ngIf="content">
                    <h1 class="text-2xl mb-2">{{ caption }}</h1>
                    <p class="text-gray-600">
                      {{ content }}
                    </p>
                  </div>
                </a>
              </ng-container>
              <ng-container *ngSwitchCase="1">
                <app-video-content [videoUrl]="videoUrl"></app-video-content>
              </ng-container>
              <ng-container *ngSwitchDefault=""> </ng-container>
            </ng-container>
          </ng-container>
          <ng-container *ngSwitchDefault="">
            <a [routerLink]="['item/' + id, { type: type }]">
              <img
                class="article-image w-full object-cover"
                [src]="imgUrl"
                onerror="javascript:this.remove()"
                style="object-fit: contain;"
                alt=""
              />
              <div *ngIf="content">
                <h1 class="text-2xl mb-2">{{ caption }}</h1>
                <p class="text-gray-600">
                  {{ content }}
                </p>
              </div>
            </a>
          </ng-container>
        </ng-container>
      </div>
      <div class="pl-4 pr-4 pb-2">
        <app-social-panel
          [id]="id"
          [liked]="liked"
          [likesCount]="likes"
          [commentsCount]="comments"
          [sharesCount]="shares"
          [type]="2"
        ></app-social-panel>
      </div>
    </div>
  `,
  styles: [
    `
      .p-button {
        background: white;
        color: gray;
        border: none !important;
      }

      .p-button:focus {
        box-shadow: none !important;
      }

      .p-button:hover {
        background: none !important;
        color: gray;
      }

      .p-button-icon-only span {
        font-size: 4rem !important;
      }

      .article-image {
        height: 290px;
      }
      .text-right {
        text-align: right;
      }
    `,
  ],
})
export class PostsCardComponent implements OnInit {
  items: MenuItem[]
  userName: string
  @Input() name: string
  @Input() id: string
  @Input() date: Date
  @Input() type: number
  @Input() liked: boolean
  @Input() description: string
  @Input() imgUrl: string
  @Input() writtenByName: string
  @Input() caption: string
  @Input() content: string
  @Input() profileImage: string
  @Input() likes: number
  @Input() comments: number
  @Input() shares: number
  @Input() articleStatus: number
  @Input() isMyArticles: boolean
  @Input() postType: number
  @Input() videoUrl: string

  constructor(@Inject(LOCALE_ID) public locale: string) {}

  ngOnInit(): void {
    if (!this.profileImage) {
      this.profileImage = 'assets/images/program/Program_Home_page.jpg'
    }
    this.items = [
      {
        items: [
          {
            label: this.locale === 'en' ? 'Edit' : 'تعديل',
            icon: 'pi pi-pencil',
            routerLink: this.articleStatus == 2 ? null : ['edit/' + this.id],
            disabled: this.articleStatus == 2,
          },
        ],
      },
    ]
  }
}
