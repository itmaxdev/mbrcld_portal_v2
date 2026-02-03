import { NgModule } from '@angular/core'
import { RouterModule, Routes } from '@angular/router'
import { ChangeEmailComponent } from './change-email/change-email.component'
import { ChangePasswordComponent } from './change-password/change-password.component'
import { AccountSettingsComponent } from './account-settings.component'
import { LogoutComponent } from './logout/logout.component'
import { DeleteAccountComponent } from './delete-account/delete-account.component'

const routes: Routes = [
  {
    path: '',
    component: AccountSettingsComponent,
    canActivate: [],
    children: [
      {
        path: 'change-email',
        component: ChangeEmailComponent,
      },
      {
        path: 'delete-account',
        component: DeleteAccountComponent,
      },
      {
        path: 'change-password',
        component: ChangePasswordComponent,
      },
      {
        path: 'logout',
        component: LogoutComponent,
      },
      {
        path: '',
        pathMatch: 'full',
        redirectTo: 'change-email',
      },
      {
        path: '**',
        redirectTo: 'change-email',
      },
    ],
  },
]

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AccountSettingsRoutingModule {}
