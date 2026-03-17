import { PhoneNumberInputComponent } from '../components/phone-number-input.component'
import { Directive, Host } from '@angular/core'
import { AbstractControl, NG_VALIDATORS, ValidationErrors, Validator } from '@angular/forms'
import { parsePhoneNumber } from 'libphonenumber-js'

@Directive({
  selector: '[appValidatePhoneNumber]',
  providers: [
    {
      provide: NG_VALIDATORS,
      useExisting: ValidatePhoneNumberDirective,
      multi: true,
    },
  ],
})
export class ValidatePhoneNumberDirective implements Validator {
  constructor(@Host() private component: PhoneNumberInputComponent) {}

  validate(control: AbstractControl): ValidationErrors | null {
    const raw = this.component.nationalPhoneNumber || ''
    const nationalPhoneNumber = raw.trim()
    const countryCode = this.component.countryCode
    if (!nationalPhoneNumber) {
      return { required: true }
    }

    // 1) Reject anything that is not digits (spaces are allowed visually)
    const digitsOnlyInput = nationalPhoneNumber.replace(/\s/g, '')
    if (!/^\d+$/.test(digitsOnlyInput)) {
      return { digitsOnly: true }
    }

    // 2) Try strict validation with libphonenumber first
    try {
      const phoneNumber = parsePhoneNumber(nationalPhoneNumber, countryCode)
      if (phoneNumber && phoneNumber.isValid()) {
        return null
      }
    } catch {
      // Ignore parse errors and fall back to simple length validation below
    }

    // 3) Fallback: basic length check (E.164 max 15 digits)
    const digitCount = digitsOnlyInput.length
    if (digitCount >= 6 && digitCount <= 15) {
      return null
    }

    return { phoneNumber: true }
  }

  registerOnValidatorChange(fn: () => void): void {}
}
