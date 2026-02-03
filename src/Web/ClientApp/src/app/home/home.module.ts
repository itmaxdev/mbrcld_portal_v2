import { NgModule } from '@angular/core'
import { UserToolbarComponent } from 'src/app/home/common/components/user-toolbar.component'
import { HomeRoutingModule } from 'src/app/home/home-routing.module'
import { HomeComponent } from 'src/app/home/home.component'
import { NavBarComponent } from 'src/app/home/nav-bar/nav-bar.component'
import { ToolbarComponent } from 'src/app/home/toolbar/toolbar.component'
import { SharedModule } from 'src/app/shared/shared.module'
import { HomeSharedModule } from './shared/home-shared.module'

@NgModule({
  declarations: [HomeComponent, NavBarComponent, ToolbarComponent, UserToolbarComponent],
  exports: [HomeComponent, NavBarComponent, ToolbarComponent, UserToolbarComponent],
  imports: [HomeRoutingModule, SharedModule, HomeSharedModule],
})
export class HomeModule {}
