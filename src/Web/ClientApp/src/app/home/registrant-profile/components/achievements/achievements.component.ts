import { Component, OnInit } from '@angular/core'
import { FormControl, FormGroup, Validators } from '@angular/forms'
import { ActivatedRoute, Router } from '@angular/router'
import { ConfirmationService } from 'primeng/api'
import { ProfileAchievementsFacadeService } from '../../../profile/profile-achievements/profile-achievements-facade.service'
import { MembershipsFacadeService } from '../../../profile/memberships/memberships-facade.service'
import { IAchievement } from '../../../profile/profile-achievements/profile-achievements.interface'
import { IMembership } from '../../../profile/memberships/memberships.interface'
import { inputLanguage } from 'src/app/shared/validators'

@Component({
  selector: 'app-registrant-achievements',
  templateUrl: './achievements.component.html',
  styleUrls: ['./achievements.component.scss'],
  providers: [ConfirmationService],
})
export class RegistrantAchievementsComponent implements OnInit {
  // Achievements
  achievementForm: FormGroup
  achievements: IAchievement[] = []
  editingAchievement: IAchievement = {}

  private _showAchievementDialog = false
  get showAchievementDialog() {
    return this._showAchievementDialog
  }

  set showAchievementDialog(val: boolean) {
    this._showAchievementDialog = val
    if (!val) {
      this.editingAchievement = {}
      if (this.achievementForm) this.achievementForm.reset()
    }
  }

  // Memberships
  membershipForm: FormGroup
  membershipSelectionForm: FormGroup
  memberships: IMembership[] = []
  editingMembership: IMembership = {}

  private _showMembershipDialog = false
  get showMembershipDialog() {
    return this._showMembershipDialog
  }

  set showMembershipDialog(val: boolean) {
    this._showMembershipDialog = val
    if (!val) {
      this.editingMembership = {}
      if (this.membershipForm) this.membershipForm.reset()
    }
  }

  isMemberOfInstitution = false
  isFormSubmitting = false
  ready = false

  financialImpactOptions = [
    { value: 1, label: $localize`Less than 50K AED` },
    { value: 2, label: $localize`50K – 100K AED` },
    { value: 3, label: $localize`100K – 500K AED` },
    { value: 4, label: $localize`More than 500K AED` },
    { value: 5, label: $localize`Other` },
  ]

  populationImpactOptions = [
    { value: 1, label: $localize`A Function` },
    { value: 2, label: $localize`A Team` },
    { value: 3, label: $localize`The Organization` },
    { value: 4, label: $localize`The City or Country` },
  ]

  achievementImpactOptions = [
    { value: 0, label: 'Emirate' },
    { value: 1, label: 'Country' },
    { value: 2, label: 'Regional' },
    { value: 3, label: 'Global' },
  ]

  membershipLevelOptions = [
    { value: 0, label: 'Country' },
    { value: 1, label: 'Regional' },
    { value: 2, label: 'Global' },
  ]

  activeOptions = [
    { value: true, label: $localize`Yes` },
    { value: false, label: $localize`No` },
  ]

  booleanOptions = [
    { value: true, label: $localize`Yes` },
    { value: false, label: $localize`No` },
  ]

  yearOptions: any[] = []

  constructor(
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private achievementsFacade: ProfileAchievementsFacadeService,
    private membershipsFacade: MembershipsFacadeService,
    private confirmationService: ConfirmationService
  ) {
    this.generateYearOptions()
  }

  async ngOnInit() {
    this.buildForms()
    await this.fetchContent()
    this.ready = true
  }

  async fetchContent() {
    try {
      const [achievements, memberships, isActiveMember] = await Promise.all([
        this.achievementsFacade.loadAchievements(),
        this.membershipsFacade.loadMemberships(),
        this.membershipsFacade.getIsActiveMember(),
      ])
      this.achievements = achievements
      this.memberships = memberships
      this.isMemberOfInstitution = isActiveMember
      this.membershipSelectionForm.patchValue({ isMember: isActiveMember })
    } catch (error) {
      console.error('Error fetching achievements content:', error)
    }
  }

  private generateYearOptions() {
    const currentYear = new Date().getFullYear()
    for (let i = currentYear; i >= 2000; i--) {
      this.yearOptions.push({ label: i.toString(), value: i.toString() })
    }
  }

