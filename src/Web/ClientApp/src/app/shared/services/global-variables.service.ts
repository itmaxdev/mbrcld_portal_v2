import { Injectable } from '@angular/core'

@Injectable({
  providedIn: 'root',
})
export class GlobalVariablesService {
  public roomId: string
  public previousUser: string
  public groupParticipantMessages = true
  public isProfilePercentageCompleted = null
  public profileIsOutDated = false
}
