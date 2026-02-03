import { ProfileFacade } from '../common/profile-facade.service'
import { Injectable } from '@angular/core'
import { tap } from 'rxjs/operators'
import {
  ProfileClient,
  IEditUserLearningPreferencesCommand,
  EditUserLearningPreferencesCommand,
  IGetUserLearningPreferencesViewModel,
} from 'src/app/shared/api.generated.clients'

@Injectable({
  providedIn: 'root',
})
export class LearningPreferencesFacadeService {
  constructor(private profileClient: ProfileClient, private profileFacade: ProfileFacade) {}

  submitLearningPreferences(request: IEditUserLearningPreferencesCommand): Promise<any> {
    return this.profileClient
      .learningPreferencesPost(new EditUserLearningPreferencesCommand(request))
      .pipe(
        tap(() => {
          this.profileFacade.loadFormProgress()
        })
      )
      .toPromise()
  }

  loadLearningPreferences(): Promise<IGetUserLearningPreferencesViewModel> {
    return this.profileClient.learningPreferencesGet().toPromise()
  }
}
