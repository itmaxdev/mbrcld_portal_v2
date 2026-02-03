import { NgModule } from '@angular/core'
import { CommonModule } from '@angular/common'
import { MenuModule } from 'primeng/menu'
import { ProjectIdeasListComponent } from './project-ideas-list/project-ideas-list.component'
import { ProjectIdeasRoutingModule } from './project-ideas-routing.module'
import { HomeSharedModule } from '../shared/home-shared.module'
import { SharedModule } from 'src/app/shared/shared.module'
import { InputNumberModule } from 'primeng/inputnumber'
import { ProjectIdeasCreateComponent } from './common/project-ideas-create.component'
import { ProjectIdeasViewComponent } from './common/project-ideas-view.component'
import { ProjectViewByIdComponent } from './common/project-view-by-id.component'

@NgModule({
  declarations: [
    ProjectIdeasListComponent,
    ProjectIdeasCreateComponent,
    ProjectIdeasViewComponent,
    ProjectViewByIdComponent,
  ],
  imports: [
    CommonModule,
    ProjectIdeasRoutingModule,
    HomeSharedModule,
    SharedModule,
    InputNumberModule,
    MenuModule,
  ],
})
export class ProjectIdeasModule {}
