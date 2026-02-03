import { Component, Input, ViewEncapsulation } from '@angular/core'

@Component({
  selector: 'app-suggested-card',
  template: `
    <a [routerLink]="'view/' + url">
      <p-card [header]="caption" [style]="{ width: '360px' }" styleClass="p-card-shadow">
        <ng-template pTemplate="header" class="headerImgBlock">
          <img alt="Card" class="suggested-header-img" [src]="imageUrl" />
          <div *ngIf="isExistRating" class="flex flex-row items-center p-2 pb-0">
            <span class="rating-value text-xl mr-2">{{ ratingCount }}</span>
            <p-rating
              *ngIf="isExistRating"
              [(ngModel)]="ratingCount"
              [readonly]="true"
              [cancel]="false"
            ></p-rating>
          </div>
        </ng-template>
        <p class="text-content">
          {{ content }}
        </p>
        <div *ngIf="isExistBtn" pTemplate="footer">
          <p-button [label]="btnName"></p-button>
        </div>
      </p-card>
    </a>
  `,
  styles: [
    `
      .p-card {
        box-shadow: none;
        border: 1px solid rgba(0, 0, 0, 0.1);
        border-radius: 0.7rem;
        width: 100% !important;
      }

      .p-card-body {
        padding-top: 0;
      }

      .p-card-content {
        padding: 0 !important;
      }

      .p-card-title {
        height: 60px;
        font-size: 14px !important;
        width: 100%;
        overflow: hidden;
        text-overflow: ellipsis;
        display: -webkit-box;
        -webkit-line-clamp: 3;
        -webkit-box-orient: vertical;
      }

      .text-content {
        height: 105px;
        overflow: hidden;
        width: 100%;
        display: -webkit-box;
        -webkit-line-clamp: 5;
        -webkit-box-orient: vertical;
      }

      .p-rating {
        position: relative;
        bottom: -1px;
      }

      .p-card-header {
        padding: 0.5rem !important;
        border-radius: 0.5rem;
      }

      .suggested-header-img {
        border-radius: 0.7rem;
        width: 100%;
        height: 180px;
        object-fit: cover;
      }

      .p-button.p-component.p-ripple {
        width: 100%;
      }

      .pi.pi-star {
        color: #ff9900 !important;
      }

      .pi.pi-rating-icon {
        font-size: 1rem !important;
      }

      .rating-value {
        color: #ff9900 !important;
      }

      .p-ripple {
        box-shadow: none !important;
      }
    `,
  ],
  encapsulation: ViewEncapsulation.None,
})
export class SuggestedCardComponent {
  @Input() role: number
  @Input() caption: string
  @Input() content: string
  @Input() imageUrl: string
  @Input() url = ''
  @Input() btnName = 'Apply'
  @Input() isExistBtn = true
  @Input() ratingCount: number
  @Input() isExistRating = true
}
