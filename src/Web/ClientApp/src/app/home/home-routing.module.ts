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
        path: 'dashboard',
        loadChildren: () => import('./dashboard/dashboard.module').then((m) => m.DashboardModule),
      },
      {
        path: 'calendar',
        loadChildren: () => import('./calendar/calendar.module').then((m) => m.CalendarModule),
      },
      {
        path: 'knowledge',
        loadChildren: () =>
          import('./knowledge-hub/knowledge-hub.module').then((m) => m.KnowledgeHubModule),
      },
      {
        path: 'account-settings',
        loadChildren: () =>
          import('./account-settings/account-settings.module').then((m) => m.AccountSettingsModule),
      },
      {
        path: 'events',
        loadChildren: () => import('./events/events.module').then((m) => m.EventsModule),
      },
      {
        path: 'survey',
        loadChildren: () => import('./survey/survey.module').then((m) => m.SurveyModule),
      },
      {
        path: 'articles',
        loadChildren: () => import('./articles/articles.module').then((m) => m.ArticlesModule),
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
export class HomeRoutingModule {}
