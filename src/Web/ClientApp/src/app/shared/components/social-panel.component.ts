import { Component, Input, OnInit, ViewEncapsulation } from '@angular/core'
import { tap } from 'rxjs/operators'
import { PanHistoriesClient } from 'src/app/shared/api.generated.clients'

@Component({
  selector: 'app-social-panel',
  template: `
    <div>
      <div class="articleTools">
        <!-- LIKE -->
        <div class="tool likeBox" (click)="setLike()">
          <div class="iconBox" [ngStyle]="{ color: isLiked ? 'red' : 'currentcolor' }">
            <svg width="24" height="24" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
              <path
                d="M20.8401 4.60999C20.3294 4.099 19.7229 3.69364 19.0555 3.41708C18.388 3.14052 17.6726 2.99817 16.9501 2.99817C16.2276 2.99817 15.5122 3.14052 14.8448 3.41708C14.1773 3.69364 13.5709 4.099 13.0601 4.60999L12.0001 5.66999L10.9401 4.60999C9.90843 3.5783 8.50915 2.9987 7.05012 2.9987C5.59109 2.9987 4.19181 3.5783 3.16012 4.60999C2.12843 5.64169 1.54883 7.04096 1.54883 8.49999C1.54883 9.95903 2.12843 11.3583 3.16012 12.39L4.22012 13.45L12.0001 21.23L19.7801 13.45L20.8401 12.39C21.3511 11.8792 21.7565 11.2728 22.033 10.6053C22.3096 9.93789 22.4519 9.22248 22.4519 8.49999C22.4519 7.77751 22.3096 7.0621 22.033 6.39464C21.7565 5.72718 21.3511 5.12075 20.8401 4.60999Z"
                fill="currentcolor"
              />
            </svg>
          </div>
          <span>{{ likesCount }} likes</span>
        </div>

        <!-- COMMENT -->
        <div *ngIf="type == 2" class="tool commentBox" (click)="addCommentState = true">
          <div class="iconBox">
            <svg width="24" height="24" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
              <rect width="24" height="24" fill="white" />
              <path
                d="M21 11.5C21.0034 12.8199 20.6951 14.1219 20.1 15.3C19.3944 16.7118 18.3098 17.8992 16.9674 18.7293C15.6251 19.5594 14.0782 19.9994 12.5 20C11.1801 20.0035 9.87812 19.6951 8.7 19.1L3 21L4.9 15.3C4.30493 14.1219 3.99656 12.8199 4 11.5C4.00061 9.92179 4.44061 8.37488 5.27072 7.03258C6.10083 5.69028 7.28825 4.6056 8.7 3.90003C9.87812 3.30496 11.1801 2.99659 12.5 3.00003H13C15.0843 3.11502 17.053 3.99479 18.5291 5.47089C20.0052 6.94699 20.885 8.91568 21 11V11.5Z"
                fill="currentcolor"
              />
            </svg>
          </div>
          <span>{{ commentsCount }} Comments</span>
        </div>
      </div>

      <p-dialog
        [(visible)]="addCommentState"
        [modal]="true"
        [style]="{ 'width': '600px', 'border-radius': '0.75rem', 'overflow': 'hidden' }"
        [closable]="false"
        styleClass="comment-dialog"
      >
        <ng-template pTemplate="header">
          <div
            class="topBox"
            style="width: 100%; display: flex; justify-content: space-between; align-items: center;"
          >
            <div class="title" style="font-size: 1.25rem; font-weight: 600;">Comments</div>
            <button
              type="button"
              class="plusBtn"
              (click)="addCommentState = false"
              style="background: none; border: none; cursor: pointer; padding: 0;"
            >
              <svg
                width="24"
                height="24"
                viewBox="0 0 24 24"
                fill="none"
                xmlns="http://www.w3.org/2000/svg"
              >
                <path
                  d="M18 6L6 18"
                  stroke="currentcolor"
                  stroke-width="2"
                  stroke-linecap="round"
                  stroke-linejoin="round"
                />
                <path
                  d="M6 6L18 18"
                  stroke="currentcolor"
                  stroke-width="2"
                  stroke-linecap="round"
                  stroke-linejoin="round"
                />
              </svg>
            </button>
          </div>
        </ng-template>

        <div class="formBox">
          <form class="form-v3 filledColor">
            <div class="inputBox">
              <div class="input-field normal full">
                <label for="description">Your Comment</label>
                <textarea
                  class="materialize-textarea"
                  [(ngModel)]="newComment"
                  name="newComment"
                  id="description"
                  placeholder="Enter your comment"
                  i18n-placeholder
                ></textarea>
              </div>
              <div class="input-field normal full buttonWrap">
                <button type="button" class="more wAuto" (click)="addComment()">
                  <span>Save</span>
                </button>
              </div>
            </div>
          </form>
        </div>

        <div class="grid gap-2" style="margin-top: 20px;" *ngIf="commentsData?.length > 0">
          <app-comment-section
            *ngFor="let comment of commentsData"
            [id]="comment.id"
            [date]="comment.actionDate"
            [userName]="comment.contactName"
            [comment]="comment.comment"
          ></app-comment-section>
        </div>
      </p-dialog>

      <button
        *ngIf="activeState"
        pButton
        class="p-button-text p-button-secondary p-button-sm"
        icon="pi pi-times"
        (click)="toggle()"
        i18n-label
        label="Hide Comments"
        style="margin-top: 10px;"
      ></button>

      <div *ngIf="activeState" class="grid gap-2" style="margin-top: 10px;">
        <app-comment-section
          *ngFor="let comment of commentsData"
          [id]="comment.id"
          [date]="comment.actionDate"
          [userName]="comment.contactName"
          [comment]="comment.comment"
        ></app-comment-section>
      </div>
    </div>
  `,
  styles: [
    `
      ::ng-deep .p-dialog .p-dialog-content {
        border-bottom-left-radius: 0.75rem !important;
        border-bottom-right-radius: 0.75rem !important;
      }

      ::ng-deep .p-dialog .p-dialog-header {
        border-top-left-radius: 0.75rem !important;
        border-top-right-radius: 0.75rem !important;
      }

      ::ng-deep .p-component-overlay {
        border-radius: 0.75rem !important;
      }

      .buttonWrap {
        display: flex;
        justify-content: flex-end;
      }
    `,
  ],
  encapsulation: ViewEncapsulation.None,
})
export class SocialPanelComponent implements OnInit {
  @Input() type: number
  @Input() liked: boolean
  @Input() id: string
  @Input() likesCount: number
  @Input() sharesCount: number
  @Input() commentsCount: number
  isLiked: boolean
  newComment = ''
  postId: string = undefined
  activeState = false
  addCommentState = false
  articlesId: string = undefined
  commentsData: any[]
  userFullName: string
  comments: string = $localize`Comments`
  likes: string = $localize`Likes`

