import { Component, OnInit } from '@angular/core'
import { FormControl, FormGroup, Validators } from '@angular/forms'
import { ActivatedRoute, Router } from '@angular/router'
import { ConfirmationService, MessageService } from 'primeng/api'
import { SkillAndInterestFacadeService } from '../../../profile/skills-and-interests/skills-and-interests-facade.service'
import {
  LanguagesFacadeService,
  ILanguageOption,
  IProficiencyLevelOption,
} from '../../../profile/languages/languages-facade.service'
import { ILanguageSkill } from '../../../profile/languages/models'
import { ISkillAndInterest } from '../../../profile/skills-and-interests/models'

@Component({
  selector: 'app-registrant-skills',
  templateUrl: './skills.component.html',
  styleUrls: ['./skills.component.scss'],
  providers: [ConfirmationService, MessageService],
})
export class RegistrantSkillsComponent implements OnInit {
  // Skills
  skillsForm: FormGroup
  skills: any[] = []

  private _showSkillDialog = false
  get showSkillDialog() {
    return this._showSkillDialog
  }

  set showSkillDialog(val: boolean) {
    this._showSkillDialog = val
    if (!val) {
      if (this.skillsForm) this.skillsForm.reset()
    }
  }

  // Languages
  languageForm: FormGroup
  languages: ILanguageSkill[] = []
  languageOptions: ILanguageOption[] = []
  editingLanguage: ILanguageSkill = {}

  proficiencyOptions: IProficiencyLevelOption[] = [
    { value: 1, label: $localize`Basic` },
    { value: 2, label: $localize`Intermediate` },
    { value: 3, label: $localize`Fluent` },
    { value: 4, label: $localize`Native` },
  ]

  private _showLanguageDialog = false
  get showLanguageDialog() {
    return this._showLanguageDialog
  }

  set showLanguageDialog(val: boolean) {
    this._showLanguageDialog = val
    if (!val) {
      this.editingLanguage = {}
      if (this.languageForm) this.languageForm.reset()
    }
  }

  ready = false
  isFormSubmitting = false

  constructor(
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private skillFacade: SkillAndInterestFacadeService,
    private languageFacade: LanguagesFacadeService,
    private confirmationService: ConfirmationService,
    private messageService: MessageService
  ) {}

  async ngOnInit() {
    this.buildForms()
    await this.fetchContent()
    this.ready = true
  }

  async fetchContent() {
    try {
      await Promise.all([
        this.skillFacade.loadSkillAndInterest().then((data) => (this.skills = data)),
        this.languageFacade.loadLanguageSkills().then((data) => (this.languages = data)),
        this.languageFacade
          .loadLanguageOptions()
          .then((options) => this.languageOptions.push(...options)),
      ])
    } catch (error) {
      console.error('Error fetching skills/languages content:', error)
    }
  }

  private buildForms() {
    this.skillsForm = new FormGroup({
      name: new FormControl('', [Validators.required]),
    })

    this.languageForm = new FormGroup({
      languageId: new FormControl('', [Validators.required]),
      level: new FormControl('', [Validators.required]),
    })
  }

  // Skills Methods
  openAddSkillDialog() {
    this.showSkillDialog = true
  }

  async saveSkill() {
    if (this.skillsForm.invalid || this.isFormSubmitting) {
      this.skillsForm.markAllAsTouched()
      return
    }

    this.isFormSubmitting = true
    try {
      const formValue = this.skillsForm.value
      const id = await this.skillFacade.addSkillAndInterest(formValue)
      this.skills.push({ id, ...formValue })
      this.showSkillDialog = false
    } catch (error) {
      console.error('Error saving skill:', error)
    } finally {
      this.isFormSubmitting = false
    }
  }

  async removeSkill(skill: any) {
    this.confirmationService.confirm({
      message: $localize`Are you sure you want to delete this skill?`,
      accept: async () => {
        await this.skillFacade.removeSkillAndInterest(skill.id)
        this.skills = this.skills.filter((s) => s.id !== skill.id)
      },
    })
  }

  // Language Methods
  openAddLanguageDialog() {
    this.showLanguageDialog = true
  }

  openEditLanguageDialog(language: ILanguageSkill) {
    this.editingLanguage = language
    this.languageForm.patchValue(language)
    this.showLanguageDialog = true
  }

  async saveLanguage() {
    if (this.languageForm.invalid || this.isFormSubmitting) {
      this.languageForm.markAllAsTouched()
      return
    }

    const formValue = this.languageForm.value

    // Check distinct for adds
    if (
      !this.editingLanguage.id &&
      this.languages.some((x) => x.languageId === formValue.languageId)
    ) {
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
      if (this.editingLanguage.id) {
        await this.languageFacade.updateLanguage(this.editingLanguage.id, formValue)
        Object.assign(this.editingLanguage, formValue)
      } else {
        const id = await this.languageFacade.addLanguage(formValue)
        this.languages.push({ id, ...formValue })
      }
      this.showLanguageDialog = false
    } catch (error) {
      console.error('Error saving language:', error)
    } finally {
      this.isFormSubmitting = false
    }
  }

  async removeLanguage(language: ILanguageSkill) {
    this.confirmationService.confirm({
      message: $localize`Are you sure you want to delete this language?`,
      accept: async () => {
        await this.languageFacade.removeLanguage(language.id)
        this.languages = this.languages.filter((l) => l.id !== language.id)
      },
    })
  }

  getLanguageLabel(languageId: string) {
    return this.languageOptions.find((e) => e.value === languageId)?.label || '--'
  }

  getProficiencyLevelLabel(value: number) {
    return this.proficiencyOptions.find((e) => e.value === value)?.label || '--'
  }

  // Navigation
  onSubmit() {
    this.router.navigate(['../module-team'], { relativeTo: this.activatedRoute })
  }

  goBack() {
    this.router.navigate(['../preferences'], { relativeTo: this.activatedRoute })
  }
}
