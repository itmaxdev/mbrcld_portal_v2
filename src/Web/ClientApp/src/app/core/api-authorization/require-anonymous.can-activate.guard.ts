import { AuthorizationService } from './authorization.service'
import { Injectable } from '@angular/core'
import { CanActivate, Router } from '@angular/router'
import { Observable } from 'rxjs'
import { map } from 'rxjs/operators'

@Injectable()
export class RequireAnonymousCanActivateGuard implements CanActivate {
  constructor(private authService: AuthorizationService, private router: Router) {}

  canActivate(): Observable<boolean> {
    return this.authService.checkIfCurrentlyAuthenticated().pipe(
      map((isAuthenticated) => {
        if (isAuthenticated) {
          this.router.navigate(['/'])
        }
        return !isAuthenticated
      })
    )
  }
}
