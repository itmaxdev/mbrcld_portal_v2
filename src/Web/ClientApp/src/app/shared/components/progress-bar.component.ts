import { Component, Input, OnInit, ViewEncapsulation } from '@angular/core'

@Component({
  selector: 'app-progress-bar',
  template: ` <p-progressBar [value]="value"></p-progressBar> `,
  styles: [
    `
      .p-progressbar {
        border-radius: 1rem !important;
      }
      .p-progressbar-label {
        text-align: start;
        color: #fff !important;
        text-shadow: 1px 1px 2px #444;
        padding-inline-start: 1.5rem !important;
        font-weight: normal !important;
      }

      .p-progressbar-value {
        background: #3ea447 !important;
      }
    `,
  ],
  encapsulation: ViewEncapsulation.None,
})
export class ProgressBarComponent implements OnInit {
  @Input() value: number = null

  ngOnInit(): void {}
}
