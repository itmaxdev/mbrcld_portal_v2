import { Injectable } from '@angular/core'
import {
  RegisterClient,
  RegisterNewUserCommand,
  IRegisterNewUserCommand,
} from 'src/app/shared/api.generated.clients'

export { ApiProblemDetails, IRegisterNewUserCommand } from 'src/app/shared/api.generated.clients'

@Injectable()
export class RegisterService {
  constructor(private client: RegisterClient) {}

  register(data: IRegisterNewUserCommand): Promise<void> {
    return this.client.register(new RegisterNewUserCommand(data)).toPromise()
  }
}
