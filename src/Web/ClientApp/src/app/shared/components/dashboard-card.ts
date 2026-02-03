import { Component, Input, OnInit, Inject, LOCALE_ID } from '@angular/core'
import { IKPI } from 'src/app/shared/api.generated.clients'

@Component({
  selector: 'app-dashboard-card',
  template: ` <div class="card">
    <div>
      <div>
        <p class="card-value text-white text-2xl">
          {{ value }}
        </p>
      </div>
      <div class="card-block" *ngIf="localeLanguage === 'en'">
        <p class="card-name text-white text-xl">
          {{ name }}
        </p>
      </div>
      <div class="card-block" *ngIf="localeLanguage === 'ar'">
        <p class="card-name-ar text-white text-xl">{{ name_AR }}</p>
      </div>
    </div>

    <div *ngFor="let detail of details; let i = index">
      <p *ngIf="localeLanguage === 'en'" class="card-details text-base">
        {{ detail.name + ' ' + detail.value }}
      </p>
      <p *ngIf="localeLanguage === 'ar'" class="card-details text-base">
        {{ detail.name_AR + ' ' + detail.value }}
      </p>
    </div>
  </div>`,
  styles: [
    `
      .card {
        padding: 10px;
        background-color: #0071be;
        height: 155px;
        width: 200px;
        border: none;
        border-radius: 8px;
        margin: 25px;
      }
      .card-block {
        display: flex;
      }
      .card-name {
        margin-left: 10px;
        font-size: 21px;
      }
      .card-name-ar {
        margin-left: 7px !important;
      }
      .card-value {
        font-size: 30px;
        margin-left: 10px;
      }
      .card-details {
        margin-left: 10px;
      }
    `,
  ],
})
export class DashboardCardComponent implements OnInit {
  @Input() name: string
  @Input() name_AR: string
  @Input() value: number
  @Input() details: IKPI[]

  public localeLanguage: string

  constructor(@Inject(LOCALE_ID) locale: string) {
    this.localeLanguage = locale
  }

  ngOnInit(): void {}
}
