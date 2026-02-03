import * as moment from 'moment'
import { Component, OnInit } from '@angular/core'
import { ActivatedRoute, Router } from '@angular/router'
import { EliteMentorSessionsClient } from 'src/app/shared/api.generated.clients'

@Component({
  selector: 'app-mentor-sessions',
  templateUrl: './mentor-sessions.component.html',
})
export class MentorSessionsComponent implements OnInit {
  mentorSessions: Array<any>

  constructor(
    private mentorSessionsServise: EliteMentorSessionsClient,
    private router: Router,
    private activatedRoute: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.mentorSessionsServise.eliteMentorSession().subscribe((data) => {
      if (data) this.mentorSessions = data

      this.mentorSessions.forEach((session) => {
        session.newSessionDate = session.date
        session.formattedDate = moment(session.date).lang('en').format('MMM DD -  hh:mm')
      })
    })
  }

  goToMentor(id: string): void {
    this.router.navigate([`../eliteclub-mentor/${id}`], { relativeTo: this.activatedRoute })
  }

  setSessionDate(sessionId: string, newDate: Date) {
    this.mentorSessionsServise
      .setDate({ date: newDate, eliteMentorSessionId: sessionId })
      .subscribe(() => {
        const session = this.mentorSessions.find((session) => session.id == sessionId)
        session.date = newDate
        session.formattedDate = moment(newDate).lang('en').format('MMM DD -  hh:mm')
      })
  }
}
