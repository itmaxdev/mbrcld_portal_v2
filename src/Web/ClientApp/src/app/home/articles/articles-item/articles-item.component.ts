import { Component, OnInit } from '@angular/core'
import { ActivatedRoute } from '@angular/router'
import { Location } from '@angular/common'
import { ArticlesClient, ListArticlesViewModel } from 'src/app/shared/api.generated.clients'

@Component({
  selector: 'app-articles-item',
  templateUrl: './articles-item.component.html',
  styleUrls: ['./articles-item.component.scss'],
})
export class ArticlesItemComponent implements OnInit {
  id: string
  ready = false
  articleData: ListArticlesViewModel

  constructor(
    private route: ActivatedRoute,
    private articles: ArticlesClient,
    private _location: Location
  ) {}

  goBack() {
    this._location.back()
  }

  async ngOnInit() {
    this.ready = false
    this.id = this.route.snapshot.paramMap.get('id')
    await Promise.all([
      this.articles.articlesGetById(this.id).subscribe((data) => {
        this.articleData = data
        this.ready = true
      }),
    ])
  }
}
