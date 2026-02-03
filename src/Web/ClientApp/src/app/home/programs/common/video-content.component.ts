import { Component, Input, OnInit } from '@angular/core'
import { DomSanitizer } from '@angular/platform-browser'

@Component({
  selector: 'app-video-content',
  template: `
    <div>
      <iframe [src]="videoUrl" width="100%" height="400px" align="left">
        Video not found
      </iframe>
    </div>
  `,
})
export class VideoContentComponent implements OnInit {
  @Input() name: string
  @Input() videoUrl: any

  constructor(private _sanitizer: DomSanitizer) {}

  ngOnInit(): void {
    if (this.videoUrl) {
      this.videoUrl = this._sanitizer.bypassSecurityTrustResourceUrl(
        'https://player.vimeo.com/video/' + this.videoUrl.split('/').pop()
      )
    }
  }
}
