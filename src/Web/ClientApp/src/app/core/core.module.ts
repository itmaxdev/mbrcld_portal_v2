import { LanguageHeaderInterceptor } from './internationalization'
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http'
import { Inject, LOCALE_ID, NgModule, Optional, SkipSelf } from '@angular/core'
import { OAuthModule, OAuthStorage } from 'angular-oauth2-oidc'
import {
  AuthorizationHeaderInterceptor,
  RequireAnonymousCanActivateGuard,
  RequireAuthenticationCanActivateGuard,
  RequireAuthenticationCanLoadGuard,
  SecureStorage,
} from './api-authorization'

@NgModule({
  imports: [HttpClientModule, OAuthModule.forRoot()],
  providers: [
    RequireAnonymousCanActivateGuard,
    RequireAuthenticationCanActivateGuard,
    RequireAuthenticationCanLoadGuard,
    {
      provide: OAuthStorage,
      useClass: SecureStorage,
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthorizationHeaderInterceptor,
      multi: true,
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: LanguageHeaderInterceptor,
      multi: true,
    },
  ],
})
export class CoreModule {
  constructor(@Optional() @SkipSelf() core: CoreModule, @Inject(LOCALE_ID) locale) {
    if (core) {
      throw new Error('CoreModule should be imported only in the root module')
    }

    if (locale === 'ar') {
      document.querySelector('html').setAttribute('dir', 'rtl')
    } else {
      document.querySelector('html').setAttribute('dir', 'ltr')
    }
  }
}
