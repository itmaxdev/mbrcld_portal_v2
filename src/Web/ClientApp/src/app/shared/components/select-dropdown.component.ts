import { Component, Input, Output, EventEmitter } from '@angular/core'

/**
 * Generic dropdown that matches the look of app-nationality-dropdown
 * (same wrapper pattern: label + component, no raw p-dropdown in parent template).
 * Use with [(ngModel)] for template-driven forms.
 */
@Component({
  selector: 'app-select-dropdown',
  template: `
    <p-dropdown
      [inputId]="inputId"
      [options]="options"
      [ngModel]="model"
      (ngModelChange)="onModelChange($event)"
      [optionLabel]="optionLabel"
      [placeholder]="placeholder"
      [showClear]="showClear"
      [appendTo]="appendTo"
      [filter]="true"
      [filterPlaceholder]="filterPlaceholder"
      style="display: block"
      styleClass="form-dropdown-filled"
    ></p-dropdown>
  `,
})
export class SelectDropdownComponent {
  @Input() inputId: string
  @Input() options: any[]
  @Input() optionLabel: string
  @Input() placeholder: string = $localize`Select`
  @Input() showClear = true
  @Input() appendTo = 'body'
  @Input() filterPlaceholder: string = $localize`Search`

  @Input() model: any
  @Output() modelChange = new EventEmitter<any>()

  @Output() onChange = new EventEmitter<any>()

  onModelChange(value: any) {
    this.modelChange.emit(value)
    this.onChange.emit({ value })
  }
}
