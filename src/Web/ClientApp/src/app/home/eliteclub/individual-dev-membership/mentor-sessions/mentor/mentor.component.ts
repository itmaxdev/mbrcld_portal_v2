import { Component, OnInit } from '@angular/core'
import { ActivatedRoute } from '@angular/router'
import { Mentor, MentorsClient } from 'src/app/shared/api.generated.clients'
import { SectionDataService } from 'src/app/shared/services/section-data.service'

@Component({
  selector: 'app-mentor',
  templateUrl: './mentor.component.html',
})
export class MentorComponent implements OnInit {
  mentor: Mentor

  constructor(
    private mentorService: MentorsClient,
    private activatedRoute: ActivatedRoute,
    private section: SectionDataService
  ) {}

  goBack() {
    this.section.redirectBack(2)
  }

  ngOnInit(): void {
    const mentorId = this.activatedRoute.snapshot.paramMap.get('id')
    if (mentorId) {
      this.mentorService.mentor(mentorId).subscribe((data) => {
        if (data) this.mentor = data
      })
    }
  }
}
