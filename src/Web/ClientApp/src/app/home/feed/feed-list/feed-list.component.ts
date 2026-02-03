import * as moment from 'moment'
import { Component, OnInit } from '@angular/core'
import { ListPostsViewModel, PostsClient, SearchArticlesViewModel } from 'src/app/shared/api.generated.clients'

interface DatesOfPosts {
  id: string | null | undefined
}

@Component({
  selector: 'app-feed-list',
  templateUrl: './feed-list.component.html',
  styleUrls: ['./feed-list.component.scss'],
})
export class FeedListComponent implements OnInit {
  ready = false
  role: number
  postsData: ListPostsViewModel[]
  searchPostsData: SearchArticlesViewModel[]
  postsDates: DatesOfPosts[] = []

  constructor(private posts: PostsClient) {}

  getAllPosts() {
    this.ready = false
    this.posts.postsGet().subscribe((data) => {
      this.postsData = data
      this.postsData.map((post) => {
        const newDate = moment([
          moment(post.postDate).year(),
          moment(post.postDate).month(),
          moment(post.postDate).date(),
        ])
          .lang('en')
          .fromNow()

        this.postsDates[post.id] = newDate
        this.ready = true
      })
    })
  }

  searchPosts(event) {
    let text = event.target.value.trim()
    text = text ? text : ''
    if (text) {
      this.ready = false
      this.posts.search(text).subscribe((data) => {
        //this.postsData = data
        this.postsDates = []
        this.postsData.map((post) => {
          const newDate = moment([
            moment(post.postDate).year(),
            moment(post.postDate).month(),
            moment(post.postDate).date(),
          ])
            .lang('en')
            .fromNow()

          this.postsDates[post.id] = newDate
        })
        this.ready = true
      })
    } else {
      this.getAllPosts()
    }
  }

  ngOnInit(): void {
    this.role = JSON.parse(localStorage.getItem('profile_info')).role
    this.getAllPosts()
  }
}
