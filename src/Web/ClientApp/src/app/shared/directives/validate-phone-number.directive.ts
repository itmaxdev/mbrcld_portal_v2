import { PhoneNumberInputComponent } from '../components/phone-number-input.component'
import { Directive, Host } from '@angular/core'
import { AbstractControl, NG_VALIDATORS, ValidationErrors, Validator } from '@angular/forms'
import { isValidNumberForRegion } from 'libphonenumber-js'

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

  validate(control: AbstractControl): ValidationErrors {
    const nationalPhoneNumber = this.component.nationalPhoneNumber
    const countryCode = this.component.countryCode
    if (nationalPhoneNumber && !isValidNumberForRegion(nationalPhoneNumber, countryCode)) {
      return { phoneNumber: true }
    }
  }

  registerOnValidatorChange(fn: () => void): void {}
}
