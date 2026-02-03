import { IEducationQualification } from './models'
import { Injectable } from '@angular/core'
import {
  EducationQualificationsClient,
  AddEducationQualificationCommand,
} from 'src/app/shared/api.generated.clients'
import { ProfileFacade } from '../common/profile-facade.service'
import { tap } from 'rxjs/operators'

@Injectable({
  providedIn: 'root',
})
export class EducationFacadeService {
  constructor(
    private educationClient: EducationQualificationsClient,
    private profileFacade: ProfileFacade
  ) {}

  loadEducationQualifications(): Promise<IEducationQualification[]> {
    return this.educationClient.educationQualificationsGet().toPromise()
  }

  addEducationQualification(request: IEducationQualification): Promise<string> {
    return this.educationClient
      .educationQualificationsPost(new AddEducationQualificationCommand(request))
      .pipe(
        tap((id) => {
          this.profileFacade.loadFormProgress()
        })
      )
      .toPromise()
  }

  updateEducation(id, request: IEducationQualification): Promise<void> {
    return this.educationClient
      .educationQualificationsPut(id, new AddEducationQualificationCommand(request))
      .toPromise()
  }

  removeEducationQualification(id: string): Promise<void> {
    return this.educationClient
      .educationQualificationsDelete(id)
      .pipe(
        tap(() => {
          this.profileFacade.loadFormProgress()
        })
      )
      .toPromise()
  }
}
