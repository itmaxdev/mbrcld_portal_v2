import { Component, OnInit, LOCALE_ID, Inject } from '@angular/core'
import { Router } from '@angular/router'
import { Location } from '@angular/common'
import { ChatClient } from '../shared/api.generated.clients'

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
})
export class HomeComponent implements OnInit {
  public roomId: string
  public role: number
  public chatUrl: string = undefined
  public isChatUrl: boolean

  constructor(
    private chatClient: ChatClient,
    private router: Router,
    private location: Location,
    @Inject(LOCALE_ID) public locale: string
  ) {
    this.router.events.subscribe((val) => {
      const currentUrl = this.location.path()
      if (currentUrl.includes('modules')) {
        this.chatUrl = currentUrl
      } else {
        this.chatUrl = undefined
      }

      if (currentUrl.includes('chat')) {
        this.isChatUrl = true
      } else {
        this.isChatUrl = false
      }
    })
  }

  ngOnInit() {
    const profileInfo = JSON.parse(localStorage.getItem('profile_info'))
    this.role = profileInfo.role

    this.chatClient.chat('12341234-1234-1234-1234-123412341234').subscribe((data) => {
      const chatData = JSON.parse(data)
      if (chatData.length > 0) {
        this.roomId = chatData[0].id
      }
    })
  }
}
