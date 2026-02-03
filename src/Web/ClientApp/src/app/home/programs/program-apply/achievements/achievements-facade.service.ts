import { IAchievement } from './achievements.interface'
import { Injectable } from '@angular/core'
import { AchievementsClient, AddAchievementCommand } from 'src/app/shared/api.generated.clients'

@Injectable({
  providedIn: 'root',
})
export class AchievementsFacadeService {
  constructor(private achievementsClient: AchievementsClient) {}

  loadAchievements(): Promise<IAchievement[]> {
    return this.achievementsClient.achievementsGet().toPromise()
  }

  addAchievement(request: IAchievement): Promise<string> {
    return this.achievementsClient.achievementsPost(new AddAchievementCommand(request)).toPromise()
  }

  updateReference(id, request: IAchievement): Promise<void> {
    return this.achievementsClient
      .achievementsPut(id, new AddAchievementCommand(request))
      .toPromise()
  }

  removeReference(id: string): Promise<void> {
    return this.achievementsClient.achievementsDelete(id).toPromise()
  }
}
