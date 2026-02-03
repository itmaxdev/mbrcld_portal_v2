import { NgModule } from '@angular/core'
import { Routes, RouterModule } from '@angular/router'
import { ArticlesDetailsComponent } from './articles-details/articles-details.component'
import { ArticlesItemComponent } from './articles-item/articles-item.component'
import { ArticlesListComponent } from './articles-list/articles-list.component'

const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    component: ArticlesListComponent,
  },
  {
    path: 'item/:id',
    component: ArticlesItemComponent,
  },
  {
    path: 'edit/:id',
    component: ArticlesDetailsComponent,
  },
  {
    path: 'create',
    component: ArticlesDetailsComponent,
  },
]

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ArticlesRoutingModule {}
