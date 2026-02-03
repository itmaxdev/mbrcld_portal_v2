import { NgModule } from '@angular/core'
import { RouterModule, Routes } from '@angular/router'
import { ProjectIdeasCreateComponent } from './common/project-ideas-create.component'
import { ProjectViewByIdComponent } from './common/project-view-by-id.component'
import { ProjectIdeasListComponent } from './project-ideas-list/project-ideas-list.component'

const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    component: ProjectIdeasListComponent,
  },
  {
    path: 'create',
    component: ProjectIdeasCreateComponent,
  },
  {
    path: 'edit/:ideaId',
    component: ProjectIdeasCreateComponent,
  },
  {
    path: ':ideaId',
    component: ProjectViewByIdComponent,
  },
]

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ProjectIdeasRoutingModule {}
