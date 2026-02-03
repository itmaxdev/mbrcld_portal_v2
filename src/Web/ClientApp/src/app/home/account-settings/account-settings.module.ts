import { NgModule } from '@angular/core'
import { SharedModule } from 'src/app/shared/shared.module'
import { AccountSettingsRoutingModule } from './account-settings-routing.module'
import { AccountSettingsComponent } from './account-settings.component'
import { ChangeEmailComponent } from './change-email/change-email.component'
import { ChangePasswordComponent } from './change-password/change-password.component'
import { CaptchaModule } from 'primeng/captcha'
import { LogoutComponent } from './logout/logout.component'
import { DeleteAccountComponent } from './delete-account/delete-account.component'
import { ConfirmationService } from 'primeng/api'

@NgModule({
  declarations: [
    AccountSettingsComponent,
    ChangeEmailComponent,
    ChangePasswordComponent,
    DeleteAccountComponent,
    LogoutComponent,
  ],
  imports: [AccountSettingsRoutingModule, SharedModule, CaptchaModule],
  providers: [ConfirmationService]
})
export class AccountSettingsModule {}
