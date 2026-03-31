import { Component, OnInit } from '@angular/core'
import { Location } from '@angular/common'
import { ConfirmationService, MessageService } from 'primeng/api'
import { AccountClient, DeleteAccountCommand } from 'src/app/shared/api.generated.clients'
import { AuthorizationService } from 'src/app/core/api-authorization'

@Component({
  selector: 'app-delete-account',
  templateUrl: './delete-account.component.html',
  styleUrls: ['./delete-account.component.scss'],
  providers: [ConfirmationService, MessageService],
})
export class DeleteAccountComponent {
  userRole: number
  constructor(
    private _location: Location,
    private accountClient: AccountClient,
    private authService: AuthorizationService,
    private messageService: MessageService,
    private confirmationService: ConfirmationService
  ) {
    const raw = localStorage.getItem('profile_info')
    const profileInfo = raw ? JSON.parse(raw) : null
    this.userRole = parseInt(String(profileInfo?.role ?? 0))
  }

  ngOnInit(): void {}

  goBack() {
    this._location.back()
  }

  deleteAccount() {
    const profileInfo = JSON.parse(localStorage.getItem('profile_info'))
    if (profileInfo) {
      this.accountClient
        .deleteAccount({ email: profileInfo.email } as DeleteAccountCommand)
        .subscribe(
          (res) => {
            this.messageService.add({
              severity: 'info',
              summary: 'Succees',
              detail: $localize`User deleted successfully.`,
              sticky: true,
            })
            this.authService.logout()
            localStorage.removeItem('profile_info')
            localStorage.removeItem('uaeCode')
            localStorage.removeItem('eliteclubId')
            this.authService.login()
          },
          (error: any) => {
            this.messageService.add({
              severity: 'error',
              summary: $localize`Error`,
              detail: $localize`Something is wrong.`,
              life: 10000,
            })
            throw error
          }
        )
    }
  }

  confirm(saveType: number) {
    this.confirmationService.confirm({
      message: $localize`Are you sure you want to delete this account?`,
      header: $localize`Confirmation`,
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.deleteAccount()
      },
      reject: () => {},
    })
  }
}
