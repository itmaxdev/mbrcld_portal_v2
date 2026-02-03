import { ChangeDetectorRef, Component, forwardRef, Input, OnInit } from '@angular/core'
import { ControlValueAccessor, FormGroupDirective, NG_VALUE_ACCESSOR } from '@angular/forms'
import mobileExamples from 'libphonenumber-js/examples.mobile.json'
import { CountryListService } from 'src/app/core/country-list.service'
import * as countriesJsonModule from 'src/assets/data/countries.json'
import {
  CountryCode,
  formatIncompletePhoneNumber,
  getCountryCallingCode,
  getExampleNumber,
  parsePhoneNumberFromString,
  parsePhoneNumber,
  PhoneNumber,
} from 'libphonenumber-js'

const countries: ListItem[] = (countriesJsonModule as any).default

@Component({
  selector: 'app-phone-number',
  template: `
    <div class="flex flex-row app-phone-input" dir="ltr">
      <ng-container *ngIf="countryOptions$ | async as options">
        <p-dropdown
          [options]="options"
          [filter]="true"
          [virtualScroll]="true"
          [itemSize]="30"
          [disabled]="disabled"
          [(ngModel)]="countryCode"
          (onChange)="onChangeCountry()"
          (onBlur)="onBlur()"
        >
          <ng-template pTemplate="selectedItem" let-country>
            <div class="flex flex-row items-center f16">
              <div [class]="country.value | lowercase" class="flag"></div>
              <span class="ms-2">{{ prefix }}</span>
            </div>
          </ng-template>
          <ng-template pTemplate="item" let-country>
            <div class="flex flex-row items-center f16">
              <div [class]="country.value | lowercase" class="flag"></div>
              <span class="mx-2">{{ country.label }}</span>
            </div>
          </ng-template>
        </p-dropdown>
      </ng-container>
      <input
        pInputText
        type="tel"
        [id]="inputId"
        [placeholder]="template"
        [disabled]="disabled"
        [(ngModel)]="nationalPhoneNumber"
        (input)="onInputPhoneNumber()"
        (blur)="onBlur()"
      />
    </div>
  `,
  styles: [
    `
      ::ng-deep .app-phone-input .p-dropdown-panel {
        width: 250px;
      }

      ::ng-deep .app-phone-input .p-dropdown {
        border-right: none !important;
        border-top-right-radius: 0 !important;
        border-bottom-right-radius: 0 !important;
      }

      ::ng-deep .app-phone-input .p-inputtext[type='tel'] {
        border-left: none !important;
        border-top-left-radius: 0 !important;
        border-bottom-left-radius: 0 !important;
      }

      ::ng-deep .app-phone-input .cdk-virtual-scroll-viewport {
        max-width: 100%;
      }

      ::ng-deep [dir='rtl'] .app-phone-input .cdk-virtual-scroll-viewport {
        direction: rtl;
      }
    `,
  ],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => PhoneNumberInputComponent),
      multi: true,
    },
  ],
})
export class PhoneNumberInputComponent implements OnInit, ControlValueAccessor {
  countryCode: CountryCode = 'AE'
  nationalPhoneNumber: string
  prefix: string
  template: string

  disabled = false

  @Input()
  inputId: string

  private onChange: (_: any) => void
  private onTouched: () => void

  countryOptions$: Promise<ListItem[]>

  constructor(
    public form: FormGroupDirective,
    private changeDetector: ChangeDetectorRef,
    private countryListSvc: CountryListService
  ) {}

  ngOnInit() {
    this.countryOptions$ = this.countryListSvc.getCountryList() as any
    this.updatePrefix()
    this.updateTemplate()
  }

  onInputPhoneNumber() {
    const formattedPhoneNumber = formatIncompletePhoneNumber(
      this.getInternationalPhoneNumber(this.nationalPhoneNumber),
      this.countryCode
    )

    if (formattedPhoneNumber) {
      const pn = this.getNationalPhoneNumberPortion(formattedPhoneNumber)
      if (pn !== this.nationalPhoneNumber) {
        this.nationalPhoneNumber = pn
        this.changeDetector.detectChanges()
      }
    }

    this.emitChangeEvent()
  }

  onChangeCountry(): void {
    this.nationalPhoneNumber = ''
    this.updatePrefix()
    this.updateTemplate()
    this.emitChangeEvent()
  }

  writeValue(value: string): void {
    const phoneNumber = parsePhoneNumberFromString(value || '')
    if (phoneNumber?.country && phoneNumber.country !== this.countryCode) {
      this.countryCode = phoneNumber.country
      this.updatePrefix()
      this.updateTemplate()
    }
    this.nationalPhoneNumber = this.getNationalPhoneNumber(phoneNumber)
  }

  registerOnChange(fn: any): void {
    this.onChange = fn
  }

  registerOnTouched(fn: any): void {
    this.onTouched = fn
  }

  setDisabledState(isDisabled: boolean): void {
    this.disabled = isDisabled
  }

  onBlur() {
    this.onTouched && this.onTouched()
  }

  private updateTemplate() {
    this.template = this.getNationalPhoneNumber(getExampleNumber(this.countryCode, mobileExamples))
  }

  private updatePrefix() {
    this.prefix = '+' + getCountryCallingCode(this.countryCode)
  }

  private emitChangeEvent() {
    if (this.onChange) {
      try {
        const phoneNumber = parsePhoneNumber(this.nationalPhoneNumber, this.countryCode)
        if (phoneNumber) {
          this.onChange(phoneNumber.formatInternational())
        } else {
          this.onChange('')
        }
      } catch (e) {
        this.onChange('')
      }
    }
  }

  private getInternationalPhoneNumber(phoneNumber: string) {
    phoneNumber = phoneNumber?.trim()
    return phoneNumber?.startsWith(this.prefix) ? phoneNumber : this.prefix + phoneNumber
  }

  private getNationalPhoneNumber(phoneNumber: PhoneNumber) {
    return phoneNumber ? this.getNationalPhoneNumberPortion(phoneNumber.formatInternational()) : ''
  }

  private getNationalPhoneNumberPortion(phoneNumber: string) {
    return phoneNumber?.replace(this.prefix, '').trim()
  }
}

interface ListItem {
  label: string
  value: CountryCode
}
