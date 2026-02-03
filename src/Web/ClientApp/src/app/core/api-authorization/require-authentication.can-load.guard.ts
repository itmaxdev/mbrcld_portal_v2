import { AuthorizationService } from './authorization.service'
import { Injectable } from '@angular/core'
import { CanLoad, Route, UrlSegment } from '@angular/router'
import { Observable } from 'rxjs'
import { tap } from 'rxjs/operators'

@Injectable()
export class RequireAuthenticationCanLoadGuard implements CanLoad {
  constructor(private authService: AuthorizationService) {}

  canLoad(route: Route, segments: UrlSegment[]): Observable<boolean> {
    return this.authService.checkIfCurrentlyAuthenticated().pipe(
      tap((isAuthenticated) => {
        if (!isAuthenticated) {
          const returnUrl = segments.map((s) => s.path).join('/')
          this.authService.login(returnUrl)
        }
      })
    )
  }
}
