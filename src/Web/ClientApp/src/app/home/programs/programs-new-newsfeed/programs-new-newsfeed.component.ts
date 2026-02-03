import { Component, OnInit } from '@angular/core'
import { MenuItem } from 'primeng/api'
import { ActivatedRoute } from '@angular/router'
import { SectionDataService } from 'src/app/shared/services/section-data.service'
import {
  ListContentsBySectionIdViewModel,
  ListUserNewsFeedsViewModel,
  NewsFeedsClient,
} from 'src/app/shared/api.generated.clients'

@Component({
  selector: 'app-programs-new-newsfeed',
  templateUrl: './programs-new-newsfeed.component.html',
  styleUrls: ['./programs-new-newsfeed.component.scss'],
})
export class ProgramsNewNewsFeedComponent implements OnInit {
  items: MenuItem[]
  ready = false
  icons: Array<string>
  typeNames: Array<string>
  sectionContents: ListContentsBySectionIdViewModel[]
  sectionId: string
  id: string
  moduleId: string
  newsFeedList: ListUserNewsFeedsViewModel[]

  constructor(
    public sectionData: SectionDataService,
    private route: ActivatedRoute,
    private newsFeedsClient: NewsFeedsClient
  ) {}

  goToPreviewPage() {
    this.sectionData.redirectBack(1)
  }

  async ngOnInit() {
    // this.sectionId = this.route.snapshot.paramMap.get('sectionId')
    this.moduleId = this.route.snapshot.paramMap.get('modulesId')

    this.ready = false

    await Promise.all([
      this.newsFeedsClient.instructorOrAdminNewsfeeds(this.moduleId).subscribe((data) => {
        this.newsFeedList = data
        this.ready = true
      }),
    ])

    this.icons = [
      'https://icons.iconarchive.com/icons/custom-icon-design/mono-general-2/512/document-icon.png',
      'https://image.flaticon.com/icons/png/512/14/14879.png',
      'https://icons-for-free.com/iconfiles/png/512/file+attachment+clip+file+paperclip+icon-1320190556905027615.png',
      'https://upload.wikimedia.org/wikipedia/commons/thumb/6/68/Video_camera_icon.svg/1024px-Video_camera_icon.svg.png',
      'https://icons-for-free.com/iconfiles/png/512/bx+file+blank-1325051877402682455.png',
    ]
    this.typeNames = ['text', 'video', 'document', 'meeting', 'sticknote']
    this.items = [
      {
        label: 'Text',
        routerLink: ['add-text'],
      },
      {
        label: 'Video',
        routerLink: ['add-video'],
      },
      { label: 'Document', routerLink: ['add-document'] },
      { label: 'Meeting', routerLink: ['add-meeting'] },
      { label: 'Stick Note', routerLink: ['add-sticknote'] },
    ]
    // this.ready = true
    // this.ready = false
  }
}
