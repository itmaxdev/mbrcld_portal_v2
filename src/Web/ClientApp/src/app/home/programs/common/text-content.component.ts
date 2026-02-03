import { Component, Input, OnInit, ViewEncapsulation } from '@angular/core'

@Component({
  selector: 'app-text-content',
  template: `
    <div>
      <h1 class="text-xl font-semibold text-blue-800">{{ name }}</h1>
      <div class="content-text content-block text-lg ql-editor" [innerHTML]="text" dir="ltr"></div>
    </div>
  `,
  styles: [
    `
      .content-block ol,
      ul {
        margin-block-start: 1em;
        margin-block-end: 1em;
        padding-inline-start: 40px;
      }
      .content-block ol {
        list-style: decimal !important;
      }
      .content-block ul {
        list-style: disc !important;
      }

      .content-text {
        color: #718096;
      }
    `,
  ],
  encapsulation: ViewEncapsulation.None,
})
export class TextContentComponent implements OnInit {
  @Input() name = ''
  @Input() text = ''

  ngOnInit(): void {}
}
