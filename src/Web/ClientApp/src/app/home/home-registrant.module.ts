import { NgModule } from '@angular/core'
import { SharedModule } from 'src/app/shared/shared.module'
import { HomeRegistrantRoutingModule } from './home-registrant-routing.component'
import { HomeModule } from './home.module'
import { HomeSharedModule } from './shared/home-shared.module'

@NgModule({
  imports: [HomeRegistrantRoutingModule, HomeModule, SharedModule, HomeSharedModule],
})
export class HomeRegistrantModule {}