  activeIconHeart = 'assets/images/ico-heart-active.svg'
  inactiveIconHeart = 'assets/images/ico-heart-inactive.svg'
  likeImg: string

  constructor(private panHistories: PanHistoriesClient) {}

  toggle() {
    this.activeState = !this.activeState
  }

  async addComment() {
    if (this.newComment !== '') {
      const comment: string = this.newComment
      this.commentsData.unshift({
        actionDate: new Date(),
        comment: this.newComment,
        contactName: this.userFullName,
      })
      this.commentsCount++
      this.newComment = ''

      await Promise.all([
        this.panHistories
          .comment(this.id, undefined, comment)
          .pipe(tap(() => {}))
          .toPromise(),
      ])
    }
  }

  setLike() {
    this.isLiked ? (this.likeImg = this.inactiveIconHeart) : (this.likeImg = this.activeIconHeart)
    this.isLiked ? this.likesCount-- : this.likesCount++
    this.isLiked = !this.isLiked

    if (this.type) {
      switch (this.type) {
        case 1:
          this.articlesId = this.id
          break
        case 2:
          this.postId = this.id
          break
      }
    }

    this.panHistories
      .like(this.articlesId, this.postId, undefined, this.isLiked)
      .pipe(tap(() => {}))
      .toPromise()
  }

  sortCommentsData(data) {
    return data.sort((a, b) => {
      return <any>new Date(b.actionDate) - <any>new Date(a.actionDate)
    })
  }

  async ngOnInit() {
    this.commentsCount = 0
    const profileInfo = JSON.parse(localStorage.getItem('profile_info'))
    this.userFullName = `${profileInfo.firstName} ${profileInfo.middleName} ${profileInfo.lastName}`
    if (!this.likesCount) {
      this.likesCount = 0
    }
    this.isLiked = this.liked
    this.isLiked ? (this.likeImg = this.activeIconHeart) : (this.likeImg = this.inactiveIconHeart)

    await Promise.all([
      this.panHistories.postComments(this.id).subscribe((data) => {
        this.commentsData = this.sortCommentsData(data)
        this.commentsCount = this.commentsData.length
      }),
    ])
  }
}
