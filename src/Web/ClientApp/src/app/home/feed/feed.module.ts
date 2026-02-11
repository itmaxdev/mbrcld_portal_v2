import { NgModule } from '@angular/core'
// import { UserToolbarComponent } from 'src/app/home/common/components/user-toolbar.component'
// import { HomeRoutingModule } from 'src/app/home/home-routing.module'
import { FeedComponent } from 'src/app/home/feed/feed.component'
// import { NavBarComponent } from 'src/app/home/nav-bar/nav-bar.component'
import { SharedModule } from 'src/app/shared/shared.module'
import { TabViewModule } from 'primeng/tabview'
import { FeedRoutingModule } from './feed-routing.module'
// import { UserInfoComponent } from 'src/app/home/common/components/user-info/user-info.component'
import { HomeSharedModule } from '../shared/home-shared.module'
import { FeedListComponent } from './feed-list/feed-list.component'
import { FeedDetailsComponent } from './feed-details/feed-details.component'
import { SlidePanelComponent } from '../slide-panel/slide-panel.component'
import { SideBoxComponent } from 'src/app/shared/components/sidebox/sideBox.component'

@NgModule({
  declarations: [
    SideBoxComponent,
    FeedComponent,
    FeedListComponent,
    FeedDetailsComponent,
    SlidePanelComponent,
  ],
  imports: [FeedRoutingModule, TabViewModule, SharedModule, HomeSharedModule],
  exports: [SlidePanelComponent],
})
export class FeedModule {}
