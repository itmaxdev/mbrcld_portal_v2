import { Component, Input } from '@angular/core'
import { AbstractControl } from '@angular/forms'

interface PasswordValidationRule {
  message: string
  hasError(control: AbstractControl): boolean
}

const VALIDATION_RULES: PasswordValidationRule[] = [
  {
    hasError: (c) => c.hasError('passwordLength'),
    message: $localize`Must be between 8 and 32 characters`,
  },
  {
    hasError: (c) => c.hasError('atLeastOneUpperCaseLetter'),
    message: $localize`Must contain at least 1 upper case letter [A-Z]`,
  },
  {
    hasError: (c) => c.hasError('atLeastOneLowerCaseLetter'),
    message: $localize`Must contain at least 1 lower case letter [a-z]`,
  },
  {
    hasError: (c) => c.hasError('atLeastOneDigit'),
    message: $localize`Must contain at least 1 digit [0-9]`,
  },
]

@Component({
  selector: 'app-password-validator',
  template: `
    <div
      *ngIf="control && control.value && visible"
      class="absolute bg-gray-100 border border-gray-400 rounded p-2 z-10"
      [style]="{
        bottom: '-' + popover.height + 'px',
        left: 0,
        right: 0
      }"
    >
      <div #popover>
        <ng-template #validationMessage let-hasError="hasError" let-message="message">
          <div [class]="{ 'p-invalid': hasError, 'text-green-600': !hasError }">
            <i
              class="pi"
              [class]="{
                'pi-times-circle': hasError,
                'pi-check-circle': !hasError
              }"
            ></i>
            {{ message }}
          </div>
        </ng-template>

        <ng-container *ngFor="let rule of validationRules">
          <ng-template
            [ngTemplateOutlet]="validationMessage"
            [ngTemplateOutletContext]="{
              hasError: rule.hasError(control),
              message: rule.message
            }"
          ></ng-template>
        </ng-container>
      </div>
    </div>
  `,
})
export class PasswordValidatorComponent {
  get validationRules(): PasswordValidationRule[] {
    return VALIDATION_RULES
  }

  @Input()
  control: AbstractControl

  @Input()
  visible: boolean
}
