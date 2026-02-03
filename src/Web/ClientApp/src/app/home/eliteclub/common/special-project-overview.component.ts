import { Component, Input, OnInit, ViewEncapsulation, Inject, LOCALE_ID } from '@angular/core'
import { MenuItem } from 'primeng/api'

@Component({
  selector: 'app-special-project-overview',
  template: ` <div class="border rounded-lg border-gray-400 flex gap-2 p-2 justify-between">
    <a [routerLink]="data.id" class="w-full">
      <h1 class="text-caption text-2xl pb-2 sm:text-xl sm:pb-0 text-black">{{ data.name }}</h1>
      <p class="text-caption text-2xl pb-2 sm:text-xl sm:pb-0 text-gray-500">
        {{ data.alumniName }}
      </p>
      <p class="info-text text-lg text-gray-500">{{ data.description }}</p>
    </a>
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
      <p-menu #menu [popup]="true" [model]="items"></p-menu>
    </div>
  </div>`,
  styles: [
    `
      .info-text {
        max-height: 70px;
        overflow: hidden;
        width: 100%;
        display: -webkit-box;
        -webkit-line-clamp: 3;
        -webkit-box-orient: vertical;
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
    `,
  ],
  encapsulation: ViewEncapsulation.None,
})
export class SpecialProjectOverviewComponent implements OnInit {
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
            label: this.locale === 'en' ? 'Edit' : 'تعديل',
            icon: 'pi pi-pencil',
            routerLink: ['edit/' + this.data.id],
          },
        ],
      },
    ]
  }
}
