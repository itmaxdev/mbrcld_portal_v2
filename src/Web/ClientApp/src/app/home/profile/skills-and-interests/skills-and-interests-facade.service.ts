import { ISkillAndInterest } from './models'
import { ProfileFacade } from '../common/profile-facade.service'
import { Injectable } from '@angular/core'
import { tap } from 'rxjs/operators'
import {
  SkillsAndInterestsClient,
  ListUserSkillsAndInterestsViewModel,
  AddInterestCommand,
} from 'src/app/shared/api.generated.clients'

@Injectable({
  providedIn: 'root',
})
export class SkillAndInterestFacadeService {
  constructor(private client: SkillsAndInterestsClient, private profileFacade: ProfileFacade) {}

  addSkillAndInterest(request: ISkillAndInterest): Promise<string> {
    return this.client
      .skillsAndInterestsPost(new AddInterestCommand(request))
      .pipe(
        tap((id) => {
          this.profileFacade.loadFormProgress()
        })
      )
      .toPromise()
  }

  removeSkillAndInterest(id: string): Promise<void> {
    return this.client
      .skillsAndInterestsDelete(id)
      .pipe(
        tap(() => {
          this.profileFacade.loadFormProgress()
        })
      )
      .toPromise()
  }

  loadSkillAndInterest(): Promise<ListUserSkillsAndInterestsViewModel[]> {
    return this.client.skillsAndInterestsGet().toPromise()
  }
}
