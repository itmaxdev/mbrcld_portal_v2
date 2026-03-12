import * as moment from 'moment'
import { Component, Input, OnInit, ViewEncapsulation } from '@angular/core'

@Component({
  selector: 'app-comment-section',
  template: `
    <div class="comment-container">
      <div class="comment-inner">
        <!-- Left: Avatar + Name/Date -->
        <div class="comment-author">
          <img class="avatar" src="assets/images/avatar.png" />
          <div class="author-info">
            <p class="author-name">{{ userName }}</p>
            <p class="author-date" dir="ltr">{{ date }}</p>
          </div>
        </div>
        <!-- Right: Comment text -->
        <div class="comment-body">
          <p class="comment-text">{{ comment }}</p>
        </div>
      </div>
    </div>
  `,
  styles: [
    `
      .comment-container {
        border: 1px solid #dedede;
        background-color: #f4f4f2;
        border-radius: 0.75rem;
        padding: 12px 16px;
      }

      .comment-inner {
        display: flex;
        align-items: flex-start;
        gap: 16px;
      }

      .comment-author {
        display: flex;
        align-items: center;
        gap: 10px;
        flex-shrink: 0;
        min-width: 160px;
      }

      .avatar {
        width: 40px;
        height: 40px;
        border-radius: 50%;
        object-fit: cover;
        flex-shrink: 0;
      }

      .author-info {
        display: flex;
        flex-direction: column;
      }

      .author-name {
        font-size: 0.95rem;
        font-weight: 600;
        color: #374151;
        margin: 0;
        white-space: nowrap;
      }

      .author-date {
        font-size: 0.8rem;
        color: #6b7280;
        margin: 2px 0 0 0;
      }

      .comment-body {
        flex: 1;
        border-left: 2px solid #dedede;
        padding-left: 16px;
      }

      .comment-text {
        font-size: 0.95rem;
        color: #1f2937;
        margin: 0;
        line-height: 1.5;
        word-break: break-word;
      }
    `,
  ],
  encapsulation: ViewEncapsulation.None,
})
export class CommentSectionComponent implements OnInit {
  @Input() date: any
  @Input() id: string
  @Input() comment: string
  @Input() userName: string

  constructor() {}

  ngOnInit(): void {
    this.date = moment(this.date).lang('en').fromNow()
  }
}
