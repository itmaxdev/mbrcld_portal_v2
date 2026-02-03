import * as moment from 'moment'
import {
  Component,
  Input,
  OnInit,
  Output,
  ViewEncapsulation,
  LOCALE_ID,
  Inject,
} from '@angular/core'
import { SectionDataService } from '../services/section-data.service'
import { EventEmitter } from '@angular/core'
import { SecureStorage } from 'src/app/core/api-authorization'
import { MaterialStatuses } from '../api.generated.clients'

@Component({
  selector: 'app-materials-card',
  template: `
    <div
      class="materials-card border gap-4 border-gray-300 rounded-lg w-full grid-cols-1 justify-between p-6 grid"
    >
      <div class="header flex justify-between">
        <a [routerLink]="isModules ? ['materials/' + materialUrl] : ['modules/' + materialUrl]">
          <div class="header-caption flex cursor-pointer">
            <div class="count mr-4 relative flex justify-center">
              <p class="inline text-xl text-blue-600" i18n>
                {{ order }}
              </p>
            </div>
            <div class="caption max-w-xs">
              <p
                class="caption-text text-lg text-blue-600"
                [pTooltip]="name"
                tooltipPosition="top"
                i18n
              >
                {{ language === 'en' ? name : name_AR }}
              </p>
            </div>
          </div>
        </a>
        <div *ngIf="isModules && isInstructor" class="edit-icon-block">
          <a class="cursor-pointer" (click)="openMaterialDialog()">
            <img src="assets/images/ico-edit-gray.png" alt="" class="w-6" />
          </a>
        </div>
      </div>
      <div *ngIf="isModules" class="content">
        <p class="content-text text-xl" i18n>
          {{ description }}
        </p>
      </div>
      <div class="footer grid">
        <div *ngIf="!isInstructor" class="status flex items-center">
          <img
            src="https://upload.wikimedia.org/wikipedia/commons/thumb/c/c4/Globe_icon.svg/1024px-Globe_icon.svg.png"
            alt=""
            class="w-4 h-4 mr-2"
          />
          <p class="modules-fields">
            <span>
              {{ completed }}
            </span>
            <span i18n>% is Completed</span>
          </p>
        </div>
        <div class="status flex items-center">
          <img
            src="https://upload.wikimedia.org/wikipedia/commons/thumb/c/c4/Globe_icon.svg/1024px-Globe_icon.svg.png"
            alt=""
            class="w-4 h-4 mr-2"
          />
          <p class="modules-fields" i18n>{{location ? location : 'Online'}}</p>
        </div>
        <div *ngIf="status == 936510001 && publishDate" class="status flex items-center">
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
          <p class="modules-fields">{{ startDate }}</p>
        </div>
        <div class="status flex items-center">
          <img
            src="https://upload.wikimedia.org/wikipedia/commons/thumb/c/c4/Globe_icon.svg/1024px-Globe_icon.svg.png"
            alt=""
            class="w-4 h-4 mr-2"
          />
          <p class="modules-fields modules-duration">{{ duration }}</p>
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

      .content-text {
        overflow: hidden;
        width: 100%;
        display: -webkit-box;
        -webkit-line-clamp: 5;
        -webkit-box-orient: vertical;
      }

      .header-caption:hover .caption-text {
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
export class MaterialsCardComponent implements OnInit {
  @Input() id: string
  @Input() role: number
  @Input() name: string
  @Input() name_AR: string
  @Input() order: string
  @Input() duration: string
  @Input() completed: number
  @Input() startDate: string
  @Input() publishDate: string
  @Input() status: MaterialStatuses
  @Input() isModules = false
  @Input() materialUrl: string
  @Input() description: string
  @Input() isInstructor: boolean
  @Input() location: string
  @Output() openDialog = new EventEmitter()
  language: string

  constructor(
    private sectionData: SectionDataService,
    private ls: SecureStorage,
    @Inject(LOCALE_ID) private locale: string
  ) {
    this.language = locale
  }

  openMaterialDialog() {
    this.openDialog.emit(this.id)
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
      this.publishDate = moment(this.publishDate).lang('en').format('[Start] DD/MM/YY')
    } else {
      this.publishDate = 'N/A'
    }

    this.duration = this.sectionData.convertMinuteToHours(this.duration)
  }
}
