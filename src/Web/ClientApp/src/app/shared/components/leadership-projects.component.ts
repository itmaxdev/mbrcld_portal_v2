import * as moment from 'moment'
import { Component, Input, OnInit, ViewEncapsulation } from '@angular/core'

interface IStatusData {
  statusName: string
  statusColor: string
}

@Component({
  selector: 'app-leadership-projects',
  template: `
    <div class="flex">
      <div
        *ngIf="month"
        class="border border-gray-500 rounded-l-lg p-4 text-center w-32 grid items-center content-center"
      >
        <p *ngIf="month && !withoutDueCaption" class="text-gray-500" i18n>DUE</p>
        <p class="uppercase text-blue-500 font-bold">{{ month }}</p>
      </div>
      <div
        [ngClass]="{ 'rounded-r-lg': month, 'rounded-lg': !month }"
        class="descriptionSection border p-4 flex justify-between w-full items-center"
      >
        <div>
          <p class="font-bold text-black">{{ caption }}</p>
          <div class="content-of-projects">
            <p *ngIf="content" class="content-of-projects-text text-gray-500 h-6">{{ content }}</p>
            <p class="content-of-projects-text text-gray-500 h-6">
              <span i18n>Status</span>
              <span>:</span>
              <span [ngStyle]="{ color: statuses[status - 1].statusColor }" class="ml-1">
                {{ statuses[status - 1].statusName }}</span
              >
            </p>
          </div>
        </div>
        <a *ngIf="!isReadOnly" [routerLink]="[routerLink + '/' + id]">
          <img
            class="transition duration-300 cursor-pointer hover:bg-gray-400 w-8 rounded-full"
            src="assets/images/ico-arrow-right-gray.png"
            alt=""
          />
        </a>
      </div>
    </div>
  `,
  styles: [
    `
      .p-tabview-nav {
        border: 1px solid #dee2e6 !important;
        border-radius: 8px !important;
        padding-left: 20px !important;
      }

      .p-tabview-panels {
        padding: 1rem 0 !important;
      }

      .content-of-projects {
        overflow: hidden;
        text-overflow: ellipsis;
      }

      .content-of-projects-text {
        overflow: hidden;
        display: -webkit-box;
        -webkit-line-clamp: 2;
        -webkit-box-orient: vertical;
      }

      .searchInput {
        width: 100%;
        padding: 12px 5px 12px 30px;
        border-radius: 7px;
      }

      .searchIcon {
        position: relative;
        top: 32px;
        left: 8px;
      }

      .cardContainer {
        display: flex;
        flex-wrap: wrap;
        column-gap: 15px;
        row-gap: 30px;
      }

      .descriptionSection {
        border-color: #a0aec0 !important;
      }
    `,
  ],
  encapsulation: ViewEncapsulation.None,
})
export class LeadershipProjectsComponent implements OnInit {
  @Input() id: string
  @Input() month: string
  @Input() clock: string
  @Input() endDate: Date
  @Input() status: number
  @Input() caption: string
  @Input() content: string
  @Input() isGrouped = false
  @Input() disableRouterLink = false
  @Input() withoutDueCaption = false
  isReadOnly = false
  routerLink = 'project'
  readOnlyStatuses: number[] = []
  statuses: IStatusData[]

  constructor() {
    this.statuses = [
      { statusName: 'Draft', statusColor: 'red' },
      { statusName: 'Submitted for Approval', statusColor: '#FFBF00' },
      { statusName: 'In Progress', statusColor: 'yellow' },
      { statusName: 'Under Review', statusColor: 'blue' },
      { statusName: 'Completed', statusColor: 'green' },
      { statusName: 'Grouped', statusColor: 'gray' },
      { statusName: 'Ready for Presentation', statusColor: 'green' },
      { statusName: 'Accepted', statusColor: 'green' },
      { statusName: 'Rejected', statusColor: 'red' },
      { statusName: 'UnderReview', statusColor: 'blue' },
    ]
    this.readOnlyStatuses = [2, 4, 5, 6, 7]
  }

  ngOnInit(): void {
    this.isReadOnly = false
    if (this.readOnlyStatuses.includes(this.status) || this.disableRouterLink) {
      this.isReadOnly = true
    }
    if (this.isGrouped) {
      this.isReadOnly = false
      this.routerLink = 'groupped-project'
    }
    this.month = this.endDate ? moment(this.endDate).lang('en').format('MMM DD') : null
    this.clock = moment(this.endDate).lang('en').format('hh:mm a')
  }
}
