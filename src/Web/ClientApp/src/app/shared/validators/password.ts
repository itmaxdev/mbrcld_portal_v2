import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms'

interface PasswordValidatorOptions {
  minLength?: number
  maxLength?: number
  atLeastOneUpperCaseLetter?: boolean
  atLeastOneLowerCaseLetter?: boolean
  atLeastOneDigit?: boolean
}

export const password = (options?: PasswordValidatorOptions): ValidatorFn => {
  return (control: AbstractControl): ValidationErrors => {
    const value: string = control.value

    const errors: ValidationErrors = {}

    if (value) {
      if (
        (options?.minLength && value.length < options.minLength) ||
        (options?.maxLength && value.length > options.maxLength)
      ) {
        errors.passwordLength = {
          min: options.minLength,
          max: options.maxLength,
        }
      }

      if (options?.atLeastOneUpperCaseLetter && /[A-Z]/.test(value) === false) {
        errors.atLeastOneUpperCaseLetter = true
      }

      if (options?.atLeastOneLowerCaseLetter && /[a-z]/.test(value) === false) {
        errors.atLeastOneLowerCaseLetter = true
      }

      if (options?.atLeastOneDigit && /[0-9]/.test(value) === false) {
        errors.atLeastOneDigit = true
      }
    }

    if (Object.keys(errors).length === 0) {
      return null
    }

    return errors
  }
}
