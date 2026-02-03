import { Directive, HostBinding, Input, OnInit } from '@angular/core'
import { AbstractControl, FormGroupDirective } from '@angular/forms'

const ERROR_MESSAGES = {
  'required': $localize`This field is required`,
  'email': $localize`This is not a valid email`,
  'url': $localize`This is not a valid url`,
  'minlength': $localize`This should be at least \${requiredLength} characters long`,
  'maxlength': $localize`This should be at most \${requiredLength} characters long`,
  'language': $localize`Please provide input in the correct language`,
  'phoneNumber': $localize`This is not a valid phone number`,
  '*': $localize`Please check the current value`,
}

@Directive({
  selector: '[appControlValidationError]',
})
export class ControlValidationErrorDirective implements OnInit {
  @Input()
  control: AbstractControl

  @Input()
  controlName: string

  @Input()
  messages: [{ key: string }]

  constructor(private form: FormGroupDirective) {}

  ngOnInit(): void {
    if (!this.control && this.controlName) {
      this.control = this.form.form.get(this.controlName)
    }
  }

  @HostBinding('style.display')
  get display(): string | null {
    return this.showValidationError ? null : 'none'
  }

  @HostBinding('innerText')
  get errorMessage(): string {
    if (!this.showValidationError) {
      return ''
    }

    const errors = this.control.errors
    const firstError = Object.keys(errors)[0]
    return this.getErrorMessage(firstError, errors[firstError])
  }

  private get showValidationError(): boolean {
    return (
      (this.control.touched && this.control.invalid) ||
      (this.form.submitted && this.control.invalid)
    )
  }

  private getErrorMessage(error: string, data: any): string {
    if (this.messages) {
      if (this.messages[error]) {
        return this.replacePlaceholdersWithValues(this.messages[error], data)
      }
    }

    const messageTemplate =
      ERROR_MESSAGES[error] || (this.messages || {})['*'] || ERROR_MESSAGES['*']

    return this.replacePlaceholdersWithValues(messageTemplate, data)
  }

  private replacePlaceholdersWithValues(template: string, data: any): string {
    if (template.indexOf('$') === -1) {
      return template
    }

    const placeholders = /\$\{[^}]+\}/g.exec(template)
    if (placeholders.length === 0) {
      return template
    }

    data = data || {}

    for (const x of placeholders) {
      const key = x.substr(2, x.length - 3)
      template = template.replace(x, data[key])
    }

    return template
  }
}
