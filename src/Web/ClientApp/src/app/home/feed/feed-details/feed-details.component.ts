import { Component, OnInit } from '@angular/core'
import { ActivatedRoute } from '@angular/router'
import { ArticlesClient, PostsClient } from 'src/app/shared/api.generated.clients'
import { SectionDataService } from 'src/app/shared/services/section-data.service'

@Component({
  selector: 'app-feed-details',
  templateUrl: './feed-details.component.html',
  styleUrls: ['./feed-details.component.scss'],
})
export class FeedDetailsComponent implements OnInit {
  id: string
  ready = false
  data: any
  constructor(
    private route: ActivatedRoute,
    private posts: PostsClient,
    private section: SectionDataService,
    private articles: ArticlesClient
  ) {}

  goBack() {
    this.section.redirectBack(2)
  }

  async ngOnInit() {
    this.ready = false
    const recordType = this.route.snapshot.paramMap.get('type')
    this.id = this.route.snapshot.paramMap.get('id')
    if (recordType == '1') {
      await Promise.all([
        this.articles.articlesGetById(this.id).subscribe((data) => {
          this.data = data
          this.ready = true
        }),
      ])
    } else {
      await Promise.all([
        this.posts.postsGetById(this.id).subscribe((data) => {
          this.data = data
          this.ready = true
        }),
      ])
    }
  }
}
