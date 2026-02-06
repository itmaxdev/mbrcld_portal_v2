declare let DatepickerInit: any

import { Component, Inject, LOCALE_ID, OnInit, AfterViewInit } from '@angular/core'
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser'
import { debounceTime } from 'rxjs/operators'
import { Panel3ViewModel, RightPanelClient } from 'src/app/shared/api.generated.clients'

@Component({
  selector: 'app-slide-panel',
  templateUrl: './slide-panel.component.html',
  styleUrls: ['./slide-panel.component.scss'],
})
export class SlidePanelComponent implements OnInit, AfterViewInit {
  role: number
  currentProgram: any
  currentModule: any
  videoUrl: SafeResourceUrl
  listItems: any[] = []
  currentLang: string

  eventDates: string[] = [
    '2025-11-05',
    '2025-11-15',
    '2025-11-25',
    '2025-12-14',
    '2025-12-11',
    '2025-12-24',
  ]

  highlightEvents(date: Date): { classes: string; content: string } | void {
    const today = new Date()

    const todayString = `${today.getFullYear()}-${String(today.getMonth() + 1).padStart(
      2,
      '0'
    )}-${String(today.getDate()).padStart(2, '0')}`

    const yyyy = date.getFullYear()
    const mm = String(date.getMonth() + 1).padStart(2, '0')
    const dd = String(date.getDate()).padStart(2, '0')
    const dateString = `${yyyy}-${mm}-${dd}`

    const classes: string[] = []
    let isHighlighted = false

    if (dateString === todayString) {
      classes.push('todayHighlight')
      isHighlighted = true
    }

    if (this.eventDates.includes(dateString)) {
      classes.push('eventHighlight')
      isHighlighted = true
    }

    if (isHighlighted) {
      return {
        classes: classes.join(' '),
        content: `<span>${date.getDate()}</span>`,
      }
    }
  }

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

  ngAfterViewInit(): void {
    const inlineCalendarSmall = document.getElementById('inlineCalenderSmall')

    if (inlineCalendarSmall) {
      new DatepickerInit(inlineCalendarSmall, {
        autohide: false,
        format: 'dd/mm/yyyy',
        beforeShowDay: (date: Date) => this.highlightEvents(date),
      })
    }
  }
}
