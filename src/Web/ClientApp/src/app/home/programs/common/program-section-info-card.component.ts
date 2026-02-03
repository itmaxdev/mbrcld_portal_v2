import { Component, EventEmitter, Input, Output } from '@angular/core'

@Component({
  selector: 'app-program-section-info-card',
  template: `
    <div class="border border-gray-400 rounded-lg py-3 px-4 bg-card-light">
      <div class="flex flex-row items-stretch max-w-full overflow-hidden">
        <div class="flex items-stretch flex-grow pe-4 overflow-hidden">
          <ng-content></ng-content>
        </div>
        <div class="flex flex-col justify-start flex-grow-0 flex-shrink-0">
          <button
            *ngIf="showEditButton"
            pButton
            type="button"
            icon="pi pi-pencil"
            title="Edit"
            i18n-title
            class="p-button-rounded p-button-outlined p-button-secondary"
            (click)="onEdit.emit()"
          ></button>
          <button
            *ngIf="showDeleteButton"
            pButton
            type="button"
            icon="pi pi-times"
            title="Delete"
            i18n-title
            class="p-button-rounded p-button-outlined p-button-secondary"
            (click)="onDelete.emit()"
          ></button>
        </div>
      </div>
    </div>
  `,
  styles: [
    `
      button + button {
        margin-top: 0.75rem;
      }
    `,
  ],
})
export class ProgramSectionInfoCardComponent {
  @Input()
  showEditButton = true

  @Input()
  showDeleteButton = true

  @Output()
  onEdit = new EventEmitter<void>()

  @Output()
  onDelete = new EventEmitter<void>()
}
