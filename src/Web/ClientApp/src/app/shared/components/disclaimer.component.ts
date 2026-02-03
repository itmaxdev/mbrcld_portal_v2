import { Component, EventEmitter, Input, Output } from '@angular/core'
import { ProfileClient } from '../api.generated.clients'

export enum DisclaimerTypes {
  ArticlesDisclaimer = 1,
  IdeaHubDisclaimer,
  SpecialProjectDisclaimer,
}

@Component({
  selector: 'app-disclaimer',
  template: `
    <p-dialog
      header="Disclaimer"
      styleClass="w-full max-w-sm"
      [modal]="true"
      [focusTrap]="true"
      [focusOnShow]="true"
      [closeOnEscape]="true"
      [resizable]="false"
      [draggable]="false"
      [(visible)]="isVisible"
    >
      <div class="content">
        <div class="text">
          <p class="text-gray-800 text-base font-semibold">{{ disclaimerText }}</p>
        </div>
        <button
          pButton
          label="Approve"
          i18n-label
          class="h-12 p-button-primary self-end"
          (click)="approve()"
        ></button>
      </div>
    </p-dialog>
  `,
  styles: [
    `
      .content {
        display: flex;
        flex-direction: column;
        row-gap: 29px;
      }
    `,
  ],
})
export class DisclaimerComponent {
  @Input() disclaimerType: DisclaimerTypes
  @Input() disclaimerText = 'test'
  @Input() isVisible: boolean
  @Output() onApprove = new EventEmitter()

  constructor(private profileClient: ProfileClient) {}

  approve() {
    this.profileClient.userDisclaimerPut(this.disclaimerType).subscribe(() => {
      this.onApprove.emit()
    })
  }
}
