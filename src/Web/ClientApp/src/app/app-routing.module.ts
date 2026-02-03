import { NgModule } from '@angular/core'
import { RouterModule, Routes } from '@angular/router'
import { RequireAuthenticationCanLoadGuard } from 'src/app/core/api-authorization'
import {
  RouteGuardCanActivateGuard,
  DefaultRouteGuard,
} from './core/api-authorization/route-guard.can-activate.guard'

const appRoutes: Routes = [
  {
    path: 'authorize',
    loadChildren: () =>
      import('./authorization/authorization.module').then((m) => m.AuthorizationModule),
  },
  {
    path: 'registrant',
    canActivate: [RouteGuardCanActivateGuard],
    loadChildren: () => import('./home/home-registrant.module').then((m) => m.HomeRegistrantModule),
    canLoad: [RequireAuthenticationCanLoadGuard],
    data: {
      role: 1,
    },
  },
  {
    path: 'applicant',
    canActivate: [RouteGuardCanActivateGuard],
    loadChildren: () => import('./home/home.module').then((m) => m.HomeModule),
    canLoad: [RequireAuthenticationCanLoadGuard],
    data: {
      role: 2,
    },
  },
  {
    path: 'alumni',
    canActivate: [RouteGuardCanActivateGuard],
    loadChildren: () => import('./home/home-alumni.module').then((m) => m.HomeAlumniModule),
    canLoad: [RequireAuthenticationCanLoadGuard],
    data: {
      role: 3,
    },
  },
  {
    path: 'instructor',
    canActivate: [RouteGuardCanActivateGuard],
    loadChildren: () => import('./home/home-instructor.module').then((m) => m.HomeInstructorModule),
    canLoad: [RequireAuthenticationCanLoadGuard],
    data: {
      role: 4,
    },
  },
  {
    path: 'direct-manager',
    canActivate: [RouteGuardCanActivateGuard],
    loadChildren: () =>
      import('./home/home-direct-manager.module').then((m) => m.HomeDirectManagerModule),
    canLoad: [RequireAuthenticationCanLoadGuard],
    data: {
      role: 5,
    },
  },
  {
    path: 'admin',
    canActivate: [RouteGuardCanActivateGuard],
    loadChildren: () => import('./home/home-admin.module').then((m) => m.HomeAdminModule),
    canLoad: [RequireAuthenticationCanLoadGuard],
    data: {
      role: 6,
    },
  },
  {
    path: '',
    pathMatch: 'full',
    redirectTo: '',
    canActivate: [DefaultRouteGuard],
  },
  {
    path: 'home',
    pathMatch: 'full',
    redirectTo: '',
    canActivate: [DefaultRouteGuard],
  },
  {
    path: '**',
    redirectTo: '',
  },
]

@NgModule({
  imports: [RouterModule.forRoot(appRoutes, { useHash: false })],
  exports: [RouterModule],
})
export class AppRoutingModule {}
