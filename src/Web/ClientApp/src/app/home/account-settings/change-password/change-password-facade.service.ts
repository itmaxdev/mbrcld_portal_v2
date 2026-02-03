import { Injectable } from '@angular/core'
import {
  AccountClient,
  ChangePasswordCommand,
  IChangePasswordCommand,
} from 'src/app/shared/api.generated.clients'

@Injectable({
  providedIn: 'root',
})
export class ChangePasswordFacadeService {
  constructor(private client: AccountClient) {}

  changePassword(request: IChangePasswordCommand) {
    return this.client.changePassword(new ChangePasswordCommand(request)).toPromise()
  }
}
