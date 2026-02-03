import { ITrainingCourse } from './models'
import { Injectable } from '@angular/core'
import {
  AddTrainingCourseCommand,
  TrainingCoursesClient,
} from 'src/app/shared/api.generated.clients'
import { ProfileFacade } from '../common/profile-facade.service'
import { tap } from 'rxjs/operators'

@Injectable({
  providedIn: 'root',
})
export class TrainingCoursesFacadeService {
  constructor(
    private trainingCoursesClient: TrainingCoursesClient,
    private profileFacade: ProfileFacade
  ) {}

  loadTrainingCourses(): Promise<ITrainingCourse[]> {
    return this.trainingCoursesClient.trainingCoursesGet().toPromise()
  }

  addTrainingCourse(request: ITrainingCourse): Promise<string> {
    return this.trainingCoursesClient
      .trainingCoursesPost(new AddTrainingCourseCommand(request))
      .pipe(
        tap((id) => {
          this.profileFacade.loadFormProgress()
        })
      )
      .toPromise()
  }

  updateTrainingCourse(id: string, request: ITrainingCourse): Promise<void> {
    return this.trainingCoursesClient
      .trainingCoursesPut(id, new AddTrainingCourseCommand(request))
      .toPromise()
  }

  removeTrainingCourse(id: string): Promise<void> {
    return this.trainingCoursesClient
      .trainingCoursesDelete(id)
      .pipe(
        tap(() => {
          this.profileFacade.loadFormProgress()
        })
      )
      .toPromise()
  }
}
