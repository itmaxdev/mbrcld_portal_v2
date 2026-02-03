import { NgModule } from '@angular/core'
import { MenuModule } from 'primeng/menu'
import { CommonModule } from '@angular/common'
import { SharedModule } from 'src/app/shared/shared.module'
import { HomeSharedModule } from '../shared/home-shared.module'
import { ArticlesRoutingModule } from './articles-routing.module'
import { ArticlesListComponent } from './articles-list/articles-list.component'
import { ArticlesDetailsComponent } from './articles-details/articles-details.component'
import { ArticlesItemComponent } from './articles-item/articles-item.component'

@NgModule({
  declarations: [ArticlesListComponent, ArticlesDetailsComponent, ArticlesItemComponent],
  imports: [CommonModule, ArticlesRoutingModule, HomeSharedModule, SharedModule, MenuModule],
})
export class ArticlesModule {}
