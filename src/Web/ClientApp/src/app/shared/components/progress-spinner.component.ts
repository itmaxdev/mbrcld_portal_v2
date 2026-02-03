import { Component, OnInit } from '@angular/core'

@Component({
  selector: 'app-progress-spinner',
  template: `
    <div class="flex flex-grow justify-center items-center py-8">
      <p-progressSpinner></p-progressSpinner>
    </div>
  `,
})
export class ProgressSpinnerComponent implements OnInit {
  ngOnInit(): void {}
}
