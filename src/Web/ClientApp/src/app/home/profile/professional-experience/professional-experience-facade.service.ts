import { IIndustryOption, IProfessionalExperience, ISectorOption } from './models'
import { ProfileFacade } from '../common/profile-facade.service'
import { Injectable } from '@angular/core'
import { of } from 'rxjs'
import { map, tap } from 'rxjs/operators'
import {
  AddProfessionalExperienceCommand,
  ProfessionalExperiencesClient,
  MetadataClient,
} from 'src/app/shared/api.generated.clients'

@Injectable({
  providedIn: 'root',
})
export class ProfessionalExperienceFacadeService {
  private cachedIndustryOptions: IIndustryOption[] = null
  private cachedSectorOptions: ISectorOption[] = null

  constructor(
    private client: ProfessionalExperiencesClient,
    private metadata: MetadataClient,
    private profileFacade: ProfileFacade
  ) {}

  updateFormProgress() {
    this.profileFacade.loadFormProgress()
  }

  addProfessionalExperience(request: IProfessionalExperience): Promise<string> {
    return this.client
      .professionalExperiencesPost(new AddProfessionalExperienceCommand(request))
      .pipe(
        tap((id) => {
          this.profileFacade.loadFormProgress()
        })
      )
      .toPromise()
  }

  updateProfessionalExperience(id: string, request: IProfessionalExperience): Promise<any> {
    return this.client
      .professionalExperiencesPut(id, new AddProfessionalExperienceCommand(request))
      .toPromise()
  }

  removeProfessionalExperience(id: string): Promise<void> {
    return this.client
      .professionalExperiencesDelete(id)
      .pipe(
        tap(() => {
          this.profileFacade.loadFormProgress()
        })
      )
      .toPromise()
  }

  loadProfessionalExperiences(): Promise<IProfessionalExperience[]> {
    return this.client.professionalExperiencesGet().toPromise()
  }

  loadIndustryOptions(): Promise<IIndustryOption[]> {
    if (this.cachedIndustryOptions) {
      return of(this.cachedIndustryOptions).toPromise()
    }
    return this.metadata
      .industries()
      .pipe(
        map((options) => options.map((o) => ({ label: o.label, value: o.id }))),
        tap((options) => (this.cachedIndustryOptions = options))
      )
      .toPromise()
  }

  loadSectorOptions(): Promise<ISectorOption[]> {
    if (this.cachedSectorOptions) {
      return of(this.cachedSectorOptions).toPromise()
    }
    return this.metadata
      .sectors()
      .pipe(
        map((options) => options.map((o) => ({ label: o.label, value: o.id }))),
        tap((options) => (this.cachedSectorOptions = options))
      )
      .toPromise()
  }
}
