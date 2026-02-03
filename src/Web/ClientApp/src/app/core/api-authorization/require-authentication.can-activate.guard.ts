import { AuthorizationService } from './authorization.service'
import { Injectable } from '@angular/core'
import { ActivatedRouteSnapshot, CanActivate, RouterStateSnapshot } from '@angular/router'
import { Observable } from 'rxjs'
import { tap } from 'rxjs/operators'

@Injectable()
export class RequireAuthenticationCanActivateGuard implements CanActivate {
  constructor(private authService: AuthorizationService) {}

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> {
    return this.authService.checkIfCurrentlyAuthenticated().pipe(
      tap((isAuthenticated) => {
        if (!isAuthenticated) {
          this.authService.login(state.url)
        }
      })
    )
  }
}
