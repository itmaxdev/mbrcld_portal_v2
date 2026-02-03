import { NgModule } from '@angular/core'
import { Routes, RouterModule } from '@angular/router'
import { KnowledgeHubListComponent } from './knowledge-hub-list/knowledge-hub-list.component'
import { KnowledgeHubGroupItemComponent } from './knowledge-hub-group-item/knowledge-hub-group-item.component'
import { ScholarshipDetailsComponent } from './scholarship-details/scholarship-details.component'
const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    component: KnowledgeHubListComponent,
  },
  {
    path: ':id',
    component: ScholarshipDetailsComponent,
  },
  // {
  //   path: ':groupName',
  //   component: KnowledgeHubGroupItemComponent,
  // },
]

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class KnowledgeHubRoutingModule {}
