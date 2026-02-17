import { NgModule } from '@angular/core'
import { RouterModule, Routes } from '@angular/router'
import { RequireAuthenticationCanActivateGuard } from 'src/app/core/api-authorization'
import { HomeComponent } from 'src/app/home/home.component'
import { MessageChatComponent } from './programs/message-chat/message-chat.component'

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
        path: 'ideas',
        loadChildren: () =>
          import('./project-ideas/project-ideas.module').then((m) => m.ProjectIdeasModule),
      },
      // {
      //   path: 'profile',
      //   loadChildren: () => import('./profile/profile.module').then((m) => m.ProfileModule),
      // },
      {
        path: 'profile',
        loadChildren: () =>
          import('./registrant-profile/registrant-profile.module').then(
            (m) => m.RegistrantProfileModule
          ),
      },
      {
        path: 'dashboard',
        loadChildren: () => import('./dashboard/dashboard.module').then((m) => m.DashboardModule),
      },
      {
        path: 'meeting',
        loadChildren: () => import('./meeting/meeting.module').then((m) => m.MeetingModule),
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
        path: 'eliteclub',
        loadChildren: () => import('./eliteclub/eliteclub.module').then((m) => m.EliteclubModule),
      },
      {
        path: 'chat/:roomId',
        component: MessageChatComponent,
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
export class HomeAlumniRoutingModule {}
