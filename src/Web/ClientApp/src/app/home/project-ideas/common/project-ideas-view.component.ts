import { Component, Input, OnInit, ViewEncapsulation, Inject, LOCALE_ID } from '@angular/core'
import { MenuItem } from 'primeng/api'

@Component({
  selector: 'app-project-ideas-view',
  template: ` <div class="border rounded-lg border-gray-400 flex gap-2 justify-between">
    <div class="w-full">
      <div class="ideaHeader">
        <div [ngClass]="returnCardClass()" *ngIf="showUnPublishedCard(returnCardStatus())">
          {{ returnCardStatus() }}
        </div>
        <div *ngIf="isMyIdea && isEditable" class="toggleBtn">
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
          <p-menu #menu [popup]="true" [model]="items" i18n-label></p-menu>
        </div>
      </div>
      <div class="p-2 infoText">
        <h1 class="text-caption text-2xl pb-2 sm:text-xl sm:pb-0 text-black">{{ data.name }}</h1>
        <p class="text-caption text-2xl pb-2 sm:text-xl sm:pb-0 text-gray-500">
        {{ data.alumniName }}
        </p>
        <p class="info-text text-lg text-gray-500">Description: {{ data.description }}</p>
      </div>

      <div class="footer border-gray-400">
        <div class="cost">
          <p *ngIf="isMyIdea" i18n>Cost: {{ data.budget }} AED</p>
        </div>
        <div class="viewDetails">
          <a [routerLink]="data.id" class="info-text-viewDetails text-lg details-link" i18n>
            View Details
          </a>
        </div>
      </div>
    </div>
  </div>`,
  styles: [
    `
      .info-text-viewDetails {
        overflow: hidden;
        width: 100%;
        display: -webkit-box;
        -webkit-line-clamp: 3;
        -webkit-box-orient: vertical;
      }

      .info-text {
        max-height: 70px;
        overflow: hidden;
        width: 100%;
        display: -webkit-box;
        -webkit-line-clamp: 3;
        -webkit-box-orient: vertical;
      }

      .toggleBtn {
        margin-top: 20px;
        margin-right: 20px;
        margin-left: 20px;
      }

      .toggleBtn .p-button {
        background: white;
        color: gray;
        border: none !important;
      }

      .toggleBtn .p-button:focus {
        box-shadow: none !important;
      }

      .toggleBtn .p-button:hover {
        background: none !important;
        color: gray;
      }

      .toggleBtn .p-button-icon-only span {
        font-size: 4rem !important;
      }

      .ideaStatus {
        border-radius: 9px;
        padding: 15px;
        width: 160px;
        height: 20px;
        display: flex;
        align-items: center;
        justify-content: center;
        margin: 1.5rem 10px 10px 0.5rem;
      }

      .statusPublished {
        border: 1px solid #0071be;
        background: #d1f5e9;
        color: #0071be;
      }

      .statusApproved {
        border: 1px solid green;
        color: green;
        background: #d1f5e9;
      }

      .statusDenied {
        border: 1px solid red;
        color: red;
        background: #fde0e1;
      }

      .footer {
        padding: 13px;
        border-width: 1px 0 0 0;
        width: 100%;
        height: 50px;
        display: flex;
        justify-content: space-between;
      }

      .cost {
        font-size: 1.25rem;
        height: 100%;
        padding: 13px;
        display: flex;
        align-items: center;
      }

      .details-link {
        text-decoration: underline;
        color: #184296;
      }

      .ideaHeader {
        display: flex;
        justify-content: space-between;
      }

      .viewDetails {
        height: 100%;
        padding: 13px;
        display: flex;
        align-items: center;
      }

      .infoText {
        padding-bottom: 1.8rem;
      }
    `,
  ],
  encapsulation: ViewEncapsulation.None,
})
export class ProjectIdeasViewComponent implements OnInit {
  public items: MenuItem[]
  public isEditable = false
  @Input() data: any
  @Input() isMyIdea: boolean

  constructor(@Inject(LOCALE_ID) private locale: string) {}

  ngOnInit() {
    if (this.isMyIdea) {
      if (this.data.projectIdeaStatus !== 2) {
        this.isEditable = true
      }
    }
    this.items = [
      {
        items: [
          {
            label: this.locale == 'en' ? 'Edit' : 'تعديل',
            icon: 'pi pi-pencil',
            routerLink: ['edit/' + this.data.id],
          },
        ],
      },
    ]
  }

  returnCardClass() {
    if (this.isMyIdea) {
      if (this.data.projectIdeaStatus === 1) {
        return 'ideaStatus statusDenied'
      } else {
        return 'ideaStatus statusDenied'
      }
    } else {
      return 'ideaStatus statusApproved'
    }
  }
  showUnPublishedCard(cardvalue){
      if(cardvalue == "Published") return false;
      else if(cardvalue == "نشر") return false;
      else return true;
  }
  returnCardStatus() {
    if (this.isMyIdea) {
      if (this.data.projectIdeaStatus === 1) {
        if (this.locale === 'ar') {
          return 'مسودة'
        }
        return 'Draft'
      } else {
        if (this.locale === 'ar') {
          return 'قيد الموافقة'
        }
        return 'Pending Approval'
      }
    } else {
      if (this.locale === 'ar') {
        return 'نشر'
      }
      return 'Published'
    }
  }
}
