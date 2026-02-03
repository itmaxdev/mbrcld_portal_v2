import { NgModule } from '@angular/core'
import { SharedModule } from 'src/app/shared/shared.module'
import { HomeAdminRoutingModule } from './home-Admin-routing.component'
import { HomeModule } from './home.module'
import { HomeSharedModule } from './shared/home-shared.module'

@NgModule({
  imports: [HomeAdminRoutingModule, HomeModule, SharedModule, HomeSharedModule],
})
export class HomeAdminModule {}
