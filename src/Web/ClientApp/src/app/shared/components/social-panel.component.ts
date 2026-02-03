import { Component, Input, OnInit, ViewEncapsulation } from '@angular/core'
import { tap } from 'rxjs/operators'
import { PanHistoriesClient } from 'src/app/shared/api.generated.clients'

@Component({
  selector: 'app-social-panel',
  template: `
    <div>
      <div class="flex items-center">
        <div
          class="flex mr-4 p-4 duration-400 rounded-lg cursor-pointer items-center hover:bg-gray-200"
          (click)="setLike()"
        >
          <img [src]="likeImg" alt="" class="w-6 icon" />
        </div>
        <div
          *ngIf="type == 2"
          (click)="addCommentState = !addCommentState"
          class="flex mr-4 p-4 duration-400 rounded-lg cursor-pointer items-center hover:bg-gray-200"
        >
          <img src="assets/images/ico-comments.svg" alt="" class="w-6 icon" />
        </div>
        <div class="text-sm text-gray" i18n>{{ likesCount }} People liked this</div>
      </div>
      <button
        pButton
        class="p-button-text p-button-secondary p-button-sm"
        [icon]="activeState ? 'pi pi-caret-up' : 'pi pi-caret-down'"
        iconPos="right"
        (click)="toggle()"
        i18n-label
        label="Read Comments"
      ></button>
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
