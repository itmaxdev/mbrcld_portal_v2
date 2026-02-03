import { AuthorizationService } from './authorization.service'
import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http'
// import { Injectable, Injector, isDevMode } from '@angular/core'
import { Injectable, Injector } from '@angular/core'
import { from, Observable } from 'rxjs'
import { switchMap, tap } from 'rxjs/operators'
import { Router } from '@angular/router'

const UnprotectedRoutes = ['/api/register', '/api/metadata', '/api/reset-password']

@Injectable()
export class AuthorizationHeaderInterceptor implements HttpInterceptor {
  private _authService?: AuthorizationService = undefined

  private get authService(): AuthorizationService {
    if (!this._authService) {
      this._authService = this.injector.get(AuthorizationService)
    }
    return this._authService
  }

  constructor(private injector: Injector, private router: Router) { }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    if (req.url.indexOf('/api/') === -1 || this.isUnprotectedRoute(req.url)) {
      return next.handle(req)
    }

    return from(this.authService.getValidAccessToken()).pipe(
      tap({
        error: async () => {
          const currentRoute = this.router.routerState.snapshot.url
          await this.authService.logout()
          await this.authService.login(currentRoute)
        },
      }),
      switchMap((accessToken) => {
        const request = req.clone({
          headers: req.headers.set('Authorization', `Bearer ${accessToken}`),
        })
        return next.handle(request)
      })
    )
  }

  private isUnprotectedRoute(url: string) {
    return UnprotectedRoutes.some((route) => url.indexOf(route) !== -1)
  }
}
