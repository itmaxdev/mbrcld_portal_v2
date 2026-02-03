import { NgModule } from '@angular/core'
import { Routes, RouterModule } from '@angular/router'
import { FeedDetailsComponent } from './feed-details/feed-details.component'
import { FeedListComponent } from './feed-list/feed-list.component'
import { FeedComponent } from './feed.component'
const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    component: FeedListComponent,
  },
  {
    path: 'item/:id',
    pathMatch: 'full',
    component: FeedDetailsComponent,
  },
]

@NgModule({
  imports: [RouterModule.forChild(routes)],
})
export class FeedRoutingModule {}
