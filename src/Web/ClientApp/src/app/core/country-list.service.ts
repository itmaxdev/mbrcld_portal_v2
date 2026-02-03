import { Inject, Injectable, LOCALE_ID } from '@angular/core'
import { HttpClient } from '@angular/common/http'
import { ReplaySubject } from 'rxjs'
import { first } from 'rxjs/operators'

@Injectable({
  providedIn: 'root',
})
export class CountryListService {
  private countries = new ReplaySubject<ICountryListItem[]>()
  private initialized = false

  constructor(@Inject(LOCALE_ID) private locale: string, private http: HttpClient) {}

  getCountryList(): Promise<ICountryListItem[]> {
    if (!this.initialized) {
      this.loadList()
      this.initialized = true
    }

    return this.countries.pipe(first()).toPromise()
  }

  private loadList() {
    if (this.locale === 'ar') {
      this.http.get('assets/data/countries_ar.json').subscribe((data: ICountryListItem[]) => {
        this.countries.next(data)
      })
    } else {
      this.http.get('assets/data/countries.json').subscribe((data: any[]) => {
        this.countries.next(data)
      })
    }
  }
}

export interface ICountryListItem {
  label: string
  value: string
}
