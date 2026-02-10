import { Component, Input, OnInit, ViewEncapsulation } from '@angular/core'
import { tap } from 'rxjs/operators'
import { PanHistoriesClient } from 'src/app/shared/api.generated.clients'

@Component({
  selector: 'app-social-panel',
  template: `
    <div>
      <div class="flex items-center">
        <div
          class="flex mr-4 duration-400 rounded-lg cursor-pointer items-center hover:bg-gray-200"
          (click)="setLike()"
        >
          <svg
            width="24"
            height="24"
            viewBox="0 0 24 24"
            fill="none"
            xmlns="http://www.w3.org/2000/svg"
            [class.likeIcon]="isLiked"
          >
            <path
              d="M20.8401 4.60999C20.3294 4.099 19.7229 3.69364 19.0555 3.41708C18.388 3.14052 17.6726 2.99817 16.9501 2.99817C16.2276 2.99817 15.5122 3.14052 14.8448 3.41708C14.1773 3.69364 13.5709 4.099 13.0601 4.60999L12.0001 5.66999L10.9401 4.60999C9.90843 3.5783 8.50915 2.9987 7.05012 2.9987C5.59109 2.9987 4.19181 3.5783 3.16012 4.60999C2.12843 5.64169 1.54883 7.04096 1.54883 8.49999C1.54883 9.95903 2.12843 11.3583 3.16012 12.39L4.22012 13.45L12.0001 21.23L19.7801 13.45L20.8401 12.39C21.3511 11.8792 21.7565 11.2728 22.033 10.6053C22.3096 9.93789 22.4519 9.22248 22.4519 8.49999C22.4519 7.77751 22.3096 7.0621 22.033 6.39464C21.7565 5.72718 21.3511 5.12075 20.8401 4.60999Z"
              [attr.fill]="isLiked ? 'currentColor' : 'none'"
              stroke="currentColor"
              stroke-width="2"
            />
          </svg>
        </div>
        <div
          *ngIf="type == 2"
          (click)="addCommentState = !addCommentState"
          class="flex mr-4 duration-400 rounded-lg cursor-pointer items-center hover:bg-gray-200"
        >
          <svg
            width="24"
            height="24"
            viewBox="0 0 24 24"
            fill="none"
            xmlns="http://www.w3.org/2000/svg"
          >
            <rect width="24" height="24" fill="white" />
            <path
              d="M21 11.5C21.0034 12.8199 20.6951 14.1219 20.1 15.3C19.3944 16.7118 18.3098 17.8992 16.9674 18.7293C15.6251 19.5594 14.0782 19.9994 12.5 20C11.1801 20.0035 9.87812 19.6951 8.7 19.1L3 21L4.9 15.3C4.30493 14.1219 3.99656 12.8199 4 11.5C4.00061 9.92179 4.44061 8.37488 5.27072 7.03258C6.10083 5.69028 7.28825 4.6056 8.7 3.90003C9.87812 3.30496 11.1801 2.99659 12.5 3.00003H13C15.0843 3.11502 17.053 3.99479 18.5291 5.47089C20.0052 6.94699 20.885 8.91568 21 11V11.5Z"
              fill="none"
              stroke="currentColor"
              stroke-width="2"
            />
          </svg>
        </div>
        <div class="text-sm text-gray" i18n>{{ likesCount }} People liked this</div>
        <button
          pButton
          class="p-button-text p-button-secondary p-button-sm"
          [icon]="activeState ? 'pi pi-caret-up' : 'pi pi-caret-down'"
          iconPos="right"
          (click)="toggle()"
          i18n-label
          label="Read Comments"
        ></button>
      </div>
      <p-accordion>
        <p-accordionTab [(selected)]="addCommentState">
          <div class="mb-2 flex items-center">
            <input
              pInputText
              [(ngModel)]="newComment"
              class="pl-2 w-full rounded-lg mr-2"
              type="text"
              name="newComment"
              placeholder="Enter your comment"
              i18n-placeholder
            />
            <p-button label="Add" (click)="addComment()" i18n-label></p-button>
          </div>
        </p-accordionTab>
        <p-accordionTab [(selected)]="activeState">
          <div class="grid gap-2">
            <app-comment-section
              *ngFor="let comment of commentsData"
              [id]="comment.id"
              [date]="comment.actionDate"
              [userName]="comment.contactName"
              [comment]="comment.comment"
            ></app-comment-section>
          </div>
        </p-accordionTab>
      </p-accordion>
    </div>
  `,
  styles: [
    `
      .articleImg {
        pointer-events: auto;
      }
      .articleImg * {
        pointer-events: auto;
      }
      .p-accordion-header {
        display: none;
      }
      .p-accordion-content {
        border: 0 solid #dee2e6 !important;
        border-radius: 0.5rem !important;
      }
      .icon {
        max-width: unset;
      }
      .likeIcon {
        color: #ef4444; /* red when liked */
      }
      .p-accordion {
        display: inline-block !important;
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
