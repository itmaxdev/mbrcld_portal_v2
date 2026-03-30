import { Injectable } from '@angular/core'
import { CanActivate, ActivatedRouteSnapshot, Router } from '@angular/router'

@Injectable({
  providedIn: 'root',
})
export class RouteGuardCanActivateGuard implements CanActivate {
  constructor() {}

  public canActivate(next: ActivatedRouteSnapshot) {
    const raw = localStorage.getItem('profile_info')
    if (!raw) return false

    const profileInfo = JSON.parse(raw)
    const role = profileInfo?.role
    if (role == null) return false
    return role == Number(next.data.role)
  }
}

@Injectable({
  providedIn: 'root',
})
export class DefaultRouteGuard implements CanActivate {
  constructor(private router: Router) {}

  public canActivate(next: ActivatedRouteSnapshot) {
    if (localStorage.getItem('profile_info')) {
      const raw = localStorage.getItem('profile_info')
      const profileInfo = raw ? JSON.parse(raw) : null
      const role = profileInfo?.role
      if (role == null) return false
      switch (role) {
        case 1:
          this.router.navigate(['/registrant'])
          break
        case 2:
          this.router.navigate(['/applicant'])
          break
        case 3:
          this.router.navigate(['/alumni'])
          break
        case 4:
          this.router.navigate(['/instructor'])
          break
        case 5:
          this.router.navigate(['/direct-manager'])
          break
        case 6:
          this.router.navigate(['/admin'])
          break
      }
    }
    return false
  }
}
