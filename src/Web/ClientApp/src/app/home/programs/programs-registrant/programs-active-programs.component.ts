import { Component, Input, OnInit, ViewEncapsulation } from '@angular/core'
import { Inject, LOCALE_ID } from '@angular/core'
import { ListActiveProgramsViewModel } from 'src/app/shared/api.generated.clients'

@Component({
  selector: 'app-programs-active-programs',
  template: `
    <a [routerLink]="programUrl" class="inner">
      <div class="imgBox">
        <picture>
          <img [src]="data.pictureUrl" alt="" width="442" height="242" loading="lazy" />
        </picture>
      </div>

      <div class="contentBox">
        <div class="title">
          {{ language === 'en' ? data.name : data.name_AR }}
        </div>

        <div class="textBox sm">
          <p>
            {{ language === 'en' ? data.description : data.description_AR }}
          </p>
        </div>
      </div>
    </a>
  `,

  encapsulation: ViewEncapsulation.None,
})
export class ProgramActiveProgramsComponent implements OnInit {
  @Input() data: ListActiveProgramsViewModel
  public programId: string
  public language: string

  constructor(@Inject(LOCALE_ID) locale) {
    this.language = locale
  }

  get programUrl() {
    if (this.data.openForRegistration) return 'program/' + this.programId
    else return 'program/previous/' + this.programId
  }

  ngOnInit() {
    this.programId = this.data.id
  }
}
