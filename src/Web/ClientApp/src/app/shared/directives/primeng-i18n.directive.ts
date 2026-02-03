import { Directive, Host, Inject, LOCALE_ID, OnInit, Optional } from '@angular/core'
import { Calendar } from 'primeng/calendar'
import { FileUpload } from 'primeng/fileupload'
import { ConfirmDialog } from 'primeng/confirmdialog'
import { localeData } from 'moment'
import 'moment/locale/ar-sa'

@Directive({ selector: '[appPrimeNGi18n]' })
export class PrimeNGi18nDirective implements OnInit {
  constructor(
    @Inject(LOCALE_ID) private locale: string,
    @Host() @Optional() private fileUpload: FileUpload,
    @Host() @Optional() private calendar: Calendar,
    @Host() @Optional() private confirmDialog: ConfirmDialog
  ) {}

  ngOnInit(): void {
    if (this.locale === 'en') {
      return
    }

    if (this.fileUpload) {
      this.localizeFileUpload()
    }

    if (this.calendar) {
      this.localizeCalendar()
    }

    if (this.confirmDialog) {
      this.localizeConfirmDialog()
    }
  }

  private localizeFileUpload() {
    this.fileUpload.chooseLabel = $localize`:PrimeNG File Upload|:Choose`
    this.fileUpload.uploadLabel = $localize`:PrimeNG File Upload|:Upload`
    this.fileUpload.cancelLabel = $localize`:PrimeNG File Upload|:Cancel`
    this.fileUpload.invalidFileSizeMessageSummary = $localize`:PrimeNG File Upload|:{0}: Invalid file size, `
    this.fileUpload.invalidFileSizeMessageDetail =
      ' ' + $localize`:@@programUploadVideoMaxSize:Maximum upload size is` + ' {0} ' //$localize`:PrimeNG File Upload|:maximum upload size is {0}.`
    this.fileUpload.invalidFileTypeMessageSummary = $localize`:PrimeNG File Upload|:{0}: Invalid file type, `
    this.fileUpload.invalidFileTypeMessageDetail = $localize`:PrimeNG File Upload|:allowed file types: {0}.`
    this.fileUpload.invalidFileLimitMessageSummary = $localize`:PrimeNG File Upload|:Maximum number of files exceeded, `
    this.fileUpload.invalidFileLimitMessageDetail = $localize`:PrimeNG File Upload|:limit is {0} at most.`
  }

  private localizeCalendar() {
    if (this.locale === 'ar') {
      const data = localeData('ar-sa')

      this.calendar.locale = {
        dayNames: data.weekdays(),
        dayNamesShort: data.weekdaysShort(),
        dayNamesMin: data.weekdaysMin(),
        monthNames: data.months(),
        monthNamesShort: data.monthsShort(),
        today: $localize`:PrimeNG Calendar|:Today`,
        clear: $localize`:PrimeNG Calendar|:Clear`,
      }
    }
  }

  private localizeConfirmDialog() {
    this.confirmDialog.acceptLabel = $localize`:PrimeNG Confirm Dialog|:Yes`
    this.confirmDialog.rejectLabel = $localize`:PrimeNG Confirm Dialog|:No`
  }
}
