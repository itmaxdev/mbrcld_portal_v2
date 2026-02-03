import { NgModule } from '@angular/core'

import { SharedModule } from 'src/app/shared/shared.module'
import { UserInfoComponent } from 'src/app/home/common/components/user-info/user-info.component'

@NgModule({
  declarations: [UserInfoComponent],
  exports: [UserInfoComponent],
  imports: [SharedModule],
})
export class HomeSharedModule {}
