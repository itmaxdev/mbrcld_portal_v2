import { Inject, LOCALE_ID, Pipe, PipeTransform } from '@angular/core'

@Pipe({
  name: 'lang',
})
export class LanguagePipe implements PipeTransform {
  constructor(@Inject(LOCALE_ID) private locale: string) {}

  transform<T>(value: T, propertyName: keyof T): any {
    if (this.locale === 'ar') {
      const arabicPropName = propertyName + '_AR'
      if (Object.prototype.hasOwnProperty.call(value, arabicPropName)) {
        return value[arabicPropName]
      }
    }

    return value[propertyName]
  }
}
