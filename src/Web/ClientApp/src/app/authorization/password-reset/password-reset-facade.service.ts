import { IResetPassword } from './model'
import { Injectable } from '@angular/core'
import { ResetPasswordClient, ResetPasswordCommand } from 'src/app/shared/api.generated.clients'

@Injectable({
  providedIn: 'root',
})
export class PasswordResetFacadeService {
  constructor(private client: ResetPasswordClient) {}

  resetPassword(request: IResetPassword) {
    return this.client.resetPassword(new ResetPasswordCommand(request)).toPromise()
  }
}
