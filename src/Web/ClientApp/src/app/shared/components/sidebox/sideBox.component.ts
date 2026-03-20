import { Component, OnInit, ViewChild, ElementRef, Inject, LOCALE_ID } from '@angular/core'
import Datepicker from 'vanillajs-datepicker/js/Datepicker'
import { Panel3ViewModel, RightPanelClient } from '../../api.generated.clients'
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser'
import { debounceTime } from 'rxjs/operators'

@Component({
  selector: 'app-side-box',
  templateUrl: './sideBox.component.html',
  styleUrls: ['./sideBox.component.scss'],
})
export class SideBoxComponent {
  role: number
  currentProgram: any
  currentModule: any
  videoUrl: SafeResourceUrl
  listItems: any[] = []
  currentLang: string

  @ViewChild('inlineCalenderSmall', { static: false })
  calendarEl!: ElementRef

  private eventDates: string[] = [
    '2025-11-05',
    '2025-11-15',
    '2025-11-25',
    '2025-12-14',
    '2025-12-11',
    '2025-12-24',
  ]

  constructor(
    @Inject(LOCALE_ID) locale: string,
    private rightPanelClient: RightPanelClient,
    private _sanitizer: DomSanitizer
  ) {
    this.currentLang = locale
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

  getLists1() {
    this.rightPanelClient.listsGet().subscribe((res: Panel3ViewModel[]) => {
      if (res) {
        this.listItems = res
        console.log(this.listItems)
      }
    })
  }

  getLists() {
    this.rightPanelClient.listsGet().subscribe((res: Panel3ViewModel[]) => {
      if (res) {
        this.listItems = res
        setTimeout(() => {
          this.triggerCountAnimation()
        }, 100)
      }
    })
  }

  triggerCountAnimation(): void {
    const counters = document.querySelectorAll('[data-animcount]')
    counters.forEach((el) => {
      const target = +(el.getAttribute('data-animcount') ?? 0)
      const inner = el.querySelector('i')
      if (!inner || target === 0) return

      let current = 0
      const duration = 1500
      const totalFrames = duration / 16
      const step = target / totalFrames

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

  ngAfterViewInit(): void {
    if (!this.calendarEl) return
    new Datepicker(this.calendarEl.nativeElement, {
      autohide: false,
      format: 'dd/mm/yyyy',
      beforeShowDay: this.highlightEvents.bind(this),
    })
  }

  private highlightEvents(date: Date) {
    const today = new Date()

    const todayString = this.formatDate(today)
    const dateString = this.formatDate(date)

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
    return
  }

  private formatDate(date: Date): string {
    const yyyy = date.getFullYear()
    const mm = String(date.getMonth() + 1).padStart(2, '0')
    const dd = String(date.getDate()).padStart(2, '0')
    return `${yyyy}-${mm}-${dd}`
  }
}
