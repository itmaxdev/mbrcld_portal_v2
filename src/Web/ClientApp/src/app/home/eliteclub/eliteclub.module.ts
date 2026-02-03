import { NgModule } from '@angular/core'
import { CommonModule } from '@angular/common'
import { TabViewModule } from 'primeng/tabview'
import { KeyFilterModule } from 'primeng/keyfilter'
import { InputTextareaModule } from 'primeng/inputtextarea'
import { SharedModule } from 'src/app/shared/shared.module'
import { MentorComponent } from './individual-dev-membership/mentor-sessions/mentor/mentor.component'
import { HomeSharedModule } from '../shared/home-shared.module'
import { SessionComponent } from './elite-membership/session/session.component'
import { OverviewComponent } from './overview/overview.component'
import { EliteclubComponent } from './eliteclub.component'
import { EliteclubRoutingModule } from './eliteclub-routing.module'
import { MentorSessionsComponent } from './individual-dev-membership/mentor-sessions/mentor-sessions.component'
import { EliteMembershipComponent } from './elite-membership/elite-membership.component'
import { SpecialProjectsComponent } from './individual-dev-membership/special-projects/special-projects.component'
import { EliteClubMembersComponent } from './elte-club-members/elite-club-members.component'
import { EliteCommunicationComponent } from './individual-dev-membership/elite-communication/elite-communication.component'
import { IndividualDevMembershipComponent } from './individual-dev-membership/individual-dev-membership.component'
import { SpecialProjectOverviewComponent } from './common/special-project-overview.component'
import { ProjectIdeasCreateComponent } from './common/special-project-create.component'
import { ProjectViewByIdComponent } from './common/special-project-view-id.component'
import { SecureStorage } from 'src/app/core/api-authorization/oauth-storage'

@NgModule({
  declarations: [
    MentorComponent,
    SessionComponent,
    OverviewComponent,
    EliteclubComponent,
    MentorSessionsComponent,
    EliteMembershipComponent,
    SpecialProjectsComponent,
    EliteClubMembersComponent,
    EliteCommunicationComponent,
    IndividualDevMembershipComponent,
    SpecialProjectOverviewComponent,
    ProjectIdeasCreateComponent,
    ProjectViewByIdComponent,
  ],
  imports: [
    CommonModule,
    SharedModule,
    TabViewModule,
    KeyFilterModule,
    HomeSharedModule,
    InputTextareaModule,
    EliteclubRoutingModule,
  ],
  providers: [SecureStorage],
})
export class EliteclubModule {}
