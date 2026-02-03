import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms'

export const requiredIf = (required: { required: boolean }): ValidatorFn => {
  return (control: AbstractControl): ValidationErrors => {
    return !control.value && required.required ? { required: true } : null
  }
}

export const requiredIf2 = (required: () => boolean): ValidatorFn => {
  return (control: AbstractControl): ValidationErrors => {
    return !control.value && required() ? { required: true } : null
  }
}
