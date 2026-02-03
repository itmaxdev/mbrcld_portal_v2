import { MenuItem } from 'primeng/api'
import { ActivatedRoute } from '@angular/router'
import { Component, OnInit } from '@angular/core'
import { SectionDataService } from 'src/app/shared/services/section-data.service'
import {
  ContentsClient,
  ListContentsBySectionIdViewModel,
} from 'src/app/shared/api.generated.clients'

@Component({
  selector: 'app-programs-new-content',
  templateUrl: './programs-new-content.component.html',
  styleUrls: ['./programs-new-content.component.scss'],
})
export class ProgramsNewContentComponent implements OnInit {
  items: MenuItem[]
  sectionId: string
  ready = false
  icons: Array<string>
  typeNames: Array<string>
  sectionContents: ListContentsBySectionIdViewModel[]

  constructor(
    public sectionData: SectionDataService,
    private contents: ContentsClient,
    private route: ActivatedRoute
  ) {}

  goToPreviewPage() {
    this.sectionData.redirectBack(1)
  }

  async ngOnInit() {
    this.sectionId = this.route.snapshot.paramMap.get('sectionId')
    this.ready = false

    await Promise.all([
      this.contents.sectionContents(this.sectionId).subscribe((data) => {
        this.sectionContents = data
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
    this.typeNames = ['content', 'video', 'attachment', 'meeting', 'sticknote']

    this.items = [
      {
        label: 'Content',
        routerLink: ['add-content'],
      },
      {
        label: 'Video',
        routerLink: ['add-video'],
      },
      { label: 'Document', routerLink: ['add-document'] },
      { label: 'Meeting', routerLink: ['add-meeting'] },
      { label: 'Stick Note', routerLink: ['add-sticknote'] },
    ]
  }
}
