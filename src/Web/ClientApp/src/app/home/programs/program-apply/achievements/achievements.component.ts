import { AchievementsFacadeService } from './achievements-facade.service'
import { IAchievement } from './achievements.interface'
import { Component, OnInit } from '@angular/core'
import { FormControl, FormGroup, Validators } from '@angular/forms'
import { ActivatedRoute, Router } from '@angular/router'
import { ConfirmationService } from 'primeng/api'
import { inputLanguage } from 'src/app/shared/validators'

const financialImpactOptions: DropdownOption[] = [
  { value: 1, label: $localize`Less than 50K AED` },
  { value: 2, label: $localize`50K – 100K AED` },
  { value: 3, label: $localize`100K – 500K AED` },
  { value: 4, label: $localize`More than 500K AED` },
  { value: 5, label: $localize`Other` },
]

const populationImpactOptions: DropdownOption[] = [
  { value: 1, label: $localize`A Function` },
  { value: 2, label: $localize`A Team` },
  { value: 3, label: $localize`The Organization` },
  { value: 4, label: $localize`The City or Country` },
]

const achievementImpactOptions: DropdownOption[] = [
  { value: 0, label: 'Emirate' },
  { value: 1, label: 'Country' },
  { value: 2, label: 'Regional' },
  { value: 3, label: 'Global' },
]

const yearOptions = []

@Component({
  selector: 'app-main-achievements',
  templateUrl: './achievements.component.html',
  styleUrls: ['./achievements.component.scss'],
  providers: [ConfirmationService],
})
export class AchievementsComponent implements OnInit {
  achievementForm: FormGroup
  achievements: IAchievement[] = []
  editingAchievement: IAchievement = {}
  isFormSubmitting = false
  activeIndex = 0
  ready = false

  private _showAchievementDialog = false

  get showAchievementDialog() {
    return this._showAchievementDialog
  }

  set showAchievementDialog(show: boolean) {
    this._showAchievementDialog = show
    if (!show) {
      this.editingAchievement = {}
      this.achievementForm.reset()
    }
  }

  get financialImpactOptions(): any[] {
    return financialImpactOptions
  }

  get populationImpactOptions(): any[] {
    return populationImpactOptions
  }

  get achievementImpactOptions(): any[] {
    return achievementImpactOptions
  }

  setYearOptions() {
    if (yearOptions.length == 0) {
      var currentyear = new Date().getFullYear();
      for (let i = 2000; i <= currentyear; i++) {
        yearOptions.push({ label: i.toString(), value: i.toString() })
      }
    }
  }

  get yearOptions(): any[] {
    return yearOptions
  }

  constructor(
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private facade: AchievementsFacadeService,
    private confirmationService: ConfirmationService
  ) {
    this.setYearOptions()
  }

  async ngOnInit() {
    this.buildForm()

    await this.facade.loadAchievements().then((achievements) => {
      this.achievements = achievements
    })
    this.ready = true
  }

  private buildForm(): void {
    this.achievementForm = new FormGroup({
      description: new FormControl('', [
        Validators.required,
        Validators.minLength(150),
        inputLanguage('en'),
      ]),
      description_AR: new FormControl('', [
        Validators.required,
        Validators.minLength(150),
        inputLanguage('ar'),
      ]),
      summaryOfAchievement: new FormControl('', [
        Validators.required,
        inputLanguage('en'),
        Validators.maxLength(350),
      ]),
      summaryOfAchievement_AR: new FormControl('', [
        Validators.required,
        inputLanguage('ar'),
        Validators.maxLength(350),
      ]),
      populationImpact: new FormControl('', [Validators.required]),
      achievementImpact: new FormControl('', [Validators.required]),
      financialImpact: new FormControl('', [Validators.required]),
      organization: new FormControl('', [Validators.required, inputLanguage('en')]),
      organization_AR: new FormControl('', [Validators.required, inputLanguage('ar')]),
      yearOfAchievement: new FormControl('', [Validators.required]),
    } as FormModel)
  }

  getFinancialImpactLabel(value: number) {
    return this.financialImpactOptions.find((o) => o.value === value)?.label
  }

  getPopulationImpactLabel(value: number) {
    return this.populationImpactOptions.find((o) => o.value === value)?.label
  }

  getAchievementImpactLabel(value: number) {
    return this.achievementImpactOptions.find((o) => o.value === value)?.label
  }

  async addAchievement() {
    if (!this.achievementForm.valid || this.isFormSubmitting) {
      return
    }

    const formValue = this.achievementForm.value as FormModel

    this.isFormSubmitting = true
    try {
      const id = await this.facade.addAchievement(formValue)
      this.achievements.push({ id, ...formValue })
      this.showAchievementDialog = false
    } catch (error) {
      // TODO
    } finally {
      this.isFormSubmitting = false
    }
  }

  removeAchievement(achievement: IAchievement) {
    this.confirmationService.confirm({
      message: $localize`Are you sure?`,
      accept: async () => {
        await this.facade.removeReference(achievement.id)
        this.achievements.splice(this.achievements.indexOf(achievement), 1)
      },
    })
  }

  async updateAchievement() {
    this.isFormSubmitting = true
    const formValues = this.achievementForm.value

    await this.facade.updateReference(this.editingAchievement.id, formValues)
    Object.assign(this.editingAchievement, formValues)

    this.showAchievementDialog = false
    this.isFormSubmitting = false
  }

  openAddAchievementDialog() {
    this._showAchievementDialog = true
  }

  openAchievementDialogForEditing(reference: IAchievement) {
    this.editingAchievement = reference
    this.achievementForm.patchValue(reference)
    this.showAchievementDialog = true
  }

  goToNextForm(): void {
    this.router.navigate(['../references'], { relativeTo: this.activatedRoute })
  }
}

type FormModel = { [key in keyof IAchievement]: any }

interface DropdownOption {
  label: string
  value: number
}
