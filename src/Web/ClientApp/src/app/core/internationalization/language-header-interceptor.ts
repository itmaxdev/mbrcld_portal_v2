import { Inject, Injectable, LOCALE_ID } from '@angular/core'
import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http'
import { Observable } from 'rxjs'

@Injectable()
export class LanguageHeaderInterceptor implements HttpInterceptor {
  constructor(@Inject(LOCALE_ID) private locale: string) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(
      req.clone({
        headers: req.headers.set('X-Accept-Language', this.getLocale()),
      })
    )
  }

  private getLocale(): string {
    if (this.locale === 'ar') {
      return 'ar-SA'
    } else {
      return 'en-US'
    }
  }
}
