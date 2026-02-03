import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms'

export const email = (): ValidatorFn => {
  return (control: AbstractControl): ValidationErrors => {
    const pattern = /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/
    if (!control.value || pattern.test(control.value)) {
      return null
    }
    return { email: true }
  }
}
