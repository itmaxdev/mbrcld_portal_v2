import { NgModule } from '@angular/core'
import { HomeAlumniRoutingModule } from 'src/app/home/home-alumni-routing.module'
import { SharedModule } from 'src/app/shared/shared.module'
import { HomeSharedModule } from './shared/home-shared.module'

@NgModule({
  imports: [SharedModule, HomeSharedModule, HomeAlumniRoutingModule],
  declarations: [],
})
export class HomeAlumniModule {}
