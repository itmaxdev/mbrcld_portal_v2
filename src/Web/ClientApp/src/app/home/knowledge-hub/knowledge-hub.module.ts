import { NgModule } from '@angular/core'
import { CommonModule } from '@angular/common'
import { SharedModule } from 'src/app/shared/shared.module'
import { HomeSharedModule } from '../shared/home-shared.module'
import { KnowledgeHubRoutingModule } from './knowledge-hub-routing.module'
import { KnowledgeHubListComponent } from './knowledge-hub-list/knowledge-hub-list.component'
import { KnowledgeHubCardComponent } from './common/knowledge-hub-card.component'
import { KnowledgeHubGroupItemComponent } from './knowledge-hub-group-item/knowledge-hub-group-item.component'
import { TabViewModule } from 'primeng/tabview'
import { ScholarshipCardComponent } from './scholarship-card/scholarship-card.component'
import { ScholarshipDetailsComponent } from './scholarship-details/scholarship-details.component'
@NgModule({
  declarations: [
    ScholarshipDetailsComponent,
    ScholarshipCardComponent,
    KnowledgeHubListComponent,
    KnowledgeHubCardComponent,
    KnowledgeHubGroupItemComponent,
  ],
  imports: [TabViewModule, CommonModule, KnowledgeHubRoutingModule, HomeSharedModule, SharedModule],
})
export class KnowledgeHubModule {}
