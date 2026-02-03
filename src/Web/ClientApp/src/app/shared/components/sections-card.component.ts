import {
  Component,
  Input,
  OnInit,
  Output,
  ViewEncapsulation,
  LOCALE_ID,
  Inject,
} from '@angular/core'
import { ActivatedRoute } from '@angular/router'
import { SectionDataService } from '../services/section-data.service'
import { EventEmitter } from '@angular/core'
import * as moment from 'moment'
import { MaterialStatuses } from '../api.generated.clients'

@Component({
  selector: 'app-sections-card',
  template: `
    <div
      class="materials-card grid-cols-1 border border-gray-300 rounded-lg gap-4 p-6 pr-0 w-full grid"
    >
      <div class="header flex justify-between">
        <a [routerLink]="['content/' + id]">
          <div class="header-caption flex cursor-pointer">
            <div class="count mr-4 relative flex justify-center">
              <p class="inline text-xl text-blue-600">
                {{ order }}
              </p>
            </div>
            <div class="caption max-w-xs">
              <p
                class="caption-text text-lg text-blue-600 hover:underline"
                [pTooltip]="name"
                tooltipPosition="top"
              >
                {{ name }}
              </p>
            </div>
          </div>
        </a>
        <div *ngIf="isInstructor" class="edit-icon-block pr-6">
          <a class="cursor-pointer" (click)="openSectionDialog()">
            <img src="assets/images/ico-edit-gray.png" alt="" class="w-6" />
          </a>
        </div>
      </div>
      <div class="footer grid">
        <div *ngIf="!isInstructor">
          <span class="text-sm" [class]="statusInfo.color" i18n>{{ statusInfo.text }}</span>
        </div>
        <div class="status flex items-center">
          <img
            src="https://upload.wikimedia.org/wikipedia/commons/thumb/c/c4/Globe_icon.svg/1024px-Globe_icon.svg.png"
            alt=""
            class="w-4 h-4 mr-2"
          />
          <p class="modules-fields" i18n>Online</p>
        </div>
        <div *ngIf="sectionStatus == 936510001 && publishDate" class="status flex items-center">
          <img
            src="https://upload.wikimedia.org/wikipedia/commons/thumb/c/c4/Globe_icon.svg/1024px-Globe_icon.svg.png"
            alt=""
            class="w-4 h-4 mr-2"
          />
          <p class="modules-fields">{{ publishDate }}</p>
        </div>
        <div *ngIf="startDate" class="status flex items-center">
          <img
            src="https://upload.wikimedia.org/wikipedia/commons/thumb/c/c4/Globe_icon.svg/1024px-Globe_icon.svg.png"
            alt=""
            class="w-4 h-4 mr-2"
          />
          <p class="modules-fields" i18n>{{ startDate }}</p>
        </div>
        <div class="status flex items-center">
          <img
            src="https://upload.wikimedia.org/wikipedia/commons/thumb/c/c4/Globe_icon.svg/1024px-Globe_icon.svg.png"
            alt=""
            class="w-4 h-4 mr-2"
          />
          <p class="modules-fields modules-duration" i18n>{{ duration }}</p>
        </div>
      </div>
    </div>
  `,
  styles: [
    `
      .count {
        width: 22px;
      }
      .caption {
        overflow: hidden;
        text-overflow: ellipsis;
      }
      .caption-text {
        height: 92px;
        overflow: hidden;
        width: 140px;
        display: -webkit-box;
        -webkit-line-clamp: 4;
        -webkit-box-orient: vertical;
      }
      .content {
        width: 100%;
        overflow: hidden;
        text-overflow: ellipsis;
      }
      .header-content:hover .caption-text {
        text-decoration: underline;
      }
      .modules-fields {
        color: #718096;
      }
      .modules-duration {
        direction: ltr;
      }
    `,
  ],
  encapsulation: ViewEncapsulation.None,
})
export class SectionsCardComponent implements OnInit {
  statusInfo: any
  @Input() id: string
  @Input() role: number
  @Input() name: string
  @Input() order: string
  @Input() status: number
  @Input() duration: string
  @Input() startDate: string
  @Input() publishDate: string
  @Input() sectionStatus: MaterialStatuses
  @Input() materialUrl: string
  @Input() isInstructor: boolean
  @Output() openModal = new EventEmitter()
  materialsId: string = undefined

  constructor(
    private route: ActivatedRoute,
    private sectionData: SectionDataService,
    @Inject(LOCALE_ID) private locale: string
  ) {}

  openSectionDialog() {
    this.openModal.emit(this.id)
  }

  ngOnInit(): void {
    if (this.startDate) {
      if (this.locale == 'en') {
        this.startDate = moment(this.startDate).lang('en').format('[Start] DD/MM/YY')
      } else {
        this.startDate = moment(this.startDate).lang('en').format('[تاريخ البدء] DD/MM/YY')
      }
    } else {
      this.startDate = 'N/A'
    }
    if (this.publishDate) {
      this.publishDate = moment(this.publishDate).lang('en').format('[Publish] DD/MM/YY')
    } else {
      this.publishDate = 'N/A'
    }
    this.materialsId = this.route.snapshot.paramMap.get('materialsId')
    this.statusInfo = this.sectionData.checkStatus(this.status)
    this.duration = this.sectionData.convertMinuteToHours(this.duration)
  }
}
