import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms'

const charSets: LanguageCharacters = {
  en: /[A-Za-z0-9]/,
  ar: /[\u0621-\u064A\u0660-\u0669]/,
}

export const inputLanguage = (lang: Language): ValidatorFn => {
  return (control: AbstractControl): ValidationErrors => {
    const value = control.value

    for (const key of Object.keys(charSets).filter((x) => x !== lang) as Language[]) {
      if (charSets[key].test(value)) {
        return { language: lang }
      }
    }
    return null
  }
}

type Language = 'en' | 'ar'

type LanguageCharacters = { [key in Language]: RegExp }
