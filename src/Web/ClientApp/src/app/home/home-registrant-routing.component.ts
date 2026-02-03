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
        path: 'programs',
        loadChildren: () => import('./programs/programs.module').then((m) => m.ProgramsModule),
      },
      {
        path: 'profile',
        loadChildren: () => import('./profile/profile.module').then((m) => m.ProfileModule),
      },
      {
        path: 'events',
        loadChildren: () => import('./events/events.module').then((m) => m.EventsModule),
      },
      {
        path: 'articles',
        loadChildren: () => import('./articles/articles.module').then((m) => m.ArticlesModule),
      },
      {
        path: 'account-settings',
        loadChildren: () =>
          import('./account-settings/account-settings.module').then((m) => m.AccountSettingsModule),
      },
      {
        path: 'calendar',
        loadChildren: () => import('./calendar/calendar.module').then((m) => m.CalendarModule),
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
export class HomeRegistrantRoutingModule {}
