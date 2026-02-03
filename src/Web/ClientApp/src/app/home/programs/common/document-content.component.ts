import { Component, Input, OnInit } from '@angular/core'

@Component({
  selector: 'app-document-content',
  template: `
    <div>
      <h1 class="text-xl font-semibold text-blue-800 mb-2">{{ name }}</h1>
      <a [href]="documentUrl" download>
        <button
          pButton
          pRipple
          type="button"
          class="p-button-outlined"
          icon="pi pi-download"
          iconPos="left"
          [label]="'Download ' + name"
        ></button>
      </a>
    </div>
  `,
})
export class DocumentContentComponent implements OnInit {
  @Input() name: string
  @Input() documentUrl: string

  ngOnInit(): void {}
}
