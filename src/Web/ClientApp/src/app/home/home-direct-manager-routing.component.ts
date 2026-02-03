import { NgModule } from '@angular/core'
import { RouterModule, Routes } from '@angular/router'
import { RequireAuthenticationCanActivateGuard } from 'src/app/core/api-authorization'
import { HomeComponent } from 'src/app/home/home.component'

const homeRoutes: Routes = [
  {
    path: '',
    component: HomeComponent,
    canActivate: [RequireAuthenticationCanActivateGuard],
    children: [
      {
        path: 'feed',
        loadChildren: () => import('./feed/feed.module').then((m) => m.FeedModule),
      },
      {
        path: 'applicants',
        loadChildren: () =>
          import('./dm-applicants/dm-applicants.module').then((m) => m.DmApplicantsModule),
      },
      {
        path: 'dashboard',
        loadChildren: () =>
          import('./dm-dashboard/dm-dashboard.module').then((m) => m.DmDashboardModule),
      },
      {
        path: 'calendar',
        loadChildren: () =>
          import('./dm-calendar/dm-calendar.module').then((m) => m.DmCalendarModule),
      },
      {
        path: 'attendance',
        loadChildren: () =>
          import('./dm-attendance/dm-attendance.module').then((m) => m.DmAttendanceModule),
      },
      {
        path: 'account-settings',
        loadChildren: () =>
          import('./account-settings/account-settings.module').then((m) => m.AccountSettingsModule),
      },
      {
        path: '',
        pathMatch: 'full',
        redirectTo: 'feed',
      },
      {
        path: '**',
        redirectTo: 'feed',
      },
    ],
  },
]

@NgModule({
  imports: [RouterModule.forChild(homeRoutes)],
  exports: [RouterModule],
})
export class HomeDirectManagerRoutingModule {}
