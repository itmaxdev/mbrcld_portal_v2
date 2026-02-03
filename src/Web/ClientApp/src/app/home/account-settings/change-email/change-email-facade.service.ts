import { Injectable } from '@angular/core'
import {
  AccountClient,
  ChangeEmailCommand,
  IChangeEmailCommand,
} from 'src/app/shared/api.generated.clients'

@Injectable({
  providedIn: 'root',
})
export class ChangeEmailFacadeService {
  constructor(private client: AccountClient) {}

  changeEmail(request: IChangeEmailCommand) {
    return this.client.changeEmail(new ChangeEmailCommand(request)).toPromise()
  }
}
