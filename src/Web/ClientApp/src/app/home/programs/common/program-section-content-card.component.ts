import { tap } from 'rxjs/operators'
import { ActivatedRoute } from '@angular/router'
import { Component, Input, OnInit } from '@angular/core'
import { ContentsClient } from 'src/app/shared/api.generated.clients'
import { SectionDataService } from 'src/app/shared/services/section-data.service'

@Component({
  selector: 'app-program-section-content-card',
  template: `
    <div class="card flex">
      <div
        class="countBlock py-2 px-4 items-center text-center border border-gray-500 rounded-l-lg"
      >
        <p class="text-xl">{{ number }}</p>
      </div>
      <div
        class="py-2 px-6 flex justify-between items-center w-full border border-l-0 border-gray-500 rounded-r-lg"
      >
        <div class="flex items-center">
          <img class="w-6 mr-4" [src]="iconUrl" alt="" />
          <p class="text-xl text-gray-700 underline">{{ cardTitle }}</p>
        </div>
        <div class="grid grid-cols-2 gap-4">
          <a
            [routerLink]="['./edit-' + type + '/' + id]"
            [state]="{ data: { sectionId: sectionId, contentId: id } }"
          >
            <img class="w-6 cursor-pointer" src="assets/images/ico-edit-gray.png" alt="" />
          </a>
          <img
            class="w-6 cursor-pointer"
            src="assets/images/ico-delete-gray.png"
            alt=""
            (click)="deleteCard()"
          />
        </div>
      </div>
    </div>
  `,
  styles: [
    `
      .countBlock {
        width: 55px;
      }
    `,
  ],
})
export class ProgramSectionContentCardComponent implements OnInit {
  @Input() cardTitle = ''
  @Input() iconUrl = ''
  @Input() number: number
  @Input() type: string
  @Input() id: number
  sectionId: string

  constructor(
    public sectionData: SectionDataService,
    private contents: ContentsClient,
    private route: ActivatedRoute
  ) {}

  async deleteCard() {
    await Promise.all([
      this.contents
        .contentsDelete(this.id.toString())
        .pipe(
          tap(() => {
            window.location.reload()
          })
        )
        .toPromise(),
    ])
  }

  ngOnInit(): void {
    this.sectionId = this.route.snapshot.paramMap.get('sectionId')
  }
}
