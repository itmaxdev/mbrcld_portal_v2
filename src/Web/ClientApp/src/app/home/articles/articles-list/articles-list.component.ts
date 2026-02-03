import { Component, OnInit, LOCALE_ID, Inject } from '@angular/core'
import {
  ArticlesClient,
  ListArticlesViewModel,
  ListUserArticlesViewModel,
  ProfileClient,
} from 'src/app/shared/api.generated.clients'
import { DisclaimerTypes } from 'src/app/shared/components/disclaimer.component'

@Component({
  selector: 'app-articles-list',
  templateUrl: './articles-list.component.html',
  styleUrls: ['./articles-list.component.scss'],
})
export class ArticlesListComponent implements OnInit {
  role: string
  ready = false
  searching = true
  searchedValue: string
  articleOnLabel = ''
  articleOffLabel = ''
  myArticles = false
  isMyArticles = false
  articleData: ListArticlesViewModel[] | ListUserArticlesViewModel
  showDisclaimer = false

  constructor(
    private articles: ArticlesClient,
    private profileClient: ProfileClient,
    @Inject(LOCALE_ID) public locale: string
  ) {
    this.articleOnLabel = locale === 'en' ? 'All Articles' : 'جميع المقالات'
    this.articleOffLabel = locale === 'en' ? 'My Articles' : 'مقالاتي'
  }

  async filterMyArticles() {
    this.searching = false

    if (this.myArticles) {
      await Promise.all([this.getMyArticles()])
    } else {
      await Promise.all([this.getAllArticlesList()])
    }
  }

  getMyArticles() {
    this.isMyArticles = true
    const profileInfo = JSON.parse(localStorage.getItem('profile_info'))
    const pictureUrl = profileInfo.profilePictureUrl

    return this.articles.userArticles().subscribe((data) => {
      this.articleData = data
      this.articleData.map((data) => {
        data.profilePictureUrl = pictureUrl
      })
      this.ready = true
      this.searching = true
    })
  }

  getAllArticlesList = () => {
    return this.articles.articlesGet().subscribe((data) => {
      this.articleData = data
      this.ready = true
      this.searching = true
      this.isMyArticles = false
    })
  }

  searchMyArticle(searchedValue) {
    this.searching = false
    const value = searchedValue.target.value ? searchedValue.target.value : ''

    if (value !== '') {
      this.articles.searchUserArticles(value).subscribe((data) => {
        this.articleData = data
        this.searching = true
      })
    } else {
      this.getMyArticles()
    }
  }

  searchArticle(searchedValue) {
    if (!this.isMyArticles) {
      this.searching = false
      const value = searchedValue.target.value ? searchedValue.target.value : ''

      if (value !== '') {
        this.articles.search(value).subscribe((data) => {
          this.articleData = data
          this.searching = true
        })
      } else {
        this.getAllArticlesList()
      }
    } else {
      this.searchMyArticle(searchedValue)
    }
  }

  handleDiclaimerApprove() {
    this.showDisclaimer = false
    this.getAllArticlesList()
  }

  ngOnInit() {
    this.role = JSON.parse(localStorage.getItem('profile_info')).role
    this.profileClient.userDisclaimerGet(DisclaimerTypes.ArticlesDisclaimer).subscribe((data) => {
      if (data.disclaimer) this.getAllArticlesList()
      else this.showDisclaimer = true
    })
  }

  getLocale(): string {
    if (this.locale === 'ar') {
      return 'في حال تم اعتماد المقال، يحق للمركز نشره في جميع القنوات المتاحة'
    } else {
      return 'Upon article approval, the center has the right to publish it in multiple channels'
    }
  }
}
