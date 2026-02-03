import { Component, Inject, Input, LOCALE_ID } from '@angular/core'

interface IListItem {
  label: string
  value: string
  class: string
}

const languages: IListItem[] = [
  {
    label: 'EN',
    value: 'en-US',
    class: 'gb',
  },
  {
    label: 'AR',
    value: 'ar-SA',
    class: 'ae',
  },
]

@Component({
  selector: 'app-language-switcher',
  template: `
    <p-dropdown
      [(ngModel)]="selectedLang"
      [options]="options"
      (onChange)="changeLanguage($event)"
      appendTo="body"
      [class.alt]="alt"
      styleClass="bg-transparent border-none"
    >
      <ng-template pTemplate="selectedItem" let-language>
        <div class="flex flex-row items-center f16">
          <div [class]="language.class | lowercase" class="flag"></div>
          <span class="ms-2">{{ language.label }}</span>
        </div>
      </ng-template>
      <ng-template pTemplate="item" let-language>
        <div class="flex flex-row items-center f16">
          <div [class]="language.class | lowercase" class="flag"></div>
          <span class="ms-2">{{ language.label }}</span>
        </div>
      </ng-template>
    </p-dropdown>
  `,
  styles: [
    `
      :host ::ng-deep .p-dropdown.p-focus {
        box-shadow: none !important;
      }

      :host .alt ::ng-deep span {
        color: white !important;
      }

      :host .alt ::ng-deep .p-dropdown-trigger {
        color: white !important;
      }
    `,
  ],
})
export class LanguageSwitcherComponent {
  @Input() alt = false

  selectedLang: string

  get options(): IListItem[] {
    return languages
  }

  constructor(@Inject(LOCALE_ID) locale: string) {
    this.selectedLang = locale === 'ar' ? 'ar-SA' : 'en-US'
  }

  changeLanguage(lang: IListItem) {
    const referer = encodeURIComponent(location.href)
    const langValue = encodeURIComponent(lang.value)
    location.href = `/SetPreferredLanguage?lang=${langValue}&referer=${referer}`
  }
}
