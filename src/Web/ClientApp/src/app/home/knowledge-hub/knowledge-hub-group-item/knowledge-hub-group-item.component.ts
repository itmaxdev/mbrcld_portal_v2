import { Component, OnInit } from '@angular/core'
import { ActivatedRoute } from '@angular/router'
import { CardInterface } from 'src/app/shared/components/enrolled-card.component'

interface IKnowledgeItem {
  caption: string
  description: string
  data: Array<CardInterface>
}

@Component({
  selector: 'app-knowledge-hub-group-item',
  templateUrl: './knowledge-hub-group-item.component.html',
  styleUrls: ['./knowledge-hub-group-item.component.scss'],
})
export class KnowledgeHubGroupItemComponent implements OnInit {
  groupName: string

  knowledgeData: IKnowledgeItem = {
    caption: 'Technology',
    description: '',
    data: [
      {
        caption: 'Hello world',
        content:
          'Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industrys standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged.',
        url:
          'https://bs-uploads.toptal.io/blackfish-uploads/blog/post/seo/og_image_file/og_image/15991/top-18-most-common-angularjs-developer-mistakes-41f9ad303a51db70e4a5204e101e7414.png',
        ratingCount: 5,
      },
      {
        caption: 'How are you',
        content:
          'Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industrys standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged.',
        url: 'https://hackernoon.com/hn-images/1*HSisLuifMO6KbLfPOKtLow.jpeg',
        ratingCount: 3,
      },
      {
        caption: 'Im fine!',
        content:
          'Lorem Ipsum the leap into electronic typesetting, remaining essentially unchanged.',
        url: 'https://miro.medium.com/max/3920/1*oZqGznbYXJfBlvGp5gQlYQ.jpeg',
        ratingCount: 1,
      },
    ],
  }

  constructor(private route: ActivatedRoute) {
    this.groupName = this.route.snapshot.paramMap.get('groupName')
  }

  ngOnInit(): void {}
}
