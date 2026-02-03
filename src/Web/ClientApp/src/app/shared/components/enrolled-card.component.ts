import { Component, Input, OnInit, Output, ViewEncapsulation } from '@angular/core'
import { EventEmitter } from '@angular/core'
import { Router } from '@angular/router'

export interface CardInterface {
  caption: string
  content: string
  url: string
  progressValue?: number
  ratingCount?: number
}

@Component({
  selector: 'app-enrolled-card',
  template: `
    <a
      [routerLink]="!isClickable ? null : [routeLink]"
      [ngClass]="{ 'cursor-default': !isClickable }"
    >
      <p-card styleClass="program-card">
        <ng-template pTemplate="header">
          <img alt="Card" [src]="url" class="header-img" />
        </ng-template>
        <p class="text-xl">{{ caption }}</p>
        <p class="info-text">{{ content }}</p>
        <div *ngIf="visibleBtn" class="edit-icon-block">
          <a
            class="cursor-pointer"
            (click)="isRoute ? null : openAcceleratedDialog()"
            [routerLink]="isRoute ? [id] : null"
          >
            <img src="assets/images/ico-edit-gray.png" alt="" class="w-6" />
          </a>
        </div>
        <div
          *ngIf="isProgressBar && role !== 4"
          [ngClass]="{ 'w-full': isAllLine, 'w-3/5': !isAllLine }"
          class="mt-4 grid grid-cols-12 gap-4 items-center"
        >
          <p-progressBar
            class="progress-bar col-span-9 sm:col-span-11"
            [value]="progressValue"
            [showValue]="false"
          ></p-progressBar>
          <span class="text-end text-xl font-semibold">{{ progressValue }}%</span>
        </div>
      </p-card>
    </a>
  `,
  styles: [
    `
      .enrolled {
        justify-items: end;
      }
      .header-img {
        width: 150px;
        height: 114px;
        object-fit: cover;
        min-width: 150px !important;
      }
      @media (max-width: 640px) {
        .header-img {
          width: 100%;
          height: 200px;
        }
      }
      .text-caption {
        white-space: nowrap;
        width: 100%;
        overflow: hidden;
        text-overflow: ellipsis;
      }
      .info-text {
        height: 70px;
        overflow: hidden;
        width: 100%;
        display: -webkit-box;
        -webkit-line-clamp: 3;
        -webkit-box-orient: vertical;
      }
      .progress-bar > .p-progressbar {
        border-radius: 20px;
        height: 0.8rem;
      }
      .progress-bar > .p-progressbar > .p-progressbar-value {
        background: #0a991b !important;
      }
    `,
  ],
  encapsulation: ViewEncapsulation.None,
})
export class EnrolledCardComponent implements OnInit {
  @Input() id: string
  @Input() url: string = undefined
  @Input() role: number
  @Input() caption: string
  @Input() content: string
  @Input() progressValue: number
  @Input() isProgressBar = true
  @Input() visibleBtn = true
  @Input() isAllLine = false
  @Input() isDashboard = false
  @Input() programId: string = undefined
  @Input() isClickable = true
  @Input() isRoute = false
  @Output() openAccelerated = new EventEmitter()
  routeLink: string

  constructor(private router: Router) {}

  openAcceleratedDialog() {
    this.openAccelerated.emit(this.id)
  }

  generateUrl() {
    if (this.isDashboard) {
      const currentUrl = this.router.url.split('/')
      currentUrl[currentUrl.length - 1] = 'programs/view'
      this.id = currentUrl.join('/') + '/' + this.id
      this.routeLink = this.id
      if (this.programId) {
        this.routeLink = `${this.id}/modules/${this.programId}`
      }
    } else {
      this.routeLink = 'view/' + this.id
    }
  }

  ngOnInit(): void {
    this.generateUrl()
  }
}
