import { NgModule } from '@angular/core'
import { SharedModule } from 'src/app/shared/shared.module'
import { HomeInstructorRoutingModule } from './home-instructor-routing.component'
import { HomeModule } from './home.module'
import { HomeSharedModule } from './shared/home-shared.module'

@NgModule({
  imports: [HomeInstructorRoutingModule, HomeModule, SharedModule, HomeSharedModule],
})
export class HomeInstructorModule {}
