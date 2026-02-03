import { Component, Input } from '@angular/core'

@Component({
  selector: 'app-card',
  template: `
    <div class="border rounded-lg border-gray-400  bg-white p-4">
      <header *ngIf="sectionTitle || sectionSubTitle" class="flex items-center mb-8 -mx-4">
        <div class="flex flex-col mx-4">
          <h1 class="text-2xl font-semibold">
            {{ underLineEmail }}
          </h1>
          <h2
            *ngIf="sectionSubTitle"
            [innerHtml]="sectionSubTitle"
            class="mt-2 text-gray-700"
            style="white-space: pre-line"
          ></h2>
        </div>
        <div class="flex flex-shrink flex-grow mx-4 justify-end items-center">
          <ng-content select="[role=actions]"></ng-content>
        </div>
      </header>
      <main class="pb-6">
        <ng-content></ng-content>
      </main>
    </div>
  `,
})
export class CardComponent {
  @Input()
  sectionTitle: string

  @Input()
  sectionSubTitle: string

  get underLineEmail() {
    const words = this.sectionTitle.split(' ')
    words.forEach((word) => {
      if (word.includes('@')) word.bold()
    })
    return words.join(' ')
  }
}
