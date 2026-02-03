import { ILanguageSkill } from './models'
import { Component, OnInit } from '@angular/core'
import { FormControl, FormGroup, Validators } from '@angular/forms'
import { ActivatedRoute, Router } from '@angular/router'
import { ConfirmationService, MessageService } from 'primeng/api'
import {
  ILanguageOption,
  LanguagesFacadeService,
  IProficiencyLevelOption,
} from './languages-facade.service'

@Component({
  selector: 'app-languages',
  templateUrl: './languages.component.html',
  styleUrls: ['./languages.component.scss'],
  providers: [ConfirmationService, MessageService],
})
export class LanguagesComponent implements OnInit {
  addLanguageForm: FormGroup
  isFormSubmitting = false
  ready = false
  role: number

  languageOptions: ILanguageOption[] = []
  editingLanguage: ILanguageSkill = {}
  languages: ILanguageSkill[] = []
  proficiencyOptions: IProficiencyLevelOption[] = [
    {
      value: 1,
      label: $localize`Basic`,
    },
    {
      value: 2,
      label: $localize`Intermediate`,
    },
    {
      value: 3,
      label: $localize`Fluent`,
    },
    {
      value: 4,
      label: $localize`Native`,
    },
  ]

  private _showAddLanguageDialog = false

  get showAddLanguageDialog() {
    return this._showAddLanguageDialog
  }

  set showAddLanguageDialog(show: boolean) {
    this._showAddLanguageDialog = show
    if (!show) {
      this.editingLanguage = {}
      this.addLanguageForm.reset()
    }
  }

  constructor(
    private confirmationService: ConfirmationService,
    private facadeService: LanguagesFacadeService,
    private messageService: MessageService,
    private router: Router,
    private activatedRoute: ActivatedRoute
  ) {}

  async ngOnInit() {
    this.buildForm()

    const profileInfo = JSON.parse(localStorage.getItem('profile_info'))
    this.role = profileInfo.role

    try {
      await Promise.all([
        this.facadeService.loadLanguageSkills().then((data) => (this.languages = data)),
        this.facadeService
          .loadLanguageOptions()
          .then((options) => this.languageOptions.push(...options)),
      ])
    } finally {
      this.ready = true
    }
  }

  openAddLanguageDialog() {
    this.showAddLanguageDialog = true
  }

  async addLanguage(): Promise<void> {
    if (!this.addLanguageForm.valid || this.isFormSubmitting) {
      return
    }

    const formValue = this.addLanguageForm.value as FormModel

    if (this.languages.some((x) => x.languageId === formValue.languageId)) {
      this.messageService.add({
        summary: $localize`Invalid operation`,
        detail: $localize`You cannot add the same language more than once`,
        severity: 'warn',
        closable: true,
        life: 5000,
      })
      return
    }

    this.isFormSubmitting = true
    try {
      const id = await this.facadeService.addLanguage(formValue)
      this.languages.push({ id, ...formValue })
      this.showAddLanguageDialog = false
    } catch (error) {
      // TODO
    } finally {
      this.isFormSubmitting = false
    }
  }

  async updateLanguage() {
    this.isFormSubmitting = true

    const formValues = this.addLanguageForm.value

    await this.facadeService.updateLanguage(this.editingLanguage.id, formValues)

    Object.assign(this.editingLanguage, formValues)

    this.showAddLanguageDialog = false
    this.isFormSubmitting = false
  }

  removeLanguage(lang: any) {
    this.confirmationService.confirm({
      message: $localize`Are you sure?`,
      accept: async () => {
        await this.facadeService.removeLanguage(lang.id)
        this.languages.splice(this.languages.indexOf(lang), 1)
      },
    })
  }

  openLanguageDialogForEditing(language: ILanguageSkill) {
    this.editingLanguage = language
    this.addLanguageForm.patchValue(language)
    this.showAddLanguageDialog = true
  }

  getLanguageLabel(languageId: string) {
    return this.languageOptions.find((e) => e.value === languageId)?.label || '--'
  }

  getProficiencyLevelLabel(value: number) {
    return this.proficiencyOptions.find((e) => e.value === value)?.label || '--'
  }

  goToNextPage() {
    this.router.navigate([this.role == 4 ? '../about-yourself' : '../skills'], {
      relativeTo: this.activatedRoute,
    })
  }

  private buildForm(): void {
    this.addLanguageForm = new FormGroup({
      languageId: new FormControl('', [Validators.required]),
      level: new FormControl('', [Validators.required]),
    } as FormModel)
  }
}

type FormModel = { [key in keyof ILanguageSkill]: any }
