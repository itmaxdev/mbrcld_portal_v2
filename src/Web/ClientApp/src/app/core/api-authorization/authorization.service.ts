import { Injectable, isDevMode } from '@angular/core'
import { Router } from '@angular/router'
import { OAuthEvent, OAuthInfoEvent, OAuthService, TokenResponse } from 'angular-oauth2-oidc'
import { BehaviorSubject, from, Observable, of, ReplaySubject, throwError } from 'rxjs'
import { catchError, filter, first, map, shareReplay, skipWhile, switchMap } from 'rxjs/operators'
import { GetUserProfileViewModel } from 'src/app/shared/api.generated.clients'

@Injectable({
  providedIn: 'root',
})
export class AuthorizationService {
  isAuthenticated$: Observable<boolean>
  profileInfo: GetUserProfileViewModel

  private isAuthenticatedSubject$ = new BehaviorSubject<boolean>(false)
  private isDoneLoadingSubject$ = new ReplaySubject<boolean>()
  private isRefreshingAccessTokenSubject$ = new BehaviorSubject<boolean>(false)
  private refreshTokenObservable$: Observable<TokenResponse>

  constructor(private oauthService: OAuthService, private router: Router) {
    this.oauthService.configure({
      oidc: false,
      skipIssuerCheck: true,
      // issuer: isDevMode() ? 'http://portal.mbrcld.ae' : null,
      clientId: 'dev',
      scope: 'openid profile offline_access',
      requireHttps: false,
    })

    this.isAuthenticated$ = this.waitForInit().pipe(
      switchMap(() => this.checkIfCurrentlyAuthenticated()),
      switchMap((authenticated) => {
        if (authenticated) {
          return of(true)
        } else {
          return from(this.getValidAccessToken()).pipe(
            map(() => true),
            catchError(() => of(false))
          )
        }
      })
    )

    this.runInitialLoginSequence()
  }

  checkIfCurrentlyAuthenticated(): Observable<boolean> {
    return this.waitForInit().pipe(
      switchMap(() => this.isAuthenticatedSubject$),
      first()
    )
  }

  login(returnUrl?: string): Promise<void> {
    return this.router
      .navigate(['authorize', 'login'], {
        queryParams: {
          returnUrl,
        },
      })
      .then()
  }

  acquireAccessTokenWithPasswordFlow(username: string, password: string): Promise<void> {
    return this.oauthService.fetchTokenUsingPasswordFlow(username, password).then()
  }

  logout(): Promise<any> {
    return this.oauthService.revokeTokenAndLogout()
  }

  getValidAccessToken(): Promise<string> {
    return this.waitForInit()
      .pipe(
        switchMap(async () => {
          if (this.oauthService.hasValidAccessToken()) {
            return this.oauthService.getAccessToken()
          } else {
            await this.refreshToken()
            return this.oauthService.getAccessToken()
          }
        })
      )
      .toPromise()
  }

  refreshToken(): Promise<void> {
    return this.waitForInit()
      .pipe(
        switchMap(() => this.checkIfCurrentlyRefreshingAccessToken()),
        switchMap((refreshing: boolean) => {
          if (refreshing) {
            return this.refreshTokenObservable$
          }

          if (!this.oauthService.getRefreshToken()) {
            this.router.navigate(['/authorize'])
            return throwError('Refresh token is empty')
          }

          this.refreshTokenObservable$ = from(this.oauthService.refreshToken()).pipe(shareReplay())
          this.isRefreshingAccessTokenSubject$.next(true)
          return this.refreshTokenObservable$
        }),
        switchMap(() => of(undefined))
      )
      .toPromise()
  }

  private async runInitialLoginSequence(): Promise<void> {
    try {
      await this.oauthService.loadDiscoveryDocument()
      if (!this.oauthService.hasValidAccessToken() && this.oauthService.getRefreshToken()) {
        try {
          await this.oauthService.refreshToken()
        } catch (_) {
          // ignore
        }
      }
    } finally {
      this.registerEventListeners()
      this.isAuthenticatedSubject$.next(this.oauthService.hasValidAccessToken())
      this.isDoneLoadingSubject$.next(true)
    }
  }

  private registerEventListeners() {
    this.oauthService.events
      .pipe(
        filter(({ type }: OAuthEvent) => type === 'token_expires'),
        filter(({ info }: OAuthInfoEvent) => info === 'access_token')
      )
      .subscribe(() => {
        this.refreshToken()
      })

    // this.oauthService.events
    //   .pipe(filter((event) => event.type === 'token_received'))
    //   .subscribe(() => {
    //     this.oauthService.loadUserProfile()
    //   })

    this.oauthService.events.subscribe(() => {
      this.isAuthenticatedSubject$.next(this.oauthService.hasValidAccessToken())
    })

    this.oauthService.events
      .pipe(
        filter((event) => event.type === 'token_refreshed' || event.type === 'token_refresh_error')
      )
      .subscribe(() => {
        this.isRefreshingAccessTokenSubject$.next(false)
        this.refreshTokenObservable$ = null
      })

    window.addEventListener('storage', () => {
      this.isAuthenticatedSubject$.next(this.oauthService.hasValidAccessToken())
    })
  }

  private waitForInit(): Observable<void> {
    return this.isDoneLoadingSubject$.pipe(
      skipWhile((done) => !done),
      first(),
      switchMap(() => of(undefined))
    )
  }

  private checkIfCurrentlyRefreshingAccessToken(): Observable<boolean> {
    return this.isRefreshingAccessTokenSubject$.pipe(first())
  }
}
