import { AuthorizationRoutingModule } from './authorization-routing.module'
import { LoginComponent } from './login/login.component'
import { RegisterComponent } from './register/register.component'
import { SharedModule } from '../shared/shared.module'
import { NgModule } from '@angular/core'
import { ReactiveFormsModule } from '@angular/forms'
import { RegisterService } from './register/register.service'
import { PasswordResetComponent } from './password-reset/password-reset.component'
import { CaptchaModule } from 'primeng/captcha'

@NgModule({
  declarations: [LoginComponent, RegisterComponent, PasswordResetComponent],
  imports: [AuthorizationRoutingModule, ReactiveFormsModule, SharedModule, CaptchaModule],
  providers: [RegisterService],
})
export class AuthorizationModule {}