  private buildForms() {
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
    })

    this.membershipForm = new FormGroup({
      institutionName: new FormControl('', [Validators.required, inputLanguage('en')]),
      institutionName_AR: new FormControl('', [Validators.required, inputLanguage('ar')]),
      active: new FormControl('', [Validators.required]),
      roleName: new FormControl('', [Validators.required, inputLanguage('en')]),
      roleName_AR: new FormControl('', [Validators.required, inputLanguage('ar')]),
      joinDate: new FormControl('', [Validators.required]),
      membershipLevel: new FormControl('', [Validators.required]),
    })

    this.membershipSelectionForm = new FormGroup({
      isMember: new FormControl(null, [Validators.required]),
    })
  }

  // Achievement Methods
  openAddAchievementDialog() {
    this.showAchievementDialog = true
  }

  openEditAchievementDialog(achievement: IAchievement) {
    this.editingAchievement = achievement
    this.achievementForm.patchValue(achievement)
    this.showAchievementDialog = true
  }

  async saveAchievement() {
    if (this.achievementForm.invalid || this.isFormSubmitting) {
      this.achievementForm.markAllAsTouched()
      return
    }

    this.isFormSubmitting = true
    try {
      const formValue = this.achievementForm.value
      if (this.editingAchievement.id) {
        await this.achievementsFacade.updateReference(this.editingAchievement.id, formValue)
      } else {
        await this.achievementsFacade.addAchievement(formValue)
      }
      await this.fetchContent()
      this.showAchievementDialog = false
    } catch (error) {
      console.error('Error saving achievement:', error)
    } finally {
      this.isFormSubmitting = false
    }
  }

  async removeAchievement(achievement: IAchievement) {
    this.confirmationService.confirm({
      message: $localize`Are you sure you want to delete this achievement?`,
      accept: async () => {
        await this.achievementsFacade.removeReference(achievement.id)
        await this.fetchContent()
      },
    })
  }

  // Membership Methods
  onMembershipSelectionChange(event: any) {
    this.isMemberOfInstitution = event.value
  }

  openAddMembershipDialog() {
    this.showMembershipDialog = true
  }

  openEditMembershipDialog(membership: IMembership) {
    this.editingMembership = membership
    const patchValue = { ...membership }
    if (membership.joinDate) {
      patchValue.joinDate = this.formatDate(membership.joinDate) as any
    }
    this.membershipForm.patchValue(patchValue)
    this.showMembershipDialog = true
  }

  private formatDate(date: any): string {
    const d = new Date(date)
    return d.toISOString().split('T')[0]
  }

  async saveMembership() {
    if (this.membershipForm.invalid || this.isFormSubmitting) {
      this.membershipForm.markAllAsTouched()
      return
    }

    this.isFormSubmitting = true
    try {
      const formValue = { ...this.membershipForm.value }
      if (formValue.joinDate) {
        formValue.joinDate = new Date(formValue.joinDate)
      }

      if (this.editingMembership.id) {
        await this.membershipsFacade.updateMembership(this.editingMembership.id, formValue)
      } else {
        await this.membershipsFacade.addMembership(formValue)
      }
      await this.fetchContent()
      this.showMembershipDialog = false
    } catch (error) {
      console.error('Error saving membership:', error)
    } finally {
      this.isFormSubmitting = false
    }
  }

  async removeMembership(membership: IMembership) {
    this.confirmationService.confirm({
      message: $localize`Are you sure you want to delete this membership?`,
      accept: async () => {
        await this.membershipsFacade.removeMembership(membership.id)
        await this.fetchContent()
      },
    })
  }

  async onSubmit() {
    if (this.isFormSubmitting) return

    this.isFormSubmitting = true
    try {
      const isMember = this.membershipSelectionForm.get('isMember').value
      await this.membershipsFacade.setIsActiveMember(!!isMember)
      this.router.navigate(['../preferences'], { relativeTo: this.activatedRoute })
    } catch (error) {
      console.error('Error on submit:', error)
    } finally {
      this.isFormSubmitting = false
    }
  }

  goBack() {
    this.router.navigate(['../training-courses'], { relativeTo: this.activatedRoute })
  }
}
