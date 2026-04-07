import { Injectable } from '@angular/core'
import { BehaviorSubject, Observable, Subject } from 'rxjs'
import { first, tap, shareReplay, finalize } from 'rxjs/operators'
import {
  IUserContactDetails,
  IUserGeneralInformation,
  IUserIdentityDetails,
} from '../common/profile.models'
import {
  EditUserContactDetailsCommand,
  EditUserGeneralInformationCommand,
  EditUserIdentityDetailsCommand,
  ProfileClient,
  IGetUserProfileViewModel,
  IListUserDocumentsViewModel,
  UserDocumentsClient,
  GetUserProfileCompletionViewModel,
  SetIsActiveMemberCommand,
} from 'src/app/shared/api.generated.clients'
import { GlobalVariablesService } from '../../../shared/services/global-variables.service'

export { IGetUserProfileViewModel } from 'src/app/shared/api.generated.clients'

@Injectable({
  providedIn: 'root',
})
export class ProfileFacade {
  private profileSubject$ = new Subject<IGetUserProfileViewModel>()
  private progressBarSubject$ = new Subject<GetUserProfileCompletionViewModel>()

  private cachedProfile: IGetUserProfileViewModel = null
  private cachedDocuments$: Observable<IListUserDocumentsViewModel[]>

  private isLoadingProfile = false
  private isLoadingProgress = false

  profilePic$ = new BehaviorSubject<string>(null)

  get profilePicChanges(): Observable<string> {
    return this.profilePic$.asObservable()
  }

  get profileChanges(): Observable<IGetUserProfileViewModel> {
    return this.profileSubject$.asObservable()
  }

  get formChanges(): Observable<GetUserProfileCompletionViewModel> {
    return this.progressBarSubject$.asObservable()
  }

  constructor(
    private globalVariables: GlobalVariablesService,
    private profileClient: ProfileClient,
    private documentsClient: UserDocumentsClient
  ) {}

  private setProfilePictureUrl(url?: string | null) {
    const normalized = this.normalizeProfilePictureUrl(url)
    this.profilePic$.next(normalized)

    // Keep localStorage in sync because multiple components read from it directly.
    try {
      const raw = localStorage.getItem('profile_info')
      if (!raw) return
      const info = JSON.parse(raw)
      info.profilePictureUrl = normalized
      localStorage.setItem('profile_info', JSON.stringify(info))
    } catch {
      // ignore storage errors
    }
  }

  private normalizeProfilePictureUrl(url?: string | null): string | null {
    if (!url) return null

    // Add a cache-buster so the browser doesn't keep a previously failed 404 cached.
    const cacheBuster = Date.now()
    if (url.includes('?')) {
      return url.replace(/([?&])v=\d+/, `$1v=${cacheBuster}`)
    }
    return `${url}?v=${cacheBuster}`
  }

  loadGeneralInformation(): Promise<IUserGeneralInformation> {
    return this.loadProfile()
  }

  storeGeneralInformation(request: IUserGeneralInformation): Promise<void> {
    this.invalidateCachedProfile()
    return this.profileClient
      .generalInformation(new EditUserGeneralInformationCommand(request))
      .toPromise()
      .then(() => {
        this.resetProfileIsOutDated()
        this.loadFormProgress()
      })
  }

  loadContactDetails(): Promise<IUserContactDetails> {
    return this.loadProfile()
  }

  storeContactDetails(request: IUserContactDetails): Promise<void> {
    this.invalidateCachedProfile()
    return this.profileClient
      .contactDetails(new EditUserContactDetailsCommand(request))
      .toPromise()
      .then(() => {
        this.resetProfileIsOutDated()
        this.loadFormProgress()
      })
  }

  loadIdentityDetails(): Promise<IUserIdentityDetails> {
    return this.loadProfile()
  }

  storeIdentity(request: IUserIdentityDetails): Promise<void> {
    this.invalidateCachedProfile()
    return this.profileClient
      .identityDetails(new EditUserIdentityDetailsCommand(request))
      .toPromise()
  }

  loadUserDocuments(): Promise<IListUserDocumentsViewModel[]> {
    if (!this.cachedDocuments$) {
      this.cachedDocuments$ = this.documentsClient.documentsGet().pipe(shareReplay(1))
    }
    return this.cachedDocuments$.toPromise()
  }

  reloadUserDocuments(): Promise<IListUserDocumentsViewModel[]> {
    this.cachedDocuments$ = null
    return this.loadUserDocuments()
  }

  removeUserDocument(identifier: string): Promise<void> {
    return this.documentsClient
      .documentsDelete(identifier)
      .toPromise()
      .then(() => {
        this.resetProfileIsOutDated()
        this.loadFormProgress()
      })
  }

  getIsActiveMember(): Promise<boolean> {
    return this.loadProfile().then((profile) => profile.isActiveMember)
  }

  setIsActiveMember(isActiveMember: boolean): Promise<void> {
    this.invalidateCachedProfile()
    return this.profileClient
      .isActiveMember(new SetIsActiveMemberCommand({ isActiveMember }))
      .toPromise()
      .then(() => {
        this.resetProfileIsOutDated()
        this.loadFormProgress()
      })
  }

  removeProfilePicture(): Promise<void> {
    this.invalidateCachedProfile()
    return this.profileClient
      .profilePictureDelete()
      .toPromise()
      .then(() => {
        this.setProfilePictureUrl(null)
      })
  }

  reloadProfile(): Promise<IGetUserProfileViewModel> {
    this.invalidateCachedProfile()
    return this.loadProfile()
  }

  loadFormProgress(): Promise<GetUserProfileCompletionViewModel> {
    if (this.isLoadingProgress) {
      return this.progressBarSubject$.pipe(first()).toPromise()
    }

    this.isLoadingProgress = true

    return this.profileClient
      .profileCompletion()
      .pipe(
        tap((formProgress) => {
          this.progressBarSubject$.next(formProgress)
          this.globalVariables.isProfilePercentageCompleted =
            formProgress.completionPercentage == 100
        }),
        finalize(() => {
          this.isLoadingProgress = false
        })
      )
      .toPromise()
  }

  async loadProfile(): Promise<IGetUserProfileViewModel> {
    if (this.cachedProfile) {
      // Ensure subscribers/localStorage still receive the cached picture URL.
      this.setProfilePictureUrl(this.cachedProfile.profilePictureUrl)
      return this.cachedProfile
    }

    if (this.isLoadingProfile) {
      return this.profileSubject$.pipe(first()).toPromise()
    }

    this.isLoadingProfile = true

    return this.profileClient
      .profile()
      .pipe(
        tap((profile) => {
          this.cachedProfile = profile
          this.profileSubject$.next(profile)
          this.setProfilePictureUrl(profile.profilePictureUrl)
        }),
        finalize(() => {
          this.isLoadingProfile = false
        })
      )
      .toPromise()
  }

  private invalidateCachedProfile() {
    this.cachedProfile = null
  }

  private resetProfileIsOutDated() {
    return this.profileClient
      .resetProfileIsOutDated()
      .pipe(
        tap(() => {
          this.globalVariables.profileIsOutDated = false
        })
      )
      .toPromise()
  }
}
