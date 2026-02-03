import { Component, Input, Output, EventEmitter } from '@angular/core'
@Component({
  selector: 'app-survey-card',
  template: `
    <div class="border rounded-lg">
      <a (click)="itemClick($event)" [href]="url" class="block cursor-pointer p-4" target="_blank">
        <h1 class="text-xl font-semibold">{{ templateName }}</h1>
        <p>{{ name }}</p>
      </a>
    </div>
  `
})
export class SurveyCardComponent {
  @Input() url: string
  @Input() name: string
  @Input() templateName: string
  @Input() disabled: string
  @Output() disabledItemClick: EventEmitter<any> = new EventEmitter();

  constructor() { }
  itemClick(e) {
    if (this.disabled == 'true') {
      e.preventDefault()
      this.disabledItemClick.emit();
      return false
    }
    return true
  }
}
