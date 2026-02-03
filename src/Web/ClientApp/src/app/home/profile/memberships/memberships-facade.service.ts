import { IMembership } from './memberships.interface'
import { ProfileFacade } from '../common/profile-facade.service'
import { Injectable } from '@angular/core'
import { tap } from 'rxjs/operators'
import { MembershipsClient, EditMembershipCommand } from 'src/app/shared/api.generated.clients'

@Injectable({
  providedIn: 'root',
})
export class MembershipsFacadeService {
  constructor(private membershipsClient: MembershipsClient, private profileFacade: ProfileFacade) {}

  setIsActiveMember(isActiveMember: boolean): Promise<void> {
    return this.profileFacade.setIsActiveMember(isActiveMember)
  }

  getIsActiveMember(): Promise<boolean> {
    return this.profileFacade.getIsActiveMember()
  }

  loadMemberships(): Promise<Array<IMembership>> {
    return this.membershipsClient.membershipsGet().toPromise()
  }

  addMembership(request: any): Promise<string> {
    return this.membershipsClient
      .membershipsPost(request)
      .pipe(
        tap(() => {
          this.profileFacade.loadFormProgress()
        })
      )
      .toPromise()
  }

  updateMembership(id: string, request: IMembership): Promise<void> {
    return this.membershipsClient.membershipsPut(id, new EditMembershipCommand(request)).toPromise()
  }

  removeMembership(id: string): Promise<void> {
    return this.membershipsClient
      .membershipsDelete(id)
      .pipe(
        tap(() => {
          this.profileFacade.loadFormProgress()
        })
      )
      .toPromise()
  }
}
