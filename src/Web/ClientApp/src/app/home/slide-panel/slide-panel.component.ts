import { Component, Inject, LOCALE_ID, OnInit } from '@angular/core'
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser'
import { debounceTime } from 'rxjs/operators'
import { Panel3ViewModel, RightPanelClient } from 'src/app/shared/api.generated.clients'

@Component({
  selector: 'app-slide-panel',
  templateUrl: './slide-panel.component.html',
  styleUrls: ['./slide-panel.component.scss'],
})
export class SlidePanelComponent implements OnInit {
  role: number
  currentProgram: any
  currentModule: any
  videoUrl: SafeResourceUrl
  listItems: any[] = []
  currentLang: string
  constructor(
    @Inject(LOCALE_ID) locale: string,
    private rightPanelClient: RightPanelClient,
    private _sanitizer: DomSanitizer
  ) {
    this.currentLang = locale
  }

  getPinned() {
    this.rightPanelClient
      .pinnedGet()
      .pipe(debounceTime(600))
      .subscribe((res: any) => {
        if (res) {
          this.videoUrl = this._sanitizer.bypassSecurityTrustResourceUrl(
            'https://player.vimeo.com/video/' + res.videoUrl.split('/').pop()
          )
        }
      })
  }

  getCurrentProgram() {
    this.rightPanelClient
      .currentProgramGet()
      .pipe(debounceTime(600))
      .subscribe((res: any) => {
        if (res) {
          this.currentProgram = res
        }
      })
  }

  getCurrentModule() {
    this.rightPanelClient
      .currentModuleGet()
      .pipe(debounceTime(600))
      .subscribe((res: any) => {
        if (res) {
          this.currentModule = res
        }
      })
  }

  getLists() {
    this.rightPanelClient.listsGet().subscribe((res: Panel3ViewModel[]) => {
      if (res) {
        this.listItems = res
      }
    })
  }

  ngOnInit(): void {
    this.role = JSON.parse(localStorage.getItem('profile_info')).role
    if (this.role == 1) {
      this.getCurrentProgram()
    } else if (this.role == 2) {
      this.getCurrentModule()
    }
    this.getPinned()
    this.getLists()
  }
}
