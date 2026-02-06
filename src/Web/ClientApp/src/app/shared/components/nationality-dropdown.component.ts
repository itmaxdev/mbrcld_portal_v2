import { Component, Input, OnInit } from '@angular/core'
import { AbstractControl, FormGroupDirective } from '@angular/forms'
import { CountryListService } from 'src/app/core/country-list.service'

interface ListItem {
  label: string
  value: string
}

@Component({
  selector: 'app-nationality-dropdown',
  template: `
    <p-dropdown
      [placeholder]="placeholder"
      [formControl]="control"
      [inputId]="inputId"
      [options]="options$ | async"
      [filter]="true"
      [virtualScroll]="true"
      [itemSize]="30"
      [appendTo]="appendTo"
      style="display:block"
    >
      <ng-template pTemplate="selectedItem" let-country>
        <div class="flex flex-row items-center f16">
          <div [class]="country.value | lowercase" class="flag"></div>
          <span class="ms-2">{{ country.label }}</span>
        </div>
      </ng-template>
      <ng-template pTemplate="item" let-country>
        <div class="flex flex-row items-center f16">
          <div [class]="country.value | lowercase" class="flag"></div>
          <span class="mx-2">{{ country.label }}</span>
        </div>
      </ng-template>
    </p-dropdown>
  `,
})
export class NationalityDropdownComponent implements OnInit {
  @Input()
  inputId: string

  @Input()
  controlName: string

  @Input()
  placeholder: string = $localize`Select a country`

  @Input()
  appendTo: string

  options$: Promise<ListItem[]>
  control: AbstractControl

  constructor(public form: FormGroupDirective, private countryListSvc: CountryListService) {}

  ngOnInit() {
    this.control = this.form.form.get(this.controlName)
    this.options$ = this.countryListSvc.getCountryList()
  }
}
