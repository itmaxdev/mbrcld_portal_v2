import * as moment from 'moment'
import { Component, OnInit } from '@angular/core'
import { Router } from '@angular/router'
import {
  DashboardClient,
  ListPostsViewModel,
  PostsClient,
  SearchArticlesViewModel,
} from 'src/app/shared/api.generated.clients'

interface DatesOfPosts {
  id: string | null | undefined
}

@Component({
  selector: 'app-feed-list',
  templateUrl: './feed-list.component.html',
  styleUrls: ['./feed-list.component.scss'],
})
export class FeedListComponent implements OnInit {
  componentName = 'feed'
  ready = false
  role: number
  postsData: ListPostsViewModel[]
  searchPostsData: SearchArticlesViewModel[]
  postsDates: DatesOfPosts[] = []
  showAlert = true
  alertMessage = 'Lorem ipsum dolor sit amet, consectetur adipiscing elit.'

  totalEvents = 0
  totalArticles = 0
  totalPrograms = 0

  /** Admin (role 6): hide Total Events / Articles / Programs cards on feed. */
  get showSummaryCards(): boolean {
    return this.role !== 6
  }

  constructor(
    private posts: PostsClient,
    private dashboard: DashboardClient,
    private router: Router
  ) {}

  /** Navigates to events / articles / programs under the current role base (registrant, admin, alumni, etc.). */
  navigateToModule(module: 'events' | 'articles' | 'programs'): void {
    const base = this.router.url.split('/').filter(Boolean)[0]
    if (base) {
      this.router.navigate(['/', base, module])
    }
  }

  closeAlert() {
    this.showAlert = false
  }

  getAllPosts() {
    this.ready = false
    this.posts.postsGet().subscribe((data) => {
      this.postsData = data
      this.postsData.forEach((post) => {
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

  loadDashboardCounts(): void {
    this.ready = false
    this.dashboard.homepage().subscribe({
      next: (data) => {
        if (data) {
          const byName = (name: string) => data.find((x) => x.name === name)?.value ?? 0
          this.totalEvents = byName('Total Events')
          this.totalPrograms = byName('Total Programs')
          this.totalArticles = byName('Total Articles')
          this.ready = true

          setTimeout(() => {
            this.triggerCountAnimation()
          }, 400)
        }
      },
    })
  }

  triggerCountAnimation(): void {
    const counters = document.querySelectorAll('[data-animcount]')
    counters.forEach((el) => {
      const target = +(el.getAttribute('data-animcount') ?? 0)
      const inner = el.querySelector('i')
      if (!inner || target === 0) return

      let current = 0
      const duration = 1500 // total ms
      const totalFrames = duration / 16 // ~93 frames
      const step = target / totalFrames // fractional step per frame

      const timer = setInterval(() => {
        current += step
        if (current >= target) {
          current = target
          clearInterval(timer)
        }
        inner.textContent = Math.floor(current).toString()
      }, 16)
    })
  }

  ngOnInit(): void {
    const raw = localStorage.getItem('profile_info')
    this.role = raw ? JSON.parse(raw)?.role ?? 0 : 0
    this.getAllPosts()
    if (this.role !== 6) {
      this.loadDashboardCounts()
    }
  }
}
