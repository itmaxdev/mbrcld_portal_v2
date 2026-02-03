import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms'

export const url = (): ValidatorFn => {
  return (control: AbstractControl): ValidationErrors => {
    const pattern = /(https?:\/\/(?:www\.|(?!www))[a-zA-Z0-9][a-zA-Z0-9-]+[a-zA-Z0-9]\.[^\s]{2,}|www\.[a-zA-Z0-9][a-zA-Z0-9-]+[a-zA-Z0-9]\.[^\s]{2,}|https?:\/\/(?:www\.|(?!www))[a-zA-Z0-9]+\.[^\s]{2,}|www\.[a-zA-Z0-9]+\.[^\s]{2,})/
    if (!control.value || pattern.test(control.value)) {
      return null
    }
    return { url: true }
  }
}
