import { Component, EventEmitter, Input, Output } from '@angular/core'

@Component({
  selector: 'app-profile-section-info-card',
  template: `
    <div
      [ngStyle]="highLight ? { borderColor: '#f6ad55', backgroundColor: '#f6ad5522' } : null"
      class="flex flex-row items-stretch border border-gray-400 rounded bg-white py-3 px-4"
    >
      <div class="flex items-stretch flex-grow pe-4">
        <ng-content></ng-content>
      </div>
      <div class="flex flex-col justify-start flex-grow-0 ps-3">
        <button
          title="Edit"
          i18n-title
          *ngIf="showEditButton"
          pButton
          type="button"
          icon="pi pi-pencil"
          class="p-button-rounded p-button-outlined p-button-secondary"
          (click)="onEdit.emit()"
        ></button>
        <button
          title="Delete"
          i18n-title
          pButton
          type="button"
          icon="pi pi-times"
          class="p-button-rounded p-button-outlined p-button-secondary"
          (click)="onDelete.emit()"
        ></button>
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
export class ProfileSectionInfoCardComponent {
  @Input()
  showEditButton = true

  @Input()
  highLight = false

  @Output()
  onEdit = new EventEmitter<void>()

  @Output()
  onDelete = new EventEmitter<void>()
}
