import { NgModule } from '@angular/core'
import { RouterModule, Routes } from '@angular/router'
import { LoginComponent } from 'src/app/authorization/login/login.component'
import { PasswordResetComponent } from 'src/app/authorization/password-reset/password-reset.component'
import { RegisterComponent } from 'src/app/authorization/register/register.component'
import { RequireAnonymousCanActivateGuard } from 'src/app/core/api-authorization'

const loginRoutes: Routes = [
  {
    path: 'login',
    component: LoginComponent,
    canActivate: [RequireAnonymousCanActivateGuard],
  },
  {
    path: 'register',
    component: RegisterComponent,
    canActivate: [RequireAnonymousCanActivateGuard],
  },
  {
    path: 'password-reset',
    component: PasswordResetComponent,
    canActivate: [],
  },
  {
    path: '',
    pathMatch: 'full',
    redirectTo: 'login',
  },
]

@NgModule({
  imports: [RouterModule.forChild(loginRoutes)],
  exports: [RouterModule],
})
export class AuthorizationRoutingModule {}
