import { NgModule } from '@angular/core'
import { RouterModule, Routes } from '@angular/router'
import { ProjectViewByIdComponent } from './common/special-project-view-id.component'
import { ProjectIdeasCreateComponent } from './common/special-project-create.component'
import { EliteclubComponent } from './eliteclub.component'
import { MentorComponent } from './individual-dev-membership/mentor-sessions/mentor/mentor.component'
import { ProgramsModulesComponent } from '../programs/programs-modules/programs-modules.component'
import { ViewApplicantPageComponent } from '../programs/view-applicant-page/view-applicant-page.component'
import { ProgramsSectionComponent } from '../programs/programs-section/programs-section.component'
import { ProgramsSectionListComponent } from '../programs/programs-section-list/programs-section-list.component'

const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    component: EliteclubComponent,
  },
  {
    path: 'eliteclub-mentor/:id',
    component: MentorComponent,
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
  {
    path: 'modules/:modulesId',
    component: ProgramsModulesComponent,
  },
  {
    path: 'view-applicant/:applicantId',
    component: ViewApplicantPageComponent,
  },
  {
    path: 'modules/:modulesId/materials/:materialsId',
    component: ProgramsSectionListComponent,
  },
  {
    path: 'modules/:modulesId/materials/:materialsId/content/:sectionId',
    component: ProgramsSectionComponent,
  },
]

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class EliteclubRoutingModule {}
