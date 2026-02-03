import * as moment from 'moment'
import { Component, Input, OnInit, Inject, LOCALE_ID } from '@angular/core'
import { EliteMembershipAttendanceClient } from 'src/app/shared/api.generated.clients'

enum SessionStatus {
  Decline = 1,
  Accept = 2,
  Pending = 3,
}

@Component({
  selector: 'app-session',
  templateUrl: './session.component.html',
  styleUrls: ['./session.component.scss'],
})
export class SessionComponent implements OnInit {
  @Input() id: string
  @Input() name: string
  @Input() fromDate: Date
  @Input() toDate: Date
  @Input() status: SessionStatus
  @Input() description: string
  fromMonth: string
  toMonth: string
  sessionStatusPrint: string

  constructor(
    @Inject(LOCALE_ID) private locale: string,
    private eliteMembershipAttendanceClient: EliteMembershipAttendanceClient
  ) {}

  ngOnInit(): void {
    this.fromMonth = moment(this.fromDate).lang('en').format('MMM DD')
    this.toMonth = moment(this.toDate).lang('en').format('MMM DD')
    if (this.locale === 'ar') {
      if (this.status == SessionStatus.Decline) this.sessionStatusPrint = 'لا ارغب'
      else if (this.status == SessionStatus.Accept) this.sessionStatusPrint = 'ارغب'
      else this.sessionStatusPrint = 'Pending'
    } else if (this.locale === 'en') {
      if (this.status == SessionStatus.Decline) this.sessionStatusPrint = 'Decline'
      else if (this.status == SessionStatus.Accept) this.sessionStatusPrint = 'Accept'
      else this.sessionStatusPrint = 'Pending'
    }
  }

  attend(isAccept = false): void {
    this.eliteMembershipAttendanceClient
      .eliteMembershipAttendancePost({
        id: this.id,
        attendanceStatus: isAccept ? SessionStatus.Accept : SessionStatus.Decline,
      })
      .subscribe(() => {
        if (this.locale === 'ar') {
          if (!isAccept) this.sessionStatusPrint = 'لا ارغب'
          else this.sessionStatusPrint = 'ارغب'
        } else if (this.locale === 'en') {
          if (!isAccept) this.sessionStatusPrint = 'Decline'
          else this.sessionStatusPrint = 'Accept'
        }
      })
  }
}
