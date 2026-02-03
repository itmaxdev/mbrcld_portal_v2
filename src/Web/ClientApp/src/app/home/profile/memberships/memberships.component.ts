import { Component, Inject, LOCALE_ID, OnInit } from '@angular/core'
import { FormGroup, FormControl, Validators } from '@angular/forms'
import { Router, ActivatedRoute } from '@angular/router'
import { IMembership } from './memberships.interface'
import { MembershipsFacadeService } from './memberships-facade.service'
import { ConfirmationService } from 'primeng/api'
import { inputLanguage } from 'src/app/shared/validators'

const membershipLevel = [
  { value: 0, label: 'Country' },
  { value: 1, label: 'Regional' },
  { value: 2, label: 'Global' },
]

@Component({
  selector: 'app-memberships',
  templateUrl: './memberships.component.html',
  styleUrls: ['./memberships.component.scss'],
  providers: [ConfirmationService],
})
export class MembershipsComponent implements OnInit {
  membershipForm: FormGroup
  mainForm: FormGroup
  memberships: Array<IMembership> = []
  isFormSubmitting = false
  ready = false
  editingMembership: IMembership = {}

  activeOptions = [
    { value: true, label: $localize`Yes` },
    { value: false, label: $localize`No` },
  ]

  get membershipLevel(): any[] {
    return membershipLevel
  }

  private _showMembershipDialog = false
  isDisabled: boolean

  constructor(
    @Inject(LOCALE_ID) public locale: string,
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private facadeService: MembershipsFacadeService,
    private confirmationService: ConfirmationService
  ) {}

  async ngOnInit() {
    this.buildForm()

    try {
      await Promise.all([
        this.facadeService.loadMemberships().then((memberships: Array<IMembership>) => {
          this.memberships = memberships
        }),
        this.facadeService.getIsActiveMember().then((isActiveMember) => {
          this.mainForm.patchValue({ isActiveMember })
          this.isDisabled = !isActiveMember
        }),
      ])
    } catch (error) {
      // TODO
    } finally {
      this.ready = true
    }
  }

  openMembershipDialog() {
    this._showMembershipDialog = true
  }

  get showMembershipDialog() {
    return this._showMembershipDialog
  }

  set showMembershipDialog(show: boolean) {
    this._showMembershipDialog = show
    if (!show) {
      this.editingMembership = {}
      this.membershipForm.reset()
    }
  }

  private buildForm(): void {
    const membershipLevel = new FormControl('', [Validators.required])
    this.mainForm = new FormGroup({
      isActiveMember: new FormControl('', [Validators.required]),
    })

    this.membershipForm = new FormGroup({
      institutionName: new FormControl('', [Validators.required, inputLanguage('en')]),
      institutionName_AR: new FormControl('', [Validators.required, inputLanguage('ar')]),
      active: new FormControl('', [Validators.required]),
      roleName: new FormControl('', [Validators.required, inputLanguage('en')]),
      roleName_AR: new FormControl('', [Validators.required, inputLanguage('ar')]),
      joinDate: new FormControl('', [Validators.required]),
      membershipLevel,
    })
  }

  async addMembership(): Promise<void> {
    try {
      const formValues = this.membershipForm.value as IMembership
      const id = await this.facadeService.addMembership(formValues)
      this.showMembershipDialog = false

      this.memberships.push({ id, ...formValues })
    } catch (error) {
      // TODO
    } finally {
      this.isFormSubmitting = false
    }
  }

  async removeMembership(id: string): Promise<void> {
    this.confirmationService.confirm({
      message: $localize`Are you sure?`,
      accept: async () => {
        await this.facadeService.removeMembership(id)
        this.memberships = this.memberships.filter((membership: any) => {
          return membership.id !== id
        })
      },
    })
  }

  openMembershipDialogForEditing(membership: IMembership) {
    this.editingMembership = membership
    this.membershipForm.patchValue(membership)
    this.showMembershipDialog = true
  }

  async updateMembership() {
    this.isFormSubmitting = true
    const formValues = this.membershipForm.value

    await this.facadeService.updateMembership(this.editingMembership.id, formValues)

    Object.assign(this.editingMembership, formValues)
    this.showMembershipDialog = false
    this.isFormSubmitting = false
  }

  async goToNextForm() {
    if (this.mainForm.dirty) {
      const isActiveMember = this.mainForm.get('isActiveMember').value as boolean
      await this.facadeService.setIsActiveMember(!!isActiveMember)
    }

    this.router.navigate(['../achievements'], { relativeTo: this.activatedRoute })
  }

  selectOption(option) {
    this.isDisabled = !option.value
  }
}
