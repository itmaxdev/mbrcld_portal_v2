import { ILanguageSkill } from './models'
import { Injectable } from '@angular/core'
import { of } from 'rxjs'
import { map, tap } from 'rxjs/operators'
import {
  LanguageSkillsClient,
  MetadataClient,
  AddLanguageSkillCommand,
  EditLanguageSkillCommand,
} from 'src/app/shared/api.generated.clients'
import { ProfileFacade } from '../common/profile-facade.service'

@Injectable({
  providedIn: 'root',
})
export class LanguagesFacadeService {
  private cachedLanguageOptions: ILanguageOption[] = null
  private cachedLanguageSkills: ILanguageSkill[] = null

  constructor(
    private languageSkillsClient: LanguageSkillsClient,
    private metadataClient: MetadataClient,
    private profileFacade: ProfileFacade
  ) {}

  async loadLanguageSkills(): Promise<ILanguageSkill[]> {
    if (this.cachedLanguageSkills) {
      return this.cachedLanguageSkills
    }
    const skills = await this.languageSkillsClient.languageSkillsGet().toPromise()
    this.cachedLanguageSkills = skills
    return skills
  }

  addLanguage(request: ILanguageSkill): Promise<string> {
    this.invalidateCachedLanguageSkills()
    return this.languageSkillsClient
      .languageSkillsPost(new AddLanguageSkillCommand(request))
      .pipe(
        tap((id) => {
          this.profileFacade.loadFormProgress()
        })
      )
      .toPromise()
  }

  updateLanguage(id: string, request: ILanguageSkill): Promise<void> {
    return this.languageSkillsClient
      .languageSkillsPut(id, new EditLanguageSkillCommand(request))
      .toPromise()
  }

  removeLanguage(id: string): Promise<any> {
    this.invalidateCachedLanguageSkills()
    return this.languageSkillsClient
      .languageSkillsDelete(id)
      .pipe(
        tap(() => {
          this.profileFacade.loadFormProgress()
        })
      )
      .toPromise()
  }

  loadLanguageOptions(): Promise<ILanguageOption[]> {
    if (this.cachedLanguageOptions) {
      return of(this.cachedLanguageOptions).toPromise()
    }

    return this.metadataClient
      .languages()
      .pipe(
        map((options) => options.map((o) => ({ value: o.id, label: o.label }))),
        tap((options) => (this.cachedLanguageOptions = options))
      )
      .toPromise()
  }

  private invalidateCachedLanguageSkills() {
    this.cachedLanguageSkills = null
  }
}

export interface ILanguageOption {
  value: string
  label: string
}

export interface IProficiencyLevelOption {
  value: number
  label: string
}
